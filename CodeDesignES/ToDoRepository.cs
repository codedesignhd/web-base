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
    public class ToDoRepository : IESRepository
    {
        #region Init
        private static string _default_index;

        public ToDoRepository(string modify_index)
        {
            _default_index = !string.IsNullOrEmpty(modify_index) ? modify_index : _default_index;
            ConnectionSettings settings = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default).DefaultIndex(_default_index).DisableDirectStreaming(true).MaximumRetries(10);
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
                    _default_index = string.Format("{0}_todo", prefix_index);
                    _instance = new ToDoRepository(_default_index);
                }
                return _instance;
            }
        }

        #endregion


        #region CRUD
        public (bool, string) Index(ToDo to_do)
        {
            return Index<ToDo>(to_do);
        }

        public bool Update(string id, object obj)
        {
            return Update<ToDo>(id, obj);
        }

        public bool Delete(string id, bool is_delete_permanent = false)
        {
            return Delete<ToDo>(id, is_delete_permanent);
        }

        public List<ToDo> GetAll(string[] fields = null)
        {
            SourceFilter so = new SourceFilter()
            {
                Includes = fields,
            };
            return GetObjectScroll<ToDo>(null, so).ToList();
        }

        public ToDo Get(string id, string[] fields = null)
        {
            return Get<ToDo>(id, fields);
        }
        #endregion
    }
}
