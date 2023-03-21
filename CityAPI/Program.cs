using CityAPI.DbContexts;
using CityAPI.Services;
using CityAPI.Stores;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// To clear all existing providers
//builder.Logging.ClearProviders();

//builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.


builder.Services.AddControllers(options =>
{
    //options.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters()
.AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
    builder.Services.AddTransient<IMailService, LocalMailService>();
#else
    builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CityDataStore>();
builder.Services.AddDbContext<CityContext>(ops => ops.UseSqlite(builder.Configuration["connectionStrings:citycc"]));


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
