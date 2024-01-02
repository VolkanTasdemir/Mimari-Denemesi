using MvT.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Dal.Entities.Main
{
    public class MenuEnt : BaseEntity
    {
        public long? MainMenuID { get; set; }
        public string? MenuName { get; set; } = null;
        public string? MenuRoot { get; set; } = null;
        public bool MenuRootActive { get; set; }

        public MenuEnt MainMenu { get; set; } = new MenuEnt();
    }
}
