﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using FFXIVPlayerWardrobe.Properties;
using Microsoft.VisualBasic.FileIO;
using GearTuple = System.Tuple<int, int, int>;

namespace FFXIVPlayerWardrobe
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

        public class Resident
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public GearSet Gear { get; set; }

            public override string ToString()
            {
                return Name;
            }

            public bool IsGoodNpc()
            {
                if (Gear.Customize[0] != 0 && Name.Length != 0)
                    return true;

                return false;
            }

            public string MakeGearString()
            {
                return $"{MainForm.GearTupleToComma(Gear.HeadGear)} - {MainForm.GearTupleToComma(Gear.BodyGear)} - {MainForm.GearTupleToComma(Gear.HandsGear)} - {MainForm.GearTupleToComma(Gear.LegsGear)} - {MainForm.GearTupleToComma(Gear.FeetGear)} - {MainForm.GearTupleToComma(Gear.EarGear)} - {MainForm.GearTupleToComma(Gear.NeckGear)} - {MainForm.GearTupleToComma(Gear.WristGear)} - {MainForm.GearTupleToComma(Gear.LRingGear)} - {MainForm.GearTupleToComma(Gear.RRingGear)}";
            }
        }

        public class CharaMakeCustomizeFeature
        {
            public int Index { get; set; }
            public int FeatureID { get; set; }
            public System.Drawing.Bitmap Icon { get; set; }
        }

        public class Weather
        {
            public int Index { get; set; }
            public string Name { get; set; }
        }

        public class TerritoryType
        {
            public int Index { get; set; }
            public WeatherRate WeatherRate { get; set; }
        }

        public class WeatherRate
        {
            public int Index { get; set; }
            public List<Weather> AllowedWeathers { get; set; }
        }

        public Dictionary<int, Item> Items = null;
        public Dictionary<int, Resident> Residents = null;
        public Dictionary<int, CharaMakeCustomizeFeature> CharaMakeFeatures = null;
        public Dictionary<int, Weather> Weathers = null;
        public Dictionary<int, WeatherRate> WeatherRates = null;
        public Dictionary<int, TerritoryType> TerritoryTypes = null;

        public void MakeItemList()
        {
            Items = new Dictionary<int, Item>();

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

                        if (rowCount == 1)
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
                Items = null;
#if DEBUG
                throw exc;
#endif
            }
        }

        public void MakeResidentList()
        {
            Residents = new Dictionary<int, Resident>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.enpcresident_exh_en)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();
                        int fCount = 0;

                        int id = 0;
                        string name = "";

                        foreach (string field in fields)
                        {
                            fCount++;

                            if (fCount == 1)
                            {
                                id = int.Parse(field);
                            }

                            if (fCount == 2)
                            {
                                name = field;
                            }
                        }

                        Console.WriteLine($"{id} - {name}");
                        Residents.Add(id, new Resident { Index = id, Name = name });
                    }
                    Console.WriteLine($"{rowCount} residentNames read");
                }

                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.enpcbase_exh)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();
                        int fCount = 0;

                        int id = 0;
                        List<byte> customize = new List<byte>();
                        GearSet gear = new GearSet();
                        string wepCSV = "";
                        int dDataCount = 0;
                        int modelId = 0;

                        foreach (string field in fields)
                        {
                            fCount++;

                            if (fCount == 1)
                            {
                                id = int.Parse(field);
                            }

                            if (fCount == 37)
                            {
                                modelId = int.Parse(field);
                            }

                            if (fCount >= 38 && fCount <= 63)
                            {
                                try
                                {
                                    customize.Add(byte.Parse(field));
                                    dDataCount++;
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Invalid: " + field);
                                }
                            }

                            if (fCount == 67)
                            {
                                gear.MainWep = MainForm.CommaToGearTuple(field);
                            }

                            if (fCount == 69)
                            {
                                gear.OffWep = MainForm.CommaToGearTuple(field);
                            }

                            if (fCount >= 71 && fCount <= 90)
                            {
                                Int32 fieldint = 0;

                                if (fCount != 73)
                                    fieldint = Int32.Parse(field);

                                var bytes = BitConverter.GetBytes(fieldint);

                                var model = BitConverter.ToUInt16(bytes, 0);

                                switch (fCount - 1)
                                {
                                    case 70:
                                        gear.HeadGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 71:
                                        gear.HeadGear = new GearTuple(gear.HeadGear.Item1, gear.HeadGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 72:
                                        break;
                                    case 73:
                                        gear.BodyGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 74:
                                        gear.BodyGear = new GearTuple(gear.BodyGear.Item1, gear.BodyGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 75:
                                        gear.HandsGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 76:
                                        gear.HandsGear = new GearTuple(gear.HandsGear.Item1, gear.HandsGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 77:
                                        gear.LegsGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 78:
                                        gear.LegsGear = new GearTuple(gear.LegsGear.Item1, gear.LegsGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 79:
                                        gear.FeetGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 80:
                                        gear.FeetGear = new GearTuple(gear.FeetGear.Item1, gear.FeetGear.Item2,
                                            int.Parse(field));
                                        break;

                                    case 81:
                                        gear.EarGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 82:
                                        gear.EarGear = new GearTuple(gear.EarGear.Item1, gear.EarGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 83:
                                        gear.NeckGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 84:
                                        gear.NeckGear = new GearTuple(gear.NeckGear.Item1, gear.NeckGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 85:
                                        gear.WristGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 86:
                                        gear.WristGear = new GearTuple(gear.WristGear.Item1, gear.WristGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 87:
                                        gear.LRingGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 88:
                                        gear.LRingGear = new GearTuple(gear.LRingGear.Item1, gear.LRingGear.Item2,
                                            int.Parse(field));
                                        break;
                                    case 89:
                                        gear.RRingGear = new GearTuple(model, bytes[2], 0);
                                        break;
                                    case 90:
                                        gear.RRingGear = new GearTuple(gear.RRingGear.Item1, gear.RRingGear.Item2,
                                            int.Parse(field));
                                        break;
                                }
                            }
                        }

                        Console.WriteLine($"{id} - {wepCSV} - {dDataCount}");

                        gear.Customize = customize.ToArray();

                        try
                        {
                            Residents[id].Gear = gear;
                        }
                        catch (KeyNotFoundException exc)
                        {
                            Console.WriteLine("Did not find corresponding entry for: " + id);
                        }
                        
                    }
                    Console.WriteLine($"{rowCount} idLookMappings read");
                }

            }
            catch (Exception exc)
            {
                Residents = null;
#if DEBUG
                throw exc;
#endif
            }
        }

        public void MakeCharaMakeFeatureList()
        {
            CharaMakeFeatures = new Dictionary<int, CharaMakeCustomizeFeature>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.charamakecustomize_exh)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        CharaMakeCustomizeFeature feature = new CharaMakeCustomizeFeature();

                        feature.Index = rowCount;
                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();
                        int fCount = 0;

                        foreach (string field in fields)
                        {
                            fCount++;

                            if (fCount == 2)
                            {
                                feature.FeatureID = int.Parse(field);
                            }

                            if (fCount == 3)
                            {
                                feature.Icon = Properties.Resources.ResourceManager.GetObject($"_{field}_tex") as Bitmap;
                            }
                        }

                        Console.WriteLine($"{rowCount} - {feature.FeatureID}");
                        CharaMakeFeatures.Add(rowCount, feature);
                    }

                    Console.WriteLine($"{rowCount} charaMakeFeatures read");
                }
            }
            catch (Exception exc)
            {
                CharaMakeFeatures = null;
#if DEBUG
                throw exc;
#endif
            }
        }

        public void MakeWeatherList()
        {
            Weathers = new Dictionary<int, Weather>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.weather_0_exh_en)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        Weather weather = new Weather();

                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();
                        int fCount = 0;

                        weather.Index = int.Parse(fields[0]);

                        foreach (string field in fields)
                        {
                            fCount++;

                            if (fCount == 3)
                            {
                                weather.Name = field;
                            }
                        }

                        Console.WriteLine($"{rowCount} - {weather.Name}");
                        Weathers.Add(weather.Index, weather);
                    }

                    Console.WriteLine($"{rowCount} weathers read");
                }
            }
            catch (Exception exc)
            {
                Weathers = null;
#if DEBUG
                throw exc;
#endif
            }
        }

        public void MakeWeatherRateList()
        {
            if(Weathers == null)
                throw new Exception("Weathers has to be loaded for WeatherRates to be read");

            WeatherRates = new Dictionary<int, WeatherRate>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.weatherrate_exh)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        WeatherRate rate = new WeatherRate();

                        rate.AllowedWeathers = new List<Weather>();

                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();

                        rate.Index = int.Parse(fields[0]);

                        for (int i = 1; i < 17;)
                        {
                            int weatherId = int.Parse(fields[i]);

                            if(weatherId == 0)
                                break;

                            rate.AllowedWeathers.Add(Weathers[weatherId]);

                            i += 2;
                        }

                        WeatherRates.Add(rate.Index, rate);
                    }

                    Console.WriteLine($"{rowCount} weatherRates read");
                }
            }
            catch (Exception exc)
            {
                WeatherRates = null;
#if DEBUG
                throw exc;
#endif
            }
        }

        public void MakeTerritoryTypeList()
        {
            if(WeatherRates == null)
                throw new Exception("WeatherRates has to be loaded for TerritoryTypes to be read");

            TerritoryTypes = new Dictionary<int, TerritoryType>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.territorytype_exh)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        TerritoryType territory = new TerritoryType();

                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();
                        int fCount = 0;

                        territory.Index = int.Parse(fields[0]);

                        foreach (string field in fields)
                        {
                            fCount++;

                            if (fCount == 14)
                            {
                                if(field != "0")
                                    territory.WeatherRate = WeatherRates[int.Parse(field)];
                            }
                        }

                        TerritoryTypes.Add(territory.Index, territory);
                    }

                    Console.WriteLine($"{rowCount} TerritoryTypes read");
                }
            }
            catch (Exception exc)
            {
                TerritoryTypes = null;
#if DEBUG
                throw exc;
#endif
            }
        }

        public CharaMakeCustomizeFeature GetCharaMakeCustomizeFeature(int index, bool getBitMap)
        {
            try
            {
                using (TextFieldParser parser = new TextFieldParser(new StringReader(Resources.charamakecustomize_exh)))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    int rowCount = 0;
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        if (rowCount != index)
                        {
                            rowCount++;
                            parser.ReadFields();
                            continue;
                        }

                        CharaMakeCustomizeFeature feature = new CharaMakeCustomizeFeature();

                        feature.Index = index;

                        //Processing row
                        rowCount++;
                        string[] fields = parser.ReadFields();
                        int fCount = 0;

                        foreach (string field in fields)
                        {
                            fCount++;

                            if (fCount == 2)
                            {
                                feature.FeatureID = int.Parse(field);
                            }

                            if (fCount == 3)
                            {
                                if (getBitMap)
                                {
                                    
                                    feature.Icon = Properties.Resources.ResourceManager.GetObject($"_{field}_tex") as Bitmap;
                                }
                            }
                        }

                        return feature;
                    }
                }
            }
            catch (Exception exc)
            {
#if DEBUG
                throw exc;
#endif
                return null;
            }

            return null;
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
