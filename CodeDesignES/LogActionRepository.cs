using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeDesign.Models;
using Nest;

namespace CodeDesign.ES
{
    public class LogActionRepository : ESRepositoryBase, IESRepository<LogAction>
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
            return Index<LogAction>(data, data.id);
        }

        public bool Delete(string id, bool isForceDelete = false)
        {
            return Delete<LogAction>(id, isForceDelete);
        }

        public List<LogAction> MultiGet(IEnumerable<string> ids, string[] fields = null)
        {
            return MultiGet<LogAction>(ids, fields);
        }

        public bool Update(string id, object obj)
        {
            return Update<LogAction>(id, obj);
        }

        public bool Delete(string id)
        {
            return Delete<LogAction>(id);
        }

        public LogAction Get(string id, string[] fields = null)
        {
            return Get<LogAction>(id, fields);
        }

        public List<LogAction> GetAll(string[] fields = null)
        {
            SourceFilter so = new SourceFilter()
            {
                Includes = fields,
            };
            return GetObjectScroll<LogAction>(null, so).ToList();
        }


        #endregion
    }
}
