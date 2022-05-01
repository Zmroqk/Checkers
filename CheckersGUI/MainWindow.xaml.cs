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
using CheckersGUI.Players;
using CheckersEngine.Analytics;

namespace CheckersGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        int BoardSize = 8;
        FieldGUI[][] GridFields;
        GameManager GameManager;
        Board CheckersBoard;

        string[] heuristics = new string[2] { "Simple", "Area" };
        string[] algorithms = new string[1] { "MinMax" };

        public event PropertyChangedEventHandler? PropertyChanged;
       
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            cmbHeuristicBlack.ItemsSource = heuristics;
            cmbHeuristicWhite.ItemsSource = heuristics;
            cmbAIAlgorithmBlack.ItemsSource = algorithms;
            cmbAIAlgorithmWhite.ItemsSource = algorithms;
            SelectedItemBlack = heuristics[0];
            SelectedItemWhite = heuristics[0];
            SelectedAlgorithmBlack = algorithms[0];
            SelectedAlgorithmWhite = algorithms[0];
            IsWhiteAI = true;
            IsBlackAI = true;
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
            IHeuristic blackHeuristic;
            IHeuristic whiteHeuristic;
            IPlayer blackPlayer;
            IPlayer whitePlayer;
            if (SelectedItemBlack == "Simple")
            {
                blackHeuristic = new SimpleHeuristic(CheckersBoard);
            }
            else
            {
                blackHeuristic = new AreaHeurestic(CheckersBoard);
            }
            if (SelectedItemWhite == "Simple")
            {
                whiteHeuristic = new SimpleHeuristic(CheckersBoard);
            }
            else
            {
                whiteHeuristic = new AreaHeurestic(CheckersBoard);
            }
            if (IsWhiteAI)
            {
                IAnalytics analytics = new SimpleAnalytics();
                anWhite.Stats = analytics;
                whitePlayer = new AIPlayer(new MinMaxAlgorithm(whiteLevel, CheckersBoard, whiteHeuristic, analytics), CheckersBoard);
            }
            else
            {
                whitePlayer = new Player(GridFields);
            }
            if (IsBlackAI)
            {
                IAnalytics analytics = new SimpleAnalytics();
                anBlack.Stats = analytics;
                blackPlayer = new AIPlayer(new MinMaxAlgorithm(blackLevel, CheckersBoard, blackHeuristic, analytics), CheckersBoard);
            }
            else
            {
                blackPlayer = new Player(GridFields);
            }
            GameManager.SetPlayerBlack(blackPlayer);
            GameManager.SetPlayerWhite(whitePlayer);
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
