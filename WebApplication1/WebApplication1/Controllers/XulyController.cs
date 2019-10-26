using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        Stopwatch watch = new Stopwatch();
        List<long> timeList = new List<long>(); //Mảng chứa thời gian
        List<int> numberOfBags = new List<int>(); //Mảng chứa số lượng túi

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult RandomDynamicPlanningAlgorithm(int txttrongluong, int txtsoluong)
        {
            int sltemp;
            int.TryParse(Math.Round(Math.Sqrt(double.Parse(int.MaxValue.ToString())), 0).ToString(), out sltemp); //random W cho túi lớn

            Random rnd = new Random();
            W = rnd.Next(1, sltemp);

            for (int i = 10; i <= 10000; i *= 10)
            {
                watch = Stopwatch.StartNew();

                n = i;
                a = new int[n + 1, W + 1];
                RandomArray();
                RearrangeArray();
                HandlingDynamicPlanning();

                watch.Stop();
                timeList.Add(watch.ElapsedMilliseconds);
                numberOfBags.Add(i);
            }
            GetAllObjects();

            ViewBag.DanhSachVat = objets;
            ViewBag.Tong = GT;
            ViewBag.VatDuocChon = mangtmp;
            ViewBag.SoLuong = numberOfBags;
            ViewBag.Millisecond = timeList;

            return PartialView("pResultTable");
        }

        //Quy hoạch động
        public IActionResult DynamicPlanningAlgorithm(string mangtl, string manggt, int txttrongluong, int txtsoluong)
        {
            var watch = Stopwatch.StartNew();
            Random rnd = new Random();

            W = txttrongluong;
            n = txtsoluong;
            a = new int[txtsoluong + 1, txttrongluong + 1];

            UpdateArray(mangtl, manggt);
            RearrangeArray();
            HandlingDynamicPlanning();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            ViewBag.Tong = GT;
            ViewBag.VatDuocChon = mangtmp;
            ViewBag.Milisecond = elapsedMs;

            return PartialView("pResultTable");
        }
        
        public void HandlingDynamicPlanning()
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

        public int Max(int a, int b)
        {
            return (a > b) ? a : b;
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