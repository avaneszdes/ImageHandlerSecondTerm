using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodLibrary
{
    public static class DataGridViewExtension
    {
        public static void Clear(this DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
        }

        public static async Task AddColAndRow(this DataGridView dataGrid, int width, int height)
        {
            var colWidt = width == 50 && height == 50 ? 12 : 30;
            await Task.Factory.StartNew(() =>
                dataGrid.Invoke(new Action(() =>
                {
                    for (int i = 0; i < width; i++)
                    {
                        dataGrid.Columns.Add(i + 1 + "", i + 1 + "");

                    }

                    dataGrid.Rows.Add(height);

                    for (int i = 0; i < width; i++)
                    {
                        dataGrid.Columns[i].Width = colWidt;

                    }

                    if (width == 50 && height == 50)
                    {
                        for (int i = 0; i < height; i++)
                        {
                            dataGrid.Rows[i].Height = 12;

                        }
                    }


                })));
        }

        public static int GetCNDataGrid(this DataGridView dataGrid, int i, int j)
        {

            if(i < 48)
            {
                var listOfRedPixels = new List<Color>
                                    {
                                   dataGrid[i, j].Style.BackColor,
                                   dataGrid[i - 1, j - 1].Style.BackColor,
                                   dataGrid[i - 1, j].Style.BackColor,
                                   dataGrid[i - 1, j + 1].Style.BackColor,
                                   dataGrid[i + 1, j - 1].Style.BackColor,
                                   dataGrid[i + 1, j].Style.BackColor,
                                   dataGrid[i + 1, j + 1].Style.BackColor,
                                   dataGrid[i, j + 1].Style.BackColor,
                                   dataGrid[i, j - 1].Style.BackColor
                                    };

                return listOfRedPixels.Where(x => x == Color.Red).Count();
            }

            return 0;
          
        }


        public static void RedrowDiagonallZond(this DataGridView dataGrid)
        {
            var rowsCount = dataGrid.Rows.Count;

            for (int j = 1; j < rowsCount - 1; j++)
            {

                if (dataGrid.GetCNDataGrid(j, j) > 1 && dataGrid[j, j].Value.ToString() != "0")
                {
                    dataGrid[j, j].Style.BackColor = Color.Blue;

                }


                if (dataGrid.GetCNDataGrid(rowsCount - j - 1, j) > 1 && dataGrid[rowsCount - j - 1, j].Value.ToString() != "0")
                {
                    dataGrid[rowsCount - j - 1, j].Style.BackColor = Color.Blue;
                }
            }
        }

       
    }
}
