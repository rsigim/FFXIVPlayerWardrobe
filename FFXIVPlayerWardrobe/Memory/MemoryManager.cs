using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFXIVPlayerWardrobe.Memory;
using GearTuple = System.Tuple<int, int, int>;


namespace FFXIVPlayerWardrobe.Memory
{
    public class MemoryManager
    {
        private readonly Mem _memory;

        public MemoryManager(Mem memory)
        {
            _memory = memory;
        }

        #region General

        public int GetTimeOffset()
        {
            return _memory.readInt(Definitions.Instance.TimePtr);
        }

        public void SetTimeOffset(int offset)
        {
            _memory.writeMemory(Definitions.Instance.TimePtr, "int", offset.ToString());
        }

        public int GetTerritoryType()
        {
            return _memory.readInt(Definitions.Instance.TerritoryTypePtr);
        }

        public int GetWeather()
        {
            return _memory.readByte(Definitions.Instance.WeatherPtr);
        }

        public void SetWeather(byte id)
        {
            _memory.writeBytes(Definitions.Instance.WeatherPtr, new[] {id});
        }

        #endregion

        #region ActorTable

        public class ActorTableEntry
        {
            public long Offset { get; set; }
            public string Name { get; set; }
            public string CompanyTag { get; set; }
            public uint ActorID { get; set; }
            public uint OwnerID { get; set; }
            public short ModelChara { get; set; }
            public uint BnpcBase { get; set; }
            public byte Job { get; set; }
            public byte Level { get; set; }
            public byte World { get; set; }
            public byte ObjectKind { get; set; }
            public GearSet Gear { get; set; }
        }

        public int GetActorTableLength()
        {
            return _memory.readByte(Definitions.Instance.ActorTableOffset);
        }

        public ActorTableEntry[] GetActorTable()
        {
            var entries = new List<ActorTableEntry>();
            var offsets = GetActorTableOffsetList();

            foreach (var offset in offsets)
            {
                var entry = GetActorTableEntry((long) offset);

                if (entry == null)
                    return null;

                entries.Add(entry);
            }

            return entries.ToArray();
        }

        public UIntPtr[] GetActorTableOffsetList()
        {
            var offsets = new List<UIntPtr>();

            var tableOffset = _memory.getCode(Definitions.Instance.ActorTableOffset, "") + 0x8;

            for (var i = 0; i < GetActorTableLength(); i++)
                offsets.Add(_memory.getCode(((long) (tableOffset + i * 8)).ToString("X") + ",0", ""));

            return offsets.ToArray();
        }

