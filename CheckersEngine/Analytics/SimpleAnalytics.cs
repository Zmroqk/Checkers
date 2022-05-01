using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersEngine.Analytics
{
    public class SimpleAnalytics : IAnalytics
    {
        public int Count { get { return _count; } set { _count = value; AnalyticsUpdated?.Invoke(this, null); } }
        private int _count;
        public double Time { get { return _time; } set { _time = value; AnalyticsUpdated?.Invoke(this, null); } }
        private double _time;
        public int VisitedNodes { get { return _visitedNodes; } set { _visitedNodes = value; AnalyticsUpdated?.Invoke(this, null); } }
        private int _visitedNodes;

        public event EventHandler AnalyticsUpdated;
    }
}
