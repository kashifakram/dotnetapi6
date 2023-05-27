using System.Text;
using CityAPI.DbContexts;
using CityAPI.Services;
using CityAPI.Stores;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddSingleton<CityDataStore>();
builder.Services.AddDbContext<CityContext>(ops => ops.UseSqlite(builder.Configuration["connectionStrings:citycc"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICityRepo, CityRepo>();

// this service to be added to secured services / APIs pipeline to grant access to resources to eligible users only
builder.Services.AddAuthentication("Bearer").AddJwtBearer(ops =>
{
    ops.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"], // validating token issuer from jwt token passed by user used to generate token
        ValidAudience = builder.Configuration["Authentication:Audience"], // validating token audience from jwt token passed by user used to generate token
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretKey"])) // validating token secret from jwt token passed by user key used to generate token
    };
});

builder.Services.AddAuthorization(ops =>
{
    ops.AddPolicy("MustBeFromFSD", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Faisalabad");
    });
});

#if DEBUG
    builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// this will be used by secured API / Service to authenticate user and verify user token configured in services before granting access to resource
app.UseAuthentication();

// this will be used as attribute on controllers / actions / assemblies to secure access to their resources
app.UseAuthorization();

app.MapControllers();

app.Run();
