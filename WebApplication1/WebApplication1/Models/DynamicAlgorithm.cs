using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class DynamicAlgorithm
    {
        public void HandlingDynamicPlanning(int W, int n, ref int[,] a, int[] listtl, int[] listgt, ref string mangtmp, ref int GT)
        {
            for (int i = 0; i <= n; i++)
            {
                a[i, 0] = 0;
            }

            for (int j = 1; j <= W; j++)
            {
                a[0, j] = 0;
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= W; j++)
                {
                    if (j - listtl[i] >= 0)
                    {
                        a[i, j] = Max(a[i - 1, j], a[i - 1, j - listtl[i]] + listgt[i]);
                    }
                    else
                    {
                        a[i, j] = a[i - 1, j];
                    }
                }
            }

            int tmp1 = n, tmp2 = W, tmp = 0;
            while ((tmp1 != 0) && (tmp2 != 0))
            {
                if (a[tmp1, tmp2] != a[tmp1 - 1, tmp2])
                {
                    mangtmp = mangtmp + tmp1 + " ";
                    tmp++;
                    GT += listgt[tmp1];
                    tmp2 -= listtl[tmp1];
                }
                tmp1--;
            }
        }

        public int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }
    }
}
