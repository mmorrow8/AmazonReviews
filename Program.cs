using AmazonReviews.Interfaces;
using AmazonReviews.Processors;
using Microsoft.Extensions.DependencyInjection;
using AmazonReviews.DataSources;

namespace AmazonReviews
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            AddSingletons(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllers();

            app.Run();
        }

        private static void AddSingletons(WebApplicationBuilder builder)
        {
            var lexiconFilePath = "data/vader_lexicon.txt";
            var useTestData = builder.Configuration.GetValue<bool>("UseTestData"); // true or false
            var productDataSource = builder.Configuration["ProductDataSource"];
            var reviewDataSource = builder.Configuration["ReviewDataSource"];

            builder.Services.AddSingleton<VaderSentimentAnalyzer>(sp => new VaderSentimentAnalyzer(lexiconFilePath));
            
            builder.Services.AddSingleton<IProductReviewDataSource>(provider =>
            {
                return useTestData
                    ? new TestProductReviewDataSource("", "")
                    : new ProductReviewDataSource(productDataSource, reviewDataSource);
            });

            builder.Services.AddSingleton<IProductReviewService, ProductReviewService>();

            builder.Services.AddSingleton<MarkovChain>(sp => new MarkovChain());

            builder.Services.AddSingleton<DataLoader>();
            builder.Services.AddHostedService<DataLoaderBackgroundService>();
        }

        public class DataLoader
        {
            public bool IsLoaded { get; private set; } = false;
            private readonly IServiceProvider _serviceProvider;

            public DataLoader(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public async Task LoadDataAsync()
            {
                using var scope = _serviceProvider.CreateScope();
                var reviewProcessor = scope.ServiceProvider.GetRequiredService<IProductReviewService>();
                var markovChain = scope.ServiceProvider.GetRequiredService<MarkovChain>();

                // Load products and reviews
                await reviewProcessor.LoadProductReviewsAsync();

                // Load Markov chain training data
                var allReviewText = await reviewProcessor.GetAllReviewTextAsync();
                await markovChain.TrainAsync(allReviewText);

                IsLoaded = true;
            }
        }

        public class DataLoaderBackgroundService : BackgroundService
        {
            private readonly IServiceProvider _serviceProvider;

            public DataLoaderBackgroundService(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                using var scope = _serviceProvider.CreateScope();
                var dataLoader = scope.ServiceProvider.GetRequiredService<DataLoader>();

                // Ensure data loading is triggered
                await dataLoader.LoadDataAsync();
            }
        }
    }
}
