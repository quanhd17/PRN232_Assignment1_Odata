using System.Text.Json.Serialization;

namespace FUNewsManagement_RazorPages.Models.Dto
{
    public class LoginDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("accountId")]
        public short AccountId { get; set; }

        [JsonPropertyName("accountName")]
        public string AccountName { get; set; } = string.Empty;

        [JsonPropertyName("accountEmail")]
        public string AccountEmail { get; set; } = string.Empty;

        [JsonPropertyName("accountRole")]
        public int AccountRole { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }
}
