using System;

namespace FFXIVPlayerWardrobe
{
    public class NameWrapper
    {
        public string Value;
        public int Index;
        public object Info;

        public NameWrapper(string value, int index, object info = null)
        {
            Value = value;
            Index = index;
            Info = info;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}