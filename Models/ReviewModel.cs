using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AmazonReviews.Models
{
    public class Review
    {
        [JsonPropertyName("overall")]
        public double Overall { get; set; }

        [JsonPropertyName("vote")]
        public string Vote { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("reviewTime")]
        public string ReviewTime { get; set; }

        [JsonPropertyName("reviewerID")]
        public string ReviewerID { get; set; }

        [JsonPropertyName("asin")]
        public string ASIN { get; set; }

        [JsonPropertyName("reviewerName")]
        public string ReviewerName { get; set; }

        [JsonPropertyName("reviewText")]
        public string ReviewText { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("unixReviewTime")]
        public long UnixReviewTime { get; set; }

        [JsonPropertyName("style")]
        public Style Style { get; set; }
    }

    public class Style
    {
        [JsonPropertyName("Format:")]
        public string Format { get; set; }

        [JsonPropertyName("Color:")]
        public string Color { get; set; }
    }
}
