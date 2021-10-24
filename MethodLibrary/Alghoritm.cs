using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodLibrary
{
    public static class Alghoritm
    {
        public static async Task<int> GetCountOfBlackPixels(int[,] dataGrid)
        {
            int countOfBlackPx = 0;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < dataGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < dataGrid.GetLength(1); j++)
                    {
                        if (dataGrid[i, j] == 1)
                        {
                            countOfBlackPx++;
                        }
                    }
                }
            });

            return countOfBlackPx;
        }

        public static int GetA4(int[,] dataGrid, int j, int i)
        {
            var left = dataGrid[j, i - 1];
            var right = dataGrid[j, i + 1];
            var top = dataGrid[j - 1, i];
            var bootom = dataGrid[j + 1, i];

            return left + right + top + bootom;

        }

        public static int GetA8(int[,] dataGrid, int j, int i)
        {
            if (i > 0 && j > 0 && i < 49 && j < 49)
            {
                var left = dataGrid[j, i - 1];
                var right = dataGrid[j, i + 1];
                var top = dataGrid[j - 1, i];
                var bootom = dataGrid[j + 1, i];
                var topleft = dataGrid[j - 1, i - 1];
                var topright = dataGrid[j - 1, i + 1];
                var leftbottom = dataGrid[j + 1, i - 1];
                var rightbootom = dataGrid[j + 1, i + 1];

                return left + right + top + bootom + topleft + topright + leftbottom + rightbootom;
            }

            return 0;

        }

        public static int GetB8(int[,] dataGrid, int j, int i)
        {
            var left = dataGrid[j, i - 1];
            var right = dataGrid[j, i + 1];
            var top = dataGrid[j - 1, i];
            var bootom = dataGrid[j + 1, i];
            var topleft = dataGrid[j - 1, i - 1];
            var topright = dataGrid[j - 1, i + 1];
            var leftbottom = dataGrid[j + 1, i - 1];
            var rightbootom = dataGrid[j + 1, i + 1];


            var a1a2 = right == 1 && topright == 1 ? 1 : 0;
            var a2a3 = topright == 1 && top == 1 ? 1 : 0;
            var a3a4 = top == 1 && topleft == 1 ? 1 : 0;
            var a4a5 = topleft == 1 && left == 1 ? 1 : 0;
            var a5a6 = left == 1 && leftbottom == 1 ? 1 : 0;
            var a6a7 = leftbottom == 1 && bootom == 1 ? 1 : 0;
            var a7a8 = bootom == 1 && rightbootom == 1 ? 1 : 0;
            var a8a1 = rightbootom == 1 && right == 1 ? 1 : 0;


            return a1a2 + a2a3 + a3a4 + a4a5 + a5a6 + a6a7 + a7a8 + a8a1;
        }

        public static int IsPointCN(int[,] dataGrid, int i, int j)
        {
            int P1 = 0;
            if (i > 0 && j > 0 && i < 49 && j < 49)
            {
                if (dataGrid[i - 1, j] == 0 && dataGrid[i - 1, j + 1] == 1)
                {
                    P1++;
                }
                if (dataGrid[i - 1, j + 1] == 0 && dataGrid[i, j + 1] == 1)
                {
                    P1++;
                }
                if (dataGrid[i, j + 1] == 0 && dataGrid[i + 1, j + 1] == 1)
                {
                    P1++;
                }
                if (dataGrid[i + 1, j + 1] == 0 && dataGrid[i + 1, j] == 1)
                {
                    P1++;
                }
                if (dataGrid[i + 1, j] == 0 && dataGrid[i + 1, j - 1] == 1)
                {
                    P1++;
                }
                if (dataGrid[i + 1, j - 1] == 0 && dataGrid[i, j - 1] == 1)
                {
                    P1++;
                }
                if (dataGrid[i, j - 1] == 0 && dataGrid[i - 1, j - 1] == 1)
                {
                    P1++;
                }
                if (dataGrid[i - 1, j - 1] == 0 && dataGrid[i - 1, j] == 1)
                {
                    P1++;
                }
            }



            return P1;
        }


        public static async Task<int> GetKonecDotsAsync(int[,] dataGrid)
        {
            var conecDotsCount = 0;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 1; i < dataGrid.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < dataGrid.GetLength(1) - 1; j++)
                    {
                        if (dataGrid[j, i] == 1 && GetA8(dataGrid, j, i) == 1)
                        {
                            conecDotsCount++;
                        }
                    }
                }
            });

            return conecDotsCount;
        }

        public static async Task<int> GetCountCnDotsAsync(int[,] dataGrid)
        {
            var cnDotsCount = 0;


            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < dataGrid.GetLength(0) - 1; i++)
                {
                    if (dataGrid[i, 3] == 3)
                    {
                        cnDotsCount++;
                    }
                }
            });


            return cnDotsCount;
        }



        public static async Task<int[,]> DoZonga(int[,] dataGrid)
        {
            int[,] arr = new int[dataGrid.GetLength(0), dataGrid.GetLength(1)];
            int[,] arr2 = new int[51, 51];
            int stepsOfAction = 1;

            return await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 51; i++)
                {
                    for (int j = 0; j < 51; j++)
                    {
                        if (dataGrid[j, i] == 1)
                        {
                            arr[i, j] = 1;
                        }
                        else
                        {
                            arr[i, j] = 0;
                        }
                    }
                }

                var counter = -1;
                while (true)
                {

                    stepsOfAction = 0;


                    Parallel.For(1, 50, i =>
                    {
                        for (int j = 1; j < 50; j++)
                        {

                            if (arr[i, j] == 1)
                            {
                                var listOfBlackPixels = new List<int>
                                    {
                                       arr[i - 1, j - 1],
                                       arr[i - 1, j],
                                       arr[i - 1, j + 1],
                                       arr[i, j - 1],
                                       arr[i, j + 1],
                                       arr[i + 1, j - 1],
                                       arr[i + 1, j + 1],
                                       arr[i + 1, j]
                                    };


                                var countOfBlackPixels = listOfBlackPixels.Where(x => x == 1).Count();

                                int P1 = 0;

                                if (arr[i - 1, j] == 0 && arr[i - 1, j + 1] == 1)
                                {
                                    P1++;
                                }
                                if (arr[i - 1, j + 1] == 0 && arr[i, j + 1] == 1)
                                {
                                    P1++;
                                }
                                if (arr[i, j + 1] == 0 && arr[i + 1, j + 1] == 1)
                                {
                                    P1++;
                                }
                                if (arr[i + 1, j + 1] == 0 && arr[i + 1, j] == 1)
                                {
                                    P1++;
                                }
                                if (arr[i + 1, j] == 0 && arr[i + 1, j - 1] == 1)
                                {
                                    P1++;
                                }
                                if (arr[i + 1, j - 1] == 0 && arr[i, j - 1] == 1)
                                {
                                    P1++;
                                }
                                if (arr[i, j - 1] == 0 && arr[i - 1, j - 1] == 1)
                                {
                                    P1++;
                                }
                                if (arr[i - 1, j - 1] == 0 && arr[i - 1, j] == 1)
                                {
                                    P1++;
                                }

                                bool three = false;
                                bool four = false;

                                if (counter % 2 != 0)
                                {
                                    three = (arr[i - 1, j] * arr[i, j + 1] * arr[i + 1, j]) == 0;
                                    four = (arr[i, j + 1] * arr[i + 1, j] * arr[i, j - 1]) == 0;
                                }
                                else
                                {
                                    three = (arr[i - 1, j] * arr[i + 1, j] * arr[i, j - 1]) == 0;
                                    four = (arr[i - 1, j] * arr[i, j + 1] * arr[i, j - 1]) == 0;
                                }


                                if (countOfBlackPixels >= 2 && countOfBlackPixels <= 6 && P1 == 1 && three && four)
                                {
                                    arr2[i, j] = 0;
                                    stepsOfAction++;
                                }
                                else
                                {
                                    arr2[i, j] = 1;
                                }

                            }
                        }
                    });

                    if (stepsOfAction == 0)
                    {

                        for (int i = 0; i < 51; i++)
                        {
                            for (int j = 0; j < 51; j++)
                            {
                                dataGrid[j, i] = 0;

                            }
                        }

                        for (int i = 0; i < 51; i++)
                        {
                            for (int j = 0; j < 51; j++)
                            {
                                dataGrid[j, i] = arr2[i, j];
                            }
                        }
                        break;
                    }
                    else
                    {
                        counter++;
                        arr = arr2;
                        arr2 = new int[51, 51];
                    }

                }

                int[,] resultArray = new int[50, 50];

                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        resultArray[i, j] = dataGrid[i + 1, j + 1];
                    }
                }

                return resultArray;
            });

        }
    }
}
