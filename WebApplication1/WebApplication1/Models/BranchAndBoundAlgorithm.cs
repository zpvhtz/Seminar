using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class BranchAndBoundAlgorithm
    {
        public struct Item
        {
            public int weight;
            public int value;
            public float profit;
        };

        public struct Node
        {
            public int level, bound, profit, weight;
        };

        public int Bound(Node u, int n, int weightMax, List<Item> arr)
        {
            if (u.weight >= weightMax)
            {
                return 0;
            }
            int profit_bound = u.profit;

            int j = u.level + 1;
            int totweight = u.weight;

            while ((j < n) && ((totweight + arr[j].weight) <= weightMax))
            {
                totweight += arr[j].weight;
                profit_bound += arr[j].value;
                j++;
            }
            if (j < n)
            {
                profit_bound += (weightMax - totweight) * arr[j].value / arr[j].weight;
            }
            return profit_bound;
        }
        public int KPBranchAndBound(int[] arrayWeight, int[] arrayValue, int weightMax)
        {
            int n = arrayValue.Length;
            List<Item> arr = new List<Item>();
            for (int i = 0; i < n; i++)
            {
                Item item = new Item();
                item.weight = arrayWeight[i];
                item.value = arrayValue[i];
                item.profit = (float)arrayValue[i] / arrayWeight[i];
                arr.Add(item);
            }
            Console.WriteLine("-----------------------------------------------KnapsackBranchAndBoundProgramming-----------------------------------------------");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            arr = arr.OrderByDescending(m => m.profit).ToList();
            Queue<Node> Q = new Queue<Node>();
            Node u, v;
            u = new Node();
            v = new Node();
            u.level = -1;
            u.profit = 0;
            u.weight = 0;
            Q.Enqueue(u);

            int maxProfit = 0;
            while (Q.Any())
            {
                u = Q.Peek();
                Q.Dequeue();

                if (u.level == -1)
                {
                    v.level = 0;
                }

                if (u.level == (n - 1))
                {
                    continue;
                }

                v.level = u.level + 1;

                v.weight = u.weight + arr[v.level].weight;
                v.profit = u.profit + arr[v.level].value;

                if (v.weight <= weightMax && v.profit > maxProfit)
                {
                    maxProfit = v.profit;
                }

                v.bound = Bound(v, n, weightMax, arr);

                if (v.bound > maxProfit)
                {
                    Q.Enqueue(v);
                }

                v.weight = u.weight;
                v.profit = u.profit;
                v.bound = Bound(v, n, weightMax, arr);
                if (v.bound > maxProfit)
                {
                    Q.Enqueue(v);
                }
            }
            watch.Stop();
            var elapsedMsBranchAndBound = watch.ElapsedMilliseconds;
            Console.WriteLine("--------------------Elapse time by Branch And Bound Programing: " + elapsedMsBranchAndBound + "ms");
            return maxProfit;
        }
    }
}
