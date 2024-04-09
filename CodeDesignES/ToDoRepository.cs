using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeDesign.Models;
using Elasticsearch.Net;
using Nest;

namespace CodeDesign.ES
{
    public class ToDoRepository : AbstractESRepository, IESRepository<ToDo>
    {
        #region Init

        public ToDoRepository(string modify_index)
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


        private static ToDoRepository _instance;
        public static ToDoRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _index = string.Format("{0}_todo", prefix_index);
                    _instance = new ToDoRepository(_index);
                }
                return _instance;
            }
        }

        #endregion

        #region CRUD
        public (bool success, string id) Index(ToDo data, string id = "", string route = "")
        {
            return Index<ToDo>(data, data.id);
        }

        public bool Delete(string id, bool isForceDelete = false)
        {
            return Delete<ToDo>(id, isForceDelete);
        }

        public List<ToDo> MultiGet(IEnumerable<string> ids, string[] fields = null)
        {
            return MultiGet<ToDo>(ids, fields);
        }

        public bool Update(string id, object obj)
        {
            return Update<ToDo>(id, obj);
        }

        public bool Delete(string id)
        {
            return Delete<ToDo>(id);
        }

        public ToDo Get(string id, string[] fields = null)
        {
            return Get<ToDo>(id, fields);
        }

        public List<ToDo> GetAll(string[] fields = null)
        {
            SourceFilter so = new SourceFilter()
            {
                Includes = fields,
            };
            return GetObjectScroll<ToDo>(null, so).ToList();
        }


        #endregion
    }
}
