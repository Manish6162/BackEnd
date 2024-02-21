using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Cosmos;
using BackEnd.Entities;
using Azure.Storage.Blobs;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // CORS policy for development, modify as needed for production
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        // Swagger configuration
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "YourApiName", Version = "v1" });
        });

        // Cosmos DB configuration
        services.AddSingleton<CosmosClient>(x =>
        {
            return new CosmosClient(Configuration.GetConnectionString("CosmosConnection"));
        });

        services.AddSingleton<BlobServiceClient>(x =>
        {
            return new BlobServiceClient(Configuration.GetConnectionString("YourBlobConnectionString"));
        });


        services.AddScoped<BlobServiceClient>(sp =>
        {
            var connectionString = Configuration.GetConnectionString("YourBlobConnectionString");
            return new BlobServiceClient(connectionString);
        });
        // Register CosmosDbContext
        services.AddSingleton<CosmosDbContext>();

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackEnd Api's");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        // CORS policy for development, modify as needed for production
        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
