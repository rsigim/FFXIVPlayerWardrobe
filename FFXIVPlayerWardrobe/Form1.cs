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
using FFXIVMonReborn;
using FFXIVPlayerWardrobe.Properties;
using Newtonsoft.Json;
using GearTuple = System.Tuple<int, int, int>;
using WepTuple = System.Tuple<int, int, int, int>;

namespace FFXIVPlayerWardrobe
{
    public partial class Form1 : Form
    {
        private const int GEAR_HEAD_OFF = -0x70;
        private const int GEAR_BODY_OFF = -0x6C;
        private const int GEAR_HANDS_OFF = -0x68;
        private const int GEAR_LEGS_OFF = -0x64;
        private const int GEAR_FEET_OFF = -0x60;
        private const int GEAR_EAR_OFF = -0x5C;
        private const int GEAR_NECK_OFF = -0x58;
        private const int GEAR_WRIST_OFF = -0x54;
        private const int GEAR_RRING_OFF = -0x50;
        private const int GEAR_LRING_OFF = -0x4C;

        private const int WEP_MAINH_OFF = -0x2F0;
        private const int WEP_OFFH_OFF = -0x2F8;

        public Form1()
        {
            InitializeComponent();
        }

        private Mem _memory;
        private BackgroundWorker _worker = new BackgroundWorker();
        private ExdCsvReader _exdProvider = new ExdCsvReader();

        private IntPtr _customizeOffset = IntPtr.Zero;

        private GearSet _gearSet = new GearSet();
        private GearSet _cGearSet = new GearSet();

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
#if DEBUG
                CheckResidentList();
                CheckItemList();
#endif

                this.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();

                if (Properties.Settings.Default.FirstLaunch)
                {
                    MessageBox.Show(
                        "Please save the character data you are going to log in as in the FFXIV Lobby(right click->Save Appearance Data).\nThen, log in and press OK.");
                    Properties.Settings.Default.FirstLaunch = false;
                    Properties.Settings.Default.Save();
                }

                var chooser = new CharaSaveChooseForm("Please choose the exact character you are playing as.");
                chooser.StartPosition = FormStartPosition.CenterScreen;
                chooser.ShowDialog();

