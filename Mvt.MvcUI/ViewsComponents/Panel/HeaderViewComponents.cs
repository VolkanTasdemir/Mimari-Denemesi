using Microsoft.AspNetCore.Mvc;
using MvT.Bll.Concretes.Stok;

namespace Mvt.MvcUI.ViewsComponents.Panel
{
    public class HeaderViewComponents : ViewComponent
    {
        CategoryManager categoryManager;
        public IViewComponentResult Invoke()
        {
            var deger = categoryManager.GetAll();
            return View(deger);
        }
    }
}
