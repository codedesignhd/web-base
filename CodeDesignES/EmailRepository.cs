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
    public class EmailRepository : AbstractESRepository, IESRepository<Email>
    {
        #region Init
        public EmailRepository(string modify_index)
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


        private static EmailRepository _instance;
        public static EmailRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _index = string.Format("{0}_email", prefix_index);
                    _instance = new EmailRepository(_index);
                }
                return _instance;
            }
        }

        #endregion


        #region CRUD
        public (bool success, string id) Index(Email data, string id = "", string route = "")
        {
            return Index<Email>(data, data.id);
        }

        public bool Delete(string id, bool isForceDelete = false)
        {
            return Delete<Email>(id, isForceDelete);
        }

        public List<Email> MultiGet(IEnumerable<string> ids, string[] fields = null)
        {
            return MultiGet<Email>(ids, fields);
        }

        public bool Update(string id, object obj)
        {
            return Update<Email>(id, obj);
        }

        public bool Delete(string id)
        {
            return Delete<Email>(id);
        }

        public Email Get(string id, string[] fields = null)
        {
            return Get<Email>(id, fields);
        }

        public List<Email> GetAll(string[] fields = null)
        {
            SourceFilter so = new SourceFilter()
            {
                Includes = fields,
            };
            return GetObjectScroll<Email>(null, so).ToList();
        }


        #endregion

        #region Service
        public List<Email> GetMailResend(int max_error, string[] fields = null)
        {
            return new List<Email>();
        }
        #endregion
    }
}
