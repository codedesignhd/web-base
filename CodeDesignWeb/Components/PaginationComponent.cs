using CodeDesign.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.Web.Components
{
    [ViewComponent(Name = "Pagination")]
    public class PaginationComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Pagination pagination)
        {
            return View(pagination);
        }
    }
}
