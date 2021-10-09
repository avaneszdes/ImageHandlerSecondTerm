using Emgu.CV;
using Emgu.CV.Flann;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using MethodLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ImageHandlerSecondTerm
{
    public partial class Main : Form
    {
        List<int>? listOfDifferentNumbers;
        string[] valuesForSmallDataGrid = { "К", "КПП", "ДПП", "ЕУС" };
        string fileName = string.Empty;
        Chart chart1 = new();
        public Main()
        {
            InitializeComponent();
            burgerPanel.Height = 1500;
            burgerPanel.Location = new Point(0, -400);
            panel1.Location = new Point(0, 0);
            panel1.Width = this.Width;

            chart1.Location = new Point(735, 10);
            chart1.Width = 710;
            chart1.Height = 200;

            this.tabPage2.Controls.Add(chart1); 
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //burgerPanel.Visible = false;
            //dataGridView3 = await WinFormsComponentHandler.AddColAndRow(4, 5, dataGridView3);
            //dataGridView3.RowHeadersWidth = 60;
            //dataGridView3.EnableHeadersVisualStyles = false;
            //for (int i = 1; i < 5; i++)
            //{
            //    dataGridView3[i - 1, 0].Value = valuesForSmallDataGrid[i - 1];
            //    dataGridView3.Rows[i].HeaderCell.Value = i.ToString();
            //    dataGridView3.Rows[i].HeaderCell.Style.BackColor = Color.FromArgb(170, 228, 166);
            //    dataGridView3.Columns[i - 1].Width = 70;
            //}

            //dataGridView3.ScrollBars = ScrollBars.None;


            ChartArea chartarea1 = new ChartArea();
            chart1.ChartAreas.Add(chartarea1);

            chart1.Series.Clear();
            chart1.Series.Add("Series1");
            chart1.Series[0].ChartType = SeriesChartType.Point;
            chart1.ChartAreas[0].BorderColor = Color.FromArgb(128, 180, 255);
            chart1.ChartAreas[0].BackColor = Color.FromArgb(128, 180, 255);
            chart1.ChartAreas[0].BackSecondaryColor = Color.FromArgb(128, 180, 255);
            chart1.Series[0].ChartArea = chart1.ChartAreas[0].Name;
            chart1.BackColor = Color.FromArgb(128, 180, 255);
            chart1.Series[0].BorderColor = Color.FromArgb(128, 180, 255);
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

                for (int i = 0; i < 49; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        var item = int.Parse(dataGridView1[j, i].Value.ToString()!);
                        var itemPlusStep = int.Parse(dataGridView1[j + 1, i + 1].Value.ToString()!);
                        if (item == itemPlusStep)
                        {
                            var avg = int.Parse(dataGridView1[j, i].Value.ToString()!);
                            var color = Color.FromArgb(avg, avg, avg);
                            dataGridView1[j, i].Style.BackColor = color;
                            dataGridView1[j + 1, i + 1].Style.BackColor = color;
                        }
                    }
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

                for (int i = 0; i < 49; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        var item = int.Parse(dataGridView1[j, i].Value.ToString()!);
                        var itemPlusStep = int.Parse(dataGridView1[j + 1, i + 1].Value.ToString()!);
                        if (item == itemPlusStep)
                        {
                            var avg = int.Parse(dataGridView1[j, i].Value.ToString()!);
                            var color = Color.FromArgb(avg, avg, avg);
                            dataGridView1[j, i].Style.BackColor = color;
                            dataGridView1[j + 1, i + 1].Style.BackColor = color;
                        }
                    }
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

                for (int i = 0; i < 49; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        var item = int.Parse(dataGridView1[j, i].Value.ToString()!);
                        var itemPlusStep = int.Parse(dataGridView1[j + 1, i + 1].Value.ToString()!);
                        if (item == itemPlusStep)
                        {
                            var avg = int.Parse(dataGridView1[j, i].Value.ToString()!);
                            var color = Color.FromArgb(avg, avg, avg);
                            dataGridView1[j, i].Style.BackColor = color;
                            dataGridView1[j + 1, i + 1].Style.BackColor = color;
                        }
                    }
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

                for (int i = 0; i < 49; i++)
                {
                    for (int j = 0; j < 49; j++)
                    {
                        var item = int.Parse(dataGridView1[j, i].Value.ToString()!);
                        var itemPlusStep = int.Parse(dataGridView1[j + 1, i + 1].Value.ToString()!);
                        if (item == itemPlusStep)
                        {
                            var avg = int.Parse(dataGridView1[j, i].Value.ToString()!);
                            var color = Color.FromArgb(avg, avg, avg);
                            dataGridView1[j, i].Style.BackColor = color;
                            dataGridView1[j + 1, i + 1].Style.BackColor = color;
                        }
                    }
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

        private void button2_Click(object sender, EventArgs e)
        {

            var firstImgIsNotnull = dataGridView3[3, 1].Value != null;
            var secondImgIsNotnull = dataGridView3[3, 2].Value != null;
            var thredImgIsNotnull = dataGridView3[3, 3].Value != null;
            var fourghtImgIsNotnull = dataGridView3[3, 4].Value != null;


            if (firstImgIsNotnull && secondImgIsNotnull)
            {
                label2.Text = "1-2 = " + Math.Sqrt((Math.Pow((double.Parse(dataGridView3[1, 1].Value.ToString()!) - double.Parse(dataGridView3[1, 2].Value.ToString()!)), 2) +
                    Math.Pow((double.Parse(dataGridView3[2, 1].Value.ToString()!) - double.Parse(dataGridView3[2, 2].Value.ToString()!)), 2) +
                    Math.Pow((double.Parse(dataGridView3[3, 1].Value.ToString()!) - double.Parse(dataGridView3[3, 2].Value.ToString()!)), 2))).ToString();
            }

            if (firstImgIsNotnull && thredImgIsNotnull)
            {
                label3.Text = "1-3 = " + Math.Sqrt((Math.Pow((double.Parse(dataGridView3[1, 1].Value.ToString()!) - double.Parse(dataGridView3[1, 3].Value.ToString()!)), 2) +
                    Math.Pow((double.Parse(dataGridView3[2, 1].Value.ToString()!) - double.Parse(dataGridView3[2, 3].Value.ToString()!)), 2) +
                    Math.Pow((double.Parse(dataGridView3[3, 1].Value.ToString()!) - double.Parse(dataGridView3[3, 3].Value.ToString()!)), 2))).ToString();
            }

            if (firstImgIsNotnull && fourghtImgIsNotnull)
            {
                label4.Text = "1-4 = " + Math.Sqrt((Math.Pow((double.Parse(dataGridView3[1, 1].Value.ToString()!) - double.Parse(dataGridView3[1, 4].Value.ToString()!)), 2) +
                    Math.Pow((double.Parse(dataGridView3[2, 1].Value.ToString()!) - double.Parse(dataGridView3[2, 4].Value.ToString()!)), 2) +
                    Math.Pow((double.Parse(dataGridView3[3, 1].Value.ToString()!) - double.Parse(dataGridView3[3, 4].Value.ToString()!)), 2))).ToString();
            }
        }

        private async void pictureBox5_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Clear();
            dataGridView4.Columns.Clear();
            dataGridView5.Columns.Clear();
            dataGridView5.Rows.Clear();

            var isOpen = WinFormsComponentHandler.IsOpenFileDialogOk(openFileDialog1);
            if (!string.IsNullOrEmpty(isOpen))
            {
                var bitmap = new Bitmap(isOpen);
                if (!BitmapHandler.PictureValidate(bitmap, bitmap.Width, bitmap.Height))
                {
                    return;
                }
                pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox5.Image = bitmap;

                //var pathToRus = @"C:\Users\avane\source\repos\ImageHandlerSecondTerm\ImageHandlerSecondTerm\bin\Debug\";

                //var tesseract = new Tesseract(pathToRus, "rus", OcrEngineMode.TesseractLstmCombined);

                //tesseract.SetImage(new Image<Bgr, Byte>(isOpen));

                //tesseract.Recognize();

                //label5.Text = tesseract.GetUTF8Text();

                dataGridView4 = await WinFormsComponentHandler.AddColAndRow(bitmap.Width, bitmap.Height, dataGridView4);
                dataGridView4 = await WinFormsComponentHandler.AddDataToDataGridView(BitmapHandler.GetBinaryArrayFromBitmap(bitmap), dataGridView4);
                dataGridView4 = await WinFormsComponentHandler.DrowCells(bitmap, dataGridView4, "binary");

                var dataGrid4 = await WinFormsComponentHandler.GetArrayFromDataGridView(dataGridView4);
               

                var zongaSunyaresult = await Alghoritm.DoZonga(dataGrid4);

                dataGridView4 = await WinFormsComponentHandler.AddDataToDataGridView(zongaSunyaresult, dataGridView4);

                var countOfBlackPixels = Alghoritm.GetCountOfBlackPixels(dataGrid4);

                dataGridView4 = await WinFormsComponentHandler.ReDrowCells(dataGridView4);

                //tesseract.Dispose();

                dataGridView5 = await WinFormsComponentHandler.AddColAndRow(4, await countOfBlackPixels, dataGridView5);
               

                dataGridView5.Columns[0].HeaderCell.Value = "A4";
                dataGridView5.Columns[1].HeaderCell.Value = "A8";
                dataGridView5.Columns[2].HeaderCell.Value = "B8";
                dataGridView5.Columns[3].HeaderCell.Value = "CN";

                
                dataGridView5.EnableHeadersVisualStyles = false;
                chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                var row = 0;
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        if (int.Parse(dataGridView4[j, i].Value.ToString()!) == 1)
                        {

                            dataGridView5.Rows[row].HeaderCell.Value = $"[{i + 1},{j + 1}]";
                            dataGridView5.Rows[row].HeaderCell.Style.BackColor = Color.FromArgb(170, 228, 166);
                            dataGridView5.AutoResizeRowHeadersWidth(row, DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                            dataGridView5[0, row].Value = Alghoritm.GetA4(dataGrid4,i, j);
                            dataGridView5[1, row].Value = Alghoritm.GetA8(dataGrid4,i, j);
                            dataGridView5[2, row].Value = Alghoritm.GetB8(dataGrid4,i, j);
                            dataGridView5[3, row].Value = Alghoritm.GetCN(dataGrid4,i, j);
                            row++;
                        }
                    }
                }

                var dataGrid5 = await WinFormsComponentHandler.GetArrayFromDataGridView(dataGridView5);
                var fileName = isOpen.Split("\\");
                var conDots =  Alghoritm.GetKonecDotsAsync(dataGrid4);
                var cnDots =  Alghoritm.GetCountCnDotsAsync(dataGrid5);

                var x1 = int.Parse(textBox2.Text);
                var y1 = int.Parse(textBox3.Text);

                var con = await conDots;
                var cn = await cnDots;


               
                if (dataGridView6.Rows.Count == 0)
                {
                    var datapoint = new DataPoint();
                    datapoint.Font = new Font("Arial", 33, FontStyle.Italic);
                    datapoint.SetValueXY(x1, y1);
                    datapoint.Color = Color.Red;
                    datapoint.MarkerSize = 10;
                    datapoint.MarkerColor = Color.Red;
                    datapoint.BorderDashStyle = ChartDashStyle.Solid;
                    chart1.Series[0].Points.AddXY(x1,y1);
                    chart1.Series[0].Points[0] = datapoint;


                    dataGridView6 = await WinFormsComponentHandler.AddColAndRow(4, 2, dataGridView6);
                    dataGridView6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridView6.Columns[0].HeaderCell.Value = "File name";
                    dataGridView6.Columns[1].HeaderCell.Value = "Конечных";
                    dataGridView6.Columns[2].HeaderCell.Value = "Ветвления";
                    dataGridView6.Columns[3].HeaderCell.Value = "Результат";

                    
                    dataGridView6[0, 0].Value = fileName[^1];
                    dataGridView6[1, 0].Value = con;
                    dataGridView6[2, 0].Value = await cnDots;
                    dataGridView6[3, 0].Value = Math.Round(Math.Sqrt(Math.Pow(x1 - con, 2) + Math.Pow(y1 - cn, 2)), 3);
                    dataGridView6[0, 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView6[1, 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView6[2, 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView6[3, 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


                    chart1.Series[0].Points.AddXY(con, cn);

                    chart1.Series.Add("Line");
                    chart1.Series["Line"].Points.Add(new DataPoint(x1, y1));
                    chart1.Series["Line"].Points.Add(new DataPoint(con, cn));
                    chart1.Series["Line"].ChartType = SeriesChartType.Line;


                    int idx = chart1.Series["Line"].Points.AddXY(con,cn);
                    chart1.Series["Line"].Points[idx].Label = Math.Round(Math.Sqrt(Math.Pow(x1 - con, 2) + Math.Pow(y1 - cn, 2)), 3).ToString();
                }
                else
                {
                    dataGridView6.Rows.Add(1);
                    var index = dataGridView6.Rows.Count - 2;
                    dataGridView6[0, index].Value = fileName[^1];
                    dataGridView6[1, index].Value = con;
                    dataGridView6[2, index].Value = cn;
                    dataGridView6[3, index].Value = Math.Round(Math.Sqrt(Math.Pow(x1 - con, 2) + Math.Pow(y1 - cn, 2)), 3);
                    dataGridView6[0, index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView6[1, index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView6[2, index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridView6[3, index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


                    chart1.Series[0].Points.AddXY(con, cn);


                    chart1.Series.Add("Line" + index);
                    chart1.Series["Line" + index].Points.Add(new DataPoint(x1, y1));
                    chart1.Series["Line" + index].Points.Add(new DataPoint(con, cn));
                    chart1.Series["Line" + index].ChartType = SeriesChartType.Line;

                    int idx = chart1.Series["Line" + index].Points.AddXY(con, cn);
                    chart1.Series["Line" + index].Points[idx].Label = Math.Round(Math.Sqrt(Math.Pow(x1 - con, 2) + Math.Pow(y1 - cn, 2)), 3).ToString();

                    var x = chart1.Series["Line" + index].Points[chart1.Series[index].Points.Count - 1].XValue;
                    var y = chart1.Series["Line" + index].Points[chart1.Series[index].Points.Count - 1].YValues;
                    var x2 = chart1.Series["Line"].Points[0].XValue;
                    var y2 = chart1.Series["Line"].Points[0].YValues;

                    chart1.Series["Line" + index].Points[0].BorderColor = Color.Red;

                    if (x == x2 && y[0] == y2[0])
                    {
                        chart1.Series[0].Points[chart1.Series[0].Points.Count - 1].Color = Color.Red;

                        var datapoint = new DataPoint();
                        datapoint.Font = new Font("Arial", 33, FontStyle.Italic);
                        datapoint.SetValueXY(x1, y1);
                        datapoint.Color = Color.Red;
                        datapoint.MarkerSize = 10;
                        datapoint.MarkerColor = Color.Red;
                        datapoint.BorderDashStyle = ChartDashStyle.Solid;
                        chart1.Series[0].Points[chart1.Series[0].Points.Count - 1] = datapoint;
                    }
                    else
                    {
                        chart1.Series[0].Points[chart1.Series[0].Points.Count - 1].Color = Color.Green;
                    }


                }
            }
        }


        private async void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGrid4 = await WinFormsComponentHandler.GetArrayFromDataGridView(dataGridView4);
            if (Alghoritm.GetCN(dataGrid4,e.RowIndex, e.ColumnIndex) >= 3)
            {
                MessageBox.Show("Пиксель ветвления");
            }
            else if (Alghoritm.GetA8(dataGrid4, e.RowIndex, e.ColumnIndex) == 1)
            {
                MessageBox.Show("Конечный пиксель");
            }
            else if (Alghoritm.GetA8(dataGrid4, e.RowIndex, e.ColumnIndex) == 0)
            {
                MessageBox.Show("Изолированный пиксель");
            }
            else if (Alghoritm.GetCN(dataGrid4, e.RowIndex, e.ColumnIndex) == 2)
            {
                MessageBox.Show("Связующий пиксель");
            }

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }
    }
}

