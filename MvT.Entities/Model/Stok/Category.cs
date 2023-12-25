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
        //public string CategoryName { get; set; }
        //public string CategoryDesciription { get; set; }
        //public string CategoryJson { get; set; }
        public bool   CategoryActive { get; set; }
    }
}
