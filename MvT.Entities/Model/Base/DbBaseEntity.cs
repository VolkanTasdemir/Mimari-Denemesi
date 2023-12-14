using MvT.Entities.Enums;
using MvT.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Entities.Model.Base
{
    public abstract class DbBaseEntity : IDbEntity
    {
        public DbBaseEntity()
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
