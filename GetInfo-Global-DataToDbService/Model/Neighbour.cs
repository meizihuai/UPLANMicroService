using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetInfo_Global_DataToDbService
{
    public class Neighbour
    {
        public string Type { get; set; }
        public int EARFCN { get; set; }
        public int PCI { get; set; }
        public int RSRP { get; set; }
        public int RSRQ { get; set; }
    }
}
