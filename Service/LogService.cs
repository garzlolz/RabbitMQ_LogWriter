using MQConsumer.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQConsumer.Service
{
    public class LogService
    {
        async public static void WriteLog(LogInfo logInfo)
        {
            var path = ConfigurationManager.AppSettings.Get("LogPath");
            string StoreIdFolderName = string.IsNullOrEmpty(logInfo.StoreId) ? "其他" : logInfo.StoreId;
            string logPath = $@"{path}\{StoreIdFolderName}\{logInfo.DateTime:yyyy-MM-dd}\";
            string logFileName = logInfo.DateTime.Hour + "點" + ".txt";
            string nowTime = int.Parse(logInfo.DateTime.Hour.ToString()).ToString("00") + ":" + int.Parse(logInfo.DateTime.Minute.ToString()).ToString("00") + ":" + int.Parse(logInfo.DateTime.Second.ToString()).ToString("00");

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            if (!File.Exists(logPath + "\\" + logFileName))
            {
                File.Create(logPath + "\\" + logFileName).Close();
            }

            using (StreamWriter sw = File.AppendText(logPath + "\\" + logFileName))
            {
                string x = logInfo.Message.Substring(0, 2);
                if (logInfo.Message.Substring(0, 2) == "開始" || logInfo.Message.IndexOf("登入") > 0)
                {
                    sw.WriteLine("____________________________________________________________________________");
                }
                if (logInfo.IsDBLog)
                {
                    CallAPIService.RemoteDBLog(logInfo);
                }
                sw.WriteLine(nowTime + $" [{logInfo.Process}][{logInfo.MachineCode ?? "無機碼"}] ----> ");
                sw.WriteLine(logInfo.Message);
                if (x == "回傳" || x == "失敗" || x == "登入")
                    sw.WriteLine("____________________________________________________________________________");
            }
        }
    }

}
