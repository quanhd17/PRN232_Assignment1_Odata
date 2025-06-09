using BusinessObject.Models;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_RazorPages.Models
{
    public class SystemAccount
    {
        [Key]
        public short AccountId { get; set; }
        public string AccountName { get; set; } = null!;

        public string AccountEmail { get; set; } = null!;

        public int AccountRole { get; set; }

        public string AccountPassword { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
