using DotNetEnv;
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

            //bool useMongoDb = builder.Configuration.GetValue<bool>("FeatureFlags:UseMongoDb");

            bool useMongoDb = builder.Configuration.GetValue<bool>("FeatureFlags:UseMongoDb");

            if (useMongoDb)
            {
                // Whenever someone asks for MongoDbOptions, bind it from appsettings.json.
                builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection(MongoDbOptions.SectionName));

                // Configure MongoDB client
                builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
                {
                    var mongoDbOptions = builder.Configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>();
                    return new MongoClient(mongoDbOptions?.ConnectionString);
                });

                builder.Services.AddScoped<ISubscriberRepository, MongoDbSubscriberRepository>();
            }
            else
            {
                builder.Services.AddSingleton<ISubscriberRepository, InMemorySubscriberRepository>();
            }

            builder.Services.AddScoped<INewsletterService, NewsletterService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
