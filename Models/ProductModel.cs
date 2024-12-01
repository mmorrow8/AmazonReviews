using AmazonReviews.Processors;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AmazonReviews.Models
{
    public class Product
    {
        [JsonPropertyName("category")]
        public List<string> Category { get; set; }

        [JsonPropertyName("tech1")]
        public string Tech1 { get; set; }

        [JsonPropertyName("description")]
        public List<string> Description { get; set; }

        [JsonPropertyName("fit")]
        public string Fit { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("also_buy")]
        public List<string> AlsoBuy { get; set; }

        [JsonPropertyName("tech2")]
        public string Tech2 { get; set; }

        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonPropertyName("feature")]
        public List<string> Feature { get; set; }

        [JsonPropertyName("rank")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> Rank { get; set; }

        [JsonPropertyName("also_view")]
        public List<string> AlsoView { get; set; }

        [JsonPropertyName("main_cat")]
        public string MainCat { get; set; }

        [JsonPropertyName("similar_item")]
        public string SimilarItem { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("asin")]
        public string Asin { get; set; }

        [JsonPropertyName("imageURL")]
        public List<string> ImageURL { get; set; }

        [JsonPropertyName("imageURLHighRes")]
        public List<string> ImageURLHighRes { get; set; }
    }
}
