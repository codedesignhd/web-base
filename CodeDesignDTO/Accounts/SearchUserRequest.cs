using CodeDesign.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Accounts
{
    public class SearchUserRequest : PagingRequest
    {
        public string q { get; set; }

    }
}
