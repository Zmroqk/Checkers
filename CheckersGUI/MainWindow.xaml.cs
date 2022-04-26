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

namespace CheckersGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int BoardSize = 8;
        FieldGUI[][] GridFields;
        GameManager GameManager;
        Board CheckersBoard;

        public MainWindow()
        {
            InitializeComponent();
            GameManager = new GameManager();
            CheckersBoard = GameManager.Board;
            GenerateBoard();
            //Task.Run(() =>
            //{
                
            //});       
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
            IHeuristic heuristic = new SimpleHeuristic(CheckersBoard);
            GameManager.SetPlayerBlack(new AIPlayer(new MinMaxAlgorithm(1, CheckersBoard, heuristic), CheckersBoard));
            GameManager.SetPlayerWhite(new AIPlayer(new MinMaxAlgorithm(1, CheckersBoard, heuristic), CheckersBoard));
        }
    }
}
