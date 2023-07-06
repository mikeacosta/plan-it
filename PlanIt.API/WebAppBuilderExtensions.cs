using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlanIt.API.DbContexts;
using PlanIt.API.Repositories;
using Serilog;

namespace PlanIt.API;

internal static class WebAppBuilderExtensions
{
    public static WebApplicationBuilder ConfigureHost(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();
        return builder;
    }
    
    // Add services to the container
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PlanIt",
                Version = "v1",
                Description = "Travel Planner API",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Joe Blow",
                    Email = "joeblow@gmail.com",
                    Url = new Uri("https://twitter.com/joeblow"),
                },
                License = new OpenApiLicense
                {
                    Name = "PlanIt API LICX",
                    Url = new Uri("https://example.com/license"),
                }
            });
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);            
        });

        builder.Services.AddDbContext<PlanItDbContext>(
            dbContextOptions => dbContextOptions.UseMySQL(
                builder.Configuration["ConnectionStrings:PlanItDBConnectionString"]));

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();
        builder.Services.AddScoped<IRatingRepository, RatingRepository>();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return builder;
    }
    
    // Configure the request/response pipeline
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}