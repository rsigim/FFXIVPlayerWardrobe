using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVPlayerWardrobe.Memory
{
    class MemoryManager
    {
        private readonly Mem _memory;

        public MemoryManager(Mem memory)
        {
            _memory = memory;
        }

        public int GetTimeOffset()
        {
            return _memory.readInt(Definitions.TIMEOFFSETPTR);
        }

        public void SetTimeOffset(int offset)
        {
            _memory.writeMemory(Definitions.TIMEOFFSETPTR, "int", offset.ToString());
        }

        public int GetTerritoryType()
        {
            return _memory.readInt(Definitions.TERRITORYTYPEOFFSETPTR);
        }

        public int GetWeather()
        {
            return _memory.readByte(Definitions.WEATHEROFFSETPTR);
        }

        public void SetWeather(byte id)
        {
            _memory.writeBytes(Definitions.WEATHEROFFSETPTR, new[] {id});
        }
    }
}
