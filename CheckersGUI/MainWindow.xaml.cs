using System;
using System.Collections.Generic;
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
using CheckersGUI.Components;
using CheckersEngine;
using CheckersEngine.Players;
using CheckersEngine.Heuristics;
using System.ComponentModel;

namespace CheckersGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string WinnerLabelContent
        {
            get { return _winnerLabelContent; }
            set { 
                _winnerLabelContent = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WinnerLabelContent))); 
            }
        }
        string _winnerLabelContent;

        int BoardSize = 8;
        FieldGUI[][] GridFields;
        GameManager GameManager;
        Board CheckersBoard;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void GenerateBoard()
        {
            GridFields = new FieldGUI[BoardSize][];
            for(int i = 0; i < BoardSize; i++)
            {
                GridFields[i] = new FieldGUI[BoardSize];
                Color currentColor;
                if (i % 2 == 0)
                    currentColor = Colors.White;
                else
                    currentColor = Colors.Black;
                for (int j = 0; j < BoardSize; j++)
                {
                    GridFields[i][j] = new FieldGUI(currentColor, CheckersBoard[i, j]);
                    if (currentColor == Colors.White)
                        currentColor = Colors.Black;
                    else
                        currentColor = Colors.White;
                    Grid.SetRow(GridFields[i][j], i);
                    Grid.SetColumn(GridFields[i][j], j);
                    Board.Children.Add(GridFields[i][j]);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WinnerLabelContent = "";
            if(GameManager != null)
            {
                GameManager.EndGame();
            }
            GameManager = new GameManager();
            CheckersBoard = GameManager.Board;
            if(GridFields != null)
            {
                for(int i = 0; i < BoardSize; i++)
                {
                    for (int j = 0; j < BoardSize; j++)
                    {
                        Board.Children.Remove(GridFields[i][j]);
                    }
                }
            }           
            GenerateBoard();
            CheckersBoard.OnWinnerAnnouncement += CheckersBoard_OnWinnerAnnouncement;
            short blackLevel = short.Parse(txbBlack.Text);
            short whiteLevel = short.Parse(txbWhite.Text);
            IHeuristic heuristic = new SimpleHeuristic(CheckersBoard);
            GameManager.SetPlayerBlack(new AIPlayer(new MinMaxAlgorithm(blackLevel, CheckersBoard, heuristic), CheckersBoard));
            GameManager.SetPlayerWhite(new AIPlayer(new MinMaxAlgorithm(whiteLevel, CheckersBoard, heuristic), CheckersBoard));
            Task.Run(() => {
                GameManager.StartGame();
            });
        }

        private void CheckersBoard_OnWinnerAnnouncement(object? sender, IPlayer e)
        {
            if (e == null)
                WinnerLabelContent = "Draw";
            else
                WinnerLabelContent = $"{(e.Color == CheckersColor.White ? "White" : "Black")} wins";
        }
    }
}
