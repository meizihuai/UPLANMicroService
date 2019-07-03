using System;
using System.Collections.Generic;
using System.Text;

namespace UplanModels
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
