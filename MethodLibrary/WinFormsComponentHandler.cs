using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodLibrary
{
    public class WinFormsComponentHandler
    {
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

                return resultList;
            });
        }
    }
}
