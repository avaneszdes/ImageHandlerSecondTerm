﻿using MethodLibrary;
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
        string[] valuesForSmallDataGrid = { "К", "КПП", "ДПП", "ЕУС", "R" };
        public Main()
        {
            InitializeComponent();
            burgerPanel.Height = 1500;
            burgerPanel.Location = new Point(0,-400);
            panel1.Location = new Point(0, 0);
            panel1.Width = this.Width;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            burgerPanel.Visible = false;
            dataGridView3 = await WinFormsComponentHandler.AddColAndRow(4, 5, dataGridView3);
            dataGridView3.RowHeadersWidth = 60;
            dataGridView3.EnableHeadersVisualStyles = false;
            for (int i = 1; i < 5; i++)
            {
                dataGridView3[i - 1,0].Value = valuesForSmallDataGrid[i];
                dataGridView3.Rows[i].HeaderCell.Value = i.ToString();
                dataGridView3.Rows[i].HeaderCell.Style.BackColor = Color.FromArgb(170, 228, 166);
                dataGridView3.Columns[i - 1].Width = 70;
            }

            dataGridView3.ScrollBars = ScrollBars.None;
               
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


        private async Task RenderDataGridViews(PictureBox picture, string fileName)
        {

            var bitmap = new Bitmap(fileName);
            if (!BitmapHandler.PictureValidate(bitmap, bitmap.Width, bitmap.Height))
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
            listOfDifferentNumbers = await WinFormsComponentHandler.GetDiffItemsFromDataGrid(dataGridView1, bitmapWidth, bitmapHeight);
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

            var getPResult = await BitmapHandler.GetPByAngleAsync(int.Parse(textBox1.Text), bitmap, dataGridView1, listOfDifferentNumbers);
            dataGridView2 = await WinFormsComponentHandler.AddDataToDataGridView(getPResult, dataGridView2);
        }


        private async void OpenImage1_Click(object sender, EventArgs e)
        {
            var isOpen = WinFormsComponentHandler.IsOpenFileDialogOk(openFileDialog1);
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridViews(pictureBox1, isOpen);
                var result = BitmapHandler.GetArrayForResultTable(listOfDifferentNumbers!, dataGridView2);
                for (int i = 0; i < 4; i++)
                {
                    dataGridView3[i, 1].Value = result[i];
                }
            }
        }

        private async void OpenImage2_Click(object sender, EventArgs e)
        {
            var isOpen = WinFormsComponentHandler.IsOpenFileDialogOk(openFileDialog1);
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridViews(pictureBox2, isOpen);
                var result = BitmapHandler.GetArrayForResultTable(listOfDifferentNumbers!, dataGridView2);
                for (int i = 0; i < 4; i++)
                {
                    dataGridView3[i, 2].Value = result[i];
                }
            }
        }

        private async void OpenImage3_Click(object sender, EventArgs e)
        {
            var isOpen = WinFormsComponentHandler.IsOpenFileDialogOk(openFileDialog1);
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridViews(pictureBox3, isOpen);
                var result = BitmapHandler.GetArrayForResultTable(listOfDifferentNumbers!, dataGridView2);
                for (int i = 0; i < 4; i++)
                {
                    dataGridView3[i, 3].Value = result[i];
                }
            }
        }

        private async void OpenImage4_Click(object sender, EventArgs e)
        {
            var isOpen = WinFormsComponentHandler.IsOpenFileDialogOk(openFileDialog1);
            if (!string.IsNullOrEmpty(isOpen))
            {
                await RenderDataGridViews(pictureBox4, isOpen);
                var result = BitmapHandler.GetArrayForResultTable(listOfDifferentNumbers!, dataGridView2);
                for (int i = 0; i < 4; i++)
                {
                    dataGridView3[i, 4].Value = result[i];
                }
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void menu_Click(object sender, EventArgs e)
        {
            burgerPanel.Visible = true;
            burgerPanel.Location = new Point(0, 0);
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            burgerPanel.Location = new Point(-500, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            burgerPanel.Location = new Point(-500, 0);
        }
    }
}
