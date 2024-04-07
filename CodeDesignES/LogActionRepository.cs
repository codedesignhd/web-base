using System;
using System.Collections.Generic;
using System.Text;
using CodeDesign.Models;
using Nest;

namespace CodeDesign.ES
{
    public class LogActionRepository : IESRepository
    {
        #region Init
        private static string _index;

        public LogActionRepository(string modify_index)
        {
            _index = !string.IsNullOrEmpty(modify_index) ? modify_index : _index;
            ConnectionSettings settings = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default).DefaultIndex(_index).DisableDirectStreaming(true).MaximumRetries(10);
            client = new ElasticClient(settings);
            var ping = client.Ping(p => p.Pretty(true));
            if (ping.ServerError != null && ping.ServerError.Error != null)
            {
                throw new Exception("START ES FIRST");
            }
        }


        private static LogActionRepository _instance;
        public static LogActionRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _index = string.Format("{0}_logaction", prefix_index);
                    _instance = new LogActionRepository(_index);
                }
                return _instance;
            }
        }

        #endregion

        #region CRUD
        public (bool success, string id) Index(LogAction action)
        {
            return Index<LogAction>(action);
        }
        #endregion
    }
}
