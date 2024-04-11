using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CodeDesign.Dtos
{
    public class RegisterUserRequest
    {
        private string _username;
        public string username
        {
            get
            {
                if (!string.IsNullOrEmpty(_username))
                {
                    return _username.Trim().ToLower();
                }
                return _username;
            }

            set
            {
                _username = value;
            }
        }
        private string _fullname;
        public string fullname
        {
            get
            {
                if (!string.IsNullOrEmpty(_fullname))
                {
                    return _fullname.Trim();
                }
                return _fullname;
            }
            set
            {
                _fullname = value;
            }
        }

        private string _email;
        public string email
        {
            get
            {
                if (!string.IsNullOrEmpty(_email))
                {
                    return _email.Trim().ToLower();
                }
                return _email;
            }

            set
            {
                _email = value;
            }
        }
        private string _password;
        public string password
        {
            get
            {
                if (!string.IsNullOrEmpty(_password))
                {
                    return _password.Trim();
                }
                return _password;
            }

            set
            {
                _password = value;
            }
        }
    }
}
