using MvT.Bll.Abstract.Main;
using MvT.Bll.System;
using MvT.Dal.Repositories.Abstract.Main;
using MvT.Entities.Interface;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Bll.Concretes.Main
{
    public class BaseManager<T> : IManager<T> where T : class, IDbEntity
    {
        protected IRepository<T> _IRepository;

        private readonly string tabloAdiDegistir = MvTUtilityFunctioncs.TabloAdiniDegistirMsg(typeof(T).Name);

        public BaseManager(IRepository<T> ir)
        {
            _IRepository = ir;

        }

        public string Insert(T rec)
        {
            Task<T> returnRec = _IRepository.Insert(rec);
            if (returnRec != null)
                return tabloAdiDegistir + "Kayıt işlemi başarılı.";
            else
                return tabloAdiDegistir + "Kayıt işlemi başarısız.";
        }

        public string InsertRange(List<T> listRec)
        {
            Task<List<T>> returnlistRec = _IRepository.InsertRange(listRec);
            if (returnlistRec != null)
                return tabloAdiDegistir + "Kayıt işlemi başarılı.";
            else
                return tabloAdiDegistir + "Kayıt işlemi başarısız.";
        }

        public string Update(T rec)
        {
            Task<T> returnRec = _IRepository.Update(rec);
            if (returnRec != null)
                return tabloAdiDegistir + " Güncelleme işlemi başarılı.";
            else
                return tabloAdiDegistir + " Güncelleme işlemi başarısız.";
        }

        public string UpdateRange(List<T> listRec)
        {
            Task<List<T>> returnlistRec = _IRepository.UpdateRange(listRec);
            if (returnlistRec != null)
                return tabloAdiDegistir + "Güncelleme işlemi başarılı.";
            else
                return tabloAdiDegistir + "Güncelleme işlemi başarısız.";
        }

        public string Delete(T rec)
        {
            Task<T> returnRec = _IRepository.Insert(rec);
            if (returnRec != null)
                return tabloAdiDegistir + " Silme işlemi başarılı.";
            else
                return tabloAdiDegistir + " Silme işlemi başarısız.";
        }

        public Task<T> Find(long id)
        {
            Task<T> returnRec = _IRepository.Find(id);
            return returnRec;
        }

        public Task<List<T>> GetAll()
        {
            Task<List<T>> returnRecList = _IRepository.GetAll();
            return returnRecList;
        }

        public Task<List<T>> GetModifieds()
        {
            Task<List<T>> returnRecList = _IRepository.GetModifieds();
            return returnRecList;
        }

    }
}
