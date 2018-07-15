using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFXIVPlayerWardrobe.Forms
{
    public partial class WeatherSelector : Form
    {
        public ExdCsvReader.Weather Choice = null;

        private readonly List<ExdCsvReader.Weather> AllowedWeathers;

        public WeatherSelector(List<ExdCsvReader.Weather> allowedWeathers, int currentWeather)
        {
            InitializeComponent();

            AllowedWeathers = allowedWeathers;

            for(int i = 0; i < allowedWeathers.Count; i++)
            {
                comboBox1.Items.Add(allowedWeathers[i].Name);

                if (allowedWeathers[i].Index == currentWeather)
                    comboBox1.SelectedIndex = i;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Choice = AllowedWeathers[comboBox1.SelectedIndex];
            Close();
        }
    }
}
