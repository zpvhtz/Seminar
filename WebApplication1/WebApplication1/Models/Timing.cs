using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Timing
    {
        TimeSpan start = new TimeSpan(0);
        TimeSpan duration = new TimeSpan(0);
        public TimeSpan Duration => duration;
        public void Start()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            start = Process.GetCurrentProcess().Threads[0].UserProcessorTime;
        }
        public void Stop()
        {
            duration = Process.GetCurrentProcess().Threads[0].UserProcessorTime.Subtract(start);
        }
    }
}
