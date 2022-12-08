using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQConsumer.Class
{
    public class LogInfo
    {

        public string Source { get; set; }
        public string StoreName { get; set; }
        public string StoreId { get; set; }
        public string MachineCode { get; set; }
        public string Category { get; set; }
        public string ClientIp { get; set; }
        public string Process { get; set; }
        public string Message { get; set; }
        public bool IsDBLog { get; set; }
        public DateTime DateTime { get; set; }
    }
}
