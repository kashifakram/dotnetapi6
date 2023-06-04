using System.Text;
using CityAPI.DbContexts;
using CityAPI.Services;
using CityAPI.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CityAPI
{
    internal static class StartupHelperExtensions
    {
        // Add services to the container
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(ops =>
                {
                    ops.ReturnHttpNotAcceptable = true;
                    ops.CacheProfiles.Add("30MinsCacheProfile", new CacheProfile { Duration = 1800 });
                })
                .AddXmlDataContractSerializerFormatters()
                .AddNewtonsoftJson()
                .ConfigureApiBehaviorOptions(ops =>
                {
                    ops.InvalidModelStateResponseFactory = context =>
                    {
                        // create a validation problem details object
                        var problemDetailsFactory =
                            context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                        var validationProblemDetails =
                            problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext,
                                context.ModelState);

                        // add additional info not added by default
                        validationProblemDetails.Detail = "See the errors field for details";
                        validationProblemDetails.Instance = context.HttpContext.Request.Path;

                        // report invalid model state responses as validation issues
                        validationProblemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
                        validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        validationProblemDetails.Title = "One or more validation errors occurred.";

                        return new UnprocessableEntityObjectResult(validationProblemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

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
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretKey"]))
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

            builder.Services.AddResponseCaching();
            builder.Services.AddHttpCacheHeaders(expirationModelOptions =>
                {
                    expirationModelOptions.MaxAge = 60;
                    expirationModelOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
                },
                validationModelOptions =>
                {
                    validationModelOptions.MustRevalidate = true;
                });

            return builder.Build();

        }

        // Configure the request/response pipelien
        public  static WebApplication ConfigurePipeline(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happend, try again later.");
                    });
                });
            }

#if DEBUG
            app.UseResponseCaching();
#else
            app.UseHttpCacheHeaders();
#endif
            app.UseHttpsRedirection();
            app.UseRouting();


            // this will be used by secured API / Service to authenticate user and verify user token configured in services before granting access to resource
            app.UseAuthentication();

            // this will be used as attribute on controllers / actions / assemblies to secure access to their resources
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
