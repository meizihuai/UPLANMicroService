using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace GetInfo_Global_DataToDbService
{
    public class QoERToDbHelper
    {
        MQCustomer mQCustomer;
        public QoERToDbHelper()
        {

        }
        public async void StartWork()
        {
            int sleepSecond = 3;
            while (true)
            {
                Module.Log("连接QoER队列中...");
                try
                {
                    if (mQCustomer != null)
                    {
                        mQCustomer.Dispose();
                    }
                    mQCustomer = MQCustomer.Creat();
                    if (mQCustomer.Connect(Module.MQ_Data_QoER_QueueName))
                    {
                        mQCustomer.OnMsg += MQCustomer_OnMsg;
                        Module.Log("QoER队列准备完成");
                        return;
                    }
                    else
                    {
                        Module.Log($"QoER队列连接失败，{sleepSecond}秒后重试");
                    }
                }
                catch (Exception e)
                {
                    Module.Log($"连接QoER队列失败，{e.ToString()}");
                }
                await Task.Delay(sleepSecond*1000);
            }          
        }

        private void MQCustomer_OnMsg(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg)) return;
                try
                {
                    QoERinfo qoERinfo = JsonConvert.DeserializeObject<QoERinfo>(msg);
                    if (qoERinfo != null)
                    {
                        InsertQoER(qoERinfo);
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {
                StartWork();
            }
        }

        private  void InsertQoER(QoERinfo info)
        {
            if (info == null) return;
            NormalResponse np = info.Check();
            if (!np.result) return;
            if (string.IsNullOrEmpty(info.DateTime)) info.DateTime = TimeUtil.Now().ToString("yyyy-MM-dd HH:mm:ss");
            Module.Log("[QoER]入库");
            using(var db=new MyDbContext())
            {
                try
                {                   
                    db.QoERTable.Add(info);
                    int count = db.SaveChanges();
                    if (count == 1)
                    {
                        Module.Log("  -->[QoER]入库成功");
                    }
                    else
                    {
                        Module.Log("  -->[QoER]入库失败");
                    }
                }
                catch (Exception e)
                {
                    Module.Log($"  -->[QoER]入库失败,{e.ToString()}");
                }              
            }
        }
    }
}
