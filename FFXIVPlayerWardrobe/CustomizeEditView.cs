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
    public partial class CustomizeEditView : Form
    {
        private Mem _memory;
        private byte[] _customize;
        private string _offsetCustomize;

        public CustomizeEditView(Mem memory, byte[] customize, string offsetCustomize)
        {   
            InitializeComponent();

            _memory = memory;
            _customize = customize;
            _offsetCustomize = offsetCustomize;
        }

        private void CustomizeEditView_Load(object sender, EventArgs e)
        {

        }
    }
}
