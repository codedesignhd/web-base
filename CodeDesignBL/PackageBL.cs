using CodeDesignDtos.Responses;
using CodeDesignES;
using log4net;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CodeDesignBL
{
    public class PackageBL : BaseBL
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PackageBL));

        #region Init
        private static PackageBL _instance;
        public static PackageBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new PackageBL();
                }
                return _instance;
            }
        }
        #endregion
        public Response DeletePack(string id)
        {
            Response response = new Response();
            if (string.IsNullOrWhiteSpace(id))
            {
                response.message = "Gói cước không tồn tại";
                return response;
            }
            response.success = PackageRepository.Instance.Delete(id);
            response.message = response.success ? "Thành công" : "Có lỗi xảy ra";
            return response;
        }
    }
}
