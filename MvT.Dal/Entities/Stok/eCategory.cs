using MvT.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Entities.Stok
{
    public class ECategory : BaseEntity
    {
        public long? MainCategoryID { get; set; }
        public string CategoryName { get; set; } 
        public string CategoryDesciription { get; set; }
        public string CategoryJson { get; set; }
        public bool CategoryActive { get; set; }
    }
}
