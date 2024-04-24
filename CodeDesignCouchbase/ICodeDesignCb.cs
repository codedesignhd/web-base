using System;
using System.Collections.Generic;
using System.Text;
using Couchbase;

namespace CodeDesign.Couchbase
{
    public interface ICodeDesignCb
    {
        bool Insert<T>(string key, T doc, TimeSpan expiration) where T : class;
        bool Update(string key, object doc);
        bool UpdateExpiration(string key, TimeSpan expiration);
        bool Delete(string key);
        Dictionary<string, bool> DeleteMany(IEnumerable<string> keys);
        bool Exist(string key);
        T Get<T>(string key) where T : class;
        Dictionary<string, T> GetMany<T>(IList<string> keys) where T : class;
    }
}
