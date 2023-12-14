using MvT.Dal.Repositories.Abstract;
using MvT.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Repositories.Concretes
{
    public class BaseManager<T> : IManager<T> where T : class, IDbEntity
    {
        public Task Insert(T rec)
        {
            throw new NotImplementedException();
        }

        public Task InsertRange(List<T> recList)
        {
            throw new NotImplementedException();
        }
        public Task Update(T rec)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRange(List<T> recList)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T Rec)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(List<T> recList)
        {
            throw new NotImplementedException();
        }

        public Task<T> Find(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetModifieds()
        {
            throw new NotImplementedException();
        }

        
    }
}
