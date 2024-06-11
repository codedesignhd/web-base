using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace CodeDesignDtos.Caches
{
    public class ResetPasswordCache
    {
        [JsonProperty("cod")]
        public string Code { get; set; }
        [JsonProperty("exp")]
        public long ExpireDate { get; set; }
        [JsonProperty("usr")]
        public string Username { get; set; }

    }
}
