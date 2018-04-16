using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFXIVMonReborn;

namespace FFXIVPlayerWardrobe
{
    public partial class ResidentSelectForm : Form
    {
        private ExdCsvReader.Resident[] _residents;
        public ExdCsvReader.Resident Choice = null;

        public ResidentSelectForm(ExdCsvReader.Resident[] residents)
        {
            InitializeComponent();

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

            dataGridView1.DataSource = table;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            _residents = residents;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
                Close();

            Choice = _residents[dataGridView1.CurrentCell.RowIndex];
            Close();
        }
    }
}
