using MvT.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CratedDate = DateTime.Now;
            DateStatus = (short)DataStatus.Inserted;
        }
        public int Id { get; set; }
        public DateTime CratedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short DateStatus { get; set; }
    }
}
