using MvT.Dal.Repositories.Abstract.Main;
using MvT.Entities.Model.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Bll.Concretes.Main
{
    public class MenuManager : BaseManager<Menu>
    {
        public MenuManager(IRepository<Menu> repository) : base(repository)
        {

        }
    }
}
