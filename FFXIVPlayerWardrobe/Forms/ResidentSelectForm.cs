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
    public partial class ResidentSelectForm : Form
    {
        private ExdCsvReader.Resident[] _residents;
        public ExdCsvReader.Resident Choice = null;

        public ResidentSelectForm(ExdCsvReader.Resident[] residents)
        {
            InitializeComponent();
            textBox1.SetWatermark("Search...");

            DataTable table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Name");
            table.Columns.Add("Gear");

            foreach (var resident in residents)
            {
                var r = table.NewRow();
                r["ID"] = resident.Index;
                r["Name"] = resident.Name;
                r["Gear"] = resident.MakeGearString();
                table.Rows.Add(r);
            }

            residentGridView.DataSource = table;

            residentGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            residentGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            residentGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            _residents = residents;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if(residentGridView.SelectedCells.Count == 0)
                Close();

            Choice = _residents[residentGridView.CurrentCell.RowIndex];
            Close();
        }

        private void searchNextButton_Click(object sender, EventArgs e)
        {
            string term = textBox1.Text.ToLower();
            int start = 0;

            if (residentGridView.SelectedCells.Count != 0)
                start = residentGridView.SelectedCells[0].RowIndex + 1;

            for (int row = start; row < residentGridView.RowCount; row++)
            {
                if (residentGridView.Rows[row].Cells["Name"].Value.ToString().ToLower().Contains(term))
                {
                    residentGridView.ClearSelection();
                    residentGridView.CurrentCell = residentGridView.Rows[row].Cells[0];
                    residentGridView.FirstDisplayedScrollingRowIndex = row;
                    break;
                }
            }
        }

        private void copyCustomizeToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (residentGridView.SelectedCells.Count == 0)
                return;

            Clipboard.SetText(Util.ByteArrayToString(_residents[residentGridView.CurrentCell.RowIndex].Gear.Customize));
        }
    }
}
