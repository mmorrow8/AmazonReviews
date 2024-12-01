using AmazonReviews.DTO;
using AmazonReviews.Interfaces;
using AmazonReviews.Models;

namespace AmazonReviews.Processors
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewDataSource _dataSource;
        private List<ReviewDTO> reviews = new();
        private List<ProductDTO> products;

        public ProductReviewService(IProductReviewDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task LoadProductReviewsAsync()
        {
            var reviewsData = await _dataSource.GetReviewsAsync();
            reviews = reviewsData.Select(review => new ReviewDTO
            {
                ReviewerName = review.ReviewerName,
                ReviewText = review.ReviewText,
                ASIN = review.ASIN
            }).ToList();

            var productsData = await _dataSource.GetProductsAsync();
            products = productsData.Select(product => new ProductDTO
            {
                Title = product.Title,
                Asin = product.Asin,
                ImageURL = product.ImageURL,
                ImageURLHighRes = product.ImageURLHighRes
            }).ToList();

            await Task.CompletedTask;
        }

        public async Task<List<ReviewDTO>> GetAllReviewsAsync()
        {
            return reviews;
        }

        public async Task<List<string>> GetAllReviewTextAsync()
        {
            return reviews.Select(review => review.ReviewText).ToList();
        }

        public async Task<ReviewDTO> GetProductReviewAsync()
        {
            // Generate a review
            var random = new Random();
            var reviewIndex = random.Next(reviews.Count);

            var review = reviews[reviewIndex];
            review.StarRating = random.Next(1, 6); // Random star rating between 1 and 5
            
            
            review.Product = products.Where(p => p.Asin == review.ASIN).FirstOrDefault();
            return review;
        }
    }
}
