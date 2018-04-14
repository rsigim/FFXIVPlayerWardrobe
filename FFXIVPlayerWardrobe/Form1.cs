using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFXIVMonReborn;
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

        private const int WEP_MAIN_OFF = -0x2F0;

        public Form1()
        {
            InitializeComponent();
        }

        private Mem _memory;
        private BackgroundWorker _worker = new BackgroundWorker();
        private ExdCsvReader _exdProvider = new ExdCsvReader();

        private IntPtr _customizeOffset = IntPtr.Zero;

        private byte[] _originalCustomize = null;

        private byte[] _currentCustomize = null;

        private GearTuple _headGear = null;
        private GearTuple _bodyGear = null;
        private GearTuple _handsGear = null;
        private GearTuple _legsGear = null;
        private GearTuple _feetGear = null;
        private GearTuple _earGear = null;
        private GearTuple _neckGear = null;
        private GearTuple _wristGear = null;
        private GearTuple _rRingGear = null;
        private GearTuple _lRingGear = null;

        private GearTuple _headGearC = null;
        private GearTuple _bodyGearC = null;
        private GearTuple _handsGearC = null;
        private GearTuple _legsGearC = null;
        private GearTuple _feetGearC = null;
        private GearTuple _earGearC = null;
        private GearTuple _neckGearC = null;
        private GearTuple _wristGearC = null;
        private GearTuple _rRingGearC = null;
        private GearTuple _lRingGearC = null;

        private WepTuple _mainWep = null;

        private WepTuple _mainWepC = null;

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
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
                    Environment.Exit(0);

                _originalCustomize = chooser.Choice;

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
                    MessageBox.Show("An error occurred reading the process list.\n\n" + exc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                if (ffxivProcess == null)
                {
                    MessageBox.Show("FFXIV is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                    Environment.Exit(0);
#endif
                }

                _memory = new Mem();
                if (!_memory.OpenProcess(ffxivProcess.ProcessName))
                {
                    MessageBox.Show("An error occurred opening the FFXIV process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred during initialization.\n\n" + exc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if !DEBUG
                Environment.Exit(0);
#endif
            }

        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var exc = unhandledExceptionEventArgs.ExceptionObject as Exception;
#if DEBUG
            throw unhandledExceptionEventArgs.ExceptionObject as Exception;
#else
            MessageBox.Show("An error occured. This may be caused by malformatted inputs.\n\n" + exc, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
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
            _originalCustomize = _memory.readBytes(_customizeOffset.ToString("X"), 26);

            _headGear = ReadGearTuple(_customizeOffset + GEAR_HEAD_OFF);
            _bodyGear = ReadGearTuple(_customizeOffset + GEAR_BODY_OFF);
            _handsGear = ReadGearTuple(_customizeOffset + GEAR_HANDS_OFF);
            _legsGear = ReadGearTuple(_customizeOffset + GEAR_LEGS_OFF);
            _feetGear = ReadGearTuple(_customizeOffset + GEAR_FEET_OFF);
            _earGear = ReadGearTuple(_customizeOffset + GEAR_EAR_OFF);
            _neckGear = ReadGearTuple(_customizeOffset + GEAR_NECK_OFF);
            _wristGear = ReadGearTuple(_customizeOffset + GEAR_WRIST_OFF);
            _rRingGear = ReadGearTuple(_customizeOffset + GEAR_RRING_OFF);
            _lRingGear = ReadGearTuple(_customizeOffset + GEAR_LRING_OFF);

            _mainWep = ReadWepTuple(_customizeOffset + WEP_MAIN_OFF);
        }

        private void FillDefaults()
        {
            customizeTextBox.Text = Util.ByteArrayToString(_originalCustomize);

            headGearTextBox.Text = GearTupleToComma(_headGear);
            bodyGearTextBox.Text = GearTupleToComma(_bodyGear);
            handsGearTextBox.Text = GearTupleToComma(_handsGear);
            legsGearTextBox.Text = GearTupleToComma(_legsGear);
            feetGearTextBox.Text = GearTupleToComma(_feetGear);
            earGearTextBox.Text = GearTupleToComma(_earGear);
            neckGearTextBox.Text = GearTupleToComma(_neckGear);
            wristGearTextBox.Text = GearTupleToComma(_wristGear);
            rRingGearTextBox.Text = GearTupleToComma(_rRingGear);
            lRingGearTextBox.Text = GearTupleToComma(_lRingGear);

            mainWepTextBox.Text = WepTupleToComma(_mainWep);
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
            _memory.writeBytes(_customizeOffset, _originalCustomize);
            RestoreDefaultGear();
            FillDefaults();
        }

        private void customizeApplyButton_Click(object sender, EventArgs e)
        {
            _currentCustomize = Util.StringToByteArray(customizeTextBox.Text.Replace(" ", string.Empty));
            WriteCurrentCustomize();
        }

        private void WriteCurrentCustomize()
        {
            if (_currentCustomize == null)
                return;

            _memory.writeBytes(_customizeOffset, _currentCustomize);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "FFXIVPlayerWardrobe - amibu/goaaats\n\nUsing Memory.dll by erfg(https://github.com/erfg12/memory.dll)");
        }

        private void RestoreDefaultGear()
        {
            _memory.writeBytes(_customizeOffset + GEAR_HEAD_OFF, GearTupleToByteAry(_headGear));
            _memory.writeBytes(_customizeOffset + GEAR_BODY_OFF, GearTupleToByteAry(_bodyGear));
            _memory.writeBytes(_customizeOffset + GEAR_HANDS_OFF, GearTupleToByteAry(_handsGear));
            _memory.writeBytes(_customizeOffset + GEAR_LEGS_OFF, GearTupleToByteAry(_legsGear));
            _memory.writeBytes(_customizeOffset + GEAR_FEET_OFF, GearTupleToByteAry(_feetGear));
            _memory.writeBytes(_customizeOffset + GEAR_EAR_OFF, GearTupleToByteAry(_earGear));
            _memory.writeBytes(_customizeOffset + GEAR_NECK_OFF, GearTupleToByteAry(_neckGear));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_wristGear));
            _memory.writeBytes(_customizeOffset + GEAR_RRING_OFF, GearTupleToByteAry(_rRingGear));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_lRingGear));

            _memory.writeBytes(_customizeOffset + WEP_MAIN_OFF, WepTupleToByteAry(_mainWep));
        }

        private void WriteCurrentGearTuples()
        {
            if (_headGearC == null)
                return;

            _memory.writeBytes(_customizeOffset + GEAR_HEAD_OFF, GearTupleToByteAry(_headGearC));
            _memory.writeBytes(_customizeOffset + GEAR_BODY_OFF, GearTupleToByteAry(_bodyGearC));
            _memory.writeBytes(_customizeOffset + GEAR_HANDS_OFF, GearTupleToByteAry(_handsGearC));
            _memory.writeBytes(_customizeOffset + GEAR_LEGS_OFF, GearTupleToByteAry(_legsGearC));
            _memory.writeBytes(_customizeOffset + GEAR_FEET_OFF, GearTupleToByteAry(_feetGearC));
            _memory.writeBytes(_customizeOffset + GEAR_EAR_OFF, GearTupleToByteAry(_earGearC));
            _memory.writeBytes(_customizeOffset + GEAR_NECK_OFF, GearTupleToByteAry(_neckGearC));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_wristGearC));
            _memory.writeBytes(_customizeOffset + GEAR_RRING_OFF, GearTupleToByteAry(_rRingGearC));
            _memory.writeBytes(_customizeOffset + GEAR_WRIST_OFF, GearTupleToByteAry(_lRingGearC));

            _memory.writeBytes(_customizeOffset + WEP_MAIN_OFF, WepTupleToByteAry(_mainWepC));
        }

        private void WriteGear_Click(object sender, EventArgs e)
        {
            _headGearC = CommaToGearTuple(headGearTextBox.Text);
            _bodyGearC = CommaToGearTuple(bodyGearTextBox.Text);
            _handsGearC = CommaToGearTuple(handsGearTextBox.Text);
            _legsGearC = CommaToGearTuple(legsGearTextBox.Text);
            _feetGearC = CommaToGearTuple(feetGearTextBox.Text);
            _earGearC = CommaToGearTuple(earGearTextBox.Text);
            _neckGearC = CommaToGearTuple(neckGearTextBox.Text);
            _wristGearC = CommaToGearTuple(wristGearTextBox.Text);
            _rRingGearC = CommaToGearTuple(rRingGearTextBox.Text);
            _lRingGearC = CommaToGearTuple(lRingGearTextBox.Text);

            _mainWepC = CommaToWepTuple(mainWepTextBox.Text);

            WriteCurrentGearTuples();
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

        private void openItemsHeadButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Head).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                headGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsBodyButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Body).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                bodyGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsHandsButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Hands).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                handsGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsLegsButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Legs).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                legsGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsFeetButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Feet).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                feetGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsEarsButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ears).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                earGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsNeckButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Neck).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                neckGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsWristsButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Wrists).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                wristGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsRRingButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ring).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                rRingGearTextBox.Text = p.Choice.ModelMain;
        }

        private void openItemsLRingButton_Click(object sender, EventArgs e)
        {
            ItemPicker p = new ItemPicker(_exdProvider.Items.Values.Where(c => c.Type == ExdCsvReader.ItemType.Ring).ToArray());
            p.ShowDialog();

            if (p.Choice != null)
                lRingGearTextBox.Text = p.Choice.ModelMain;
        }

        private void customizeChooserButton_Click(object sender, EventArgs e)
        {
            CharaSaveChooseForm chooser = new CharaSaveChooseForm("Select the saved character you want to load.");
            chooser.ShowDialog();

            if (chooser.Choice != null)
                customizeTextBox.Text = Util.ByteArrayToString(chooser.Choice);
        }
    }
}
