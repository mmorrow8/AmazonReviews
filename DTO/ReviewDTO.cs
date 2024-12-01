using AmazonReviews.Models;
using System.Text.Json.Serialization;

namespace AmazonReviews.DTO
{
    public class ReviewDTO
    {
        public string ASIN { get; set; }

        public string ReviewerName { get; set; }

        public string ReviewText { get; set; }

        public int StarRating { get; set; }

        public ProductDTO Product { get; set; }
    }
}
