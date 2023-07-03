using Microsoft.EntityFrameworkCore;
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
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<PlanItDbContext>(
            dbContextOptions => dbContextOptions.UseMySQL(
                builder.Configuration["ConnectionStrings:PlanItDBConnectionString"]));

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IExperienceRepository, ExperienceRepository>();

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