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
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        _client = new DockerClientConfiguration().CreateClient();
        var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
            Image = "quay.io/tembo/pgmq-pg",
            Name = "postgres",
            Env =  ["POSTGRES_PASSWORD=postgres", "listen_addresses = '*'"],
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

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _client.Containers.StopContainerAsync("postgres", new ContainerStopParameters());
        await _client.Containers.RemoveContainerAsync("postgres", new ContainerRemoveParameters());
        _client.Dispose();
    }
}