using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetInfo_Global_RootService
{
   public interface IAccountService
    {
        /// <summary>
        /// 用户注册账号
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<NormalResponse> Regist(UserInfo info);
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="usr">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        Task<NormalResponse> Login(string usr, string pwd);
        /// <summary>
        /// 获取Token信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<NormalResponse> GetTokenInfo(string token);
        /// <summary>
        /// 获取新Token，校验是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<NormalResponse> GetNewToken(string token);


    }
}
