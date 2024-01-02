using MvT.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Repositories.Abstract.Main
{
    public interface IRepository<T> where T : class, IDbEntity
    {
        //burada sadece veritabanı tablosundaki kolonlraa göre işlem yapılıyor.
        //Modify 
        Task<T> Insert(T rec);
        Task<List<T>> InsertRange(List<T> listRec);
        Task<T> Update(T rec);
        Task<List<T>> UpdateRange(List<T> listRec);
        Task<int> Delete(T recId);
        //Task DeleteRange(List<T> recList);

        //List
        Task<List<T>> GetAll();
        Task<List<T>> GetModifieds();//Update olanlar
        //Find
        Task<T> Find(long id);

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
