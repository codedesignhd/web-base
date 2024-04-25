using CodeDesign.ES.Constants;
using CodeDesign.ES.Models;
using CodeDesign.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CodeDesign.ES
{
    public class PackageRepository : ESRepositoryBase, IESRepository<Package>
    {
        #region Init
        public PackageRepository(string modify_index)
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


        private static PackageRepository _instance;
        public static PackageRepository Instance
        {
            get
            {
                if (_instance is null)
                {
                    _index = string.Format("{0}_pkgs", prefix_index);
                    _instance = new PackageRepository(_index);
                }
                return _instance;
            }
        }

        #endregion

        #region Core func
        public bool Delete(string id, bool isForceDelete = false)
        {
            return base.Delete<Package>(id, isForceDelete);
        }

        public Package Get(string id, string[] fields = null)
        {
            return base.Get<Package>(id, fields);
        }

        public (bool success, string id) Index(Package data, string id = "", string route = "")
        {
            return base.Index<Package>(data, id, route);
        }

        public List<Package> MultiGet(IEnumerable<string> ids, string[] fields = null)
        {
            return base.MultiGet<Package>(ids, fields);
        }

        public bool Update(string id, object doc)
        {
            return base.Update<Package>(id, doc);
        }
        #endregion

        #region Features Func
        public SearchResult<Package> GetPublicPackage(Dictionary<string, SortDir> sort = null, string[] fields = null)
        {
            SearchResult<Package> result = new SearchResult<Package>();
            List<QueryContainer> filter = new List<QueryContainer>
            {
                new TermQuery{Field="thuoc_tinh",Value=ThuocTinhHeThong.GoiCuocHienThi}
            };
            SearchRequest request = new SearchRequest(_index)
            {
                Query = new BoolQuery { Filter = filter, Must = CustomMustNot() },
                Size = ESConsts.MaxResultWindow,
                Sort = CustomSort(sort),
                Source = CustomSource(fields),
            };
            var res = client.Search<Package>(request);
            if (res.IsValid)
            {
                result.total = res.Total;
                result.documents = res.Hits
                    .Select(ToDocument)
                    .ToList();
            }

            return result;
        }
        #endregion
    }
}
