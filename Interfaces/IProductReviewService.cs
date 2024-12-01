using AmazonReviews.DTO;
using AmazonReviews.Models;
using System.Text.Json;

namespace AmazonReviews.Interfaces
{
    public interface IProductReviewService
    {
        Task LoadProductReviewsAsync();

        Task<List<string>> GetAllReviewTextAsync();

        Task<List<ReviewDTO>> GetAllReviewsAsync();
        
        Task<ReviewDTO> GetProductReviewAsync();
    }
}
