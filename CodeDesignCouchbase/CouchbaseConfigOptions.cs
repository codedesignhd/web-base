using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Couchbase
{
    public class CouchbaseConfigOptions
    {
        public string Server { get; set; }
        public string Bucketname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsValidOption()
        {
            return !string.IsNullOrWhiteSpace(Server)
                && !string.IsNullOrWhiteSpace(Bucketname)
                && !string.IsNullOrWhiteSpace(Username)
                && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
