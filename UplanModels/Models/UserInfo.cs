using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UplanModels
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string DateTime { get; set; }
        public string Usr { get; set; }
        public string Pwd { get; set; }
        public string PwdSalt { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public int Power { get; set; }
        public string Token { get; set; }
    }
}
