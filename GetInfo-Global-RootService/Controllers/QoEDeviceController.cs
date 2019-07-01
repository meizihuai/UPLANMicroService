using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetInfo_Global_RootService
{
    /// <summary>
    /// [APP接口]QoE设备注册、认证、AID服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QoEDeviceController : ControllerBase
    {
        private IQoEDeviceService qoeDeviceService;
        public QoEDeviceController(IQoEDeviceService qoeDeviceService)
        {
            this.qoeDeviceService = qoeDeviceService;
        }
        /// <summary>
        /// 手机登录，获取AID
        /// </summary>
        /// <param name="phoneId">[*]手机唯一识别号，android用imei,iPhone用uuid</param>
        /// <param name="system">[*]操作系统，android / ios</param>
        /// <returns></returns>
        [HttpGet]
        public Task<NormalResponse> Login(string phoneId, string system = "android")
        {
            return qoeDeviceService.Login(phoneId, system);          
        }

    }
}