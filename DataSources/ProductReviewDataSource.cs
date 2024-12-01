using AmazonReviews.Interfaces;
using AmazonReviews.Models;
using System.IO.MemoryMappedFiles;
using System.Text.Json;

namespace AmazonReviews.DataSources
{
    public class ProductReviewDataSource : IProductReviewDataSource
    {
        public ProductReviewDataSource(string reviewsDataSourcePath, string productsDataSourcePath) : base(reviewsDataSourcePath, productsDataSourcePath)
        {
        }

        public virtual async Task<List<T>> GetDataAsync<T>(string path)
        {
            var items = new List<T>();

            // Create memory-mapped file
            using var memoryMappedFile = System.IO.MemoryMappedFiles.MemoryMappedFile.CreateFromFile(
                path,
                FileMode.Open,
                null,
                0,
                System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read
            );

            // Create a view stream to access the file
            using var stream = memoryMappedFile.CreateViewStream(0, 0, System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read);
            using var reader = new StreamReader(stream);

            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        var item = JsonSerializer.Deserialize<T>(line.Trim());
                        if (item != null)
                        {
                            items.Add(item);
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize line: {line}. Error: {ex.Message}");
                    }
                }
            }

            return items;
        }

        public override async Task<List<Review>> GetReviewsAsync()
        {
            return await GetDataAsync<Review>(_reviewsDataSourcePath);
        }

        public override async Task<List<Product>> GetProductsAsync()
        {
            return await GetDataAsync<Product>(_productsDataSourcePath);
        }
    }
}
