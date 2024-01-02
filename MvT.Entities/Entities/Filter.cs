using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Entities
{
    public class Filter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
        public bool IsLike { get; set; } = false;
        public bool IsBetween { get; set; } = false;
        public bool IsNotNull { get; set; } = false;
        public bool IsNull { get; set; } = false;
        public String LogicOperator { get; set; } = "AND";
    }
}
