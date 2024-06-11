using CodeDesignDtos.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignDtos.Accounts
{
    public class SearchUserRequest : PagingRequest
    {
        public string q { get; set; }

    }
}
