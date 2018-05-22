using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIVPlayerWardrobe.Forms
{
    public partial class EditCustomizeForm : Form
    {
        private readonly NameWrapper[] _raceVals =
        {
            new NameWrapper("Hyur", 1),
            new NameWrapper("Elezen", 2),
            new NameWrapper("Lalafell", 3),
            new NameWrapper("Miqo'te", 4),
            new NameWrapper("Roegadyn", 5),
            new NameWrapper("Au Ra", 6)
        };

        private readonly NameWrapper[] _tribeVals =
        {
            new NameWrapper("Midlander", 1, 1),
            new NameWrapper("Highlander", 2, 1),
            new NameWrapper("Wildwood", 3, 2),
            new NameWrapper("Duskwight", 4, 2),
            new NameWrapper("Plainsfolk", 5, 3),
            new NameWrapper("Dunesfolk", 6, 3),
            new NameWrapper("Seeker of the Sun", 7, 4),
            new NameWrapper("Keeper of the Moon", 8, 4),
            new NameWrapper("Sea Wolf", 9, 5),
            new NameWrapper("Hellsguard", 10, 5),
            new NameWrapper("Raen", 11, 6),
            new NameWrapper("Xaela", 12, 6)
        };

        private byte[] _customize = null;

        public byte[] EditedCustomize = null;

        private ExdCsvReader _exdProvider;

        public EditCustomizeForm(byte[] customize, ExdCsvReader exdProvider)
        {
            InitializeComponent();

            raceComboBox.DataSource = _raceVals;

            _customize = customize;
            _exdProvider = exdProvider;

            FillDefaults();
        }

        private void FillDefaults()
        {
            raceComboBox.SelectedIndex = _customize[0x0] - 1;
            genderComboBox.SelectedIndex = _customize[0x1];
            heightUpDown.Value = _customize[0x3];
            hairTypeUpDown.Value = _customize[0x6];
            raceFeatureSizeUpDown.Value = _customize[0x15];
            raceFeatureTypeUpDown.Value = _customize[0x16];
            bustSizeUpDown.Value = _customize[0x17];
            tribeComboBox.SelectedIndex = _customize[0x4] % 2 == 0 ? 1 : 0;
            legacyMarkCheckBox.Checked = (_customize[12] & (1 << 7)) != 0;
        }

        private void FillTribeBoxForRace(int race)
        {
            List<NameWrapper> inputs = new List<NameWrapper>();

            foreach (var tribeVal in _tribeVals)
            {
                if((int)tribeVal.Info == race)
                    inputs.Add(tribeVal);
            }

            tribeComboBox.DataSource = inputs.ToArray();
        }

        private void EditCustomizeForm_Load(object sender, EventArgs e)
        {

        }

        private void raceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var race = raceComboBox.SelectedItem as NameWrapper;
            FillTribeBoxForRace(race.Index);
        }

        private void legacyMarkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(legacyMarkCheckBox.Checked)
                _customize[12] = (byte)(_customize[12] | (1 << 7));
            else
                _customize[12] = (byte)(_customize[12] &~ (1 << 7));

            Debug.WriteLine(_customize[12].ToString("X"));
            Debug.WriteLine(Convert.ToString(_customize[12], 2).PadLeft(8, '0'));
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

        private void applyButton_Click(object sender, EventArgs e)
        {
            _customize[0x0] = (byte) (raceComboBox.SelectedIndex + 1);
            _customize[0x1] = (byte) genderComboBox.SelectedIndex;
            _customize[0x3] = (byte) heightUpDown.Value;
            _customize[0x4] = (byte) ((raceComboBox.SelectedIndex + 1) * 2 - 1 + tribeComboBox.SelectedIndex);
            _customize[0x6] = (byte) hairTypeUpDown.Value;
            _customize[0x15] = (byte) raceFeatureSizeUpDown.Value;
            _customize[0x16] = (byte) raceFeatureTypeUpDown.Value;
            _customize[0x17] = (byte) bustSizeUpDown.Value;

            EditedCustomize = _customize;
            this.Close();
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
            _customize = gs.Customize;

            FillDefaults();
        }

        private void selectSavedCharButton_Click(object sender, EventArgs e)
        {
            var c = new CharaSaveChooseForm("Choose a character to load.");
            c.ShowDialog();

            if (c.Choice != null)
            {
                _customize = c.Choice;
                FillDefaults();
            }
        }
    }
}
