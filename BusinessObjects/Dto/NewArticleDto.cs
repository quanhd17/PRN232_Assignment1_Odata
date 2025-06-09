using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class NewArticleDto
    {
        public NewsArticle NewsArticle { get; set; }
        public List<int> TagIds { get; set; }
    }
}
