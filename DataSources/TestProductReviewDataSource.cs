using AmazonReviews.Interfaces;
using AmazonReviews.Models;

namespace AmazonReviews.DataSources
{
    public class TestProductReviewDataSource : IProductReviewDataSource
    {
        public TestProductReviewDataSource(string reviewsDataSourcePath, string productsDataSourcePath) : base(reviewsDataSourcePath, productsDataSourcePath)
        {
        }
        
        public override Task<List<Product>> GetProductsAsync()
        {
            var returnList = new List<Product>()
            { 
                new Product()
                {
                    Title = "WWF Panda Calendar",
                    Asin = "B001234567",
                    ImageURL = new List<string>
                    {
                        "https://images-na.ssl-images-amazon.com/images/I/61dT3KL9b5L._SS40_.jpg",
                        "https://images-na.ssl-images-amazon.com/images/I/61vvpL77jlL._SS40_.jpg",
                        "https://images-na.ssl-images-amazon.com/images/I/51rhcw5vAPL._SS40_.jpg"
                    },
                    ImageURLHighRes = new List<string>
                    {
                        "https://images-na.ssl-images-amazon.com/images/I/61dT3KL9b5L.jpg",
                        "https://images-na.ssl-images-amazon.com/images/I/61vvpL77jlL.jpg",
                        "https://images-na.ssl-images-amazon.com/images/I/51rhcw5vAPL.jpg"
                    }
                },
            };

            return Task.FromResult(returnList);
        }

        public override Task<List<Review>> GetReviewsAsync()
        {
            var returnList = new List<Review>()
            {
                new Review
                {
                    ASIN = "B001234567",
                    ReviewerName = "John Doe",
                    ReviewText = "This product is amazing! It exceeded my expectations and I would definitely recommend it to others."
                }
            };

            return Task.FromResult(returnList);
        }
    }
}
