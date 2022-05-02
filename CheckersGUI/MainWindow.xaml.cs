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

        string[] heuristics = new string[] { "Simple", "Area", "MixedHeuristic (0.6x Simple, 0.4x Area)", "MixedHeuristic (0.4x Simple, 0.6x Area)" };
        string[] algorithms = new string[] { "MinMax", "AlphaBetaPruning" };

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
            IHeuristic blackHeuristic = SelectHeuristic(CheckersColor.Black);
            IHeuristic whiteHeuristic = SelectHeuristic(CheckersColor.White);
            IAIAlgorithm blackAIAlgorithm = SelectAlgorithm(CheckersColor.Black, blackHeuristic);
            IAIAlgorithm whiteAIAlgorithm = SelectAlgorithm(CheckersColor.White, whiteHeuristic);
            IPlayer blackPlayer;
            IPlayer whitePlayer;          
            if (IsWhiteAI)
            {
                whitePlayer = new AIPlayer(whiteAIAlgorithm, CheckersBoard);
            }
            else
            {
                whitePlayer = new Player(GridFields);
            }
            if (IsBlackAI)
            {
                blackPlayer = new AIPlayer(blackAIAlgorithm, CheckersBoard);
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

        IHeuristic SelectHeuristic(CheckersColor Color)
        {
            IHeuristic heuristic;
            if (Color == CheckersColor.White)
            {
                if (SelectedItemWhite == heuristics[0])
                {
                    heuristic = new SimpleHeuristic(CheckersBoard);
                }
                else if(SelectedItemWhite == heuristics[1])
                {
                    heuristic = new AreaHeurestic(CheckersBoard);
                }
                else if(SelectedItemWhite == heuristics[2])
                {
                    SimpleHeuristic simpleHeuristic = new SimpleHeuristic(CheckersBoard);
                    AreaHeurestic areaHeurestic = new AreaHeurestic(CheckersBoard);
                    heuristic = new MixedHeuristic(new (IHeuristic heuristic, double weight)[] { (simpleHeuristic, 0.6d), (areaHeurestic, 0.4d) });
                }
                else
                {
                    SimpleHeuristic simpleHeuristic = new SimpleHeuristic(CheckersBoard);
                    AreaHeurestic areaHeurestic = new AreaHeurestic(CheckersBoard);
                    heuristic = new MixedHeuristic(new (IHeuristic heuristic, double weight)[] { (simpleHeuristic, 0.4d), (areaHeurestic, 0.6d) });
                }
            }
            else
            {
                if (SelectedItemBlack == heuristics[0])
                {
                    heuristic = new SimpleHeuristic(CheckersBoard);
                }
                else if (SelectedItemBlack == heuristics[1])
                {
                    heuristic = new AreaHeurestic(CheckersBoard);
                }
                else if(SelectedItemBlack == heuristics[2])
                {
                    SimpleHeuristic simpleHeuristic = new SimpleHeuristic(CheckersBoard);
                    AreaHeurestic areaHeurestic = new AreaHeurestic(CheckersBoard);
                    heuristic = new MixedHeuristic(new (IHeuristic heuristic, double weight)[] { (simpleHeuristic, 0.6d), (areaHeurestic, 0.4d) });
                }
                else
                {
                    SimpleHeuristic simpleHeuristic = new SimpleHeuristic(CheckersBoard);
                    AreaHeurestic areaHeurestic = new AreaHeurestic(CheckersBoard);
                    heuristic = new MixedHeuristic(new (IHeuristic heuristic, double weight)[] { (simpleHeuristic, 0.4d), (areaHeurestic, 0.6d) });
                }
            }
            return heuristic;
        }

        IAIAlgorithm SelectAlgorithm(CheckersColor Color, IHeuristic heuristic)
        {
            IAIAlgorithm algorithm;
            IAnalytics analytics = new SimpleAnalytics();
            if (Color == CheckersColor.White)
            {
                anWhite.Stats = analytics;
                short whiteLevel = short.Parse(txbWhite.Text);
                if (SelectedAlgorithmWhite == "MinMax")
                {                    
                    algorithm = new MinMaxAlgorithm(whiteLevel, CheckersBoard, heuristic, analytics);
                }
                else
                {
                    algorithm = new AlfaBetaPruningAlgorithm(whiteLevel, CheckersBoard, heuristic, analytics);
                }
            }
            else
            {
                anBlack.Stats = analytics;
                short blackLevel = short.Parse(txbBlack.Text);
                if (SelectedAlgorithmBlack == "MinMax")
                {
                    algorithm = new MinMaxAlgorithm(blackLevel, CheckersBoard, heuristic, analytics);
                }
                else
                {
                    algorithm = new AlfaBetaPruningAlgorithm(blackLevel, CheckersBoard, heuristic, analytics);
                }
            }
            return algorithm;
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
