using System.Collections.Generic;
using CodeDesign.ES;
using CodeDesign.Models;
using log4net;
using CodeDesign.Utilities;
using CodeDesign.ES.Models;
using CodeDesign.Dtos.Responses;
using CodeDesign.Dtos.Accounts;
using CodeDesign.Dtos.Auth;
using CodeDesign.BL.Providers;
using System;
using CodeDesign.Couchbase;
using CodeDesign.Dtos.Caches;
using System.Security.Principal;
using Nest;
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

        #endregion

        #region Update Account
        /// <summary>
        /// Đổi mật khẩu (yêu cầu mật khẩu cũ)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response ChangePassword(ChangePwdRequest request)
        {
            Response response = new Response();

            Account account = AccountRepository.Instance.Get(request.username, new string[] { "password" });
            if (account is null)
            {
                response.message = "Tài khoản không tồn tại trong hệ thống";
                return response;
            }
            ///Check mật khẩu cũ xem người dùng nhập có đúng hay không rồi mới cho đổi mật khẩu
            string oldPassword = CryptoUtils.HashPasword(request.old_password);
            if (string.Equals(oldPassword, account.password))
            {
                account.password = CryptoUtils.HashPasword(request.new_password);
                response.success = AccountRepository.Instance.Update(account.username, new
                {
                    account.password,
                });
                response.message = response.success ? "Thành công" : "Có lỗi xảy ra";
            }
            else
            {
                response.message = "Mật khẩu cũ bạn nhập không chính xác";
            }
            return response;
        }


        /// <summary>
        /// Cập nhật thông tin user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response UpdateUserInfo(UpdateUserInfoRequest request)
        {
            Response response = new Response();

            Account account = AccountRepository.Instance.Get(request.username, new string[] { "password" });
            if (account is null)
            {
                response.message = "Tài khoản không tồn tại trong hệ thống";
                return response;
            }
            account.fullname = request.fullname;
            account.dob = DateTimeUtils.StringToEpoch(request.dob);
            response.success = AccountRepository.Instance.Update(account.id, new
            {
                account.fullname,
                account.dob,
            });
            response.message = response.success ? "Thành công" : "Có lỗi xảy ra";
            return response;
        }

        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <param name="username"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public bool UpdateAvatar(string username, string avatar)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                return AccountRepository.Instance.Update(username, new
                {
                    avatar = avatar,
                    last_login = DateTimeUtils.TimeInEpoch()
                });
            }
            return false;
        }

        /// <summary>
        /// Cập nhật ngày hoạt động cuối
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Xóa tài khoản khỏi hệ thống
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Kiểm tra nếu user tồn tại trong hệ thống (check bằng email, username)
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public bool IsUserExist(string identity)
        {
            if (!string.IsNullOrWhiteSpace(identity))
            {
                return AccountRepository.Instance.IsUserExist(identity);
            }
            return true;
        }


        /// <summary>
        /// Tạo email khôi phục mật khẩu và tạo token trên couchbase, nếu quá hạn thì không truy cập được link
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public Response RecoveryPassword(string identity)
        {
            if (string.IsNullOrWhiteSpace(identity))
                return new Response(false, "Không tìm thấy tài khoản");
            Account account = AccountRepository.Instance.GetByIdentity(identity);
            if (account is null)
                return new Response(false, "Không tìm thấy tài khoản");
            if (string.IsNullOrWhiteSpace(account.email))
                return new Response(false, "Tài khoản chưa thiết lập email đặt lại mật khẩu");


            string key = CouchbaseKeyProvider.GenResetPasswordKey(account.username);
            long epoch = Utilities.DateTimeUtils.TimeInEpoch();
            ResetPasswordCache cache = CouchbaseService.Instance.Get<ResetPasswordCache>(key);
            if (cache != null && cache.ExpireDate > epoch)
            {
                return new Response(false, "Bạn vừa yêu cầu đặt lại mật khẩu, vui lòng chờ ít phút trước khi gửi yêu cầu tiếp theo");
            }

            //string code = RandomUtils.GenCode(6);
            long expireDate = Utilities.DateTimeUtils.TimeInEpoch(DateTime.UtcNow.AddMinutes(5));
            cache = new ResetPasswordCache
            {
                ExpireDate = expireDate,
                //Code = code,
                Username = account.username,
            };
            bool success = CouchbaseService.Instance.Insert(key, cache, TimeSpan.FromMinutes(30));
            if (success)
            {
                string token = Utilities.CryptoUtils.Encode(account.username);
                //Tạo email gắn link đặt lại mật khẩu
                string hiddenEmail = Utilities.StringUtils.HideEmail(account.email);
                string message = string.Format("Một email chứa đường dẫn đặt lại mật khẩu đã được gửi tới địa chỉ {0}, vui lòng kiểm tra hòm thư và đặt lại mật khẩu", hiddenEmail);
                return new Response<string>(true, message)
                {
                    data = hiddenEmail,
                };
            }
            return new Response(false, "Có lỗi khi gửi email đặt lại mật khẩu, vui lòng thử lại sau");
        }
        /// <summary>
        /// Check token và trả lại username dưới dạng hash nếu hợp lệ
        /// </summary>
        public Response VerifyRecoverPasswordToken(string token)
        {
            long epoch = Utilities.DateTimeUtils.TimeInEpoch();
            string username = Utilities.CryptoUtils.Decode(token);
            string key = CouchbaseKeyProvider.GenResetPasswordKey(username);
            ResetPasswordCache cache = CouchbaseService.Instance.Get<ResetPasswordCache>(key);
            if (cache is null || cache.ExpireDate < epoch)
                return new Response(false, "Link đặt mật khẩu không hợp lệ hoặc đã hết hạn");
            return new Response(true, "Hợp lệ");
        }
        /// <summary>
        /// Decode token để xác định username và reset lại mật khẩu
        /// </summary>
        public Response ResetPassword(ResetPasswordRequest request)
        {
            string username = Utilities.CryptoUtils.Decode(request.token);
            string key = CouchbaseKeyProvider.GenResetPasswordKey(username);
            long epoch = Utilities.DateTimeUtils.TimeInEpoch();
            ResetPasswordCache cache = CouchbaseService.Instance.Get<ResetPasswordCache>(key);
            if (cache is null || cache.ExpireDate < epoch)
                return new Response(false, "Có lỗi khi xác minh người dùng");

            string newPassword = CryptoUtils.HashPasword(request.new_password);
            bool success = AccountRepository.Instance.Update(username, new
            {
                password = newPassword
            });

            return new Response(success, success ? "Mật khẩu đã được thay đổi thành công" : "Có lỗi khi thay đổi mật khẩu");
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
