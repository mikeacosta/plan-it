using PlanIt.API;
using Serilog;
using Serilog.Events;

var name = typeof(Program).Assembly.GetName().Name;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Assembly", name)
    .WriteTo.Console()
    .WriteTo.File("logs/planit.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args)
    .ConfigureHost()
    .ConfigureServices();

var app = builder.Build()
    .ConfigurePipeline();

app.Run();