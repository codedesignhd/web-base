using Microsoft.AspNetCore.Mvc;

namespace CodeDesignWeb.Components
{
    [ViewComponent(Name = "Pagination")]
    public class PaginationComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CodeDesignDtos.Pagination.Pager pagination)
        {
            return View(pagination);
        }
    }
}
