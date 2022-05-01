using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Analytics
{
    public interface IAnalytics
    {
        public int Count { get; set; }
        public double Time { get; set; }
        public int VisitedNodes { get; set; }
        public event EventHandler AnalyticsUpdated;
    }
}
