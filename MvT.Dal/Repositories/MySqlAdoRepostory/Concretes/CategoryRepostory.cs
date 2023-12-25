using MvT.Dal.Context;
using MvT.Dal.Entities;
using MvT.Entities.Model.Stok;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Repositories.MySqlAdoRepostory.Concretes
{
    public class CategoryRepostory : BaseRepository<Category>
    {
        private const string selectColumns = "*";
        private const string joinTable = " ";

        public CategoryRepostory(Login login) : base(login, selectColumns, joinTable)
        {

        }
    }
}
