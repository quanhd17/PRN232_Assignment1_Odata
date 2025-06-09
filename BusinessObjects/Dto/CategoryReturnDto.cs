using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class CategoryReturnDto : IEntity<short>
    {
        public short CategoryId { get; set; }
        short IEntity<short>.Id => CategoryId;
        public string CategoryName { get; set; } = null!;

        public string CategoryDesciption { get; set; } = null!;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }
        public virtual ParentCategoryDto? ParentCategory { get; set; }
    }
}
