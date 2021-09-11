using MethodLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHandlerSecondTerm
{
    public partial class Main : Form
    {
        List<int>? listOfDifferentNumbers;
        public Main()
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
           
            var bitmap = new Bitmap(fileName);
            if (!PictureValidate(bitmap))
            {
                return;
            }
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            
            bitmap = BitmapHandler.GetGreyscaleBitmap(bitmap);
            picture.Image = bitmap;
            var bitmapWidth = bitmap.Width;
            var bitmapHeight = bitmap.Height;


            dataGridView1 = await WinFormsComponentHandler.AddColAndRow(bitmapWidth, bitmapHeight, dataGridView1);
            dataGridView1 = await WinFormsComponentHandler.AddDataToDataGridView(BitmapHandler.GetGreyscaleArray(bitmap), dataGridView1);
            listOfDifferentNumbers = await WinFormsComponentHandler.GetDiffItemsFromDataGrid(dataGridView1 ,bitmapWidth, bitmapHeight);
            var countOfDiffElements = listOfDifferentNumbers.Count;

            dataGridView2 = await WinFormsComponentHandler.AddColAndRow(bitmapWidth, countOfDiffElements, dataGridView2);

            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.RowHeadersWidth = 70;

            //add to dataGridView2 different numbers to first column
            for (int i = 0; i < countOfDiffElements; i++)
            {
                dataGridView2.Rows[i].HeaderCell.Value = listOfDifferentNumbers[i].ToString();
                dataGridView2.Rows[i].HeaderCell.Style.BackColor = Color.FromArgb(170, 228, 166);
               

            }

            var getPResult = await GetPByAngleAsync(int.Parse(textBox1.Text), bitmap);
            dataGridView2 = await WinFormsComponentHandler.AddDataToDataGridView(getPResult, dataGridView2);
           
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

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
