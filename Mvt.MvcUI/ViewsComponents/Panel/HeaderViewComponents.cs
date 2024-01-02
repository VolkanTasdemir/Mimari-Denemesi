using Microsoft.AspNetCore.Mvc;

namespace Mvt.MvcUI.ViewsComponents.Panel
{
    public class HeaderViewComponents : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
