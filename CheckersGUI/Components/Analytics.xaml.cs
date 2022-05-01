using CheckersEngine.Analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CheckersGUI.Components
{
    /// <summary>
    /// Interaction logic for Analytics.xaml
    /// </summary>
    public partial class Analytics : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LabelProperty
            = DependencyProperty.Register(
                  "Label",
                  typeof(string),
                  typeof(Analytics),
                  new PropertyMetadata("")
              );

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public double AverageTime
        {
            get {
                return _averageTime;
            }
            set { 
                _averageTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageTime)));
            }
        }
        double _averageTime;

        public int Nodes
        {
            get
            {
                return _nodes;
            }
            set
            {
                _nodes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nodes)));
            }
        }
        int _nodes;

        public int Moves
        {
            get
            {
                return _moves;
            }
            set
            {
                _moves = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Moves)));
            }
        }
        int _moves;

        public IAnalytics? Stats { 
            get { return _stats; } 
            set {
                if (_stats != null) _stats.AnalyticsUpdated -= Analytics_AnalyticsUpdated;
                _stats = value;
                if(_stats != null) _stats.AnalyticsUpdated += Analytics_AnalyticsUpdated; 
            } 
        }
        IAnalytics? _stats;

        public Analytics()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Analytics_AnalyticsUpdated(object? sender, EventArgs e)
        {
            Moves = Stats.Count;
            Nodes = Stats.VisitedNodes;
            AverageTime = Stats.Time / 1000 / Stats.Count;
        }
    }
}
