using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UplanModels;

namespace GetInfo_Global_RootService
{
    public class AccountService : IAccountService
    {
        private readonly string url = Module.AccountService_Url;
        public Task<NormalResponse> GetNewToken(string token)
        {
            if (string.IsNullOrEmpty(url)) return Task.Run(() => { return new NormalResponse(false, "接口地址配置错误"); });
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("token", token);
            return HttpHelper.Get(url + "/api/account/GetNewToken", dik);
        }

        public Task<NormalResponse> GetTokenInfo(string token)
        {
            if (string.IsNullOrEmpty(url)) return Task.Run(() => { return new NormalResponse(false, "接口地址配置错误"); });
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("token", token);
            return HttpHelper.Get(url + "/api/account/GetTokenInfo", dik);
        }

        public Task<NormalResponse> Login(string usr, string pwd)
        {
            if (string.IsNullOrEmpty(url)) return Task.Run(() => { return new NormalResponse(false, "接口地址配置错误"); });
            Dictionary<string, object> dik = new Dictionary<string, object>();
            dik.Add("usr", usr);
            dik.Add("pwd", pwd);
            return HttpHelper.Get(url + "/api/account/Login", dik);
        }

        public Task<NormalResponse> Regist(UserInfo info)
        {
            if (string.IsNullOrEmpty(url)) return Task.Run(() => { return new NormalResponse(false, "接口地址配置错误"); });
            return HttpHelper.Post(url + "/api/account/Regist", info);
        }
    }
}
