using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FFXIVPlayerWardrobe.Memory
{
    class Definitions
    {
        private static Definitions _cachedInstance = null;
        private const string DEFINITION_JSON_URL = "https://raw.githubusercontent.com/goaaats/FFXIVPlayerWardrobe/master/definitions.json";

        public static Definitions Instance
        {
            get
            {
                if (_cachedInstance != null)
                    return _cachedInstance;

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        var result = client.DownloadString(DEFINITION_JSON_URL);
                        _cachedInstance = JsonConvert.DeserializeObject<Definitions>(result);

                        return _cachedInstance;
                    }
                    catch (Exception)
                    {
                        return new Definitions();
                    }
                }
            }
        }

        public static string Json => JsonConvert.SerializeObject(new Definitions());

        public int GEAR_HEAD_OFF = -0x80;
        public int GEAR_BODY_OFF = -0x7C;
        public int GEAR_HANDS_OFF = -0x78;
        public int GEAR_LEGS_OFF = -0x74;
        public int GEAR_FEET_OFF = -0x70;
        public int GEAR_EAR_OFF = -0x6C;
        public int GEAR_NECK_OFF = -0x68;
        public int GEAR_WRIST_OFF = -0x64;
        public int GEAR_RRING_OFF = -0x60;
        public int GEAR_LRING_OFF = -0x5C;

        public int WEP_MAINH_OFF = -0x310; //0x2f0
        public int WEP_OFFH_OFF = -0x316;

        public int CHARA_NAME_OFF = -0x1600;
        public int CHARA_RUN_COUNTER_OFF = -0x7B8;

        public string TIMEOFFSETPTR = "ffxiv_dx11.exe+18E3330,10,8,28,80"; // 4 byte
        public string WEATHEROFFSETPTR = "ffxiv_dx11.exe+18E1278,27"; // 1 byte
        public string TERRITORYTYPEOFFSETPTR = "ffxiv_dx11.exe+19369A8,4C"; // 4 byte

        public string ACTORTABLEOFFSET = "ffxiv_dx11.exe+18FF738";
    }
}
