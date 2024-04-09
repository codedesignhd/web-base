using CodeDesign.BL;
using CodeDesign.BL.Response;
using CodeDesign.Dtos;
using CodeDesign.Models;
using CodeDesign.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesign.WebAPI.Controllers
{
    //[Authorize(Policy = AppPolicy.AdminOnly)]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : BaseController
    {
        #region DI
        public ToDosController(AppDependencyProvider dependency) : base(dependency)
        {

        }
        #endregion

        [HttpGet, Route("")]
        public IActionResult GetAll()
        {
            List<CodeDesign.Models.ToDo> todos = ToDoBL.Instance.GetAll(null);
            return new JsonResult(new PagedResponse<CodeDesign.Models.ToDo>()
            {
                currentPage = 1,
                data = todos,
                totalPages = 1,
                total = todos.Count,
                success = true,
            });
        }
        [HttpPost, Route("Add")]
        public IActionResult Create(UpsertToDoDto dto)
        {
            var result = _dependencies.Validator.UpdateToDo.Validate(dto);
            if (result.IsValid)
            {
                BaseResponse response = ToDoBL.Instance.Create(dto);
                return new JsonResult(response);
            }
            return new JsonResult(new Response(false, result.GetMessage()));
        }

        [HttpGet, Route("{id}")]
        public IActionResult Detail(string id)
        {
            ToDo response = ToDoBL.Instance.GetById(id);
            return new JsonResult(new
            {
                data = response,
            });
        }

        [HttpPost, Route("{id}/Update")]
        public IActionResult Update(string id, UpsertToDoDto dto)
        {
            var result = _dependencies.Validator.UpdateToDo.Validate(dto);
            if (result.IsValid)
            {
                BaseResponse response = ToDoBL.Instance.Update(AppUser, id, dto);
                return new JsonResult(response);
            }
            return new JsonResult(new Response(false, result.GetMessage()));
        }


        [HttpPost, Route("{id}/UpdateTrangThaiThucHien")]
        public IActionResult UpdateTrangThaiThucHien(string id, TrangThaiThucHien trang_thai_thuc_hien)
        {
            BaseResponse response = ToDoBL.Instance.UpdateTrangThaiThucHien(AppUser, id, trang_thai_thuc_hien);
            return new JsonResult(response);
        }

        [HttpDelete, Route("{id}/Delete")]
        public IActionResult Delete(string id)
        {
            BaseResponse response = ToDoBL.Instance.Delete(AppUser, id);
            return new JsonResult(response);
        }
    }
}
