using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Entities.Interface
{
    public interface IEntity
    {
        public int Id { get; set; }
        public DateTime CratedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public MvT.Dal.System.DataStatus DateStatus { get; set; }
    }
}
