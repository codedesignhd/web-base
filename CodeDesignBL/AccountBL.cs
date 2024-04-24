﻿using System.Collections.Generic;
using CodeDesign.ES;
using CodeDesign.Models;
using log4net;
using CodeDesign.Utilities;
using CodeDesign.ES.Models;
using CodeDesign.Dtos.Responses;
using CodeDesign.Dtos.Accounts;
using CodeDesign.Dtos.Auth;
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

        public bool IsUniqueRefreshToken(string refreshToken)
        {
            return string.IsNullOrWhiteSpace(refreshToken) ? false : AccountRepository.Instance.IsUniqueRefreshToken(refreshToken);
        }

        public Response UpdateRefreshToken(string username, RefreshToken token)
        {
            if (string.IsNullOrWhiteSpace(username) || token is null)
                return new Response(false, "Không tìm thấy user hoặc token không hợp lệ");
            bool success = AccountRepository.Instance.Update(username, new
            {
                id = username,
                refresh_token = token,
                last_login = Utilities.DateTimeUtils.TimeInEpoch(),
            });
            return new Response(success, success ? "Thành công" : "Thất bại");
        }

        public Account GetAccountByRefreshToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return default;
            return AccountRepository.Instance.GetByRefreshToken(token);
        }


        /// <summary>
        /// Find user has username or email with password, if return account info if founded, ortherwise return default
        /// </summary>
        public Models.Account Login(AuthRequest request)
        {
            request.password = CryptoUtils.HashPasword(request.password.ChuanHoa());
            return AccountRepository.Instance.Login(request.username.ChuanHoa(), request.password);
        }




        public Models.Account Login(string username, string password)
        {
            password = CryptoUtils.HashPasword(password.ChuanHoa());
            return AccountRepository.Instance.Login(username.ChuanHoa(), password);
        }

        public Response Register(RegisterUserRequest request)
        {
            if (request != null)
            {
                request.email = request.email.ChuanHoa();
                request.username = request.username.ChuanHoa();
                List<string> duplicates = AccountRepository.Instance.GetIfDuplicate(request.username, request.email);
                if (duplicates.Contains(request.username))
                {
                    return new Response(false, "Username đã được đăng ký bởi người dùng khác");
                }
                if (duplicates.Contains(request.email))
                {
                    return new Response(false, "Email đã được đăng ký bởi người dùng khác");
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
                    return new Response(res.success, "Đăng ký thành công");
                }
                return new Response(res.success, "Đăng ký thất bại");
            }
            return new Response(false, "Lỗi dữ liệu");
        }

        public Response ChangePassword(ChangePwdRequest request)
        {
            Response response = new Response();

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
            if (!string.IsNullOrWhiteSpace(username))
            {
                bool success = AccountRepository.Instance.Update(username, new
                {
                    id = username,
                    trang_thai_du_lieu = TrangThaiDuLieu.Deleted,
                });
                return new KeyValuePair<bool, string>(success, success ? "Thành công" : "Thất bại");
            }
            return new KeyValuePair<bool, string>(false, "Tài khoản không tồn tại");
        }

        public bool IsUserExist(string identity)
        {
            if (!string.IsNullOrWhiteSpace(identity))
            {
                return AccountRepository.Instance.IsUserExist(identity);
            }
            return true;
        }
        #endregion


        #region Search
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


        public PaginatedResponse<Account> SearchUser(SearchUserRequest request)
        {
            SearchResult<Account> result = AccountRepository.Instance.Search(request.q, SearchParamsBase.Create(request.scroll_id, request.page, request.page_size, request.sort_field, request.sort_dir));

            PaginatedResponse<Account> response = PaginatedResponse<Account>.Create(request);
            response.data = result.documents;
            response.total = result.total;
            response.scroll_id = result.scroll_id;
            return response;
        }
        #endregion

    }
}
