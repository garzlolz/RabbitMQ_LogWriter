using MQConsumer.Class;
using MQConsumer.Service;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQConsumer
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                Console.WriteLine($"檢查到相同的程序,即將關閉...");
                Thread.Sleep(3000);
                Environment.Exit(0);
            }

            IntPtr handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);

            if ((ConfigurationManager.AppSettings.Get("ShowConsole") == "1"))
            {
                ShowWindow(handle, SW_SHOW);
            }
            else
            {
                ShowWindow(handle, SW_HIDE);
            }

            string queue = "info";
            try
            {
                var factory = new ConnectionFactory();
                var MQConnection = new MQConnectionInfo();
                factory.HostName = MQConnectionInfo.MQHostName;
                factory.UserName = MQConnectionInfo.MQUserName;
                factory.Password = MQConnectionInfo.MQPassword;
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.QueueDeclare(queue, true, false, false, null); // 定義處理那一個queue
                channel.BasicQos(0, 1, false);  // 每次處理1則

                var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
                // 定義收到queue的內容處理方式
                consumer.Received += (sender, e) =>
                {
                    byte[] body = e.Body.ToArray();
                    string message1 = Encoding.UTF8.GetString(body);
                    LogInfo log = JsonConvert.DeserializeObject<LogInfo>(message1);  // 將queue中的json轉回物件
                                                                                     // 以下可以更改為自己要處理的事項
                    Console.WriteLine(log.DateTime.ToString("yyyy/MM/dd HH:mm:ss:FFF") + " " + log.Message);  // 先顯示畫面上
                    LogService.WriteLog(log);
                    channel.BasicAck(e.DeliveryTag, false); // 處理完手動回應    

                };
                channel.BasicConsume(queue, false, consumer);  // 開始處理
                Console.ReadLine();
                connection.Close();
                channel.Close();
            }
            catch (System.Exception ex)
            {
                LogService.WriteLog(
                    new LogInfo
                    {
                        StoreId = "MesssageQueueConsumer",
                        ClientIp = "ConsumerError",
                        Process = "MainProcess",
                        Category = "info",
                        Message = $"{ex.Message}"
                    }
                    );
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
