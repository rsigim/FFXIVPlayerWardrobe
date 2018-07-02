using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVPlayerWardrobe.Memory
{
    class Definitions
    {
        public const int GEAR_HEAD_OFF = -0x80;
        public const int GEAR_BODY_OFF = -0x7C;
        public const int GEAR_HANDS_OFF = -0x78;
        public const int GEAR_LEGS_OFF = -0x74;
        public const int GEAR_FEET_OFF = -0x70;
        public const int GEAR_EAR_OFF = -0x6C;
        public const int GEAR_NECK_OFF = -0x68;
        public const int GEAR_WRIST_OFF = -0x64;
        public const int GEAR_RRING_OFF = -0x60;
        public const int GEAR_LRING_OFF = -0x5C;

        public const int WEP_MAINH_OFF = -0x310; //0x2f0
        public const int WEP_OFFH_OFF = -0x316;

        public const int CHARA_NAME_OFF = -0x1600;
        public const int CHARA_RUN_COUNTER_OFF = -0x7B8;

        public const string TIMEOFFSETPTR = "ffxiv_dx11.exe+018DC2D0,10,8,28,80"; // 4 byte
        public const string WEATHEROFFSETPTR = "ffxiv_dx11.exe+018DA218,27"; // 1 byte
        public const string TERRITORYTYPEOFFSETPTR = "ffxiv_dx11.exe+0192F938,4C"; // 4 byte
    }
}
