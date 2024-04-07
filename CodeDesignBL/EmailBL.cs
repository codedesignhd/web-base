using System;
using System.Collections.Generic;
using System.Text;
using CodeDesign.ES;
using CodeDesign.Models;
using Utils;

namespace CodeDesign.BL
{
    public class EmailBL : BaseBL
    {
        #region Init
        private static EmailBL _instance;
        public static EmailBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new EmailBL();
                }
                return _instance;
            }
        }
        #endregion


        public Email Get(string id, string[] fields = null)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return EmailRepository.Instance.Get(id, fields);
            }
            return null;
        }
    }
}
