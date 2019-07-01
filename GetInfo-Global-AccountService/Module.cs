using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GetInfo_Global_AccountService
{
    public class Module
    {
        public static readonly string APPName = "AccountService";
        public static readonly bool  IsLogger = false;
        public static string Version;
        public static string MysqlConnstr = "";
        public static ServiceProvider ServiceProvider;
        

        public static void Init(IConfiguration Configuration, ServiceProvider serviceProvider)
        {
            Module.Version = Configuration.GetSection("AppVersion").Value;
            Console.Title = $"{APPName} {Version}";
            Module.ServiceProvider = serviceProvider;
            MysqlConnstr = Configuration.GetConnectionString("MysqlConnection");
            Config.JWTsecret = Configuration.GetSection("JWT-Secret").Value;
            Config.JWTExp = int.Parse(Configuration.GetSection("JWT-Exp").Value);
            RedisConfig.Configname = Configuration.GetSection("Redis-Configname").Value;
            RedisConfig.Connection = Configuration.GetSection("Redis-Connection").Value;
            RedisConfig.DefaultDatabase = int.Parse(Configuration.GetSection("Redis-DefaultDatabase").Value);
            RedisConfig.InstanceName = Configuration.GetSection("Redis-InstanceName").Value;
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
