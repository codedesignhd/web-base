using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Utils
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSettings { get; }

        static ConfigurationManager()
        {
            AppSettings = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
