using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UplanModels;

namespace GetInfo_Global_RootService
{
    public interface IQoEDeviceService
    {
        /// <summary>
        /// [APP]手机登录，获取AID
        /// </summary>
        /// <param name="phoneId">[*]手机唯一识别号，android用imei,iPhone用uuid</param>
        /// <param name="system">[*]操作系统，android / ios</param>
        /// <returns></returns>
        Task<NormalResponse> Login(string phoneId, string system = "android");
        /// <summary>
        /// [APP]手机上传QoER数据
        /// </summary>
        /// <param name="qoeRinfo">[*]QoER数据类</param>
        /// <returns></returns>
        Task<NormalResponse> UploadQoER(QoERinfo qoeRinfo);
    }
}
