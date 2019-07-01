using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetInfo_Global_RootService
{
    public class QoEDeviceService : IQoEDeviceService
    {
        private readonly string accountUrl = Module.AccountService_Url;
        private readonly string dataUploadUrl = Module.QoEDataUploadService_Url;
        public Task<NormalResponse> Login(string phoneId, string system = "android")
        {
            if (string.IsNullOrEmpty(accountUrl)) return Task.Run(() => { return new NormalResponse(false, "接口地址配置错误"); });
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("phoneId", phoneId);
            dik.Add("system", system);
            return HttpHelper.Get(accountUrl + "/api/QoEDevice/Login", dik);
        }

        public Task<NormalResponse> UploadQoER(QoERinfo qoeRinfo)
        {
            if (string.IsNullOrEmpty(dataUploadUrl)) return Task.Run(() => { return new NormalResponse(false, "接口地址配置错误"); });
            return HttpHelper.Post(dataUploadUrl + "/api/DataUpload/UploadQoER", qoeRinfo);
        }
    }
}
