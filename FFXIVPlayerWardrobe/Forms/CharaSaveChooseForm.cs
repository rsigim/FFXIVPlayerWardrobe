using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIVPlayerWardrobe
{
    public partial class CharaSaveChooseForm : Form
    {
        private List<SaveDat> _dats;

        public byte[] Choice = null;

        public CharaSaveChooseForm(string text)
        {
            InitializeComponent();

            _dats = MakeSaveDatList.Make();

            foreach (var saveDat in _dats)
            {
                var gender = saveDat.CustomizeBytes[1] == 1 ? "♀️" : "♂️";
                charaSaveListBox.Items.Add(
                    $"{ByteToRaceDict[saveDat.CustomizeBytes[0]]} {gender} - {saveDat.Description}");
            }

            infoLabel.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (charaSaveListBox.SelectedIndex == -1)
            {
                Close();
                return;
            }

            Choice = _dats[charaSaveListBox.SelectedIndex].CustomizeBytes;
            Close();
        }

        private readonly Dictionary<byte, string> ByteToRaceDict = new Dictionary<byte, string>
        {
            {0, "Unknown"},
            {1, "Hyur"},
            {2, "Elezen"},
            {3, "Lalafell" },
            {4, "Miqo'te" },
            {5, "Roegadyn"},
            {6, "Au Ra"}
        };

        private void copyCustomizeToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (charaSaveListBox.SelectedIndex != -1)
                Clipboard.SetText(Util.ByteArrayToString(_dats[charaSaveListBox.SelectedIndex].CustomizeBytes));
        }
    }
}
