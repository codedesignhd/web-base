using Couchbase.Authentication;
using Couchbase;
using Couchbase.Core;
using System;
using System.Collections.Generic;
using Couchbase.Configuration.Client;
using Couchbase.Core.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using Couchbase.IO;
using Couchbase.N1QL;
using Serilog;
namespace CodeDesign.Couchbase
{
    public class CodeDesignCb : ICodeDesignCb
    {
        private readonly ILogger _logger;
        private IBucket _bucket;
        private string _bucketName;
        public CodeDesignCb()
        {
            try
            {
                start();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
        }

        public CodeDesignCb(string customBucketName)
        {
            try
            {
                start(customBucketName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
        }

        private bool start(string customBucketName = null)
        {
            try
            {
                string server = Utilities.ConfigurationManager.AppSettings["Couchbase:Server"];
                string bucketUser = Utilities.ConfigurationManager.AppSettings["Couchbase:Username"];
                string bucketPass = Utilities.ConfigurationManager.AppSettings["Couchbase:Password"];
                if (!string.IsNullOrWhiteSpace(customBucketName))
                {
                    _bucketName = customBucketName;
                }
                else
                {
                    _bucketName = Utilities.ConfigurationManager.AppSettings["Couchbase:BucketName"];
                }


                if (string.IsNullOrWhiteSpace(server)
                    || string.IsNullOrWhiteSpace(_bucketName)
                    || string.IsNullOrWhiteSpace(bucketUser)
                    || string.IsNullOrWhiteSpace(bucketPass))
                {
                    throw new ArgumentNullException("Cần cấu hình đủ thông số cho couchbase trước khi khởi động");
                }
                ClientConfiguration config = new ClientConfiguration
                {
                    Servers = new List<Uri> { new Uri(server) },
                    Serializer = () => new DefaultSerializer(new JsonSerializerSettings(), new JsonSerializerSettings())
                };
                Cluster cluster = new Cluster(config);

                PasswordAuthenticator authenticator = new PasswordAuthenticator(bucketUser, bucketPass);
                cluster.Authenticate(authenticator);
                _bucket = cluster.OpenBucket(_bucketName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return false;
        }
        public bool Insert<T>(string key, T doc, TimeSpan expiration) where T : class
        {
            try
            {
                IOperationResult<T> o = _bucket.Insert<T>(key, doc, expiration);
                return o.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return false;
        }
        public bool Update(string key, object doc)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;
            try
            {
                IOperationResult<object> o = _bucket.Upsert<object>(key, doc);
                return o.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return false;
        }
        public bool UpdateExpiration(string key, TimeSpan expiration)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                try
                {
                    IOperationResult o = _bucket.Touch(key, expiration);
                    return o.Success;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.StackTrace);
                }
                return false;
            }
            return false;
        }
        public bool Delete(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;
            try
            {
                var res = _bucket.Remove(key);
                if (!res.Success)
                {
                    if (res.Status == ResponseStatus.KeyNotFound)
                        return true;
                    else
                        _logger.Error(Newtonsoft.Json.JsonConvert.SerializeObject(res));
                }
                return res.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return false;
        }
        public Dictionary<string, bool> DeleteMany(IEnumerable<string> keys)
        {
            if (keys != null)
            {
                try
                {
                    ConcurrentDictionary<string, bool> dic = new ConcurrentDictionary<string, bool>();
                    Parallel.ForEach(keys, async key =>
                    {
                        var res = await _bucket.RemoveAsync(key);
                        if (res.Success)
                        {
                            dic.TryAdd(key, res.Success);
                        }
                    });
                    return dic.ToDictionary(it => it.Key, it => it.Value);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.StackTrace);
                }
            }
            return new Dictionary<string, bool>();
        }
        public bool Exist(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;
            return _bucket.Exists(key);
        }
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                return default;
            try
            {
                IOperationResult<T> o = _bucket.Get<T>(key);
                return o.Value;
            }
            catch (Exception ex)
            {
                _logger.Error("KEY ER: " + key);
                _logger.Error(ex.StackTrace);
            }
            return default;
        }
        public Dictionary<string, T> GetMany<T>(IList<string> keys) where T : class
        {
            try
            {
                ConcurrentDictionary<string, T> dic = new ConcurrentDictionary<string, T>();
                Parallel.ForEach(keys, async key =>
                {
                    var res = await _bucket.GetAsync<T>(key);
                    if (res.Success && !dic.ContainsKey(key))
                        dic.TryAdd(key, res.Value);
                });

                return dic.ToDictionary(it => it.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return new Dictionary<string, T>();
        }
    }
}
