using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MayinTarlasi
{
    public partial class Form2 : Form
    {
        public int MineCount { get; private set; } // Property to hold the mine count

        private const int MaxMines = 900; // Maximum mines allowed (30x30 grid)

        public Form2()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Mayın Sayısı Seçimi";
            this.ClientSize = new Size(300, 150);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Label
            Label label = new Label
            {
                Text = $"Lütfen Mayın Sayısını Girin (1 - {MaxMines}):",
                Location = new Point(20, 20),
                Size = new Size(250, 20)
            };
            this.Controls.Add(label);

            // TextBox
            TextBox textBox = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(250, 20),
                Name = "MineInputBox"
            };
            this.Controls.Add(textBox);

            // Button
            Button button = new Button
            {
                Text = "Başlat",
                Location = new Point(20, 90),
                Size = new Size(250, 30)
            };
            button.Click += (sender, e) => StartGame(textBox);
            this.Controls.Add(button);
        }

        private void StartGame(TextBox textBox)
        {
            if (int.TryParse(textBox.Text, out int mineCount) && mineCount > 0 && mineCount <= MaxMines)
            {
                MineCount = mineCount; // Store the mine count
                this.DialogResult = DialogResult.OK; // Close the form with OK result
                this.Close();
            }
            else
            {
                MessageBox.Show($"Lütfen 1 ile {MaxMines} arasında geçerli bir sayı girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
