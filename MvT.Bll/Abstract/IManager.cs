using MvT.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Repositories.Abstract
{
    internal interface IManager<T> where T : class, IDbEntity
    {
        Task Insert(T rec);
        Task InsertRange(List<T> recList);
        Task Update(T rec);
        Task UpdateRange(List<T> recList);
        Task Delete(T Rec);
        Task DeleteRange(List<T> recList);
        IQueryable<T> GetAll();
        IQueryable<T> GetModifieds();
        Task<T> Find(int id);
    }
}
