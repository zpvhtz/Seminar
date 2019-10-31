using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class RecursiveAlgorithm
    {
        public int KnapSackRecursive(int[] arrayWeight, int[] arrayValue, int weightMax, int n)
        {

            // Base Case 
            if (n == 0 || weightMax == 0)
                return 0;

            // If weight of the nth item is  
            // more than Knapsack capacity W, 
            // then this item cannot be  
            // included in the optimal solution 
            if (arrayWeight[n - 1] > weightMax)
                return KnapSackRecursive(arrayWeight, arrayValue, weightMax, n - 1);

            // Return the maximum of two cases:  
            // (1) nth item included  
            // (2) not included 
            else return Math.Max(arrayValue[n - 1] +
                KnapSackRecursive(arrayWeight, arrayValue, weightMax - arrayWeight[n - 1], n - 1),
                       KnapSackRecursive(arrayWeight, arrayValue, weightMax, n - 1));
        }
    }
}
