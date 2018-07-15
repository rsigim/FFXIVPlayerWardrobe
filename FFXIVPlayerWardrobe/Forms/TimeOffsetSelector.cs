using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FFXIVPlayerWardrobe.Memory;

namespace FFXIVPlayerWardrobe.Forms
{
    public partial class TimeOffsetSelector : Form
    {
        private MemoryManager _memoryMan;

        public TimeOffsetSelector(MemoryManager memoryMan)
        {
            InitializeComponent();

            _memoryMan = memoryMan;

            timeUpDown.Value = ((decimal) _memoryMan.GetTimeOffset() / (decimal) 5000);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timeUpDown_ValueChanged(object sender, EventArgs e)
        {
            _memoryMan.SetTimeOffset((int) (timeUpDown.Value * 5000));
        }
    }
}
