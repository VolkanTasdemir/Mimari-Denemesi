using MvT.Entities.Interface;
using MvT.Entities.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Entities.Model.Stok
{
    public class Category : DbBaseEntity
    {
        public long?  MainCategoryID { get; set; }
        public string? CategoryName { get; set; } = null;
        public string? CategoryDesciription { get; set; } = null;
        public string? CategoryJson { get; set; } = null;
        public bool   CategoryActive { get; set; }
    }
}
