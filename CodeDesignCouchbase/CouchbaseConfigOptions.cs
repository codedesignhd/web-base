using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Couchbase
{
    public class CouchbaseConfigOptions
    {
        public Uri Server { get; set; }
        public string BucketName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsInvalidOption()
        {
            return Server is null
                || string.IsNullOrWhiteSpace(BucketName)
                || string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Password);
        }
    }
}
