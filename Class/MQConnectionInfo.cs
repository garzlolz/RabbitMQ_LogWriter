using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQConsumer.Class
{
    public class MQConnectionInfo
    {
        public static string MQHostName { get => ConfigurationManager.AppSettings.Get("MQHostName") ?? ""; }
        public static string MQUserName { get => ConfigurationManager.AppSettings.Get("MQUserName") ?? ""; }
        public static string MQPassword { get => ConfigurationManager.AppSettings.Get("MQPassword") ?? ""; }

    }
}
