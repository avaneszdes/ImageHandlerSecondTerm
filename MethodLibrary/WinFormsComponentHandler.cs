using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodLibrary
{
    public static class WinFormsComponentHandler
    {
       

        public static async Task<int[,]> GetArrayFromDataGridView(DataGridView dataGridView)
        {

            return await Task.Factory.StartNew(() =>
            {
                var columnsCount = dataGridView.Columns.Count;
                var rowsCount = dataGridView.Rows.Count - 1;
                int[,] array = new int[rowsCount + 1, columnsCount + 1];
                dataGridView.Invoke(new Action(() =>
                {
                    for (int i = 1; i < rowsCount + 1; i++)
                    {
                        for (int j = 1; j < columnsCount + 1; j++)
                        {
                            array[i, j] = int.Parse(dataGridView[j - 1, i - 1].Value.ToString()!);
                        }
                    }

                }));
                return array;
            });


        }


        public static string? IsOpenFileDialogOk(OpenFileDialog fileDialog)
        {
            fileDialog = new OpenFileDialog
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
            var result = fileDialog.ShowDialog() == DialogResult.OK ? fileDialog.FileName : null;


            return result;
        }

        public static async Task AddDataToDataGridView(this DataGridView dataGrid, int[,] data)
        {
            await Task.Factory.StartNew(() =>
           {
               for (int i = 0; i < data.GetLength(0); i++)
               {
                   for (int j = 0; j < data.GetLength(1); j++)
                   {
                       dataGrid[j, i].Value = data[i, j];
                   }
               }
           });
        }

        public static async Task<List<int>> GetDiffItemsFromDataGrid(DataGridView dataGrid, int width, int height)
        {
            var resultList = new List<int>();

            return await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var number = int.Parse(dataGrid[j, i].Value.ToString()!);
                        if (!resultList.Contains(number))
                        {
                            resultList.Add(number);
                        }
                    }
                }

                resultList.Sort();

                return resultList;
            });
        }


        public static async Task DrowCellsFromBinaryBitmap(this DataGridView dataGrid, Bitmap bitmap, string type)
        {
            await Task.Factory.StartNew(() =>
            {
                if (type == "binary")
                {
                    for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < dataGrid.Columns.Count; j++)
                        {
                            var color = bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).G + bitmap.GetPixel(i, j).B;
                            if (color == 765)
                            {
                                dataGrid[i, j].Style.BackColor = Color.White;
                            }
                            else
                            {
                                dataGrid[i, j].Style.BackColor = Color.Black;
                            }

                        }
                    }

                }
                else if (type == "greyscale")
                {
                    for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < dataGrid.Columns.Count; j++)
                        {
                            var avgColor = (bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).G + bitmap.GetPixel(i, j).B) / 3;
                            var color = Color.FromArgb(avgColor, avgColor, avgColor);
                            dataGrid[i, j].Style.BackColor = color;
                        }
                    }

                }

                for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        var color = Color.FromArgb(bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).G, bitmap.GetPixel(i, j).B);
                        dataGrid[i, j].Style.BackColor = color;
                    }
                }
            });


        }

    }
}
