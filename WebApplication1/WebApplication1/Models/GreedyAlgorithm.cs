using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class GreedyAlgorithm
    {
        public List<KeyValuePair<int, int>> KnapsackGreedyProgramming(int[] arrayWeight, int[] arrayValue, int weightMax)
        {
            int n = arrayWeight.Length;
            List<KeyValuePair<int, float>> listCost = new List<KeyValuePair<int, float>>();
            for (int i = 0; i < n; i++)
            {
                listCost.Add(new KeyValuePair<int, float>(i, (float)arrayValue[i] / arrayWeight[i]));
            }
            listCost = listCost.OrderByDescending(m => m.Value).ToList();
            int remainCapacity = weightMax;
            List<KeyValuePair<int, int>> resultListItem = new List<KeyValuePair<int, int>>();
            foreach (var item in listCost)
            {
                int key = item.Key;
                if (remainCapacity >= arrayWeight[key])
                {
                    resultListItem.Add(new KeyValuePair<int, int>(arrayWeight[key], arrayValue[key]));
                    remainCapacity -= arrayWeight[key];
                }
                if (remainCapacity == 0)
                {
                    break;
                }
            }
            return resultListItem;
        }
    }
}
