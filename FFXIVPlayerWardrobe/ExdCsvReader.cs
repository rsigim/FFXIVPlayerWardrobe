using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFXIVPlayerWardrobe;
using FFXIVPlayerWardrobe.Properties;
using Microsoft.VisualBasic.FileIO;
using GearTuple = System.Tuple<int, int, int>;
using WepTuple = System.Tuple<int, int, int, int>;

namespace FFXIVMonReborn
{
    public class ExdCsvReader
    {
        public enum ItemType // ItemSearchCategory
        {
            Wep, // Everything else with a look
            Head, // 31
            Body, // 33
            Hands, // 36
            Legs, // 35
            Feet, //37
            Ears, // 40
            Neck, // 39
            Wrists, // 41
            Ring //42
        }

        public class Item
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public string ModelMain { get; set; }
            public string ModelOff { get; set; }
            public ItemType Type { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        public ExdCsvReader()
        {
            try
            {
                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.item_exh_en)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();
                        int fCount = 0;

                        int index = 0;
                        var item = new Item();

                        if(rowCount == 1)
                            continue;

                        foreach (string field in fields)
                        {
                            fCount++;

                            if (fCount == 1)
                            {
                                int.TryParse(field, out index);
                            }

                            if (fCount == 2)
                            {
                                item.Name = field;
                            }

                            if (fCount == 17)
                            {
                                int cat = int.Parse(field);
                                switch (cat)
                                {
                                    case 34: // Head
                                        item.Type = ItemType.Head;
                                        break;
                                    case 35: // Body
                                        item.Type = ItemType.Body;
                                        break;
                                    case 37:
                                        item.Type = ItemType.Hands;
                                        break;
                                    case 36:
                                        item.Type = ItemType.Legs;
                                        break;
                                    case 38:
                                        item.Type = ItemType.Feet;
                                        break;
                                    case 41:
                                        item.Type = ItemType.Ears;
                                        break;
                                    case 40:
                                        item.Type = ItemType.Neck;
                                        break;
                                    case 42:
                                        item.Type = ItemType.Wrists;
                                        break;
                                    case 43:
                                        item.Type = ItemType.Ring;
                                        break;
                                    default:
                                        item.Type = ItemType.Wep;
                                        break;
                                }
                            }

                            if (fCount == 47)
                            {
                                var tfield = field.Replace(" ", "");
                                if (item.Type == ItemType.Wep)
                                {
                                    item.ModelMain = tfield;
                                }
                                else
                                {
                                    item.ModelMain = tfield;
                                }
                            }

                            if (fCount == 48)
                            {
                                var tfield = field.Replace(" ", "");
                                if (item.Type == ItemType.Wep)
                                {
                                    item.ModelOff = tfield;
                                }
                                else
                                {
                                    item.ModelOff = tfield;
                                }
                            }
                        }

                        Debug.WriteLine(item.Name + " - " + item.Type);
                        Items.Add(index, item);
                    }
                    Debug.WriteLine($"ExdCsvReader: {rowCount} items read");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("[ExdCsvReader] Failed to parse CSV sheets. This isn't your fault.\n\n" + exc, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public Item GetItemName(int id)
        {
            Item item;
            if (Items.TryGetValue(id, out item))
            {
                return item;
            }
            else
            {
                return null;
            }
        }
    }
}