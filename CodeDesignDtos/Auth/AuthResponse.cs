using CodeDesignDtos.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignDtos.Auth
{
    public class AuthResponse : Response
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public AuthResponse()
        {

        }
        public AuthResponse(string message, bool success = false, string accessToken = null, string refreshToken = null)
        {
            this.message = message;
            this.success = success;
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }
    }
}
