using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UplanModels;

namespace GetInfo_Global_AccountService.Controllers
{
    /// <summary>
    /// 账号认证服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ITokenHelper Tk;
        public AccountController()
        {
            this.Tk = new TokenHelper();
        }
        /// <summary>
        /// 用户注册账号
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<NormalResponse> Regist(UserInfo info)
        {
            return Task.Run(() =>
            {
                if (info == null) return new NormalResponse(false, "您输入的用户信息格式有误");
                string usrId = info.Usr;
                if (string.IsNullOrEmpty(usrId)) return new NormalResponse(false, "用户名和不可为空");
                if(string.IsNullOrEmpty(info.Pwd)) return new NormalResponse(false, "密码不可为空");
                if(info.Pwd.Length<6) return new NormalResponse(false, "密码至少为6位");
                if (info.Pwd.Length >20) return new NormalResponse(false, "密码不可超过20位");
                using (MyDbContext db = new MyDbContext())
                {
                    int count = db.UserAccountTable.Where(a => a.Usr == usrId).Count();
                    if (count > 0) return new NormalResponse(false, "该用户名已存在");
                    info.DateTime = TimeUtil.Now().ToString("yyyy-MM-dd HH:mm:ss");
                    info.PwdSalt = Guid.NewGuid().ToString("N").Substring(0, 6);
                    string pwd = info.Pwd;
                    pwd = ShaHashHelper.GetSha1Hash(pwd + info.PwdSalt);
                    info.Pwd = pwd;
                    info.Status = 1;
                    info.Power = 1;
                    info.Type = "global";
                    db.UserAccountTable.Add(info);
                    int saveCount = db.SaveChanges();
                    if (saveCount > 0)
                    {
                        return new NormalResponse(true, "注册成功");
                    }
                    else
                    {
                        return new NormalResponse(false, "注册失败");
                    }
                }                                  
            });
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
            return Task.Run(() =>
            {
                using (MyDbContext db = new MyDbContext())
                {
                    UserInfo usrInfo = db.UserAccountTable.Where(a => a.Usr == usr).FirstOrDefault();
                    if (usrInfo == null) return new NormalResponse(false, "用户不存在");
                    if (!ShaHashHelper.VerifySha1Hash(pwd + usrInfo.PwdSalt, usrInfo.Pwd))
                    {
                        return new NormalResponse(false, $"用户密码错误");
                        //return new NormalResponse(false, $"用户密码错误,pwd={pwd}", ShaHashHelper.GetSha1Hash(pwd + usrInfo.PwdSalt), usrInfo);
                    }
                   
                    string token = Tk.GetNewToken(usrInfo);
                    usrInfo.Token = token;
                    usrInfo.Pwd = "";
                    usrInfo.PwdSalt = "";
                    var loginUsrInfo = new
                    {
                        usrInfo.ID,
                        usrInfo.DateTime,
                        usrInfo.Usr,
                        usrInfo.Type,
                        usrInfo.Email,
                        usrInfo.Status,
                        usrInfo.Power,
                        usrInfo.Token
                    };
                    return new NormalResponse(true, "登陆成功", "", loginUsrInfo);
                }
            });
        }
        //[HttpGet]
        //public Task<NormalResponse> GetUsr()
        //{
        //    return Task.Run(()=>
        //    {
        //        try
        //        {
        //            using (MyDbContext db = new MyDbContext())
        //            {
        //                return new NormalResponse(true, "", "", db.UserAccountTable.ToArray());
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return new NormalResponse(false, e.ToString());
        //        }
        //    });
        //}
        /// <summary>
        /// 获取Token信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<NormalResponse>GetTokenInfo(string token)
        {
            return Task.Run(() =>
            {
                JwtInfo info = Tk.DecodeToken(token);
                if (info.Status > 2)
                {
                    return new NormalResponse(false, "您的token无效");
                }
                return new NormalResponse(true, "", "", info);
            });
        }
        /// <summary>
        /// 获取新Token，校验是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<NormalResponse>GetNewToken(string token)
        {
            return Task.Run(() =>
            {
                return Tk.ChangeNewToken(token);
            });
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
