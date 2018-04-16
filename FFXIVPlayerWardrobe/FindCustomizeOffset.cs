﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFXIVMonReborn;

namespace FFXIVPlayerWardrobe
{
    public static class FindCustomizeOffset
    {
        private const int CHARA_NAME_OFF = -0x15C0;
        private const int CHARA_RUN_COUNTER_OFF = -0x790;

        public static async Task<IntPtr> Find(Mem memory, string customize, bool checkCounter = true)
        {
            IntPtr customizeOffset = IntPtr.Zero;

            var hits = await memory.AoBScan(customize, true, true);

            if (hits.Count() == 0)
            {
                MessageBox.Show($"Could not find character data - make sure you selected the correct entry and try to re-export the data.\n\nInfo:\nPass 1\ncheckCounter: {checkCounter}\nHits: {hits.Count()}\n{customize}", "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            string scanLog = "";

            foreach (var hit in hits)
            {
                string name = Encoding.UTF8.GetString(memory.readBytes((hit + CHARA_NAME_OFF).ToString("X"), 32));
                Debug.WriteLine(name);
                if (name.Split(' ')[0].All(IsValidFFXIVNameChar))
                {
                    if (checkCounter)
                    {
                        var counter1 = memory.readBytes((hit + CHARA_RUN_COUNTER_OFF).ToString("X"), 4);
                        Thread.Sleep(100);
                        var counter2 = memory.readBytes((hit + CHARA_RUN_COUNTER_OFF).ToString("X"), 4);

                        if (counter1.SequenceEqual(counter2))
                        {
                            Debug.WriteLine($"Counter break {hit.ToString("X")} - {name}");
                            continue;
                        }
                    }

                    //if (MessageBox.Show("Is this your character's name: " + name, "", MessageBoxButtons.YesNo,
                    //        MessageBoxIcon.None) == DialogResult.Yes)
                    //{
                        customizeOffset = new IntPtr(hit);
                        break;
                    //}
                }
                scanLog += name + "\n";
            }

            if (customizeOffset == IntPtr.Zero && !checkCounter)
            {
                MessageBox.Show($"Could not find character data - make sure you selected the correct entry and try to re-export the data.\n\nInfo:\nPass 2\ncheckCounter: {checkCounter}\nHits: {hits.Count()}\n{customize}\nScanned:\n{scanLog}", "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (customizeOffset == IntPtr.Zero)
                return await Find(memory, customize, false);

            return customizeOffset;
        }

        private static bool IsValidFFXIVNameChar(char c)
        {
            char[] allowed = new[] { '-', '\'' };

            bool isLetter = Char.IsLetter(c);
            bool isFfAllowed = allowed.Contains(c);

            return isLetter || isFfAllowed;
        }
    }
}
