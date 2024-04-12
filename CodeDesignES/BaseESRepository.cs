using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using CodeDesign.ES.Models;
using CodeDesign.Models;
using CodeDesign.Utilities;
using Elasticsearch.Net;
using Nest;

namespace CodeDesign.ES
{
    public abstract class BaseESRepository
    {
        protected static Uri node = new Uri(ConfigurationManager.AppSettings["ES:Server"]);
        protected static string prefix_index = ConfigurationManager.AppSettings["ES:Prefix"];
        protected static StickyConnectionPool connectionPool = new StickyConnectionPool(new[] { node });
        protected ElasticClient client;
        protected static string _index;
        protected const int MaxResultWindow = 10000;

        #region Core Function
        protected T ToDocument<T>(IMultiGetHit<T> hit) where T : ModelBase
        {
            if (hit.Found)
            {
                T obj = hit.Source;
                PropertyInfo prop = typeof(T).GetProperty("id");
                if (prop != null)
                {
                    prop.SetValue(obj, hit.Id);
                }
                return obj;
            }
            return default;
        }
        protected T ToDocument<T>(IHit<T> hit) where T : ModelBase
        {
            T obj = hit.Source;
            PropertyInfo prop = typeof(T).GetProperty("id");
            if (prop != null)
            {
                prop.SetValue(obj, hit.Id);
            }
            return obj;
        }

