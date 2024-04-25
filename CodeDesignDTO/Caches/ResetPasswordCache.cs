using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CodeDesign.Dtos.Caches
{
    public class ResetPasswordCache
    {
        [JsonPropertyName("cod")]
        public string Code { get; set; }
        [JsonPropertyName("exp")]
        public long ExpireDate { get; set; }
        [JsonPropertyName("usr")]
        public string Username { get; set; }

    }
}
