using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVPlayerWardrobe
{
    class SaveDat
    {
        public readonly string Description;
        public readonly byte[] CustomizeBytes;

        public SaveDat(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    stream.Seek(0x10, SeekOrigin.Begin);

                    CustomizeBytes = reader.ReadBytes(26);

                    stream.Seek(0x30, SeekOrigin.Begin);

                    Description = Encoding.UTF8.GetString(reader.ReadBytes(164));
                }
            }
        }

    }
}
