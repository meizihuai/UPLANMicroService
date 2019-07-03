using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UplanModels;

namespace GetInfo_Global_AccountService
{
    public interface ITokenHelper
    {
        /// <summary>
        /// 获取新Token
        /// </summary>
        /// <param name="usrInfo"></param>
        /// <returns></returns>
        string GetNewToken(UserInfo usrInfo);
        /// <summary>
        /// 定时更换token,检查过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        NormalResponse ChangeNewToken(string token);
        /// <summary>
        /// 检查token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool CheckToken(string token);
        /// <summary>
        /// 检查token所对应的用户的权限是否是管理员权限
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool CheckAdminToken(string token);
        /// <summary>
        /// 将token对应的JWT信息查询出来
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        JwtInfo DecodeToken(string token);
    }
    public class JwtInfo
    {
        public string UsrId { get; set; }
        public long Exp { get; set; }
        //token状态，0表示待定，1表示正常，2表示过期，3表示认证失败，4表示Token格式错误
        public int Status { get; set; }
        public int Power { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static JwtInfo Parse(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            try
            {
                return JsonConvert.DeserializeObject<JwtInfo>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    public class TokenHelper: ITokenHelper
    {
        private readonly string RedisTagUsr = ":usr:";
        private readonly string RedisTagToken = ":token:";
        public string GetNewToken(UserInfo usrInfo)
        {
            JwtInfo jwt = new JwtInfo
            {
                UsrId = usrInfo.Usr,
                Power = usrInfo.Power,
                Exp = TimeUtil.Now().AddMinutes(Config.JWTExp).Ticks,
                Status = 1
            };
            
            string token = Guid.NewGuid().ToString("N");

            //查询是否已存在该usr的token，若存在，清除一对
            if (CacheManager.CacheObj.Exists(RedisTagUsr + jwt.UsrId))
            {
                string oldToken = CacheManager.CacheObj.GetCache<string>(RedisTagUsr + jwt.UsrId);
                if (!string.IsNullOrEmpty(oldToken)) CacheManager.CacheObj.Delete(RedisTagToken + oldToken);
                CacheManager.CacheObj.Delete(RedisTagUsr + jwt.UsrId);
            }
            //建立token缓存
            CacheManager.CacheObj.Save(RedisTagToken + token, jwt, Config.JWTExp);
            //建立usr记录
            CacheManager.CacheObj.Save(RedisTagUsr + jwt.UsrId, token, Config.JWTExp);
            return token;
        }
       
        public NormalResponse ChangeNewToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return new NormalResponse(false, "token不可为空");
            JwtInfo info = DecodeToken(token);
            DateTime expTime = new DateTime(info.Exp);
            DateTime now = TimeUtil.Now();
            TimeSpan ts = expTime - now;
            string exp = $"{ts.Hours.ToString("00")}:{ts.Minutes.ToString("00")}:{ts.Seconds.ToString("00")}";
            if (info.Status == 1) return new NormalResponse(true, $"未到期,剩余 {exp}", "", token);
            if (info.Status == 2) return new NormalResponse(true, "已更换", "", GetNewToken(new UserInfo { Usr = info.UsrId, Power = info.Power }));
            return new NormalResponse(false, "您的token无效");
        }

        public bool CheckToken(string token)
        {
            JwtInfo info = DecodeToken(token);
            if (info == null) return false;
            return info.Status == 1;
        }
        public bool CheckAdminToken(string token)
        {
            JwtInfo info = DecodeToken(token);
            if (info == null) return false;
            return (info.Status == 1 && info.Power >= 9);
        }

        public JwtInfo DecodeToken(string token)
        {
            JwtInfo jwt = CacheManager.CacheObj.GetCache<JwtInfo>(RedisTagToken + token);
            //此处应该设置为，token最后5分钟的时候，进行更换处理，当 jwt.Exp <= TimeUtil.Now().AddMinutes(5).Ticks 就提示要过期了，并给返回新token
            if (jwt != null)
            {
                if (jwt.Exp <= TimeUtil.Now().AddMinutes(5).Ticks) jwt.Status = 2;
            }
            if (jwt == null) { jwt = new JwtInfo {UsrId="", Status = 4 }; }
            return jwt;
        }     
    }


    /// <summary>
    ///[已弃用] JWT  SHA256加密形式，但是token太长，且形成jwt和redis双处认证，耦合度高
    /// </summary>
    public class JwtHelper: ITokenHelper
    {   
        public  string GetNewToken(UserInfo usrInfo)
        {         
             var payload = new JwtInfo
            {
                 UsrId= usrInfo.Usr,
                 Power= usrInfo.Power,
                 Exp =TimeUtil.Now().AddMinutes(Config.JWTExp).Ticks,
                 Status=1
             };
            var secret =Config.JWTsecret;//服务器端秘钥，需保密
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            string token = encoder.Encode(payload, secret);
            return token;
        }
        public  NormalResponse ChangeNewToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return new NormalResponse(false, "token不可为空");
            JwtInfo info = DecodeToken(token);
            if (info.Status == 1) return new NormalResponse(true, "未到期", "", token);
            if (info.Status == 2) return new NormalResponse(true, "已更换", "", GetNewToken(new UserInfo { Usr = info.UsrId, Power = info.Power }));
            return new NormalResponse(false, "您的token无效");
        }
        public  bool CheckToken(string token)
        {
            JwtInfo info = DecodeToken(token);
            if (info == null) return false;
            return info.Status == 1;
        }
        public  bool CheckAdminToken(string token)
        {
            JwtInfo info = DecodeToken(token);
            if (info == null) return false;
            return (info.Status == 1 && info.Power >= 9);
        }
        public  JwtInfo DecodeToken(string token)
        {        
            var secret = Config.JWTsecret;
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
               // var json = decoder.Decode(token, secret, verify: true);
                JwtInfo info = decoder.DecodeToObject<JwtInfo>(token, secret, verify: true);
                info.Status = 1;
                if (info.Exp <=TimeUtil.Now().Ticks) info.Status = 2;
                return info;
            }
            catch (FormatException)
            {
                //Token格式错误，无法识别
                return new JwtInfo { Status = 4 };
            }
            catch (TokenExpiredException)
            {
                //Token过期
                return new JwtInfo { Status =2 };
            }
            catch (SignatureVerificationException)
            {
                //认证无效
                return new JwtInfo { Status = 3 };
            }
            catch(Exception e)
            {
                return new JwtInfo { Status = 4};
            }
        }
    }
}
