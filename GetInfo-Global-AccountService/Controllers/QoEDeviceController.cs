using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetInfo_Global_AccountService
{
    /// <summary>
    /// [APP接口]QoE设备注册、认证、AID服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QoEDeviceController : ControllerBase
    {
        /// <summary>
        /// 手机登录，获取AID
        /// </summary>
        /// <param name="phoneId">[*]手机唯一识别号，android用imei,iPhone用uuid</param>
        /// <param name="system">[*]操作系统，android / ios</param>
        /// <returns></returns>
        [HttpGet]
        public Task<NormalResponse> Login(string phoneId, string system = "android")
        {
            return Task.Run(()=>
            {
                if (phoneId == "") return new NormalResponse(false, "phoneId不可为空");
                system = system.ToLower();
                system = (system == "android" ? "android" : "ios");
                QoEDeviceInfo d = null;
                using (MyDbContext db = new MyDbContext())
                {
                    d = db.QoEDeviceTable.Where(a => (system == "android" ? a.IMEI : a.UUID) == phoneId).FirstOrDefault();
                    if (d != null)
                    {
                        return new NormalResponse(true, "", "", d);
                    }
                    d = new QoEDeviceInfo();
                    d.AID = AIDHelper.GetNewAid();
                    if (system == "android")
                    {
                        d.IMEI = phoneId;
                    }
                    else
                    {
                        d.UUID = phoneId;
                    }
                    d.DateTime = TimeUtil.Now().ToString("yyyy-MM-dd HH:mm:ss");
                    d.Power = 1;
                    db.QoEDeviceTable.Add(d);
                    int saveCount = db.SaveChanges();
                    if (saveCount > 0)
                    {
                        return new NormalResponse(true, "", "", d);
                    }
                    else
                    {
                        return new NormalResponse(false, "设备登陆失败，请联系系统管理员");
                    }
                }
            });                                
        }       
    }
}