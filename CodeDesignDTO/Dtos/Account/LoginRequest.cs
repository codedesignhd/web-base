﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.DTO.Dtos.Account
{
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool is_remember { get; set; } = true;
        public string redirect_uri { get; set; } = "/";
    }
}
