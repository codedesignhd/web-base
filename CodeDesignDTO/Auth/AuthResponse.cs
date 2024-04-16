using CodeDesign.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Auth
{
    public class AuthResponse : ResponseBase
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
