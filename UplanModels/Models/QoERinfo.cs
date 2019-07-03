using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UplanModels
{
    public class QoERinfo
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
        /// [-]入库日期，由后台计算
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// [*]AID
        /// </summary>
        public string AID { get; set; }
        /// <summary>
        /// [-]跨表关联ID
        /// </summary>
        public string RID { get; set; }
        /// <summary>
        /// [*]业务类型
        /// </summary>
        public string BusinessType { get; set; }
        /// <summary>
        /// [*]APP名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// [*]系统版本,Android版本 / iOS版本
        /// </summary>
        public string OSVersion { get; set; }
        /// <summary>
        /// [*]运营商
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// [*]信号强度-RSRP
        /// </summary>
        public int? RSRP { get; set; }
        /// <summary>
        /// [*]信号质量-SINR
        /// </summary>
        public int? SINR { get; set; }
        /// <summary>
        /// [*]信号质量-RSRQ
        /// </summary>
        public int? RSRQ { get; set; }
        /// <summary>
        /// [*]基站信息-TAC
        /// </summary>
        public int? TAC { get; set; }
        /// <summary>
        /// [*]基站信息-PCI
        /// </summary>
        public int? PCI { get; set; }
        /// <summary>
        /// [*]网络参数，MNC
        /// </summary>
        public int? MNC { get; set; }
        /// <summary>
        /// [*]频点
        /// </summary>
        public int? EarFcn { get; set; }
        /// <summary>
        /// [*]小区-ECI
        /// </summary>
        public int? ECI { get; set; }
        /// <summary>
        /// [-]基站ID，由ECI计算得出
        /// </summary>
        public int? ENodeBId { get; set; }
        /// <summary>
        /// [-]小区ID，由ECI计算得出
        /// </summary>
        public int? CellId { get; set; }
        /// <summary>
        /// 基站小区ID
        /// </summary>
        public string EnodebId_CellId { get; set; }
        /// <summary>
        /// [*]网络类型 2G/3G/4G/WiFi
        /// </summary>
        public string NetType { get; set; }
        /// <summary>
        /// [*]信号类型
        /// </summary>
        public string SignalType { get; set; }
        /// <summary>
        /// [*]经度
        /// </summary>
        public double? Lon { get; set; }
        /// <summary>
        /// [*]纬度
        /// </summary>
        public double? Lat { get; set; }
        /// <summary>
        /// [-]百度经度
        /// </summary>
        public double? BDLon { get; set; }
        /// <summary>
        /// [-]百度纬度
        /// </summary>
        public double? BDLat { get; set; }
        /// <summary>
        /// [-]高德经度
        /// </summary>
        public double? GDLon { get; set; }
        /// <summary>
        /// [-]高德纬度
        /// </summary>
        public double? GDLat { get; set; }
        /// <summary>
        /// [-]省，后台根据经纬度反解析出来
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// [-]市，后台根据经纬度反解析出来
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// [-]区，后台根据经纬度反解析出来
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// [-]详细地址，后台由经纬度反解析出来
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// [*]精度
        /// </summary>
        public double? Accuracy { get; set; }
        /// <summary>
        /// [*]海拔
        /// </summary>
        public double? Altitude { get; set; }
        /// <summary>
        /// [*]GPS传感器运动速度
        /// </summary>
        public double? GpsSpeed { get; set; }
        /// <summary>
        /// [*]搜索到的卫星数量
        /// </summary>
        public int? SatelliteCount { get; set; }
        /// <summary>
        /// [*]APP版本号
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// [*]是否是及时上报，1是  0否
        /// </summary>
        public int? IsUploadDataTimely { get; set; }
        /// <summary>
        /// WiFi热点名称，若没连接WiFi可为空
        /// </summary>
        public string WiFi_SSID { get; set; }
        /// <summary>
        /// WiFi热点路由器设备mac地址，若没连接WiFi可为空
        /// </summary>
        public string WiFi_MAC { get; set; }
        /// <summary>
        /// [*]平均Ping值
        /// </summary>
        public int? Ping_Avg_Rtt { get; set; }
        /// <summary>
        /// [*]CPU型号
        /// </summary>
        public string CPU { get; set; }
        /// <summary>
        /// 邻区信息
        /// </summary>
        [NotMapped]
        public List<Neighbour> NeighbourList { get; set; }
        /// <summary>
        /// [-]邻区列表[0]-PCI，后台从NeighbourList中取
        /// </summary>
        public int? Adj_PCI1 { get; set; }
        /// <summary>
        /// 邻区列表[0]-RSRP，后台从NeighbourList中取
        /// </summary>
        public int? Adj_RSRP1 { get; set; }
        /// <summary>
        /// 邻区列表[0]-EARFCN，后台从NeighbourList中取
        /// </summary>
        public int? ADJ_EARFCN1 { get; set; }
        /// <summary>
        /// [*]采集QoER数据时，屏幕是否亮屏
        /// </summary>
        public int? IsScreenOn { get; set; }
        /// <summary>
        /// [*]采集QoER数据时，GPS开关是否打开
        /// </summary>
        public int? IsGPSOpen { get; set; }
        /// <summary>
        /// [-]采集QoER数据时，是否在室外，暂由后台根据搜星数量判断
        /// </summary>
        public int? IsOutSide { get; set; }
        /// <summary>
        /// [*]采集QoER数据时，手机电量
        /// </summary>
        public int? Phone_Electric { get; set; }
        /// <summary>
        /// [*]采集QoER数据时，屏幕亮度
        /// </summary>
        public int? Phone_Screen_Brightness { get; set; }
        /// <summary>
        /// [*]xyz三轴加速度
        /// </summary>
        [NotMapped]
        public XYZaSpeedInfo XYZaSpeed { get; set; }
        /// <summary>
        /// [-]HTTP_Response_Time分值，后台计算
        /// </summary>
        public int? VMOS { get; set; }
        /// <summary>
        /// [*]HTTP请求-URL
        /// </summary>
        public string HTTP_URL { get; set; }
        /// <summary>
        /// [*]HTTP请求-响应时间
        /// </summary>
        public long? HTTP_Response_Time { get; set; }
        /// <summary>
        /// [*]HTTP请求-总下载字节数
        /// </summary>
        public long? HTTP_BufferSize { get; set; }

        public NormalResponse Check()
        {
            if (string.IsNullOrEmpty(AID)) return new NormalResponse(false, "AID不可为空");
            return new NormalResponse(true, "");
        }
        public void MakeInfo()
        {
            DateTime now = TimeUtil.Now();
            this.DateTime = now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Day = now.ToString("yyyy-MM-dd");
        }
    }
}
