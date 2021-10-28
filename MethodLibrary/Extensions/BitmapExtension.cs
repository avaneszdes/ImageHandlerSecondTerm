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
    }
}
