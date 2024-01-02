using MvT.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Entities.Stok
{
    public class CategoryEnt : BaseEntity
    {
        public long? MainCategoryID { get; set; }
        public string? CategoryName { get; set; } = null;
        public string? CategoryDesciription { get; set; } = null;
        public string? CategoryJson { get; set; } = null;
        public bool CategoryActive { get; set; }
    }
}
