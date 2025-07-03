using System.Text.Json.Serialization;

namespace FUNewsManagement_RazorPages.Models.Dto
{
    public class TagDto
    {
        [JsonPropertyName("tagId")]
        public int TagId { get; set; }

        [JsonPropertyName("tagName")]
        public string? TagName { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }
    }
}
