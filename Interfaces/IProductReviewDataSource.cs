using AmazonReviews.Models;

namespace AmazonReviews.Interfaces
{
    public abstract class IProductReviewDataSource
    {
        protected string _productsDataSourcePath { get; set; }

        protected string _reviewsDataSourcePath { get; set; }

        public IProductReviewDataSource(string productsDataSourcePath, string reviewsDataSourcePath)
        {
            _productsDataSourcePath = productsDataSourcePath;
            _reviewsDataSourcePath = reviewsDataSourcePath;
        }

        public abstract Task<List<Product>> GetProductsAsync();

        public abstract Task<List<Review>> GetReviewsAsync();
    }
}
