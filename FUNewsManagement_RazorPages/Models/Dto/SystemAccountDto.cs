using System.Text.Json.Serialization;

namespace FUNewsManagement_RazorPages.Models.Dto
{
    public class SystemAccount
    {
        [JsonPropertyName("accountId")]
        public short AccountId { get; set; }

        [JsonPropertyName("accountName")]
        public string AccountName { get; set; } = string.Empty;

        [JsonPropertyName("accountEmail")]
        public string AccountEmail { get; set; } = string.Empty;

        [JsonPropertyName("accountRole")]
        public int AccountRole { get; set; }

        [JsonPropertyName("accountPassword")]
        public string AccountPassword { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("comments")]
        public List<Comment>? Comments { get; set; } = new();
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
