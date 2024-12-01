using AmazonReviews.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AmazonReviews.DTO
{
    public class ProductDTO
    {        
        public string Title { get; set; }

        public string Asin { get; set; }

        public List<string> ImageURL { get; set; }

        public List<string> ImageURLHighRes { get; set; }
    }
}
