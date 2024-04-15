using Google.Apis.Http;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.GoogleService
{
    public abstract class GServiceBase
    {
       protected BaseClientService.Initializer initializer;
        public abstract void CreateInitializer();

    }
}
