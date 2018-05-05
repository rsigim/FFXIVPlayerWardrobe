using System;
using System.Linq;
using System.Windows.Forms;

namespace FFXIVPlayerWardrobe
{
    public partial class ItemPicker : Form
    {
        private ExdCsvReader.Item[] _items;

        public ExdCsvReader.Item Choice = null;

        public ItemPicker(ExdCsvReader.Item[] items)
        {
            InitializeComponent();

            searchTextBox.SetWatermark("Search...");

            listBox1.Items.AddRange(items);

            _items = items;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            Choice = listBox1.SelectedItem as ExdCsvReader.Item;
            Close();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string filter = searchTextBox.Text.ToLower();
            listBox1.Items.Clear();
            foreach (ExdCsvReader.Item game in _items.Where(g => g.Name.ToLower().Contains(filter)))
            {
                listBox1.Items.Add(game);
            }
        }
    }
}
