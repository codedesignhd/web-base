using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class LogAction : ModelBase
    {
        public string source_id { get; set; }
        public string target_id { get; set; }
        public DOI_TUONG target_type { get; set; }
        public ACTION action { get; set; }
        public LogInfo info { get; set; }
    }

    public class LogInfo
    {
        public string ip { get; set; }
        public string url { get; set; }
        public string browser { get; set; }
    }
}
