using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class LoginDto
    {
        public string AccountEmail { get; set; } = null!;
        public string AccountPassword { get; set; } = null!;
    }
}
