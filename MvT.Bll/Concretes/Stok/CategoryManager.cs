using MvT.Dal.Repositories.Abstract;
using MvT.Entities.Model.Stok;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Bll.Concretes.Stok
{
    public class CategoryManager : BaseManager<Category>
    {
        public CategoryManager(IRepository<Category> repository) : base(repository)
        {

        }
    }
}
