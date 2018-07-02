using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFXIVPlayerWardrobe.Forms;
using FFXIVPlayerWardrobe.Memory;
using FFXIVPlayerWardrobe.Properties;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using GearTuple = System.Tuple<int, int, int>;

namespace FFXIVPlayerWardrobe
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private Mem _memory;
        private BackgroundWorker _worker = new BackgroundWorker();
        private ExdCsvReader _exdProvider = new ExdCsvReader();
        private MemoryManager _memoryMan;

        private IntPtr _customizeOffset = IntPtr.Zero;

        private GearSet _gearSet = new GearSet();
        private GearSet _cGearSet = new GearSet();

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
#if DEBUG
                //CheckResidentList();
                //CheckItemList();
#endif
                _exdProvider.MakeWeatherList();
                _exdProvider.MakeWeatherRateList();
                _exdProvider.MakeTerritoryTypeList();

                this.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
                this.Text += " - DEBUG";
#endif

                if (Properties.Settings.Default.FirstLaunch)
                {
                    MessageBox.Show(
                        "Please save the character data you are going to log in as in the FFXIV Lobby(right click->Save Appearance Data).\nThen, log in and press OK.\n\n!!!DISCLAIMER!!!\nNever use the Aesthetician while using this tool.\nI'm not responsible for any problems that might occur.");
                    Properties.Settings.Default.FirstLaunch = false;
                    Properties.Settings.Default.Save();
                }

                var chooser = new CharaSaveChooseForm("Please choose the exact character you are playing as.");
                chooser.StartPosition = FormStartPosition.CenterScreen;
                chooser.ShowDialog();

                if (chooser.Choice == null)
                {
                    Util.ShowError("No character chosen.");
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                _gearSet.Customize = chooser.Choice;

                var searchCA = Util.ByteArrayToString(chooser.Choice).ToCharArray();
                searchCA[36] = '?';
                searchCA[37] = '?';
                var searchString = new string(searchCA);

                Process ffxivProcess = null;

                try
                {
                    var procList = Process.GetProcesses();

                    foreach (var process in procList)
                    {
                        if (process.ProcessName == "ffxiv_dx11")
                            ffxivProcess = process;
                    }
                }
                catch (Exception exc)
                {
                    Util.ShowError("An error occurred reading the process list.\n\n" + exc);
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                if (ffxivProcess == null)
                {
                    Util.ShowError("FFXIV DX11 is not running. Make sure you're using the DirectX11 version of the game.\n\nThis can be changed in the launcher by turning Config->DirectX 11 Support on.");
#if !DEBUG
                    AskGuide();
                    Environment.Exit(0);
#endif
                }

                _memory = new Mem();
                if (!_memory.OpenProcess(ffxivProcess.ProcessName))
                {
                    Util.ShowError("An error occurred opening the FFXIV process.");
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                _memoryMan = new MemoryManager(_memory);

                var scanForm = new ScanProgressForm(_memory, searchString);
                scanForm.Show();
                scanForm.Worker.RunWorkerCompleted += ScanWorker_RunWorkerCompleted;
                scanForm.Run();


#if !DEBUG
                SynchronizationContext.Current.Post((obj) => this.Visible = false, null);
#endif

                _worker.DoWork += WorkerOnDoWork;
                _worker.WorkerSupportsCancellation = true;

#if !DEBUG
                VersionCheck v = new VersionCheck();
                v.GotVersionInfo += delegate(object o, VersionCheck.VersionCheckEventArgs args)
                {
                    if (!args.Current)
                    {
                        MessageBox.Show($"Your version is not up-to-date(newest: {args.NewVersion}).\nMake sure to get it for bug fixes and new features.", "Version Check " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Diagnostics.Process.Start("https://github.com/goaaats/FFXIVPlayerWardrobe/releases/latest");
                    }
                };
                v.Run();
#endif
            }
            catch (Exception exc)
            {
                Util.ShowError("An error occurred during initialization.\n\n" + exc);
#if !DEBUG
                Environment.Exit(0);
#endif
            }

        }

        private void ScanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _customizeOffset = (IntPtr)e.Result;

#if DEBUG
            offsetLabel.Text = "CustomizeOffset: " + _customizeOffset.ToString("X");
#else
            offsetLabel.Text = "";
#endif

            SetupDefaults();
            FillDefaults();

            this.Visible = true;
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (sender is BackgroundWorker mworker && !mworker.CancellationPending)
            {
                WriteCurrentCustomize();

                if(freezeGearCheckBox.Checked)
                    WriteCurrentGearTuples();

                Thread.Sleep(10);
            }
        }

        private void SetupDefaults()
        {
            _gearSet.Customize = _memory.readBytes(_customizeOffset.ToString("X"), 26);

            _gearSet.HeadGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_HEAD_OFF);
            _gearSet.BodyGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_BODY_OFF);
            _gearSet.HandsGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_HANDS_OFF);
            _gearSet.LegsGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_LEGS_OFF);
            _gearSet.FeetGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_FEET_OFF);
            _gearSet.EarGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_EAR_OFF);
            _gearSet.NeckGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_NECK_OFF);
            _gearSet.WristGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_WRIST_OFF);
            _gearSet.RRingGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_RRING_OFF);
            _gearSet.LRingGear = ReadGearTuple(_customizeOffset + Definitions.GEAR_LRING_OFF);

            _gearSet.MainWep = ReadGearTuple(_customizeOffset + Definitions.WEP_MAINH_OFF);
            _gearSet.OffWep = ReadGearTuple(_customizeOffset + Definitions.WEP_OFFH_OFF);
        }

        private void FillDefaults()
        {
            customizeTextBox.Text = Util.ByteArrayToString(_gearSet.Customize);

            headGearTextBox.Text = GearTupleToComma(_gearSet.HeadGear);
            bodyGearTextBox.Text = GearTupleToComma(_gearSet.BodyGear);
            handsGearTextBox.Text = GearTupleToComma(_gearSet.HandsGear);
            legsGearTextBox.Text = GearTupleToComma(_gearSet.LegsGear);
            feetGearTextBox.Text = GearTupleToComma(_gearSet.FeetGear);
            earGearTextBox.Text = GearTupleToComma(_gearSet.EarGear);
            neckGearTextBox.Text = GearTupleToComma(_gearSet.NeckGear);
            wristGearTextBox.Text = GearTupleToComma(_gearSet.WristGear);
            rRingGearTextBox.Text = GearTupleToComma(_gearSet.RRingGear);
            lRingGearTextBox.Text = GearTupleToComma(_gearSet.LRingGear);

            mainWepTextBox.Text = GearTupleToComma(_gearSet.MainWep);
            offWepTextBox.Text = GearTupleToComma(_gearSet.OffWep);
        }

        private void FillCustoms()
        {
            customizeTextBox.Text = Util.ByteArrayToString(_cGearSet.Customize);

            headGearTextBox.Text = GearTupleToComma(_cGearSet.HeadGear);
            bodyGearTextBox.Text = GearTupleToComma(_cGearSet.BodyGear);
            handsGearTextBox.Text = GearTupleToComma(_cGearSet.HandsGear);
            legsGearTextBox.Text = GearTupleToComma(_cGearSet.LegsGear);
            feetGearTextBox.Text = GearTupleToComma(_cGearSet.FeetGear);
            earGearTextBox.Text = GearTupleToComma(_cGearSet.EarGear);
            neckGearTextBox.Text = GearTupleToComma(_cGearSet.NeckGear);
            wristGearTextBox.Text = GearTupleToComma(_cGearSet.WristGear);
            rRingGearTextBox.Text = GearTupleToComma(_cGearSet.RRingGear);
            lRingGearTextBox.Text = GearTupleToComma(_cGearSet.LRingGear);

            mainWepTextBox.Text = GearTupleToComma(_cGearSet.MainWep);
            offWepTextBox.Text = GearTupleToComma(_cGearSet.OffWep);
        }

        private GearTuple ReadGearTuple(IntPtr offset)
        {
            var bytes = _memory.readBytes(offset.ToString("X"), 4);
            
            return new GearTuple(BitConverter.ToInt16(bytes, 0), bytes[2], bytes[3]);
        }

        public static string GearTupleToComma(GearTuple tuple)
        {
            return $"{tuple.Item1},{tuple.Item2},{tuple.Item3}";
        }

        public static GearTuple CommaToGearTuple(string input)
        {
            var parts = input.Split(',');
            return new GearTuple(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }

        public static byte[] GearTupleToByteAry(GearTuple tuple)
        {
            byte[] bytes = new byte[4];

            BitConverter.GetBytes((Int16)tuple.Item1).CopyTo(bytes, 0);
            bytes[2] = (byte)tuple.Item2;
            bytes[3] = (byte)tuple.Item3;

            return bytes;
        }

        public static byte[] WepTupleToByteAry(GearTuple tuple)
        {
            byte[] bytes = new byte[6];

            BitConverter.GetBytes((Int16)tuple.Item1).CopyTo(bytes, 0);
            BitConverter.GetBytes((Int16)tuple.Item2).CopyTo(bytes, 2);
            BitConverter.GetBytes((Int16)tuple.Item3).CopyTo(bytes, 4);

            return bytes;
        }

        private void restoreOriginalLookButton_Click(object sender, EventArgs e)
        {
            RestoreDefaultGear();
            RestoreDefaultCustomize();
            FillDefaults();
        }

        private void customizeApplyButton_Click(object sender, EventArgs e)
        {
            try
            {
                _cGearSet.Customize = Util.StringToByteArray(customizeTextBox.Text.Replace(" ", string.Empty));
                WriteCurrentCustomize();
            }
            catch (Exception exc)
            {
                Util.ShowError("One or more fields were not formatted correctly.\n\n" + exc);
            }
        }

        private void WriteCurrentCustomize()
        {
            if (_cGearSet.Customize == null)
                return;

            _memory.writeBytes(_customizeOffset, _cGearSet.Customize);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"FFXIVPlayerWardrobe - amibu/goaaats\n\nUsing Memory.dll by erfg(https://github.com/erfg12/memory.dll)\n\nItem table: {Resources.item_exh_en.Split('\n').Length - 1} entries\nResident info table: {Resources.enpcresident_exh_en.Split('\n').Length - 1} entries\nResident base table: {Resources.enpcbase_exh.Split('\n').Length - 1} entries");
        }

        private void RestoreDefaultCustomize()
        {
            _memory.writeBytes(_customizeOffset, _gearSet.Customize);
        }

        private void RestoreDefaultGear()
        {
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_HEAD_OFF, GearTupleToByteAry(_gearSet.HeadGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_BODY_OFF, GearTupleToByteAry(_gearSet.BodyGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_HANDS_OFF, GearTupleToByteAry(_gearSet.HandsGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_LEGS_OFF, GearTupleToByteAry(_gearSet.LegsGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_FEET_OFF, GearTupleToByteAry(_gearSet.FeetGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_EAR_OFF, GearTupleToByteAry(_gearSet.EarGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_NECK_OFF, GearTupleToByteAry(_gearSet.NeckGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_WRIST_OFF, GearTupleToByteAry(_gearSet.WristGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_RRING_OFF, GearTupleToByteAry(_gearSet.RRingGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_WRIST_OFF, GearTupleToByteAry(_gearSet.LRingGear));

            _memory.writeBytes(_customizeOffset + Definitions.WEP_MAINH_OFF, WepTupleToByteAry(_gearSet.MainWep));
            _memory.writeBytes(_customizeOffset + Definitions.WEP_OFFH_OFF, WepTupleToByteAry(_gearSet.OffWep));
        }

        private void WriteCurrentGearTuples()
        {
            if (_cGearSet.HeadGear == null)
                return;

            _memory.writeBytes(_customizeOffset + Definitions.GEAR_HEAD_OFF, GearTupleToByteAry(_cGearSet.HeadGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_BODY_OFF, GearTupleToByteAry(_cGearSet.BodyGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_HANDS_OFF, GearTupleToByteAry(_cGearSet.HandsGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_LEGS_OFF, GearTupleToByteAry(_cGearSet.LegsGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_FEET_OFF, GearTupleToByteAry(_cGearSet.FeetGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_EAR_OFF, GearTupleToByteAry(_cGearSet.EarGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_NECK_OFF, GearTupleToByteAry(_cGearSet.NeckGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_WRIST_OFF, GearTupleToByteAry(_cGearSet.WristGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_RRING_OFF, GearTupleToByteAry(_cGearSet.RRingGear));
            _memory.writeBytes(_customizeOffset + Definitions.GEAR_WRIST_OFF, GearTupleToByteAry(_cGearSet.LRingGear));

            _memory.writeBytes(_customizeOffset + Definitions.WEP_MAINH_OFF, WepTupleToByteAry(_cGearSet.MainWep));
            _memory.writeBytes(_customizeOffset + Definitions.WEP_OFFH_OFF, WepTupleToByteAry(_cGearSet.OffWep));
        }

        private void WriteGear_Click(object sender, EventArgs e)
        {
            try
            {
                _cGearSet.HeadGear = CommaToGearTuple(headGearTextBox.Text);
                _cGearSet.BodyGear = CommaToGearTuple(bodyGearTextBox.Text);
                _cGearSet.HandsGear = CommaToGearTuple(handsGearTextBox.Text);
                _cGearSet.LegsGear = CommaToGearTuple(legsGearTextBox.Text);
                _cGearSet.FeetGear = CommaToGearTuple(feetGearTextBox.Text);
                _cGearSet.EarGear = CommaToGearTuple(earGearTextBox.Text);
                _cGearSet.NeckGear = CommaToGearTuple(neckGearTextBox.Text);
                _cGearSet.WristGear = CommaToGearTuple(wristGearTextBox.Text);
                _cGearSet.RRingGear = CommaToGearTuple(rRingGearTextBox.Text);
                _cGearSet.LRingGear = CommaToGearTuple(lRingGearTextBox.Text);

                _cGearSet.MainWep = CommaToGearTuple(mainWepTextBox.Text);
                _cGearSet.OffWep = CommaToGearTuple(offWepTextBox.Text);

                WriteCurrentGearTuples();
            }
            catch (Exception exc)
            {
                Util.ShowError("One or more fields were not formatted correctly.\n\n" + exc);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            restoreOriginalLookButton_Click(null, null);
        }

        private void makeNewDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupDefaults();
            FillDefaults();
        }

        private void freezeValuesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (freezeValuesToolStripMenuItem.Checked)
            {
                freezeGearCheckBox.Visible = true;
                _worker.RunWorkerAsync();
            }
            else
            {
                freezeGearCheckBox.Visible = false;
                _worker.CancelAsync();
            }
        }

        private bool CheckItemList()
        {
            if (_exdProvider.Items == null)
            {
                _exdProvider.MakeItemList();
                if (_exdProvider.Items == null)
                {
                    MessageBox.Show("Failed to read item list. This isn't your fault.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private bool CheckResidentList()
        {
            if (_exdProvider.Residents == null)
            {
                _exdProvider.MakeResidentList();
                if (_exdProvider.Residents == null)
                {
                    MessageBox.Show("Failed to read NPC list. This isn't your fault.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private bool CheckCharaMakeFeatureList()
        {
            if (_exdProvider.CharaMakeFeatures == null)
            {
                _exdProvider.MakeCharaMakeFeatureList();
                if (_exdProvider.CharaMakeFeatures == null)
                {
                    MessageBox.Show("Failed to read chara make feature list. This isn't your fault.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void openItemsHeadButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Head).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                headGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsBodyButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Body).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                bodyGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsHandsButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Hands).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                handsGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsLegsButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Legs).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                legsGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsFeetButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Feet).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                feetGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsEarsButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ears).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                earGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsNeckButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Neck).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                neckGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsWristsButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Wrists).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                wristGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsRRingButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ring).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                rRingGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsLRingButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ring).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                lRingGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsMHButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Wep && !c.ModelMain.Contains("0,0,0,0")).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                mainWepTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsOHButton_Click(object sender, EventArgs e)
        {
            if (!CheckItemList())
                return;

            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Wep && !c.ModelOff.Contains("0,0,0,0")).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                offWepTextBox.Text = p.Choice.ModelOff;
        }

        private void customizeChooserButton_Click(object sender, EventArgs e)
        {
            CharaSaveChooseForm chooser = new CharaSaveChooseForm("Select the saved character you want to load.");
            chooser.ShowDialog();

            if (chooser.Choice != null)
                customizeTextBox.Text = Util.ByteArrayToString(chooser.Choice);

            customizeApplyButton_Click(null, null);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileDialog = new System.Windows.Forms.SaveFileDialog { Filter = @"JSON|*.json" };

            var result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    WriteGear_Click(null, null);
                    customizeApplyButton_Click(null, null);

                    File.WriteAllText(fileDialog.FileName, _cGearSet.ToJson());
                    MessageBox.Show($"Gearset {fileDialog.FileName}.", "FFXIVPlayerWardrobe", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception exc)
                {
                    Util.ShowError("Could not save gearset.\n\n" + exc);
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"JSON|*.json";
            openFileDialog.Title = @"Select a gearset JSON file";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _cGearSet = GearSet.FromJson(File.ReadAllText(openFileDialog.FileName));

                // Backwards compatibility
                if(_cGearSet.OffWep == null)
                    _cGearSet.OffWep = new GearTuple(0,0,0);

                FillCustoms();
                WriteCurrentGearTuples();
                WriteCurrentCustomize();
            }
        }

        private void selectNpcButton_Click(object sender, EventArgs e)
        {
            if (!CheckResidentList())
                return;

            ResidentSelectForm f = new ResidentSelectForm(_exdProvider.Residents.Values.Where(c => c.IsGoodNpc()).ToArray());
            f.ShowDialog();

            if (f.Choice == null)
                return;

            var gs = f.Choice.Gear;

            gs.Customize = _cGearSet.Customize ?? _gearSet.Customize;

            _cGearSet = gs;

            FillCustoms();
            WriteCurrentGearTuples();
            WriteCurrentCustomize();
        }

        private void offsetLabel_Click(object sender, EventArgs e)
        {
#if DEBUG
            var mem = _memory.readBytes((_customizeOffset + Definitions.CHARA_NAME_OFF).ToString("X"), 2000);
            File.WriteAllBytes("dump.bin", mem);
            MessageBox.Show("Dumped memory to dump.bin");
#endif
        }

        private void openGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskGuide();
        }

        private void openCustomizeEditForm_Click(object sender, EventArgs e)
        {
            CheckCharaMakeFeatureList();
            try
            {
                var c = new EditCustomizeForm(Util.StringToByteArray(customizeTextBox.Text.Replace(" ", string.Empty)), _exdProvider);
                c.ShowDialog();

                if (c.EditedCustomize == null)
                    return;

                customizeTextBox.Text = Util.ByteArrayToString(c.EditedCustomize);
                customizeApplyButton_Click(null, null);
            }
            catch (Exception exc)
            {
                Util.ShowError("One or more fields were not formatted correctly.\n\n" + exc);
            }
        }

        public static void AskGuide()
        {
            var res = MessageBox.Show("Open the Usage Guide in your web browser?",
                "FFXIVPlayerWardrobe", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
                System.Diagnostics.Process.Start("https://github.com/goaaats/FFXIVPlayerWardrobe/wiki/Usage-Guide");
        }

        private void guideAskInfoLabel_Click(object sender, EventArgs e)
        {
            AskGuide();
        }

        private void setTimeOffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var input = Interaction.InputBox("This will set a offset for the current eorzea time.", "Set Time offset",
                _memoryMan.GetTimeOffset().ToString());

            if (input == "")
                return;

            try
            {
                _memoryMan.SetTimeOffset(int.Parse(input));
            }
            catch (Exception exc)
            {
                Util.ShowError("One or more fields were not formatted correctly.\n\n" + exc);
            }
        }

        private void setWeatherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int territory = _memoryMan.GetTerritoryType();

            if (!_exdProvider.TerritoryTypes.ContainsKey(territory))
            {
                Util.ShowError("Could not find your current zone. Make sure you are using the latest version.");
                return;
            }

            if (_exdProvider.TerritoryTypes[territory].WeatherRate == null)
            {
                Util.ShowError("Setting weather is not supported for your current zone.");
                return;
            }

            var c = new WeatherSelector(_exdProvider.TerritoryTypes[territory].WeatherRate.AllowedWeathers, _memoryMan.GetWeather());
            c.ShowDialog();

            if (c.Choice != null)
            {
                _memoryMan.SetWeather((byte) c.Choice.Index);
            }
        }
    }
}
