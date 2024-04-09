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
    public class AccountRepository : AbstractESRepository, IESRepository<Account>
    {
        #region Init

        public AccountRepository(string modify_index)
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


        private static AccountRepository _instance;
        public static AccountRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _index = string.Format("{0}_account", prefix_index);
                    _instance = new AccountRepository(_index);
                }
                return _instance;
            }
        }

        #endregion

        #region CRUD
        public (bool success, string id) Index(Account data, string id = "", string route = "")
        {
            return Index<Account>(data, data.username);
        }

        public bool Delete(string id, bool isForceDelete = false)
        {
            return Delete<Account>(id, isForceDelete);
        }

        public List<Account> MultiGet(IEnumerable<string> ids, string[] fields = null)
        {
            return MultiGet<Account>(ids, fields);
        }

        public bool Update(string id, object obj)
        {
            return Update<Account>(id, obj);
        }

        public bool Delete(string id)
        {
            return Delete<Account>(id);
        }

        public Account Get(string id, string[] fields = null)
        {
            return Get<Account>(id, fields);
        }

        public List<Account> GetAll(string[] fields = null)
        {
            SourceFilter so = new SourceFilter()
            {
                Includes = fields,
            };
            return GetObjectScroll<Account>(null, so).ToList();
        }


        #endregion

        #region Function
        public Account Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                List<QueryContainer> should = new List<QueryContainer>()
                {
                    new TermQuery{Field="username.keyword", Value=username},
                    new TermQuery{Field="email.keyword", Value=username},
                };

                SearchRequest req = new SearchRequest(_index)
                {
                    Query = new QueryContainer(new BoolQuery
                    {
                        Should = should,
                        MustNot = CustomMustNot()
                    }),
                    Size = 1,
                    Source = CustomSource(null, new string[] { "password", "nguoi_tao", "nguoi_sua", "trang_thai", "ngay_sua", "ngay_tao" }),
                };
                var res = client.Search<Account>(req);
                if (res.IsValid && res.Total == 1)
                {
                    return res.Hits.Select(ToDocument).First();
                }
                return default;
            }
            return default;
        }
        public bool IsUserExist(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                List<QueryContainer> should = new List<QueryContainer>()
                {
                    new TermQuery{Field="username.keyword", Value=username},
                    new TermQuery{Field="email.keyword", Value=username},
                };

                SearchRequest req = new SearchRequest(_index)
                {
                    Query = new QueryContainer(new BoolQuery
                    {
                        Should = should,
                        MustNot = CustomMustNot(),
                        MinimumShouldMatch = 1,
                    }),
                    Size = 1,
                };
                var res = client.Search<Account>(req);
                return res.IsValid && res.Total > 0;
            }
            return false;
        }

        public List<string> GetIfDuplicate(string username, string email)
        {
            List<string> duplicate_user_email = new List<string>();
            if (!string.IsNullOrWhiteSpace(username) || !string.IsNullOrWhiteSpace(email))
            {
                List<QueryContainer> should = new List<QueryContainer>();
                if (!string.IsNullOrWhiteSpace(username))
                {
                    should.Add(new TermQuery { Field = "username.keyword", Value = username });
                }
                if (!string.IsNullOrWhiteSpace(email))
                {
                    should.Add(new TermQuery { Field = "email.keyword", Value = email });
                }
                SearchRequest req = new SearchRequest(_index)
                {
                    Query = new QueryContainer(new BoolQuery { Should = should, MinimumShouldMatch = 1, MustNot = CustomMustNot() }),
                    Size = 2,
                    Aggregations = new AggregationDictionary
                    {
                        {"DUP_USER", new TermsAggregation("DUP_USER"){Field= "username.keyword" } },
                        {"DUP_EMAIL", new TermsAggregation("DUP_EMAIL"){Field="email.keyword" } },
                    }
                };
                var res = client.Search<Account>(req);
                if (res.IsValid)
                {
                    duplicate_user_email.AddRange(res.Aggregations.Terms("DUP_USER").Buckets.Select(x => x.Key));
                    duplicate_user_email.AddRange(res.Aggregations.Terms("DUP_EMAIL").Buckets.Select(x => x.Key));
                }
            }
            return duplicate_user_email;
        }


        #endregion
    }
}
