using MvT.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Entities.Interface
{
    public interface IDbEntity
    {
        public long Id { get; set; }
        public DateTime CratedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short DateStatus { get; set; }
    }
}
