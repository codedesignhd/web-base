using System;
using System.Collections.Generic;
using System.Text;
using CodeDesign.ES;
using CodeDesign.Models;

namespace CodeDesign.BL
{
    public class LogActionBL : BaseBL
    {
        #region Init
        private static LogActionBL _instance;
        public static LogActionBL Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new LogActionBL();
                }
                return _instance;
            }
        }
        #endregion

        #region CRUD
        public KeyValuePair<bool, string> Index(LogAction log_action)
        {
            var res = LogActionRepository.Instance.Index(log_action);
            return new KeyValuePair<bool, string>();
        }
        #endregion
    }
}
