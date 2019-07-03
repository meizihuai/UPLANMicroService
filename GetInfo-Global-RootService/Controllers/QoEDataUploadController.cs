using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UplanModels;

namespace GetInfo_Global_RootService.Controllers
{
    /// <summary>
    /// [APP接口]数据上传
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QoEDataUploadController : ControllerBase
    {
        private IQoEDeviceService qoeDeviceService;
        public QoEDataUploadController(IQoEDeviceService qoeDeviceService)
        {
            this.qoeDeviceService = qoeDeviceService;
        }
        /// <summary>
        /// [APP]上传QoER数据
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> UploadQoER(QoERinfo info)
        {
            return qoeDeviceService.UploadQoER(info);
        }
    }
}