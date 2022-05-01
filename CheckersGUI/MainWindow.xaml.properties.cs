using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckersGUI
{
    public partial class MainWindow
    {
        public string WinnerLabelContent
        {
            get { return _winnerLabelContent; }
            set
            {
                _winnerLabelContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WinnerLabelContent)));
            }
        }
        string _winnerLabelContent;

        public string SelectedItemWhite
        {
            get { return _selectedItemWhite; }
            set
            {
                _selectedItemWhite = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItemWhite)));
            }
        }
        string _selectedItemWhite;
        public string SelectedItemBlack
        {
            get { return _selectedItemBlack; }
            set
            {
                _selectedItemBlack = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItemBlack)));
            }
        }
        string _selectedItemBlack;

        public string SelectedAlgorithmWhite
        {
            get { return _selectedAlgorithmWhite; }
            set
            {
                _selectedAlgorithmWhite = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAlgorithmWhite)));
            }
        }
        string _selectedAlgorithmWhite;

        public string SelectedAlgorithmBlack
        {
            get { return _selectedAlgorithmBlack; }
            set
            {
                _selectedAlgorithmBlack = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAlgorithmBlack)));
            }
        }
        string _selectedAlgorithmBlack;

        public bool IsBlackAI
        {
            get { return _isBlackAI; }
            set
            {
                _isBlackAI = value;
                if(!value)
                    BlackVisibility = Visibility.Hidden;
                else
                    BlackVisibility = Visibility.Visible;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBlackAI)));
            }
        }
        bool _isBlackAI;

        public bool IsWhiteAI
        {
            get { return _isWhiteAI; }
            set
            {
                _isWhiteAI = value;
                if (!value)
                    WhiteVisibility = Visibility.Hidden;
                else
                    WhiteVisibility = Visibility.Visible;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsWhiteAI)));
            }
        }
        bool _isWhiteAI;

        public Visibility WhiteVisibility
        {
            get { return _whiteVisibility; }
            set
            {
                _whiteVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WhiteVisibility)));
            }
        }
        Visibility _whiteVisibility;

        public Visibility BlackVisibility
        {
            get { return _blackVisibility; }
            set
            {
                _blackVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlackVisibility)));
            }
        }
        Visibility _blackVisibility;
    }
}
