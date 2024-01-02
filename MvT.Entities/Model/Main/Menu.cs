using MvT.Entities.Interface;
using MvT.Entities.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Entities.Model.Main
{
    public class Menu : DbBaseEntity
    {
        public long? MainMenuID { get; set; }
        public string? MenuName { get; set; } = null;
        public string? MenuRoot { get; set; } = null;
        public bool MenuRootActive { get; set; }
    }
}
