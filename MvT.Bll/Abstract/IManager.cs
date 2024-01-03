using MvT.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Bll.Abstract
{
    public interface IManager<T> where T : class, IDbEntity
    {
        //Modify 
        string Insert(T rec);
        string InsertRange(List<T> listRec);
        string Update(T rec);
        string UpdateRange(List<T> listRec);
        string Delete(T recId);
        //Task DeleteRange(List<T> recList);

        //List
        Task<List<T>> GetAll();
        Task<List<T>> GetModifieds();//Update olanlar
        //Find
        Task<T> Find(long id);
    }
}
