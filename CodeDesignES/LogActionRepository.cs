using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeDesignModels;
using Nest;

namespace CodeDesignES
{
    public class LogActionRepository : ESRepositoryBase<LogAction>
    {
        #region Init

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
                    _instance = new LogActionRepository(string.Format("{0}logaction", prefix_index));
                }
                return _instance;
            }
        }

        #endregion
    }
}
