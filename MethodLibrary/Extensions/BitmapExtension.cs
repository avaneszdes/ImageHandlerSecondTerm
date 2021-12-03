using MethodLibrary.Extensions;
using System.Drawing;
using System.Threading.Tasks;

namespace MethodLibrary
{
    public static class BitmapExtension
    {
        public static string GetCountCnDotsFromBitmapAsync(this Bitmap bitmap)
        {

            Bitmap bitmap2 = (Bitmap)bitmap.Clone();
            var cnDotsCount = 0;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (bitmap.GetPixel(i, j) == Color.FromArgb(255, 0, 0))
                    {
                        cnDotsCount++;
                    }
                }

            }

            return cnDotsCount.ToString();

        }


        public static int GetCountCnDotsFromArray(this int[,] arr)
        {

            //[16,22]

            var cnDotsCount = 0;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    var cn = Alghoritm.IsPointCN(arr, j + 1, i + 1);
                    if (cn >= 3 && arr[j + 1, i + 1] == 1)
                    {
                        cnDotsCount++;
                    }
                }

            }

            return cnDotsCount;

        }
    }
}
