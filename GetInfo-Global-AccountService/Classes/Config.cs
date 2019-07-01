using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GetInfo_Global_AccountService
{
    public class Config
    {
        public static string JWTsecret { get; set; }
        public static int JWTExp { get; set; }
        public static  object ToObject()
        {
            var tmp = new
            {
                JWTsecret,
                JWTExp
            };
            return tmp;
        }
    }
}
