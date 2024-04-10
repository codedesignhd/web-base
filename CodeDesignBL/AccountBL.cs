﻿using System.Collections.Generic;
using CodeDesign.ES;
using CodeDesign.Models;
using log4net;
using CodeDesign.Utilities;
using CodeDesign.Dtos;
namespace CodeDesign.BL
{
    public class AccountBL : BaseBL
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AccountBL));
        #region Init
        private static AccountBL _instance;
        public static AccountBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new AccountBL();
                }
                return _instance;
            }
        }
        #endregion


        #region Login + Register
        /// <summary>
        /// Find user has username or email with password, if return account info if founded, ortherwise return default
        /// </summary>
        public Models.Account Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                password = CryptoUtils.HashPasword(password.ChuanHoa());
                _logger.Debug("Login");
                return AccountRepository.Instance.Login(username.ChuanHoa(), password);
            }
            return default;
        }

        public KeyValuePair<bool, string> Register(RegisterUserDto dto)
        {
            if (dto != null)
            {
                dto.email = dto.email.ChuanHoa();
                dto.username = dto.username.ChuanHoa();
                List<string> duplicates = AccountRepository.Instance.GetIfDuplicate(dto.username, dto.email);
                if (duplicates.Contains(dto.username))
                {
                    return new KeyValuePair<bool, string>(false, "Username đã được đăng ký bởi người dùng khác");
                }
                if (duplicates.Contains(dto.email))
                {
                    return new KeyValuePair<bool, string>(false, "Email đã được đăng ký bởi người dùng khác");
                }
                Models.Account tk = new Models.Account()
                {
                    username = dto.username,
                    email = dto.email,
                    fullname = dto.fullname,
                    role = Role.USER,
                    nguoi_tao = dto.username,
                    ngay_tao = DateTimeUtils.TimeInEpoch(),
                };
                var res = AccountRepository.Instance.Index(tk);
                if (res.success)
                {
                    return new KeyValuePair<bool, string>(true, "Đăng ký thành công");
                }
                return new KeyValuePair<bool, string>(false, "Đăng ký thất bại");
            }
            return new KeyValuePair<bool, string>(false, "Lỗi dữ liệu");
        }
        #endregion

        #region 
        public bool UpdateLastLogin(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                return AccountRepository.Instance.Update(username, new
                {
                    last_login = DateTimeUtils.TimeInEpoch()
                });
            }
            return false;
        }
        public KeyValuePair<bool, string> DeleteAccount(string username)
        {
            return new KeyValuePair<bool, string>();
        }

        #endregion


        public bool IsUserExist(string identity)
        {
            if (!string.IsNullOrWhiteSpace(identity))
            {
                return AccountRepository.Instance.IsUserExist(identity);
            }
            return true;
        }

        public Models.Account Get(string id, string[] fields = null)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return AccountRepository.Instance.Get(id, fields);
            }
            return null;
        }


        public List<Models.Account> GetAll(string[] fields = null)
        {
            return AccountRepository.Instance.GetAll(fields);
        }


        public bool CHekccAnyThing()
        {
            Account acc = new Account()
            {
                thuoc_tinh = new List<int> { (int)ThuocTinhHeThong.Da_mua_goi_cuoc, (int)ThuocTinhHeThong.Mua_goi_tra_tien }
            };

            ///thuoc_tinh: [3,5]
            ///Query: user nào đã mua gói cước và mua gói trả tiền
            ///
            ///EF:        context.Accounts.Where(acc=>acc.IsDaMuaGoiCuoc && acc.IsDaMuaGoiTraTien)
            ///Dynamic:   context.Accounts.Where(acc=>acc.thuoc_tinh.contains((int)ThuocTinhHeThong.Da_mua_goi_cuoc) && acc.thuoc_tinh.contains((int)ThuocTinhHeThong.Mua_goi_tra_tien))


            ///Table Accoount: 5 col
            ///Table Order: 6 col
            ///Order join => Account => 11 col => 1m rows
            ///View: col1, col2, col3, ....col 11.
            ///

            /// SQL: string (dạng json) =>  (id_don_hang, thoi_gian_mua)
            /// NoSql
            /// 


            ///: Template => Title: XIn chào {{username}}. Anh chị đọc đống bên dưới {{content}}

            /// Content: => Chỗ khác.
            return false;
        }
    }
}
