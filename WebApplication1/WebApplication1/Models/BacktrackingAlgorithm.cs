using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class BacktrackingAlgorithm
    {
        public int numberItemAvailable = 1000;
        public int backTrackingValue = 0;
        public bool[] arrayFlagSelected = new bool[1000];
        public bool[] arrayFlag = new bool[1000];

        public void KPBacktracking(int u, int[] arrayWeight, int[] arrayValue, int n, int weightMax)
        {
            if (u == n)
            {
                CheckCondition(arrayWeight, arrayValue, n, weightMax, arrayFlag);
                return;
            }
            arrayFlag[u] = false;
            KPBacktracking(u + 1, arrayWeight, arrayValue, n, weightMax);

            arrayFlag[u] = true;
            KPBacktracking(u + 1, arrayWeight, arrayValue, n, weightMax);
        }

        public void CheckCondition(int[] arrayWeight, int[] arrayValue, int n, int weightMax, bool[] arrayFlagCheck)
        {
            int tempWeight = 0;
            int tempValue = 0;
            bool[] arrayFlagTemp = new bool[n];
            for (int i = 0; i < n; i++)
            {
                if (arrayFlagCheck[i])
                {
                    tempWeight += arrayWeight[i];
                    tempValue += arrayValue[i];
                    arrayFlagTemp[i] = true;
                    if (tempWeight > weightMax)
                    {
                        return;
                    }
                }
            }

            if (tempWeight <= weightMax && tempValue > backTrackingValue)
            {
                backTrackingValue = tempValue;
                arrayFlagSelected = (bool[])arrayFlagTemp.Clone();
            }

        }

        public void PrintItemInBag(int[] arrayValue, int[] arrayWeight, int n)
        {
            int sumWeight = 0;
            int sumValue = 0;
            Console.WriteLine("Weight" + "\t" + "Value");
            for (int i = 0; i < n; i++)
            {
                if (arrayFlagSelected[i])
                {
                    Console.WriteLine(arrayWeight[i] + "\t" + arrayValue[i]);
                    sumWeight += arrayWeight[i];
                    sumValue += arrayValue[i];
                }
            }
            Console.WriteLine("List Item In Bag(Sum weight items: " + sumWeight + ", Sum value items: " + sumValue + "):");
        }
    }
}
