using System.Collections.Generic;
using CodeDesign.ES;
using CodeDesign.Models;
using log4net;
using CodeDesign.Utilities;
using CodeDesign.Dtos;
using CodeDesign.Dtos.Account;
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
                return AccountRepository.Instance.Login(username.ChuanHoa(), password);
            }
            return default;
        }

        public Response.Response Register(RegisterUserRequest request)
        {
            if (request != null)
            {
                request.email = request.email.ChuanHoa();
                request.username = request.username.ChuanHoa();
                List<string> duplicates = AccountRepository.Instance.GetIfDuplicate(request.username, request.email);
                if (duplicates.Contains(request.username))
                {
                    return new Response.Response(false, "Username đã được đăng ký bởi người dùng khác");
                }
                if (duplicates.Contains(request.email))
                {
                    return new Response.Response(false, "Email đã được đăng ký bởi người dùng khác");
                }
                Models.Account tk = new Models.Account()
                {
                    username = request.username,
                    email = request.email,
                    fullname = request.fullname,
                    role = Role.User,
                    nguoi_tao = request.username,
                    password = CryptoUtils.HashPasword(request.password.ChuanHoa())
                };
                var res = AccountRepository.Instance.Index(tk);
                if (res.success)
                {
                    return new Response.Response(res.success, "Đăng ký thành công");
                }
                return new Response.Response(res.success, "Đăng ký thất bại");
            }
            return new Response.Response(false, "Lỗi dữ liệu");
        }

        public Response.Response ChangePassword(ChangePwdRequest request)
        {
            Response.Response response = new Response.Response();

            return response;
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

    }
}
