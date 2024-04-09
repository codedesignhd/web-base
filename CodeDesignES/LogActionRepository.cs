using System;
using System.Collections.Generic;
using System.Text;
using CodeDesign.ES.Core.Models;
using CodeDesign.Models;
using Nest;

namespace CodeDesign.ES
{
    public class LogActionRepository : AbstractESRepository, IESRepository<LogAction>
    {
        #region Init
        public LogActionRepository(string modify_index)
        {
            _index = !string.IsNullOrEmpty(modify_index) ? modify_index : _index;
            ConnectionSettings settings = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default)
                .DefaultIndex(_index)
                .DisableDirectStreaming(true)
                .MaximumRetries(10);
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
        public (bool success, string id) Index(LogAction data, string id = "", string route = "")
        {
            return Index<LogAction>(data, id, route);
        }

        public bool Update(string id, object doc)
        {
            return Update<LogAction>(id, doc);
        }

        public bool Delete(string id, bool isForceDelete = false)
        {
            return Delete<LogAction>(id, isForceDelete);
        }

        public LogAction Get(string id, string[] fields = null)
        {
            return Get<LogAction>(id, fields);
        }

        public List<LogAction> MultiGet(IEnumerable<string> ids, string[] fields = null)
        {
            return MultiGet<LogAction>(ids, fields);
        }

        public ScrollResult<LogAction> GetScroll(string scrollId, SearchRequest request)
        {
            return GetScroll<LogAction>(scrollId, request);
        }

        #endregion
    }
}
