using System.Drawing;

namespace MethodLibrary
{
    public class BitmapHandler
    {
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
    }
}
