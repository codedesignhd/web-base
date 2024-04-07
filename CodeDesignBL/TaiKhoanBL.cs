using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using CodeDesign.DTO.Dtos.TaiKhoan;
using CodeDesign.ES;
using CodeDesign.Models;
using log4net;
using log4net.Core;
using Utils;
namespace CodeDesign.BL
{
    public class TaiKhoanBL : BaseBL
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TaiKhoanBL));
        #region Init
        private static TaiKhoanBL _instance;
        public static TaiKhoanBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new TaiKhoanBL();
                }
                return _instance;
            }
        }
        #endregion


        #region Login + Register
        /// <summary>
        /// Find user has username or email with password, if return account info if founded, ortherwise return default
        /// </summary>
        public TaiKhoan Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                password = CryptoUtils.HashPasword(password.ChuanHoa());
                _logger.Debug("Login");
                return TaiKhoanRepository.Instance.Login(username.ChuanHoa(), password);
            }
            return default;
        }

        public KeyValuePair<bool, string> Register(RegisterUserDto dto)
        {
            if (dto != null)
            {
                dto.email = dto.email.ChuanHoa();
                dto.username = dto.username.ChuanHoa();
                List<string> duplicates = TaiKhoanRepository.Instance.GetIfDuplicate(dto.username, dto.email);
                if (duplicates.Contains(dto.username))
                {
                    return new KeyValuePair<bool, string>(false, "Username đã được đăng ký bởi người dùng khác");
                }
                if (duplicates.Contains(dto.email))
                {
                    return new KeyValuePair<bool, string>(false, "Email đã được đăng ký bởi người dùng khác");
                }
                TaiKhoan tk = new TaiKhoan()
                {
                    username = dto.username,
                    email = dto.email,
                    fullname = dto.fullname,
                    role = Role.USER,
                    nguoi_tao = dto.username,
                    ngay_tao = DateTimeUtils.TimeInEpoch(),
                };
                var res = TaiKhoanRepository.Instance.Index(tk);
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
                return TaiKhoanRepository.Instance.Update(username, new
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
                return TaiKhoanRepository.Instance.IsUserExist(identity);
            }
            return true;
        }

        public TaiKhoan Get(string id, string[] fields = null)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return TaiKhoanRepository.Instance.Get(id, fields);
            }
            return null;
        }


        public List<TaiKhoan> GetAll(string[] fields = null)
        {
            return TaiKhoanRepository.Instance.GetAll(fields);
        }

    }
}
