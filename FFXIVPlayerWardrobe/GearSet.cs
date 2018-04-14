using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GearTuple = System.Tuple<int, int, int>;
using WepTuple = System.Tuple<int, int, int, int>;

namespace FFXIVPlayerWardrobe
{
    public class GearSet
    {
        public GearTuple HeadGear { get; set; }
        public GearTuple BodyGear { get; set; }
        public GearTuple HandsGear { get; set; }
        public GearTuple LegsGear { get; set; }
        public GearTuple FeetGear { get; set; }
        public GearTuple EarGear { get; set; }
        public GearTuple NeckGear { get; set; }
        public GearTuple WristGear { get; set; }
        public GearTuple RRingGear { get; set; }
        public GearTuple LRingGear { get; set; }

        public WepTuple MainWep { get; set; }

        public byte[] Customize { get; set; }
    }
}
