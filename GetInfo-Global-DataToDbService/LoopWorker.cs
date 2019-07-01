using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetInfo_Global_DataToDbService
{
    public class LoopWorker
    {
        private static List<MTask> tasks;
        public static void Start()
        {
            Module.Log("LoopWorker Start!!!");
            tasks = new List<MTask>();
            MTask mt = new MTask();
            mt.SetAction(() =>
            {
                QoERToDbHelper qoERToDbHelper = new QoERToDbHelper();
                qoERToDbHelper.StartWork();
            });
            tasks.Add(mt);
            Module.Log("===========LoopWorker启动，开启循环工作线程===========");
            int i = 0;
            foreach (MTask t in tasks)
            {
                i++;
                try
                {
                    t.Start();
                    Module.Log($"   =>Task[{i}]已启动");
                }
                catch (Exception e)
                {
                    Module.Log($"   =><error>Task[{i}]启动失败！！！" + e.ToString());
                }
            }
        }
        public static void Stop()
        {
            Module.Log("===========LoopWorker关闭，关闭所有线程===========");
            foreach (MTask t in tasks)
            {
                try
                {
                    if (t != null) t.Cancel();
                }
                catch (Exception e)
                {

                }
            }
            tasks = null;
        }

    }
}
