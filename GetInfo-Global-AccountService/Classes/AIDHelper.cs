using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GetInfo_Global_AccountService
{
    public class AIDHelper
    {
        public static string GetNewAid()
        {
            while (true)
            {
                string aid = System.Guid.NewGuid().ToString("N").Substring(0, 6);
                if (Regex.IsMatch(aid, "[A-Za-z].*[0-9]|[0-9].*[A-Za-z]"))
                {
                    using(MyDbContext db=new MyDbContext())
                    {
                        int count = db.QoEDeviceTable.Where(a => a.AID == aid).Count();
                        if (count == 0)
                        {
                            return aid;
                        }
                    }                     
                }
            }
        }
    }
}
