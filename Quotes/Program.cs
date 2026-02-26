using DotNetEnv;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MongoDB.Driver;
using Quotes.Configurations;
using Quotes.Repositories;
using Quotes.Services;

namespace Quotes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            if (builder.Configuration.GetValue<bool>("FeatureFlags:UseMongoDb"))
            {
                builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(MongoDbOptions.SectionName));

                builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
                {
                    var mongoDbOptions = builder.Configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>();
                    
                    return new MongoClient(mongoDbOptions?.ConnectionString);
                });

                builder.Services.AddScoped<ISubscriberRepository, MongoDbSubscriberRepository>();
            }
            else if (builder.Configuration.GetValue<bool>("FeatureFlags:UseCosmosDb"))
            {
                builder.Services.Configure<CosmosDbOptions>(builder.Configuration.GetSection(CosmosDbOptions.SectionName));

                builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
                {
                    var cosmosDbOptions = builder.Configuration.GetSection(CosmosDbOptions.SectionName).Get<CosmosDbOptions>();

                    return new MongoClient(cosmosDbOptions?.ConnectionString);
                });

                builder.Services.AddScoped<ISubscriberRepository, CosmosDbSubscriberRepository>();
            }
            else
            {
                builder.Services.AddSingleton<ISubscriberRepository, InMemorySubscriberRepository>();
            }

            builder.Services.AddScoped<INewsletterService, NewsletterService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Run Production without publish
            //StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
