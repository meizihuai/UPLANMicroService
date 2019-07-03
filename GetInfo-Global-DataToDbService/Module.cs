using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UplanModels;

namespace GetInfo_Global_DataToDbService
{
    public class Module
    {
        public static readonly string APPName = "QoEDataUploadService";
        public static readonly bool IsLogger = false;
        public static string Version = "1.0.0";
        public static string MysqlConnstr = "";
        public static ServiceProvider ServiceProvider;
        public static string MQHostName, MQUserName, MQPassword;
        public static string MQ_Data_QoER_QueueName, MQ_Data_QoE_QueueName, MQ_Data_HTTP_QueueName;

        public static void Init(IConfiguration Configuration, ServiceProvider serviceProvider)
        {
            Module.Version = Configuration.GetSection("AppVersion").Value;
            Console.Title = $"{APPName} {Version}";
            Module.ServiceProvider = serviceProvider;
            MysqlConnstr = Configuration.GetConnectionString("MysqlConnection");
            MQHostName = Configuration.GetSection("MQ-HostName").Value;
            MQUserName = Configuration.GetSection("MQ-UserName").Value;
            MQPassword = Configuration.GetSection("MQ-Password").Value;

            MQ_Data_QoER_QueueName = Configuration.GetSection("MQ-Data-QoER-QueueName").Value;
            MQ_Data_QoE_QueueName = Configuration.GetSection("MQ-Data-QoE-QueueName").Value;
            MQ_Data_HTTP_QueueName = Configuration.GetSection("MQ-Data-HTTP-QueueName").Value;
            Start();
        }
        public static void Start()
        {
            Log("================程序启动================");
            LoopWorker.Start();
        }
        public static void Stop()
        {
            Log("================程序关闭================");
        }
        public static void Log(string str)
        {
            Console.WriteLine(TimeUtil.Now().ToString("[HH:mm:ss] ") + str);
            if (IsLogger)
            {
                Logger.Info(str);
            }
        }
    }
}
