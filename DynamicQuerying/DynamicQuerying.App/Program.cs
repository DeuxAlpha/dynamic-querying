using DynamicQuerying.Sample.Contexts;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var logConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console();

Log.Logger = logConfig.CreateLogger();

// Add services to the container.

// builder.Services.AddLogging(loggingBuilder =>
// {
    // loggingBuilder.AddSerilog(logConfig.CreateLogger());
// });

builder.Services.AddControllers();
builder.Services.AddDbContext<SampleContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();