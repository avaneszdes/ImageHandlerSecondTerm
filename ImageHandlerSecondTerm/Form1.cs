using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHandlerSecondTerm
{
    public partial class Form1 : Form
    {
        Bitmap firstBitmap;
        List<int> listOfDifferentNumbers;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "bmp",
                Filter = "bmp files (*.bmp)|*.bmp",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                dataGridView1.Rows.Clear();
                var file = openFileDialog1.FileName;
                listOfDifferentNumbers = new List<int>();


                firstBitmap = new Bitmap(file);
                pictureBox1.Image = firstBitmap;

                var polBitmap = new Bitmap(firstBitmap);

                for (int i = 0; i < firstBitmap.Width; i++)
                {
                    for (int j = 0; j < firstBitmap.Height; j++)
                    {
                        var color = (polBitmap.GetPixel(i, j).R + polBitmap.GetPixel(i, j).G + polBitmap.GetPixel(i, j).B) / 3;
                        polBitmap.SetPixel(i, j, Color.FromArgb(color, color, color));
                    }
                }


                pictureBox1.Image = polBitmap;
                for (int i = 0; i < firstBitmap.Width; i++)
                {
                    dataGridView1.Columns.Add(i + 1 + "", i + 1 + "");

                }

                dataGridView1.Rows.Add(firstBitmap.Width);

                for (int i = 0; i < firstBitmap.Width; i++)
                {
                    DataGridViewRow row2 = dataGridView1.Rows[i];
                    dataGridView1.Columns[i].Width = 50;
                }

                await Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < firstBitmap.Width; i++)
                    {
                        for (int j = 0; j < firstBitmap.Width; j++)
                        {
                            dataGridView1[i, j].Value = (firstBitmap.GetPixel(i, j).R + firstBitmap.GetPixel(i, j).G + firstBitmap.GetPixel(i, j).B) / 3;
                        }
                    }

                });


                for (int i = 0; i < firstBitmap.Width; i++)
                {
                    for (int j = 0; j < firstBitmap.Width; j++)
                    {
                        var number = int.Parse(dataGridView1[j, i].Value.ToString()!);
                        if (!listOfDifferentNumbers.Contains(number))
                        {
                            listOfDifferentNumbers.Add(number);
                        }
                    }
                }


                //add different numbers to list
                for (int i = 0; i < firstBitmap.Width; i++)
                {
                    for (int j = 0; j < firstBitmap.Width; j++)
                    {
                        var number = int.Parse(dataGridView1[j, i].Value.ToString()!);
                        if (!listOfDifferentNumbers.Contains(number))
                        {
                            listOfDifferentNumbers.Add(number);
                        }
                    }
                }

                for (int i = 0; i < firstBitmap.Width; i++)
                {
                    if (i == 0)
                    {
                        dataGridView2.Columns.Add(i + 1 + "", "");
                    }
                    dataGridView2.Columns.Add(i + 1 + "", i + 1 + "");

                }

                dataGridView2.Rows.Add(listOfDifferentNumbers.Count());

                for (int i = 0; i < firstBitmap.Width; i++)
                {
                    dataGridView2.Columns[i].Width = 50;
                }


                //add to dataGridView2 different numbers to first column
                for (int i = 0; i < listOfDifferentNumbers.Count(); i++)
                {
                    dataGridView2[0, i].Value = listOfDifferentNumbers[i];
                }

                var getPResult = await GetPByAngleAsync(90);

                for (int i = 0; i < 15; i++)
                {
                    for (int j = 1; j < 50; j++)
                    {
                        dataGridView2[j, i].Value = getPResult[i, j - 1];
                    }
                }


            }
        }

        private async Task<int[,]> GetPByAngleAsync(int angle)
        {
            var listOfDifferentNumbersCount = listOfDifferentNumbers.Count();

            int[,] arrForP = new int[firstBitmap.Width, firstBitmap.Height];

            for (int i = 0; i < firstBitmap.Width; i++)
            {
                for (int j = 0; j < firstBitmap.Height; j++)
                {
                    arrForP[i, j] = int.Parse(dataGridView1[i, j].Value.ToString());

                    if (int.Parse(dataGridView1[j, i].Value.ToString()) == 53)
                    {
                        var ddd = 9;
                    }
                }
            }

            int[,] newArr = new int[50, 50];

            var ii = 0;
            var jj = 0;
            var count = 0;

            await Task.Factory.StartNew(() =>
            {
                foreach (var item in listOfDifferentNumbers)
                {
                    for (int step = 1; step < firstBitmap.Width; step++)
                    {
                        for (int row = 0; row < firstBitmap.Height; row++)
                        {
                            for (int col = 0; col < firstBitmap.Width; col++)
                            {
                                if (angle == 90 && col + step < 50 && arrForP[row, col] == item && arrForP[row, col + step] == item)
                                {
                                    count++;
                                }
                            }
                        }

                        newArr[ii, jj] = count;

                        jj++;
                        if (jj == 49)
                        {
                            jj = 0;
                        }
                        count = 0;
                    }

                    ii++;
                }

            });


            return newArr;
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Width = 300;
            pictureBox1.Height = 300;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Width = 50;
            pictureBox1.Height = 50;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
