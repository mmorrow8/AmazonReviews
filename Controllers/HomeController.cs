using AmazonReviews.DTO;
using AmazonReviews.Interfaces;
using AmazonReviews.Models;
using AmazonReviews.Processors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static AmazonReviews.Program;

namespace AmazonReviews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductReviewService _productReviewService;
        private readonly DataLoader _dataLoader;

        public HomeController(ILogger<HomeController> logger, IProductReviewService productReviewService, DataLoader dataLoader)
        {
            _logger = logger;
            _productReviewService = productReviewService;
            _dataLoader = dataLoader;
        }

        public async Task<IActionResult> Index()
        {
            if (!_dataLoader.IsLoaded)
            {
                return View("Loading");
            }

            List<ReviewDTO> reviews = await _productReviewService.GetAllReviewsAsync();
            return View(reviews);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
