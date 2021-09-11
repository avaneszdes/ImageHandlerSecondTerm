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
        List<int>? listOfDifferentNumbers;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //return array with values that contain 
        private async Task<int[,]> GetPByAngleAsync(int angle, Bitmap bitmap)
        {
            var bitmapWidth = bitmap.Width;
            var bitmapHeight = bitmap.Height;
            var listOfDifferentNumbersCount = listOfDifferentNumbers!.Count;

            int[,] arrForP = new int[bitmapWidth, bitmapHeight];

            for (int i = 0; i < bitmapWidth; i++)
            {
                for (int j = 0; j < bitmapHeight; j++)
                {
                    arrForP[i, j] = int.Parse(dataGridView1[i, j].Value.ToString()!);
                }
            }

            int[,] newArr = new int[listOfDifferentNumbersCount, bitmapHeight];

            var ii = 0;
            var jj = 0;
            var count = 0;

            await Task.Factory.StartNew(() =>
            {
                foreach (var item in listOfDifferentNumbers)
                {
                    for (int step = 1; step < bitmapWidth; step++)
                    {
                        for (int row = 0; row < bitmapWidth; row++)
                        {
                            for (int col = 0; col < bitmapHeight; col++)
                            {
                                if (angle == 0 && col + step < bitmapWidth && arrForP[row, col] == item && arrForP[row, col + step] == item)
                                {
                                    count++;
                                }
                                else if (angle == 135 && col + step < bitmapHeight &&
                                        row + step < bitmapWidth &&
                                        arrForP[row, col] == item &&
                                        arrForP[row + step, col + step] == item)
                                {
                                    count++;
                                }

                            }
                        }

                        newArr[ii, jj++] = count;
                        if (jj == bitmapHeight - 1)
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
            pictureBox1.Width = 150;
            pictureBox1.Height = 150;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Width = 50;
            pictureBox1.Height = 50;
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
        }

        private string? IsOpenFileDialogOk()
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

            return openFileDialog1.ShowDialog() == DialogResult.OK ? openFileDialog1.FileName : null;
        }


        private async Task RenderDataGridView(PictureBox picture, string fileName)
        {
            dataGridView1.Rows.Clear();
            listOfDifferentNumbers = new List<int>();
            var bitmap = new Bitmap(fileName);
            if (!PictureValidate(bitmap))
            {
                return;
            }
            picture.Image = bitmap;
            var polBitmap = new Bitmap(bitmap);
            var bitmapWidth = bitmap.Width;
            var bitmapHeight = bitmap.Height;

            for (int i = 0; i < bitmapWidth; i++)
            {
                for (int j = 0; j < bitmapHeight; j++)
                {
                    var color = (polBitmap.GetPixel(i, j).R + polBitmap.GetPixel(i, j).G + polBitmap.GetPixel(i, j).B) / 3;
                    polBitmap.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            }


            picture.Image = polBitmap;
            for (int i = 0; i < bitmapWidth; i++)
            {
                dataGridView1.Columns.Add(i + 1 + "", i + 1 + "");
            }

            dataGridView1.Rows.Add(bitmapHeight);

            for (int i = 0; i < bitmapWidth; i++)
            {
                DataGridViewRow row2 = dataGridView1.Rows[i];
                dataGridView1.Columns[i].Width = 50;
            }

            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < bitmapWidth; i++)
                {
                    for (int j = 0; j < bitmapHeight; j++)
                    {
                        dataGridView1[i, j].Value = (bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).G + bitmap.GetPixel(i, j).B) / 3;
                    }
                }

            });


            //add different numbers to list
            for (int i = 0; i < bitmapWidth; i++)
            {
                for (int j = 0; j < bitmapHeight; j++)
                {
                    var number = int.Parse(dataGridView1[j, i].Value.ToString()!);
                    if (!listOfDifferentNumbers.Contains(number))
                    {
                        listOfDifferentNumbers.Add(number);
                    }
                }
            }

            var countOfDiffElements = listOfDifferentNumbers.Count;

            for (int i = 0; i < bitmapWidth; i++)
            {
                if (i == 0)
                {
                    dataGridView2.Columns.Add(i + 1 + "", "");
                }
                dataGridView2.Columns.Add(i + 1 + "", i + 1 + "");

            }

            dataGridView2.Rows.Add(countOfDiffElements);

            for (int i = 0; i < bitmapWidth; i++)
            {
                dataGridView2.Columns[i].Width = 50;
            }


            //add to dataGridView2 different numbers to first column
            for (int i = 0; i < countOfDiffElements; i++)
            {
                dataGridView2[0, i].Value = listOfDifferentNumbers[i];
            }

            var getPResult = await GetPByAngleAsync(int.Parse(textBox1.Text), bitmap);

            for (int i = 0; i < countOfDiffElements; i++)
            {
                for (int j = 1; j < bitmapWidth; j++)
                {
                    dataGridView2[j, i].Value = getPResult[i, j - 1];
                }
            }
        }

        private bool PictureValidate(Bitmap bitmap)
        {
            if (bitmap.Width != 50 || bitmap.Height != 50)
            {
                MessageBox.Show("Incorrect size image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private async void OpenImage1_Click(object sender, EventArgs e)
        {
            var isOpen = IsOpenFileDialogOk();
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridView(pictureBox1, isOpen);
            }
        }

        private async void OpenImage2_Click(object sender, EventArgs e)
        {
            var isOpen = IsOpenFileDialogOk();
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridView(pictureBox2, isOpen);
            }
        }

        private async void OpenImage3_Click(object sender, EventArgs e)
        {
            var isOpen = IsOpenFileDialogOk();
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridView(pictureBox3, isOpen);
            }
        }

        private async void OpenImage4_Click(object sender, EventArgs e)
        {
            var isOpen = IsOpenFileDialogOk();
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridView(pictureBox4, isOpen);
            }
        }

    }
}
