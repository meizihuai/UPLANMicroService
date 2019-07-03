using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UplanModels;

namespace GetInfo_Global_RootService
{
    public class Module
    {
        public static readonly string APPName = "RootService";
        public static readonly bool IsLogger = false;
        public static  string Version = "1.0.1";
        public static string MysqlConnstr = "";
        public static ServiceProvider ServiceProvider;
        public static string AccountService_Url = "";
        public static string QoEDataUploadService_Url = "";

        public static void Init(IConfiguration Configuration, ServiceProvider serviceProvider)
        {
            Module.Version = Configuration.GetSection("AppVersion").Value;
            Console.Title = $"{APPName} {Version}";
            Module.ServiceProvider = serviceProvider;
            MysqlConnstr = Configuration.GetConnectionString("MysqlConnection");
            AccountService_Url = Configuration.GetSection("Getinfo-AccountService-Url").Value;
            QoEDataUploadService_Url = Configuration.GetSection("Getinfo-QoEDataUploadService-Url").Value;
            Start();
        }
        public static void Start()
        {
            Log("================程序启动================");
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
