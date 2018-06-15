using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TLBBThread.Helps
{
    public static class StaticConfig
    {
        public static readonly string JsonPath = ConfigurationManager.AppSettings["JsonPath"];
    }
}
