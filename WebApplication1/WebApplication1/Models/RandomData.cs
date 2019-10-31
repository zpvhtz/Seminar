using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem
{
    public static class RandomData
    {
        public static int Int()
        {
            Random random = new Random();
            return random.Next(1, 1500);
        }
        public static int[] ArrayInt(int n)
        {
            Random random = new Random();
            int[] resultArray = new int[n];
            for (int i = 0; i < n; i++)
            {
                resultArray[i] = random.Next(1, 1000);
            }
            return resultArray;
        }
    }
}
