using MvT.Dal.Entities;
using MvT.Entities.Model.Main;

namespace MvT.Dal.Repositories.Concretes.MySqlAdoRepostory.Main
{
    public class MenuRepostory : BaseRepository<Menu>
    {
        private const string selectColumns = "*";
        private const string joinTable = " ";

        public MenuRepostory(Login login) : base(login, selectColumns, joinTable)
        {

        }
    }
}
