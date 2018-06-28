using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFXIVPlayerWardrobe.AssetReaders;

namespace ColorViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var cmp = new CmpReader(File.ReadAllBytes("human.cmp"));

            for (int i = 0; i < cmp.Colors.Count; i++)
            {
                var item = new ListViewItem(i.ToString());
                item.BackColor = cmp.Colors[i];

                listView1.Items.Add(item);
            }
        }
    }
}
