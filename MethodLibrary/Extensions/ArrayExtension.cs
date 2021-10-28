using System;

namespace MethodLibrary.Extensions
{
    public static class ArrayExtension
    {
        public static int[,] ExpandIntArray(this int[,] array)
        {
            int[,] tempArr = new int[array.GetLength(0) + 1, array.GetLength(1) + 1];


            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    tempArr[i + 1,j + 1] = array[i,j];
                }
            }


            return tempArr;
        }


        public static int[,] RedusingIntArray(this int[,] array)
        {
            int[,] tempArr = new int[array.GetLength(0) - 1, array.GetLength(1) - 1];


            for (int i = 0; i < tempArr.GetLength(0); i++)
            {
                for (int j = 0; j < tempArr.GetLength(1); j++)
                {
                    tempArr[i, j] = array[i + 1, j + 1];
                }
            }


            return tempArr;
        }

    }
}