        protected IEnumerable<T> ToDocument<T>(ConcurrentBag<IHit<T>> hits) where T : ModelBase
        {
            foreach (var hit in hits)
            {
                yield return ToDocument(hit);
            }
        }
        protected (bool success, string id) Index<T>(T data, string id = "", string route = "") where T : ModelBase
        {
            IndexResponse re = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                re = client.Index(data, i => i.Id(id));
            }
            else
            {
                IndexRequest<T> req = new IndexRequest<T>()
                {
                    Document = data,
                };
                if (!string.IsNullOrEmpty(route))
                    req.Routing = route;
                re = client.Index(req);
            }
            return (re.Result == Result.Created, re.Id);
        }
        protected bool BulkIndex<T>(IEnumerable<T> docs) where T : ModelBase
        {
            BulkResponse res = client.Bulk(bi => bi.IndexMany(docs));
            return res.IsValid && !res.Errors;
        }
        protected bool Update<T>(string id, object doc) where T : ModelBase
        {
            if (!string.IsNullOrWhiteSpace(id) && doc != null)
            {
                UpdateResponse<T> res = client.Update<T, object>(id, u => u.Doc(doc));
                return res.IsValid && (res.Result == Result.Updated || res.Result == Result.Noop);
            }
            return false;
        }
        protected bool BulkUpdate<T>(IEnumerable<object> docs, out List<string> ids_with_error) where T : ModelBase
        {
            ids_with_error = new List<string>();
            BulkResponse res = client.Bulk(bu => bu.UpdateMany(docs, (b, doc) => b.Doc(doc)));
            if (res.ItemsWithErrors.Count() > 0)
            {
                ids_with_error = res.ItemsWithErrors.Select(x => x.Id).ToList();
            }
            return res.IsValid && !res.Errors;
        }
        protected bool Delete<T>(string id, bool isForceDelete = false) where T : ModelBase
        {
            if (!isForceDelete)
            {
                PropertyInfo prop = typeof(T).GetProperty("trang_thai_du_lieu");
                if (prop != null)
                {
                    return Update<T>(id, new
                    {
                        trang_thai_du_lieu = TrangThaiDuLieu.Deleted
                    });
                }
            }
            DeleteResponse res = client.Delete<T>(id);
            return res.IsValid && res.Result == Result.Deleted;
        }
        protected bool BulkDelete<T>(IEnumerable<string> ids, out List<string> ids_with_error, bool isForceDelete = false) where T : ModelBase
        {
            ids_with_error = new List<string>();
            if (ids != null && ids.Count() > 0)
            {
                if (!isForceDelete)
                {
                    PropertyInfo prop = typeof(T).GetProperty("trang_thai_du_lieu");
                    if (prop != null)
                    {
                        IEnumerable<object> docs = ids.Select(id => new
                        {
                            id,
                            trang_thai_du_lieu = TrangThaiDuLieu.Deleted
                        });
                        return BulkUpdate<T>(docs, out ids_with_error);
                    }
                }
                BulkResponse res = client.Bulk(bu => bu.DeleteMany<T>(ids));
                if (res.ItemsWithErrors.Count() > 0)
                {
                    ids_with_error = res.ItemsWithErrors.Select(x => x.Id).ToList();
                }
                return res.IsValid && !res.Errors;
            }
            return false;
        }
        protected T Get<T>(string id, string[] fields = null) where T : class
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                GetResponse<T> res = client.Get<T>(id, g => g.SourceIncludes(fields));
                if (res.Found)
                {
                    T obj = res.Source;
                    PropertyInfo prop = typeof(T).GetProperty("id");
                    if (prop != null)
                    {
                        prop.SetValue(obj, res.Id);
                    }
                    return obj;
                }
                return default;
            }
            return default;
        }
        protected List<T> MultiGet<T>(IEnumerable<string> ids, string[] fields = null) where T : ModelBase
        {
            MultiGetResponse res = client.MultiGet(mget => mget.GetMany<T>(ids).SourceIncludes(fields));
            if (res.IsValid)
            {
                return res.GetMany<T>(ids).Select(ToDocument).ToList();
            }
            return new List<T>();
        }
        public IEnumerable<T> GetObjectScroll<T>(QueryContainer query, SourceFilter so, string scroll_timeout = "5m", int page_size = 2000) where T : ModelBase
        {
            if (query == null)
                query = new MatchAllQuery();
            if (so == null)
                so = new SourceFilter() { };
            ConcurrentBag<IHit<T>> bag = new ConcurrentBag<IHit<T>>();
            try
            {

                SearchRequest req = new SearchRequest()
                {
                    From = 0,
                    Size = page_size,
                    Query = query,
                    Source = so,
                    Scroll = scroll_timeout,
                };

                ISearchResponse<T> searchResponse = client.Search<T>(req);
                while (true)
                {
                    if (!searchResponse.IsValid || string.IsNullOrEmpty(searchResponse.ScrollId))
                        break;

                    if (!searchResponse.Documents.Any())
                        break;

                    var tmp = searchResponse.Hits;
                    foreach (var item in tmp)
                    {
                        bag.Add(item);
                    }
                    searchResponse = client.Scroll<T>(scroll_timeout, searchResponse.ScrollId);
                }

                client.ClearScroll(new ClearScrollRequest(searchResponse.ScrollId));
            }
            catch (Exception)
            {
            }
            finally
            {
            }
            return ToDocument(bag);
        }

        public ScrollResult<T> GetScroll<T>(string scrollId, SearchRequest request) where T : ModelBase
        {
            ScrollResult<T> result = new ScrollResult<T>()
            {
                Documents = new List<T>(),
            };
            ISearchResponse<T> response;
            if (string.IsNullOrWhiteSpace(scrollId))
            {
                response = client.Search<T>(request);
            }
            else
            {
                response = client.Scroll<T>("5m", scrollId);
            }
            result.Documents = response.Hits
                .Select(ToDocument)
                .ToList();
            result.Total = response.Total;
            if (response.Total > request.Size)
            {
                result.ScrollId = response.ScrollId;
            }
            return result;
        }

        #endregion

        #region Custom Request
        protected List<QueryContainer> CustomMustNot(List<QueryContainer> must_not = null)
        {
            if (must_not == null)
            {
                must_not = new List<QueryContainer>();
            }
            must_not.Add(new TermQuery { Field = "trang_thai_du_lieu", Value = TrangThaiDuLieu.Deleted });
            return must_not;
        }

        protected SourceFilter CustomSource(string[] fields = null, string[] ignore_fields = null)
        {
            SourceFilter so = new SourceFilter();
            if (fields != null && fields.Length > 0)
            {
                so.Includes = fields;
            }
            if (ignore_fields != null && ignore_fields.Length > 0)
            {
                so.Excludes = ignore_fields;
            }
            return so;
        }

        protected SortOrder FromSort(SortDir sort_direction)
        {
            return sort_direction == SortDir.Desc ? SortOrder.Descending : SortOrder.Ascending;
        }

        protected List<ISort> CustomSort(Dictionary<string, SortDir> dic_sort = null)
        {
            List<ISort> sort = new List<ISort>();
            if (dic_sort != null && dic_sort.Count > 0)
            {
                foreach (var item in dic_sort)
                {
                    sort.Add(new FieldSort { Field = item.Key, Order = FromSort(item.Value) });
                }
            }
            return sort;
        }
        #endregion

    }

    public static class ESExt
    {
        public static ISearchRequest AddPaging(this ISearchRequest req, int page, int page_size)
        {
            req.From = (page - 1) * page_size;
            req.Size = page_size;
            return req;
        }
    }
}
