using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UplanModels
{
    [Table("QoEDeviceTable")]
    public class QoEDeviceInfo
    {
        /// <summary>
        /// [-]主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// [-]入库时间
        /// </summary>
        public string DateTime { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// AID
        /// </summary>
        public string AID { get; set; }
        /// <summary>
        /// 手机串号
        /// </summary>
        public string IMEI { get; set; }
        /// <summary>
        /// SIM卡号
        /// </summary>
        public string IMSI { get; set; }
        /// <summary>
        /// iOS设备ID
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// 群组ID
        /// </summary>
        public string GroupID { get; set; }
        /// <summary>
        /// 设备权限
        /// </summary>
        public int Power { get; set; }
        /// <summary>
        /// 设备所在省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 设备所在市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 设备所在区
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lon { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 百度坐标经度
        /// </summary>
        public double BDLon { get; set; }
        /// <summary>
        /// 百度坐标纬度
        /// </summary>
        public double BDLat { get; set; }
        /// <summary>
        /// 高德坐标经度
        /// </summary>
        public double GDLon { get; set; }
        /// <summary>
        /// 高德坐标纬度
        /// </summary>
        public double GDLat { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string PhoneModel { get; set; }
        /// <summary>
        /// APP版本号
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// SDK版本号
        /// </summary>
        public string SDKVersion { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public int IsOnline { get; set; }
        /// <summary>
        /// 运营商
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// 操作系统， android / ios
        /// </summary>
        public string System { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int RunScore { get; set; }
    }
   
}
