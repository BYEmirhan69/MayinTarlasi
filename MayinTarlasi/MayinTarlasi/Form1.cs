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
    public partial class Form1 : Form
    {
        private const int GridSize = 30; // 30x30 boyutunda bir ızgara
        private int mineCount; // Toplam mayın sayısı
        private Button[,] buttons; // 2D dizi, ızgara üzerindeki butonları saklar
        private bool[,] mines; // 2D dizi, mayınların yerlerini saklar
        private bool[,] revealed; // Açılmış hücrelerin durumunu takip eder
        private int cellsRevealed; // Açılan hücrelerin sayısını takip eder

        public Form1(int mineCount)
        {
            this.mineCount = mineCount;
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Text = "Mayın Tarlası";
            this.ClientSize = new Size(800, 850);
            this.StartPosition = FormStartPosition.CenterScreen;

            // "Reset" butonu oluştur
            Button resetButton = new Button
            {
                Text = "Reset",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(100, 40),
                Location = new Point((this.ClientSize.Width - 100) / 2, 10)
            };
            resetButton.Click += ResetButton_Click;
            this.Controls.Add(resetButton);

            // Izgarayı başlat
            buttons = new Button[GridSize, GridSize];
            mines = new bool[GridSize, GridSize];
            revealed = new bool[GridSize, GridSize];
            cellsRevealed = 0;

            int buttonSize = 25;
            int startY = resetButton.Bottom + 10;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(j * buttonSize, startY + i * buttonSize),
                        BackColor = Color.LightGray,
                        Tag = new Point(i, j)
                    };

                    buttons[i, j].Click += Button_Click;
                    this.Controls.Add(buttons[i, j]);
                }
            }

            PlaceMines(); // Mayınları yerleştir
        }

        private void PlaceMines()
        {
            Random random = new Random();
            int minesPlaced = 0;

            // Belirtilen sayıda mayını rastgele yerleştir
            while (minesPlaced < mineCount)
            {
                int row = random.Next(GridSize);
                int col = random.Next(GridSize);

                if (!mines[row, col]) // Aynı yere birden fazla mayın koymaktan kaçın
                {
                    mines[row, col] = true;
                    minesPlaced++;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Point location = (Point)clickedButton.Tag;
            int row = location.X;
            int col = location.Y;

            if (mines[row, col]) // Mayına tıklandıysa
            {
                clickedButton.BackColor = Color.Red;
                clickedButton.Text = "X";
                RevealMines(); // Tüm mayınları göster
                MessageBox.Show("Game Over! You hit a mine.", "Game Over");
                ReturnToForm2();
            }
            else
            {
                RevealSafeCells(row, col); // Güvenli hücreleri aç
                if (cellsRevealed == (GridSize * GridSize - mineCount)) // Tüm güvenli hücreler açıldıysa
                {
                    MessageBox.Show("Congratulations! You win.", "You Win");
                    ReturnToForm2();
                }
            }
        }

        private void RevealMines()
        {
            // Tüm mayınları aç ve göster
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (mines[i, j])
                    {
                        buttons[i, j].BackColor = Color.Red;
                        buttons[i, j].Text = "X";
                    }
                }
            }
        }

        private void RevealSafeCells(int row, int col)
        {
            // Geçersiz veya zaten açılmış bir hücreyi kontrol etme
            if (row < 0 || col < 0 || row >= GridSize || col >= GridSize || revealed[row, col]) return;

            revealed[row, col] = true; // Hücreyi açılmış olarak işaretle
            cellsRevealed++;

            int adjacentMines = CountAdjacentMines(row, col); // Komşu mayın sayısını bul
            buttons[row, col].BackColor = Color.White;
            buttons[row, col].Text = adjacentMines > 0 ? adjacentMines.ToString() : "";

            if (adjacentMines == 0) // Eğer komşu mayın yoksa komşu hücreleri aç
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        RevealSafeCells(row + i, col + j);
                    }
                }
            }
        }

        private int CountAdjacentMines(int row, int col)
        {
            int count = 0;

            // Komşu hücrelerdeki mayınları say
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newRow = row + i;
                    int newCol = col + j;

                    if (newRow >= 0 && newRow < GridSize && newCol >= 0 && newCol < GridSize && mines[newRow, newCol])
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ReturnToForm2(); // Yeni oyun için form2'ye geri dön
        }

        private void ReturnToForm2()
        {
            this.Hide(); // Geçerli formu gizle
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                this.Close(); // Geçerli formu kapat
                Application.Run(new Form1(form2.MineCount)); // Yeni oyunu başlat
            }
        }
    }
}
