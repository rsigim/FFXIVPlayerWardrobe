using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FFXIVPlayerWardrobe;
using GearTuple = System.Tuple<int, int, int>;

namespace ExdIndexTest
{
    [TestClass]
    public class IndexTest
    {
        private readonly ExdCsvReader _reader = new ExdCsvReader();

        [TestMethod]
        public void Item_TestItemRead()
        {
            _reader.MakeItemList();
            Assert.IsFalse(_reader.Items == null);

            var item = _reader.Items[2964]; // Judge's shirt

            Assert.AreEqual("41,2,0,0", item.ModelMain);
        }

        [TestMethod]
        public void Item_TestResidentRead()
        {
            _reader.MakeResidentList();
            Assert.IsFalse(_reader.Residents == null);

            var testGearSet = new GearSet
            {
                HeadGear = new GearTuple(0, 0, 0),
                BodyGear = new GearTuple(475, 1, 0),
                HandsGear = new GearTuple(475, 1, 0),
                LegsGear = new GearTuple(475, 1, 0),
                FeetGear = new GearTuple(475, 1, 0),
                EarGear = new GearTuple(0, 0, 0),
                NeckGear = new GearTuple(0, 0, 0),
                WristGear = new GearTuple(0, 0, 0),
                LRingGear = new GearTuple(0, 0, 0),
                RRingGear = new GearTuple(0, 0, 0),
                MainWep = new GearTuple(401,34,3),
                OffWep = new GearTuple(0,0,0),

                Customize = new byte[] {0x06,0x00,0x01,0x64,0x0c,0x65,0x02,0x81,0x7c,0x2b,0xb7,0x23,0x7f,0x22,0x02,0x2b,0x02,0x03,0x01,0x82,0xa6,0x64,0x03,0x32,0x00,0x01}
            };

            var r = _reader.Residents[1018979]; // Magnai

            Assert.AreEqual(testGearSet.ToJson(), r.Gear.ToJson());
        }
    }
}
