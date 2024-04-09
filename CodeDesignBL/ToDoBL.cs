using System;
using System.Collections.Generic;
using System.Text;
using CodeDesign.BL.Response;
using CodeDesign.DTO.Dtos.ToDo;
using CodeDesign.DTO.Vms;
using CodeDesign.ES;
using CodeDesign.Models;
using CodeDesign.Utilities;

namespace CodeDesign.BL
{
    public class ToDoBL : BaseBL
    {
        #region Init
        private static ToDoBL _instance;
        public static ToDoBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ToDoBL();
                }
                return _instance;
            }
        }
        #endregion

        #region CRUD
        public ResponseBase Create(UpsertToDoDto dto)
        {
            if (dto == null)
            {
                return new Response.Response(false, "Invalid data");
            }
            else
            {
                long thoi_gian_ket_thuc = DateTimeUtils.StringToEpoch(dto.ngay_ket_thuc);
                ToDo to_do = new ToDo()
                {
                    title = dto.title,
                    thoi_gian_ket_thuc = thoi_gian_ket_thuc,
                };
                (bool success, string id) result = ToDoRepository.Instance.Index(to_do);
                if (result.success)
                {
                    return new Response.Response(true, "Success");
                }
                return new Response.Response(false, "Error");
            }
        }

        public ResponseBase Update(AppUser user, string id, UpsertToDoDto dto)
        {
            if (string.IsNullOrWhiteSpace(id) || dto == null)
            {
                return new Response.Response(false, "Invalid data");
            }
            else
            {
                ToDo toDo = ToDoRepository.Instance.Get(id, new string[] { "nguoi_tao" });
                if (toDo != null && toDo.nguoi_tao == user.Username)
                {
                    long thoi_gian_ket_thuc = DateTimeUtils.StringToEpoch(dto.ngay_ket_thuc);
                    object doc = new
                    {
                        id,
                        dto.title,
                        thoi_gian_ket_thuc,
                        ngay_sua = DateTimeUtils.TimeInEpoch(),
                    };
                    bool success = ToDoRepository.Instance.Update(id, doc);
                    if (success)
                    {
                        return new Response.Response(true, "Update success");
                    }
                    return new Response.Response(false, "Update error");
                }
                return new Response.Response(false, "Invalid data");
            }
        }

        public ResponseBase UpdateTrangThaiThucHien(AppUser user, string id, TrangThaiThucHien trang_thai_thuc_hien)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new Response.Response(false, "Invalid data");
            }
            else
            {
                ToDo toDo = ToDoRepository.Instance.Get(id, new string[] { "nguoi_tao" });
                if (toDo != null && toDo.nguoi_tao == user.Username)
                {
                    object doc = new
                    {
                        id,
                        trang_thai_thuc_hien,
                        ngay_sua = DateTimeUtils.TimeInEpoch(),
                    };
                    bool success = ToDoRepository.Instance.Update(id, doc);
                    if (success)
                    {
                        return new Response.Response(true, "Update success");
                    }
                    return new Response.Response(false, "Update error");
                }
                return new Response.Response(false, "Invalid data");
            }
        }

        public ResponseBase Delete(AppUser user, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new Response.Response(false, "Invalid data");
            }
            else
            {
                ToDo toDo = ToDoRepository.Instance.Get(id, new string[] { "nguoi_tao" });
                if (toDo != null && toDo.nguoi_tao == user.Username)
                {
                    bool success = ToDoRepository.Instance.Delete(id);
                    if (success)
                    {
                        return new Response.Response(true, "Deleted");
                    }
                    return new Response.Response(false, "Delete error");
                }

                return new Response.Response(false, "Invalid data");

            }
        }


        public List<ToDo> GetAll(string[] fields = null)
        {
            return ToDoRepository.Instance.GetAll(fields);
        }

        public ToDo GetById(string id, string[] fields = null)
        {
            return ToDoRepository.Instance.Get(id, fields);
        }
        #endregion
    }
}
