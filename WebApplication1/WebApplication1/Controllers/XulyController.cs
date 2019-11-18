using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnapsackProblem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class XulyController : Controller
    {
        int[] listtltemp = new int[100]; //Mảng trọng lượng tạm
        int[] listgttemp = new int [100]; //Mảng giá trị tạm
        int[] listtl = new int[100]; //Mảng trọng lượng (bắt đầu từ 1)
        int[] listgt = new int[100]; //Mảng giá trị (bắt đầu từ 1)
        int GT, W=0, n=0; //Tổng giá trị && tổng trọng lượng && số lượng vật
        int[,] a; //Bảng tính toán giá trị của quy hoạch động
        string mangtmp; //chuỗi chứa các vật được chọn
        List<Objet> objets = new List<Objet>(); //Danh sách các vật (gồm trọng lượng và giá trị)

        List<long> dynamicTimeList = new List<long>(); //Mảng chứa thời gian (Dynamic)
        List<long> greedyTimeList = new List<long>(); //Mảng chứa thời gian (Greedy)
        List<long> recursiveTimeList = new List<long>(); //Mảng chứa thời gian (Recursive)
        List<long> branchAndBoundTimeList = new List<long>(); //Mảng chứa thời gian (Branch and Bound)
        List<long> backtrackingTimeList = new List<long>(); //Mảng chứa thời gian (Back-tracking)
        List<int> numberOfBags = new List<int>(); //Mảng chứa số lượng túi

        GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm();
        DynamicAlgorithm dynamicAlgorithm = new DynamicAlgorithm();
        RecursiveAlgorithm recursiveAlgorithm = new RecursiveAlgorithm();
        BranchAndBoundAlgorithm branchAndBoundAlgorithm = new BranchAndBoundAlgorithm();
        BacktrackingAlgorithm backtrackingAlgorithm = new BacktrackingAlgorithm();
        Stopwatch watch = new Stopwatch();

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult RandomAllAlgorithm()
        {
            RandomAlgorithm();

            ViewBag.DanhSachVat = objets;
            ViewBag.Tong = GT;
            ViewBag.VatDuocChon = mangtmp;

            ViewBag.SoLuong = numberOfBags;
            ViewBag.DynamicTimeList = dynamicTimeList;
            ViewBag.GreedyTimeList = greedyTimeList;
            ViewBag.RecursiveTimeList = recursiveTimeList;
            ViewBag.BranchAndBoundTimeList = branchAndBoundTimeList;

            return PartialView("pResultTable");
        }
        public void RandomAlgorithm()
        {
            int sltemp;
            int.TryParse(Math.Round(Math.Sqrt(double.Parse(int.MaxValue.ToString())), 0).ToString(), out sltemp); //random W cho túi lớn

            Random rnd = new Random();
            W = rnd.Next(1, sltemp);

            for (int i = 0; i <= 100; i += 20)
            {
                if(i == 0)
                {
                    n = 1;
                }
                else
                {
                    n = i;
                }
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();

                //Dynamic (quy hoạch động)
                watch = Stopwatch.StartNew();
                dynamicAlgorithm.HandlingDynamicPlanning(W, n, ref a, listtl, listgt, ref mangtmp, ref GT);
                watch.Stop();
                //dynamicTimeList.Add(watch.ElapsedMilliseconds);
                long ticks = watch.ElapsedTicks;
                dynamicTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                //Greedy (tham lam)
                watch = Stopwatch.StartNew();
                List<KeyValuePair<int, int>> listResultGreedy = listResultGreedy = greedyAlgorithm.KnapsackGreedyProgramming(listtltemp, listgttemp, W);
                watch.Stop();
                //greedyTimeList.Add(watch.ElapsedMilliseconds);
                ticks = watch.ElapsedTicks;
                greedyTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                //Recursive(vét cạn)
                watch = Stopwatch.StartNew();
                int temp = recursiveAlgorithm.KnapSackRecursive(listtltemp, listgttemp, W, n);
                watch.Stop();
                //recursiveTimeList.Add(watch.ElapsedMilliseconds);
                ticks = watch.ElapsedTicks;
                recursiveTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                //Branch and Bound
                watch = Stopwatch.StartNew();
                float branchAndBoundResult = branchAndBoundAlgorithm.KPBranchAndBound(listtltemp, listgttemp, W);
                watch.Stop();
                //branchAndBoundTimeList.Add(watch.ElapsedMilliseconds);
                ticks = watch.ElapsedTicks;
                branchAndBoundTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                //Back - tracking
                //watch = Stopwatch.StartNew();
                //backtrackingAlgorithm.KPBacktracking(0, listtltemp, listgttemp, n, W);
                //backtrackingAlgorithm.PrintItemInBag(listgttemp, listtltemp, n);
                //watch.Stop();
                ////branchAndBoundTimeList.Add(watch.ElapsedMilliseconds);
                //backtrackingTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                //Lưu sl túi
                numberOfBags.Add(n);
            }
            GetAllObjects();
        }

        public IActionResult RandomDynamicAlgorithm()
        {
            DynamicAlgorithm();

            ViewBag.DanhSachVat = objets;
            ViewBag.Tong = GT;
            ViewBag.VatDuocChon = mangtmp;

            ViewBag.SoLuong = numberOfBags;
            ViewBag.DynamicTimeList = dynamicTimeList;
            //ViewBag.GreedyTimeList = greedyTimeList;
            //ViewBag.RecursiveTimeList = recursiveTimeList;

            return PartialView("pDynamicTable");
        }

        public void DynamicAlgorithm()
        {
            int sltemp;
            int.TryParse(Math.Round(Math.Sqrt(double.Parse(int.MaxValue.ToString())), 0).ToString(), out sltemp); //random W cho túi lớn

            Random rnd = new Random();
            W = rnd.Next(1, sltemp);

            for (int i = 1; i <= 10000; i *= 10)
            {
                n = i;
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();

                //Dynamic (quy hoạch động)
                watch = Stopwatch.StartNew();
                dynamicAlgorithm.HandlingDynamicPlanning(W, n, ref a, listtl, listgt, ref mangtmp, ref GT);
                watch.Stop();
                //dynamicTimeList.Add(watch.ElapsedMilliseconds);
                long ticks = watch.ElapsedTicks;
                dynamicTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                //Lưu sl túi
                numberOfBags.Add(i);
            }
            GetAllObjects();
        }

        public IActionResult RandomGreedyAlgorithm()
        {
            GreedyAlgorithm();

            ViewBag.DanhSachVat = objets;
            ViewBag.Tong = GT;
            ViewBag.VatDuocChon = mangtmp;

            ViewBag.SoLuong = numberOfBags;
            //ViewBag.DynamicTimeList = dynamicTimeList;
            ViewBag.GreedyTimeList = greedyTimeList;
            //ViewBag.RecursiveTimeList = recursiveTimeList;

            return PartialView("pGreedyTable");
        }

        public void GreedyAlgorithm()
        {
            int sltemp;
            int.TryParse(Math.Round(Math.Sqrt(double.Parse(int.MaxValue.ToString())), 0).ToString(), out sltemp); //random W cho túi lớn

            Random rnd = new Random();
            W = rnd.Next(1, sltemp);

            for (int i = 1; i <= 10000; i *= 10)
            {
                n = i;
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();

                //Greedy (tham lam)
                watch = Stopwatch.StartNew();
                List<KeyValuePair<int, int>> listResultGreedy = listResultGreedy = greedyAlgorithm.KnapsackGreedyProgramming(listtltemp, listgttemp, W);
                watch.Stop();
                //greedyTimeList.Add(watch.ElapsedMilliseconds);
                long ticks = watch.ElapsedTicks;
                greedyTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                this.GT = listResultGreedy.Sum(g => g.Value);

                string tmpArray = "";
                foreach (var item in listResultGreedy)
                {
                    tmpArray += "(" + item.Key + "," + item.Value + ") ";
                }
                this.mangtmp = tmpArray;

                //Lưu sl túi
                numberOfBags.Add(i);
            }
            GetAllObjects();
        }

        public IActionResult RandomRecursiveAlgorithm()
        {
            RecursiveAlgorithm();

            ViewBag.DanhSachVat = objets;
            ViewBag.Tong = GT;
            //ViewBag.VatDuocChon = mangtmp;

            ViewBag.SoLuong = numberOfBags;
            ViewBag.RecursiveTimeList = recursiveTimeList;

            return PartialView("pRecursiveTable");
        }

        public void RecursiveAlgorithm()
        {
            int sltemp;
            int.TryParse(Math.Round(Math.Sqrt(double.Parse(int.MaxValue.ToString())), 0).ToString(), out sltemp); //random W cho túi lớn

            Random rnd = new Random();
            W = rnd.Next(1, sltemp);

            for (int i = 0; i <= 100; i += 20)
            {
                if(i == 0)
                {
                    n = 1;
                }
                else
                {
                    n = i;
                }
                
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();

                //Recursive(vét cạn)
                watch = Stopwatch.StartNew();
                int temp = recursiveAlgorithm.KnapSackRecursive(listtltemp, listgttemp, W, n);
                watch.Stop();
                //recursiveTimeList.Add(watch.ElapsedMilliseconds);
                long ticks = watch.ElapsedTicks;
                recursiveTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                this.GT = temp;
                //Lưu sl túi
                numberOfBags.Add(n);
            }

            GetAllObjects();
        }

        public IActionResult RandomBranchAndBoundAlgorithm()
        {
            BranchAndBoundAlgorithm();

            ViewBag.DanhSachVat = objets;
            ViewBag.Tong = GT;
            //ViewBag.VatDuocChon = mangtmp;

            ViewBag.SoLuong = numberOfBags;
            ViewBag.BranchAndBoundTimeList = branchAndBoundTimeList;

            return PartialView("pBranchAndBoundTable");
        }

        public void BranchAndBoundAlgorithm()
        {
            int sltemp;
            int.TryParse(Math.Round(Math.Sqrt(double.Parse(int.MaxValue.ToString())), 0).ToString(), out sltemp); //random W cho túi lớn

            Random rnd = new Random();
            W = rnd.Next(1, sltemp);

            for (int i = 1; i <= 100; i *= 10)
            {
                n = i;
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();

                //Branch and Bound
                watch = Stopwatch.StartNew();
                int branchAndBoundResult = branchAndBoundAlgorithm.KPBranchAndBound(listtltemp, listgttemp, W);
                watch.Stop();
                //branchAndBoundTimeList.Add(watch.ElapsedMilliseconds);
                long ticks = watch.ElapsedTicks;
                branchAndBoundTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                this.GT = branchAndBoundResult;
                //Lưu sl túi
                numberOfBags.Add(i);
            }

            for (int i = 200; i <= 300; i += 100)
            {
                n = i;
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();

                //Branch and Bound
                watch = Stopwatch.StartNew();
                int branchAndBoundResult = branchAndBoundAlgorithm.KPBranchAndBound(listtltemp, listgttemp, W);
                watch.Stop();
                //branchAndBoundTimeList.Add(watch.ElapsedMilliseconds);
                long ticks = watch.ElapsedTicks;
                branchAndBoundTimeList.Add((1000000000 * ticks / Stopwatch.Frequency) / 1000);

                this.GT = branchAndBoundResult;
                //Lưu sl túi
                numberOfBags.Add(i);
            }

            GetAllObjects();
        }

        //Quy hoạch động
        //public IActionResult DynamicPlanningAlgorithm(string mangtl, string manggt, int txttrongluong, int txtsoluong)
        //{
        //    var watch = Stopwatch.StartNew();
        //    Random rnd = new Random();

        //    W = txttrongluong;
        //    n = txtsoluong;
        //    a = new int[txtsoluong + 1, txttrongluong + 1];

        //    UpdateArray(mangtl, manggt);
        //    RearrangeArray();
        //    HandlingDynamicPlanning();

        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;

        //    ViewBag.Tong = GT;
        //    ViewBag.VatDuocChon = mangtmp;
        //    ViewBag.DynamicTimeList = elapsedMs;

        //    return PartialView("pResultTable");
        //}

        public void UpdateArray(string mangtl, string manggt)
        {
            listtl = new int[n + 1];
            listgt = new int[n + 1];
            listtltemp = JsonConvert.DeserializeObject<int[]>(mangtl);
            listgttemp = JsonConvert.DeserializeObject<int[]>(manggt);
        }

        public void RandomArray()
        {
            Random rnd = new Random();
            listtl = new int[n + 1];
            listgt = new int[n + 1];
            listtltemp = new int[n];
            listgttemp = new int[n];            

            for(int i = 0; i < n; i++)
            {
                listtltemp[i] = rnd.Next(1, W > int.MaxValue ? int.MaxValue : W);
                listgttemp[i] = rnd.Next(1, 1000);
            }
        }

        public void RearrangeArray()
        {
            for(int i = 0; i < listtltemp.Length; i++)
            {
                listtl[i + 1] = listtltemp[i];
            }

            for (int i = 0; i < listgttemp.Length; i++)
            {
                listgt[i + 1] = listgttemp[i];
            }
        }

        public void GetAllObjects()
        {
            for(int i = 0; i < n; i++)
            {
                objets.Add(new Objet
                {
                    Weight = listtltemp[i],
                    Value = listtltemp[i]
                });
            }
        }
    }
}