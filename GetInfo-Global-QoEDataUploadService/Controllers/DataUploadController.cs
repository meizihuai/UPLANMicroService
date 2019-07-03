using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Newtonsoft.Json;
using UplanModels;

namespace GetInfo_Global_QoEDataUploadService.Controllers
{
    /// <summary>
    /// 负责接收APP上传QoER,QoE,HTTP,黑点,投诉业务数据
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataUploadController : ControllerBase
    {
        /// <summary>
        /// [APP]上传QoER数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> UploadQoER(QoERinfo info)
        {
            return Task.Run(() =>
            {
                if (info == null) return new NormalResponse(false, "QoER格式非法");
                NormalResponse np = info.Check();
                if (!np.result) return np;
                info.MakeInfo();
                using(MQProducter p = MQProducter.Creat())
                {
                    if (p.Connect(Module.MQ_Data_QoER_QueueName))
                    {
                        string msg = JsonConvert.SerializeObject(info);
                        p.Send(msg);
                        return new NormalResponse(true, "success");
                    }
                    else
                    {
                        return new NormalResponse(false, "上传失败，后台消息队列故障");
                    }
                }            
            });
        }
    }
}