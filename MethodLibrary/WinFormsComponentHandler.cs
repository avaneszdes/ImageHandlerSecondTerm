using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodLibrary
{
    public class WinFormsComponentHandler
    {
        public static async Task<DataGridView> ReDrowCells(DataGridView dataGridView)
        {
            return await Task.Factory.StartNew(() =>
            {
                dataGridView.Invoke(new Action(() =>
                {
                    for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < dataGridView.Columns.Count - 1; j++)
                        {
                            if (dataGridView[i, j].Value.ToString() == "1")
                            {
                                dataGridView[i, j].Style.BackColor = Color.Black;
                            }
                            else
                            {
                                dataGridView[i, j].Style.BackColor = Color.White;
                            }
                        }
                    }



                }));
                return dataGridView;
            });
        }

        public static async Task<int[,]> GetArrayFromDataGridView(DataGridView dataGridView)
        {
            
            return await Task.Factory.StartNew(() =>
            {
                var columnsCount = dataGridView.Columns.Count;
                var rowsCount = dataGridView.Rows.Count;
                int[,] array = new int[rowsCount, columnsCount];
                dataGridView.Invoke(new Action(() =>
                {
                    for (int i = 0; i < rowsCount; i++)
                    {
                        for (int j = 0; j < columnsCount; j++)
                        {
                            array[i, j] = int.Parse(dataGridView[j, i].Value.ToString()!);
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

        public static async Task<DataGridView> AddColAndRow(int width, int height, DataGridView dataGrid)
        {
            return await Task.Factory.StartNew(() =>
             {
                 dataGrid.Invoke(new Action(() =>
                 {
                     for (int i = 0; i < width; i++)
                     {
                         dataGrid.Columns.Add(i + 1 + "", i + 1 + "");
                     }

                     dataGrid.Rows.Add(height - 1);

                     for (int i = 0; i < width; i++)
                     {
                         dataGrid.Columns[i].Width = 40;
                     }
                 }));
                 return dataGrid;
             });

        }


        public static async Task<DataGridView> AddDataToDataGridView(int[,] data, DataGridView dataGrid)
        {
            return await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        dataGrid[j, i].Value = data[i, j];
                    }
                }

                return dataGrid;
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


        public static async Task<DataGridView> DrowCells(Bitmap bitmap, DataGridView dataGrid, string type)
        {

            if (type == "binary")
            {
                return await Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < dataGrid.Rows.Count; i++)
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

                    return dataGrid;
                });
            }
            else if (type == "greyscale")
            {
                return await Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < dataGrid.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGrid.Columns.Count; j++)
                        {
                            var avgColor = (bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).G + bitmap.GetPixel(i, j).B) / 3;
                            var color = Color.FromArgb(avgColor, avgColor, avgColor);
                            dataGrid[i, j].Style.BackColor = color;
                        }
                    }

                    return dataGrid;
                });
            }

            return await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        var color = Color.FromArgb(bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).G, bitmap.GetPixel(i, j).B);
                        dataGrid[i, j].Style.BackColor = color;
                    }
                }

                return dataGrid;
            });
        }

    }
}
