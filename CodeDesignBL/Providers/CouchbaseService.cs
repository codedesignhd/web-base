using CodeDesign.Couchbase;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities = CodeDesignUtilities;
namespace CodeDesignBL.Providers
{
    public class CouchbaseService
    {
        static ICodeDesignCb _couchbase;
        //public CouchbaseInstance(ICodeDesignCb couchbase)
        //{
        //    _couchbase = couchbase;
        //}
        //public ICodeDesignCb GetInstance()
        //{
        //    return _couchbase;
        //}

        public static ICodeDesignCb Instance
        {
            get
            {
                if (_couchbase is null)
                {
                    CouchbaseConfigOptions options = new CouchbaseConfigOptions
                    {
                        Server = new Uri(Utilities.ConfigurationManager.AppSettings["Couchbase:Server"]),
                        BucketName = Utilities.ConfigurationManager.AppSettings["Couchbase:BucketName"],
                        Username = Utilities.CryptoUtils.Decode(Utilities.ConfigurationManager.AppSettings["Couchbase:Username"]),
                        Password = Utilities.CryptoUtils.Decode(Utilities.ConfigurationManager.AppSettings["Couchbase:Password"]),
                    };
                    _couchbase = new CodeDesignCb(options);
                }
                return _couchbase;
            }
        }

    }
}
