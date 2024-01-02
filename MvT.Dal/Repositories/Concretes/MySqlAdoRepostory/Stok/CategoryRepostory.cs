using MvT.Dal.Entities;
using MvT.Dal.Repositories.Concretes.MySqlAdoRepostory.Main;
using MvT.Entities.Model.Stok;

namespace MvT.Dal.Repositories.Concretes.MySqlAdoRepostory.Stok
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