        public ActorTableEntry GetActorTableEntry(long offset)
        {
            var data = _memory.readBytes(offset.ToString("X"), 0x1800);
            //Debug.WriteLine(Util.ByteArrayToHex(data));

            if (data == null)
                return null;

            return new ActorTableEntry
            {
                Offset = offset,
                /*ActorID = BitConverter.ToUInt32(data, Definitions.Instance.ActorIDOffset),
                Name = Encoding.UTF8.GetString(data, Definitions.Instance.NameOffset, 32),
                BnpcBase = BitConverter.ToUInt32(data, Definitions.Instance.BnpcBaseOffset),
                OwnerID = BitConverter.ToUInt32(data, Definitions.Instance.OwnerIDOffset),
                ModelChara = BitConverter.ToInt16(data, Definitions.Instance.ModelCharaOffset),
                Job = data[Definitions.Instance.JobOffset],
                Level = data[Definitions.Instance.LevelOffset],
                World = data[Definitions.Instance.WorldOffset],
                CompanyTag = Encoding.UTF8.GetString(data, Definitions.Instance.CompanyTagOffset, 6),*/
                Gear = new GearSet()
                {
                    HeadGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.HeadOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.HeadOffset + 2], data[Definitions.Instance.HeadOffset + 3]),
                    BodyGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.BodyOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.BodyOffset + 2], data[Definitions.Instance.BodyOffset + 3]),
                    HandsGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.HandsOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.HandsOffset + 2], data[Definitions.Instance.HandsOffset + 3]),
                    LegsGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.LegsOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.LegsOffset + 2], data[Definitions.Instance.LegsOffset + 3]),
                    FeetGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.FeetOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.FeetOffset + 2], data[Definitions.Instance.FeetOffset + 3]),
                    EarGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.EarOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.EarOffset + 2], data[Definitions.Instance.EarOffset + 3]),
                    NeckGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.NeckOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.NeckOffset + 2], data[Definitions.Instance.NeckOffset + 3]),
                    WristGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.WristOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.WristOffset + 2], data[Definitions.Instance.WristOffset + 3]),
                    LRingGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.LRingOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.LRingOffset + 2], data[Definitions.Instance.LRingOffset + 3]),
                    RRingGear = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.RRingOffset ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.RRingOffset + 2], data[Definitions.Instance.RRingOffset + 3]),
                    MainWep = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.MainWepOffset ).Take( 2 ).ToArray(), 0), BitConverter.ToInt16(data.Skip( Definitions.Instance.MainWepOffset + 2 ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.MainWepOffset + 4]),
                    OffWep = new GearTuple(BitConverter.ToInt16(data.Skip( Definitions.Instance.OffWepOffset ).Take( 2 ).ToArray(), 0), BitConverter.ToInt16(data.Skip( Definitions.Instance.OffWepOffset + 2 ).Take( 2 ).ToArray(), 0), data[Definitions.Instance.OffWepOffset + 4]),
                    Customize = data.Skip( Definitions.Instance.CustomizeOffset ).Take( 26 ).ToArray()
                },
            };
        }

        public static byte[] GearTupleToByteAry(Tuple<int, int, int> tuple)
        {
            byte[] bytes = new byte[4];

            BitConverter.GetBytes((Int16)tuple.Item1).CopyTo(bytes, 0);
            bytes[2] = (byte)tuple.Item2;
            bytes[3] = (byte)tuple.Item3;

            return bytes;
        }

        public static byte[] GearTupleToByteAryWeapon(Tuple<int, int, int> tuple)
        {
            byte[] bytes = new byte[5];

            BitConverter.GetBytes((Int16)tuple.Item1).CopyTo(bytes, 0);
            BitConverter.GetBytes((Int16)tuple.Item1).CopyTo(bytes, 2);
            bytes[4] = (byte)tuple.Item3;

            return bytes;
        }

        public bool WriteActorTableEntry(ActorTableEntry entry)
        {
            var table = GetActorTable();
            var offsets = GetActorTableOffsetList();

            if (offsets.Length < table.Length)
            {
                Debug.WriteLine("Offset table shorter than parsed actor table???");
                return false;
            }

            for (var i = 0; i < table.Length; i++)
                if (table[i].ActorID == entry.ActorID)
                {
                    Debug.WriteLine("Found at " + ((long) offsets[i]).ToString("X"));
                    //_memory.writeBytes(((long) offsets[i] + 0x30).ToString("X"), new byte[32]);
                    //_memory.writeMemory(((long) offsets[i] + 0x30).ToString("X"), "string", entry.Name);
                    //_memory.writeMemory(((long) offsets[i] + 0x80).ToString("X"), "int", entry.BnpcBase.ToString());
                    //_memory.writeMemory(((long) offsets[i] + 0x16FC).ToString("X"), "int", entry.ModelChara.ToString());
                    _memory.writeBytes( ( (long) offsets[i] + Definitions.Instance.CustomizeOffset ).ToString( "X" ),
                        entry.Gear.Customize );

                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.HeadOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.HeadGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.BodyOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.BodyGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.HandsOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.HandsGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.LegsOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.LegsGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.FeetOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.FeetGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.EarOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.EarGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.NeckOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.NeckGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.WristOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.WristGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.RRingOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.RRingGear));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.LRingOffset ).ToString( "X" ), GearTupleToByteAry(entry.Gear.LRingGear));

                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.MainWepOffset ).ToString( "X" ), GearTupleToByteAryWeapon(entry.Gear.MainWep));
                    _memory.writeBytes(( (long) offsets[i] + Definitions.Instance.OffWepOffset ).ToString( "X" ), GearTupleToByteAryWeapon(entry.Gear.OffWep));

                    _memory.writeMemory(( (long) offsets[i] + Definitions.Instance.ObjectKindOffset ).ToString( "X" ), "byte", 2.ToString());
                    _memory.writeMemory(( (long) offsets[i] + Definitions.Instance.RefreshSwitchOffset ).ToString( "X" ), "int", "2");
                    System.Threading.Thread.Sleep( 50 );
                    _memory.writeMemory(( (long) offsets[i] + Definitions.Instance.RefreshSwitchOffset ).ToString( "X" ), "int", "0");
                    System.Threading.Thread.Sleep( 50 );
                    _memory.writeMemory(( (long) offsets[i] + Definitions.Instance.ObjectKindOffset ).ToString( "X" ), "byte", 1.ToString());
                    return true;
                }

            return false;
        }

        #endregion
    }
}