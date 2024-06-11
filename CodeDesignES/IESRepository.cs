﻿using CodeDesignModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignES
{
    public interface IESRepository<T> where T : ModelBase
    {
        (bool success, string id) Index(T data, string id = "", string route = "");
        bool Update(string id, object doc);

        bool Delete(string id, bool isForceDelete = false);

        T Get(string id, string[] fields = null);

        List<T> MultiGet(IEnumerable<string> ids, string[] fields = null);
    }
}
