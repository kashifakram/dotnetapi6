using CityAPI;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var app = builder.ConfigureServices().ConfigurePipeline();

// for demo purposes, delete the database & migrate on startup so 
// we can start with a clean slate
#if DEBUG
    await app.ResetDatabaseAsync();
#endif

// run the app
app.Run();