                if (chooser.Choice == null)
                {
                    MessageBox.Show("No character chosen.", "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                _gearSet.Customize = chooser.Choice;

                Process ffxivProcess = null;

                try
                {
                    var procList = Process.GetProcesses();

                    foreach (var process in procList)
                    {
                        if (process.ProcessName == "ffxiv" || process.ProcessName == "ffxiv_dx11")
                            ffxivProcess = process;
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("An error occurred reading the process list.\n\n" + exc, "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                if (ffxivProcess == null)
                {
                    MessageBox.Show("FFXIV is not running.", "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                _memory = new Mem();
                if (!_memory.OpenProcess(ffxivProcess.ProcessName))
                {
                    MessageBox.Show("An error occurred opening the FFXIV process.", "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                var scanForm = new ScanProgressForm(_memory, Util.ByteArrayToString(chooser.Choice));
                scanForm.Show();
                scanForm.Worker.RunWorkerCompleted += ScanWorker_RunWorkerCompleted;
                scanForm.Run();


#if !DEBUG
                SynchronizationContext.Current.Post((obj) => this.Visible = false, null);
#endif

                _worker.DoWork += WorkerOnDoWork;
                _worker.WorkerSupportsCancellation = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred during initialization.\n\n" + exc, "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                Environment.Exit(0);
#endif
            }

        }

        private void ScanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _customizeOffset = (IntPtr)e.Result;
            offsetLabel.Text = "CustomizeOffset: " + _customizeOffset.ToString("X");

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

            _gearSet.HeadGear = ReadGearTuple(_customizeOffset + GEAR_HEAD_OFF);
            _gearSet.BodyGear = ReadGearTuple(_customizeOffset + GEAR_BODY_OFF);
            _gearSet.HandsGear = ReadGearTuple(_customizeOffset + GEAR_HANDS_OFF);
            _gearSet.LegsGear = ReadGearTuple(_customizeOffset + GEAR_LEGS_OFF);
            _gearSet.FeetGear = ReadGearTuple(_customizeOffset + GEAR_FEET_OFF);
            _gearSet.EarGear = ReadGearTuple(_customizeOffset + GEAR_EAR_OFF);
            _gearSet.NeckGear = ReadGearTuple(_customizeOffset + GEAR_NECK_OFF);
            _gearSet.WristGear = ReadGearTuple(_customizeOffset + GEAR_WRIST_OFF);
            _gearSet.RRingGear = ReadGearTuple(_customizeOffset + GEAR_RRING_OFF);
            _gearSet.LRingGear = ReadGearTuple(_customizeOffset + GEAR_LRING_OFF);

            _gearSet.MainWep = ReadWepTuple(_customizeOffset + WEP_MAINH_OFF);
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

            mainWepTextBox.Text = WepTupleToComma(_gearSet.MainWep);
            offWepTextBox.Text = WepTupleToComma(_gearSet.OffWep);
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

            mainWepTextBox.Text = WepTupleToComma(_cGearSet.MainWep);
            offWepTextBox.Text = WepTupleToComma(_cGearSet.OffWep);
        }

        private WepTuple ReadWepTuple(IntPtr offset)
        {
            var bytes = _memory.readBytes(offset.ToString("X"), 8);

            return new WepTuple(BitConverter.ToInt16(bytes, 0), BitConverter.ToInt16(bytes, 2), BitConverter.ToInt16(bytes, 4), BitConverter.ToInt16(bytes, 6));
        }

        public static string WepTupleToComma(WepTuple tuple)
        {
            return $"{tuple.Item1},{tuple.Item2},{tuple.Item3},{tuple.Item4}";
        }

        public static WepTuple CommaToWepTuple(string input)
        {
            var parts = input.Split(',');
            return new WepTuple(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
        }

        public static byte[] WepTupleToByteAry(WepTuple tuple)
        {
            byte[] bytes = new byte[8];

            BitConverter.GetBytes((Int16)tuple.Item1).CopyTo(bytes, 0);
            BitConverter.GetBytes((Int16)tuple.Item2).CopyTo(bytes, 2);
            BitConverter.GetBytes((Int16)tuple.Item3).CopyTo(bytes, 4);
            BitConverter.GetBytes((Int16)tuple.Item4).CopyTo(bytes, 6);

            return bytes;
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

        private void restoreOriginalLookButton_Click(object sender, EventArgs e)
        {
            _memory.writeBytes(_customizeOffset, _gearSet.Customize);
            RestoreDefaultGear();
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
                MessageBox.Show("One or more fields were not formatted correctly.\n\n" + exc, "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void RestoreDefaultGear()
        {
            _memory.writeBytes(_customizeOffset + GEAR_HEAD_OFF, GearTupleToByteAry(_gearSet.HeadGear));
            _memory.writeBytes(_customizeOffset + GEAR_BODY_OFF, GearTupleToByteAry(_gearSet.BodyGear));
            _memory.writeBytes(_customizeOffset + GEAR_HANDS_OFF, GearTupleToByteAry(_gearSet.HandsGear));
            _memory.writeBytes(_customizeOffset + GEAR_LEGS_OFF, GearTupleToByteAry(_gearSet.LegsGear));
            _memory.writeBytes(_customizeOffset + GEAR_FEET_OFF, GearTupleToByteAry(_gearSet.FeetGear));
            _memory.writeBytes(_customizeOffset + GEAR_EAR_OFF, GearTupleToByteAry(_gearSet.EarGear));
            _memory.writeBytes(_customizeOffset + GEAR_NECK_OFF, GearTupleToByteAry(_gearSet.NeckGear));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_gearSet.WristGear));
            _memory.writeBytes(_customizeOffset + GEAR_RRING_OFF, GearTupleToByteAry(_gearSet.RRingGear));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_gearSet.LRingGear));

            _memory.writeBytes(_customizeOffset + WEP_MAINH_OFF, WepTupleToByteAry(_gearSet.MainWep));
            _memory.writeBytes(_customizeOffset + WEP_OFFH_OFF, WepTupleToByteAry(_gearSet.OffWep));
        }

        private void WriteCurrentGearTuples()
        {
            if (_cGearSet.HeadGear == null)
                return;

            _memory.writeBytes(_customizeOffset + GEAR_HEAD_OFF, GearTupleToByteAry(_cGearSet.HeadGear));
            _memory.writeBytes(_customizeOffset + GEAR_BODY_OFF, GearTupleToByteAry(_cGearSet.BodyGear));
            _memory.writeBytes(_customizeOffset + GEAR_HANDS_OFF, GearTupleToByteAry(_cGearSet.HandsGear));
            _memory.writeBytes(_customizeOffset + GEAR_LEGS_OFF, GearTupleToByteAry(_cGearSet.LegsGear));
            _memory.writeBytes(_customizeOffset + GEAR_FEET_OFF, GearTupleToByteAry(_cGearSet.FeetGear));
            _memory.writeBytes(_customizeOffset + GEAR_EAR_OFF, GearTupleToByteAry(_cGearSet.EarGear));
            _memory.writeBytes(_customizeOffset + GEAR_NECK_OFF, GearTupleToByteAry(_cGearSet.NeckGear));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_cGearSet.WristGear));
            _memory.writeBytes(_customizeOffset + GEAR_RRING_OFF, GearTupleToByteAry(_cGearSet.RRingGear));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_cGearSet.LRingGear));

            _memory.writeBytes(_customizeOffset + WEP_MAINH_OFF, WepTupleToByteAry(_cGearSet.MainWep));
            _memory.writeBytes(_customizeOffset + WEP_OFFH_OFF, WepTupleToByteAry(_cGearSet.OffWep));
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

                _cGearSet.MainWep = CommaToWepTuple(mainWepTextBox.Text);
                _cGearSet.OffWep = CommaToWepTuple(offWepTextBox.Text);

                WriteCurrentGearTuples();
            }
            catch (Exception exc)
            {
                MessageBox.Show("One or more fields were not formatted correctly.\n\n" + exc, "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void CheckItemList()
        {
            if(_exdProvider.Items == null)
                _exdProvider.MakeItemList();
        }

        private void CheckResidentList()
        {
            if (_exdProvider.Residents == null)
                _exdProvider.MakeResidentList();
        }

        private void openItemsHeadButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Head).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                headGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsBodyButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Body).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                bodyGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsHandsButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Hands).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                handsGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsLegsButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Legs).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                legsGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsFeetButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Feet).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                feetGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsEarsButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ears).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                earGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsNeckButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Neck).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                neckGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsWristsButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Wrists).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                wristGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsRRingButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ring).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                rRingGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsLRingButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ring).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                lRingGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsMHButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Wep && !c.ModelMain.Contains("0,0,0,0")).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                mainWepTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsOHButton_Click(object sender, EventArgs e)
        {
            CheckItemList();
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

                    File.WriteAllText(fileDialog.FileName, JsonConvert.SerializeObject(_cGearSet));
                    MessageBox.Show($"Capture saved to {fileDialog.FileName}.", "FFXIVMon Reborn", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Could not save gearset.\n\n" + exc, "Error " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                _cGearSet = JsonConvert.DeserializeObject<GearSet>(File.ReadAllText(openFileDialog.FileName));

                // Backwards compatibility
                if(_cGearSet.OffWep == null)
                    _cGearSet.OffWep = new WepTuple(0,0,0,0);

                FillCustoms();
                WriteCurrentGearTuples();
                WriteCurrentCustomize();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            CheckResidentList();
            ResidentSelectForm f = new ResidentSelectForm(_exdProvider.Residents.Values.Where(c => c.IsGoodNpc()).ToArray());
            f.ShowDialog();

            if (f.Choice == null)
                return;

            var gs = f.Choice.Gear;

            if (noNpcCustomizeToolStripMenuItem.Checked)
            {
                gs.Customize = _cGearSet.Customize;
            }

            _cGearSet = gs;

            FillCustoms();
            WriteCurrentGearTuples();
            WriteCurrentCustomize();
        }
    }
}
