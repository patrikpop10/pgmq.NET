using System.ComponentModel;
using PGMQ.NET.Queue.Builder;
using PGMQ.NET.Queue.Interfaces;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace pgmq.NET.ComponentTests;

public class BaseTest
{
    protected string ConnectionString { get; set; } = "Host=127.0.0.1;Port=5432;Username=postgres;Password=postgres;Database=postgres;CommandTimeout=300;";
    protected string QueueName { get; set; } = "testqueue";
    protected bool CreateQueue { get; set; } = true;
    protected bool Unlogged { get; set; } = true;
    protected IQueueWriter QueueWriter { get; set; }
    protected IQueueReader QueueReader { get; set; }
    
    [ReadOnly(true)]
    private DockerClient _client;
    
    [SetUp]
    public async Task Setup()
    {
        _client = new DockerClientConfiguration().CreateClient();
        await _client.Images.PushImageAsync("quay.io/tembo/pgmq-pg:latest", new ImagePushParameters(), new AuthConfig(), new Progress<JSONMessage>());
        var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
            Image = "quay.io/tembo/pgmq-pg",
            Name = "postgres",
            Env =  ["POSTGRES_PASSWORD=postgres", "listen_addresses = '*'", 
                "POSTGRES_HOST_AUTH_METHOD=scram-sha-256",
                "POSTGRES_INITDB_ARGS=--auth-host=scram-sha-256"],
            HostConfig = new HostConfig {
                //Binds = [@"D:/Projects/pgmq.NET/test/pg_hba.conf:/var/lib/postgresql/data/", @"D:/Projects/pgmq.NET/test/postgresql.conf:/var/lib/postgresql/data/"],
                
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        "5432/tcp",
                        new List<PortBinding> { new() { HostPort = "5432" } }
                    }
                }
            } 
        });
        await _client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
        var execCreateParameters = new ContainerExecCreateParameters
        {
            AttachStdout = true,
            AttachStderr = true,
            Cmd = new List<string> { "/bin/sh", "-c", $"echo '{ await File.ReadAllTextAsync(@"D:\Projects\pgmq.NET\test\pg_hba.conf")}' > /var/lib/postgresql/data/" }
        };

        var execCreateResponse = await _client.Exec.ExecCreateContainerAsync(response.ID, execCreateParameters);
        await _client.Exec.StartContainerExecAsync(execCreateResponse.ID);
        QueueWriter = QueueBuilder.CreateQueueWriter(options => {
            options.ConnectionString = ConnectionString;
            options.QueueName = QueueName;
            options.CreateQueue = CreateQueue;
            options.Unlogged = Unlogged;
        });
        QueueReader = QueueBuilder.CreateQueueReader(options => {
            options.ConnectionString = ConnectionString;
            options.QueueName = QueueName;
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        await _client.Containers.StopContainerAsync("postgres", new ContainerStopParameters());
        await _client.Containers.RemoveContainerAsync("postgres", new ContainerRemoveParameters());
        _client.Dispose();
    }
}