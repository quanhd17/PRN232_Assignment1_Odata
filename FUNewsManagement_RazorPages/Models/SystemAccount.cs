using BusinessObject.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUNewsManagement_RazorPages.Models
{
    public partial class SystemAccount : IEntity<short>
    {
        [Key, JsonPropertyName("accountId")]
        public short AccountId { get; set; }
        short IEntity<short>.Id => AccountId;
        public string AccountName { get; set; } = null!;

        public string AccountEmail { get; set; } = null!;

        public int AccountRole { get; set; }

        public string AccountPassword { get; set; } = null!;

        public bool IsActive { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
        [JsonIgnore]
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
