# C# Conductor Worker Example

A simple C# worker example for Orkes Conductor. It demonstrates how to connect to a Conductor server, authenticate using an Orkes Application Key and Secret, register a worker, poll for tasks, and return results. This project serves as a template for developers who want to learn how to build C# workers for Conductor.

## Requirements and Setup

Before running this worker, you must:

- Have access to an Orkes Conductor instance (Orkes Cloud or self-hosted)
- Create an Application inside Orkes Console
- Generate an API Key and API Secret
- Assign Worker Permissions for each task type the worker will register (for example: "hello_world" in this demo)

If your application does not have those permissions, the worker will not be able to poll tasks.

## What This Worker Does

This example registers a worker for the task type "hello_world".

When executed, it takes an input field "name" from your workflow and returns:

{
  "message": "Hello <name>"
}

This shows how to read task input, build a TaskResult, and return output to the workflow.

## Configuration

Before running the example, update the server URL and credentials in the code:

var conductor_server = "<insert_url>";
var key_id = "<insert_key>";
var secret = "<insert_secret>";

These values correspond to:
- conductor_server → Your Orkes Conductor API endpoint
- key_id / secret → The Application credentials you generated
- These credentials must include worker permissions

Example configuration:

var conf = new Configuration
{
    BasePath = conductor_server,
    AuthenticationSettings = new OrkesAuthenticationSettings(key_id, secret)
};

## Running the Worker

Run:

dotnet run

The worker will:
- Connect to Conductor
- Poll for the "hello_world" task
- Execute tasks when available
- Shut down on Ctrl+C


## Extending This Template

To add more task workers:

1. Add new methods decorated with [WorkerTask]
2. Give each one a unique taskType
3. Ensure your Orkes Application has permissions for those tasks
4. Deploy or run on any .NET-supported environment

## Useful Links

Orkes Documentation: https://orkes.io/content/
Conductor C# SDK: https://github.com/conductor-sdk/conductor-csharp
Orkes Cloud Console: https://console.orkes.io
