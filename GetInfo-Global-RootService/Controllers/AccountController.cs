using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GetInfo_Global_RootService.Controllers
{
    /// <summary>
    /// 账号认证服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase,IAccountService
    {
        private IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        /// <summary>
        /// 用户注册账号
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> Regist(UserInfo info)
        {
            return accountService.Regist(info);
        }
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="usr">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpGet]
        public Task<NormalResponse> Login(string usr, string pwd)
        {
            return accountService.Login(usr,pwd);
        }       
        /// <summary>
        /// 获取Token信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<NormalResponse> GetTokenInfo(string token)
        {
            return accountService.GetTokenInfo(token);
        }
        /// <summary>
        /// 获取新Token，校验是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<NormalResponse> GetNewToken(string token)
        {
            return accountService.GetNewToken(token);
        }
        
    }
}