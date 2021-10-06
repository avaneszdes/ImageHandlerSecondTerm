using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MethodLibrary
{
    public class BitmapHandler
    {

        public static List<bool[,]> colors = new List<bool[,]>();
        public static Bitmap GetGreyscaleBitmap(Bitmap bitmap)
        {
            var result = new Bitmap(bitmap.Width, bitmap.Height);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = (bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).G + bitmap.GetPixel(i, j).B) / 3;
                    result.SetPixel(i, j, Color.FromArgb(color, color, color));
                }
            }

            return result;
        }

        public static int[,] GetGreyscaleArray(Bitmap bitmap)
        {
            int[,] result = new int[bitmap.Width, bitmap.Height];

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    result[j, i] = (bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).G + bitmap.GetPixel(i, j).B) / 3;
                }
            }
            return result;

        }


        public static  bool PictureValidate(Bitmap bitmap, int width, int height)
        {
            if (bitmap.Width != width || bitmap.Height != height)
            {
                MessageBox.Show("Incorrect size image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        //return array with values that contain 
        public static async Task<int[,]> GetPByAngleAsync(int angle, Bitmap bitmap, DataGridView dataGridView1, List<int> listOfDifferentNumbers)
        {
            var bitmapWidth = bitmap.Width;
            var bitmapHeight = bitmap.Height;
            var listOfDifferentNumbersCount = listOfDifferentNumbers!.Count;
            bool [,]color = new bool[50, 50];

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
            var count2 = 0;

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

                                for (int i = 0; i < step; i++)
                                {
                                    if (angle == 90 && col + step < bitmapWidth && arrForP[row, col] == item && arrForP[row, col + step] == item)
                                    {
                                        count2++;
                                    }
                                    else if (angle == 135 && col + i < bitmapHeight &&
                                            row + i < bitmapWidth &&
                                            arrForP[row, col] == item &&
                                            arrForP[row + i, col + i] == item)
                                    {
                                        count2++;
                                    }
                                }


                                if(count2 == step)
                                {
                                    count++;
                                }



                                
                                count2 = 0;
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

            colors.Add(color);
            return newArr;
        }


        public static double[] GetArrayForResultTable(List<int> listOfDifferentNumbers, DataGridView dataGridView2)
        {
            
            long K = default;

            for (int i = 0; i < listOfDifferentNumbers.Count; i++)
            {
                for (int j = 0; j < 45; j++)
                {
                    K += int.Parse(dataGridView2[j, i].Value.ToString()!);
                }
            }

            int k = 0;
            var KPP = .0;


            foreach (var item in listOfDifferentNumbers)
            {
                for (int i = 0; i < 45; i++)
                {
                    KPP += int.Parse(dataGridView2[i, k].Value.ToString()!) / Math.Pow(i + 1, 2);
                }
                k++;
            }


            KPP /= K;
            k = 0;


            var DPP = .0;

            foreach (var item in listOfDifferentNumbers)
            {
                for (int i = 0; i < 45; i++)
                {
                    DPP += int.Parse(dataGridView2[i, k].Value.ToString()!) * Math.Pow(i + 1, 2);
                }
                k++;
            }

            DPP /= K;
            k = 0;

            var EUS = .0;

            foreach (var item in listOfDifferentNumbers)
            {
                var temp = .0;
                for (int i = 0; i < 45; i++)
                {
                    temp += int.Parse(dataGridView2[i, k].Value.ToString()!);
                }

                EUS += Math.Pow(temp, 2);
                k++;
            }

            EUS /= K;

            return new []{ K, KPP, DPP, EUS };
        }


        public static int[,] GetBinaryArrayFromBitmap(Bitmap bitmap)
        {
            int[,] arr = new int[bitmap.Height, bitmap.Width];


            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var isWhite = bitmap.GetPixel(j, i).R == 255 && bitmap.GetPixel(j, i).G == 255 && bitmap.GetPixel(j, i).B == 255;
                    if (isWhite)
                    {
                        arr[i, j] = 0;
                    }
                    else
                    {
                        arr[i, j] = 1;
                    }
                    
                }
            }

            return arr;
        }
       
    }
}
