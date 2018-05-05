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

namespace FFXIVPlayerWardrobe
{
    public partial class ScanProgressForm : Form
    {
        public BackgroundWorker Worker = new BackgroundWorker();

        private Mem _memory;
        private string _customize;

        public ScanProgressForm(Mem memory, string customize)
        {
            InitializeComponent();

            this.ControlBox = false;

            _memory = memory;
            _customize = customize;

            Worker.DoWork += WorkerOnDoWork;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        public void Run()
        {
            Worker.RunWorkerAsync();
        }

        private async void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            doWorkEventArgs.Result = await FindCustomizeOffset.Find(_memory, _customize);
        }
    }
}
