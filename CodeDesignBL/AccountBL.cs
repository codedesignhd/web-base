using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using CodeDesign.DTO.Dtos.Account;
using CodeDesign.ES;
using CodeDesign.Models;
using CodeDesign.Utilities;
using log4net;
using log4net.Core;
namespace CodeDesign.BL
{
    public class AccountBL : BaseBL
    {
        #region Init

        private static readonly ILog _logger = LogManager.GetLogger(typeof(AccountBL));
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
        public Account Login(string username, string password)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                password = CryptoUtils.HashPasword(password.ChuanHoa());
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
                Account tk = new Account()
                {
                    username = dto.username,
                    email = dto.email,
                    fullname = dto.fullname,
                    role = Role.USER,
                    nguoi_tao = dto.username,
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

        public Account Get(string id, string[] fields = null)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return AccountRepository.Instance.Get(id, fields);
            }
            return default;
        }


        public List<Account> GetAll(string[] fields = null)
        {
            return new List<Account>();
        }
    }
}
