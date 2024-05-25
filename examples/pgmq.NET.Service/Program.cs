using pgmq_consumer;
using pgmq_web;
using PGMQ.NET.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQueueWriterSingleton(options => {
    options.ConnectionString = "Host=172.19.51.214;Database=postgres;Username=postgres;Password=postgres;";
    options.QueueName = "test5";
});
builder.Services.AddQueueReaderSingleton(options =>
{
    options.ConnectionString = "Host=172.19.51.214;Database=postgres;Username=postgres;Password=postgres;";
    options.QueueName = "test5";
});

builder.Services.AddHostedService<ReaderBackgroundService>();
builder.Services.AddHostedService<WriterBackgroundService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}