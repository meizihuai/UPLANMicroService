﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetInfo_Global_QoEDataUploadService.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        [HttpGet]
        public NormalResponse GetInfo()
        {
            return new NormalResponse(true, $"{Module.APPName} : {Module.Version}", "", DeviceHelper.GetLocalIP());
        }
        [HttpGet]
        public NormalResponse Health()
        {
            return new NormalResponse(true, "", "", "");
        }
        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public NormalResponse GetTime()
        {
            return new NormalResponse(true, "", "", TimeUtil.Now().ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}