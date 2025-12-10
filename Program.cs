// Import Conductor SDK namespaces and .NET dependencies
using Conductor.Client.Authentication;   // For handling API key/secret authentication (if needed)
using Conductor.Client.Worker;          // Provides classes for running Conductor workers
using Conductor.Client.Models;          // Defines Conductor models (e.g., Task, TaskResult)
using Conductor.Client.Extensions;      // Helper extensions, like task.ToTaskResult()
using Conductor.Client;                 // Core Conductor client
using Microsoft.Extensions.Hosting;     // For managing background services (used internally by WorkerHost)
using Microsoft.Extensions.Logging;     // For logging output

namespace example
{
    // This class contains worker implementation
    [WorkerTask] // marks this class as containing Conductor workers
    public class MyWorker
    {
        // define a worker for Conductor task 
        // taskType: name of task
        // batchSize: Number of tasks to poll at once
        // pollIntervalMs: How often (ms) to poll Conductor for tasks
        // workerId: an identifier for this worker process
        [WorkerTask(taskType: "hello_world", batchSize: 5, pollIntervalMs: 500, workerId: "csharp-worker")]
        public static TaskResult MyTask(Conductor.Client.Models.Task task)
        {
            // access task input data passed from workflow
            var inputData = task.InputData;

            // convert Conductor task to a TaskResult (needed for sending results back)
            var result = task.ToTaskResult();

            // add output data to return to th workflow
            result.OutputData = new Dictionary<string, object>
            {
                ["message"] = "Hello " + inputData.GetValueOrDefault("name", null)
            };
        
            return result;
        }

        // entry point of the application
        public static void Main(string[] args)
        {
            // configuration to connect to Conductor server

            var conductor_server = "<insert_url>";
            var key_id = "<insert_key>";
            var secret = "<insert_secret>";
            var conf = new Configuration
            {
                // if running Orkes Cloud, provide authentication here
                BasePath = conductor_server,
                AuthenticationSettings = new OrkesAuthenticationSettings(key_id, secret)
            };

            // create a worker host that merges polling and task execution
            var host = WorkflowTaskHost.CreateWorkerHost(conf, LogLevel.Debug);

            // start polling for tasks
            host.Start();

            Console.WriteLine("Press Ctrl+C to exit.");

            // handle Ctrl+C
            var evt = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                Console.WriteLine("Ctrl+C pressed. Shutting down...");
                evt.Set();
            };

            // block until user presses Ctrl+C
            evt.WaitOne();

            // stop the worker host gracefully
            host.StopAsync().GetAwaiter().GetResult();
        }
    }
}
