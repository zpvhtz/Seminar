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
        int[] listtltemp = new int[100];
        int[] listgttemp = new int [100];
        int[] listtl = new int[100];
        int[] listgt = new int[100];
        int GT, W=0, n=0;
        int[,] a;
        string mangtmp;
        Timing timing = new Timing();
        //Stopwatch stopwatch = new Stopwatch();

        public int max(int a, int b)
        {
            return (a > b) ? a : b;
        }
        
        public IActionResult GetTable()
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
                        a[i, j] = max(a[i - 1, j], a[i - 1, j - listtl[i]] + listgt[i]);
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
            ViewBag.Tong = GT;
            ViewBag.VatDuocChon = mangtmp;
            //stopwatch.Stop();
            //TimeSpan ts = stopwatch.Elapsed;
            ////string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            ////ViewBag.Time = elapsedTime;
            //ViewBag.Time = ts;
            timing.Stop();

            Console.WriteLine($"\r\nExecution time: {timing.Duration.Ticks} (ticks) = {timing.Duration.Milliseconds} (ms)");
            var time = timing.Duration.Ticks;
            var second = timing.Duration.Milliseconds;
            ViewBag.Time = time;
            ViewBag.Second = second;
            return PartialView("table");
        }
        public IActionResult index(string txttrongluong,string txtsoluong)
        {
            //stopwatch.Start();
            
            timing.Start();
            return View("index");
        }

        public IActionResult GetAllValue(string mangtl, string manggt, int txttrongluong, int txtsoluong)
        {
            listtltemp = JsonConvert.DeserializeObject<int[]>(mangtl);
            listgttemp = JsonConvert.DeserializeObject<int[]>(manggt);
            a = new int[txtsoluong + 1, txttrongluong + 1];
            RearrangeArray();

            this.W = txttrongluong;
            this.n = txtsoluong;

            return GetTable();
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
    }
}