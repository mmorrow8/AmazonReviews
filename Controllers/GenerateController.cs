using AmazonReviews.Interfaces;
using AmazonReviews.Models;
using AmazonReviews.Processors;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace AmazonReviews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerateController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;
        private readonly VaderSentimentAnalyzer _sentimentAnalyzer;
        private readonly MarkovChain _markovChain;

        public GenerateController(IProductReviewService productReviewService, VaderSentimentAnalyzer sentimentAnalyzer, MarkovChain markovChain)
        {
            _sentimentAnalyzer = sentimentAnalyzer;
            _productReviewService = productReviewService;
            _markovChain = markovChain;
        }

        [HttpGet]
        [Route("generate")]
        public async Task<IActionResult> GenerateReview()
        {
            var review = await _productReviewService.GetProductReviewAsync();

            //replace the original text of a product review text with a generated review
            review.ReviewText = await _markovChain.GenerateAsync(1000);

            review.StarRating = _sentimentAnalyzer.AnalyzeSentiment(review.ReviewText);

            return Ok(new
            {
                ProductName = review.Product.Title,
                ProductImage = review.Product.ImageURL,
                ProductHiRes = review.Product.ImageURLHighRes,
                ReviewerName = review.ReviewerName,
                ReviewText = review.ReviewText,
                StarRating = review.StarRating,
            });
        }
    }
}
