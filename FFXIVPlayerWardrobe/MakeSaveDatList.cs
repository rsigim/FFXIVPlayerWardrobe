using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVPlayerWardrobe
{
    static class MakeSaveDatList
    {

        public static List<SaveDat> Make()
        {
            List<SaveDat> output = new List<SaveDat>();

            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games",
                "FINAL FANTASY XIV - A Realm Reborn");

            if(!Directory.Exists(basePath))
                throw new Exception("Could not find FFXIV Directory: " + basePath);

            var files = Directory.GetFiles(basePath, "FFXIV_CHARA*.dat");

            foreach (var file in files)
            {
                output.Add(new SaveDat(File.ReadAllBytes(file)));
            }

            return output;
        }
    }
}
