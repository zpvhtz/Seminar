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
        List<int> numberOfBags = new List<int>(); //Mảng chứa số lượng túi

        GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm();
        DynamicAlgorithm dynamicAlgorithm = new DynamicAlgorithm();
        RecursiveAlgorithm recursiveAlgorithm = new RecursiveAlgorithm();
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

            return PartialView("pResultTable");
        }
        public void RandomAlgorithm()
        {
            int sltemp;
            int.TryParse(Math.Round(Math.Sqrt(double.Parse(int.MaxValue.ToString())), 0).ToString(), out sltemp); //random W cho túi lớn

            Random rnd = new Random();
            W = rnd.Next(1, sltemp);

            for (int i = 10; i <= 10000; i *= 10)
            {
                n = i;
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();

                //Dynamic (quy hoạch động)
                watch = Stopwatch.StartNew();
                dynamicAlgorithm.HandlingDynamicPlanning(W, n, ref a, listtl, listgt, ref mangtmp, ref GT);
                watch.Stop();
                dynamicTimeList.Add(watch.ElapsedMilliseconds);
                

                //Greedy (tham lam)
                watch = Stopwatch.StartNew();
                List<KeyValuePair<int, int>> listResultGreedy = listResultGreedy = greedyAlgorithm.KnapsackGreedyProgramming(listtltemp, listgttemp, W);
                watch.Stop();
                greedyTimeList.Add(watch.ElapsedMilliseconds);

                //Recursive (vét cạn)
                //watch = Stopwatch.StartNew();
                //int temp = recursiveAlgorithm.KnapSackRecursive(listtltemp, listgttemp, W, i);
                
                //watch.Stop();
                //recursiveTimeList.Add(watch.ElapsedMilliseconds);

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