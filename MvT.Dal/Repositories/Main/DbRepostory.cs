using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Repositories.Main
{
    internal interface DbRepostory<T> where T : class , MvT.Entities.Interface.IEntity
    {
    }
}
