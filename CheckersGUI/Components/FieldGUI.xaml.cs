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
using CheckersEngine;
using System.Diagnostics;

namespace CheckersGUI.Components
{
    /// <summary>
    /// Interaction logic for Field.xaml
    /// </summary>
    public partial class FieldGUI : UserControl, INotifyPropertyChanged
    {
        public Field FieldRef;

        public Brush PieceColor { 
            get
            {
                return _pieceColor;
            } 
            set
            {
                _pieceColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PieceColor)));
            }
        }
        Brush _pieceColor;

        public Brush PieceColorBorder
        {
            get
            {
                return _pieceColorBorder;
            }
            set
            {
                _pieceColorBorder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PieceColorBorder)));
            }
        }
        Brush _pieceColorBorder;

        public Brush PieceColorQueen
        {
            get
            {
                return _pieceColorQueen;
            }
            set
            {
                _pieceColorQueen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PieceColorQueen)));
            }
        }
        Brush _pieceColorQueen;

        public int HighlightThinkness
        {
            get
            {
                return _highlightThinkness;
            }
            set
            {
                _highlightThinkness = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HighlightThinkness)));
            }
        }
        int _highlightThinkness;

        public Move CurrentMove { get; set; }
        public int MoveIndex { get; set; }

        public FieldGUI()
        {
            InitializeComponent();
            DataContext = this;
            PieceColor = new SolidColorBrush(Colors.Gray);
        }

        public FieldGUI(Color color, Field fieldRef) : this()
        {
            if (color == Colors.White)
                btn.IsHitTestVisible = false;
            border.Background = new SolidColorBrush(color);
            FieldRef = fieldRef;
            FieldRef.Board.BoardChanged += Board_BoardChanged;
        }

        private void Board_BoardChanged(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (FieldRef.Piece != null)
                {
                    PieceColorBorder = new SolidColorBrush(Colors.Gray);
                    if (FieldRef.Piece.Color == CheckersColor.White)
                        PieceColor = new SolidColorBrush(Color.FromArgb(255, 210, 210, 210));
                    else
                        PieceColor = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));

                    if (FieldRef.Piece.IsQueen)
                    {
                        PieceColorQueen = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    }
                    else
                    {
                        PieceColorQueen = new SolidColorBrush(Color.FromArgb(a: 0, 0, 0, 0));
                    }
                }
                else
                {
                    PieceColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    PieceColorBorder = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }
                    
            });
        }

        public void SetColor(CheckersEngine.CheckersColor color, bool isQueen)
        {
            PieceColorBorder = new SolidColorBrush(Colors.Gray);
            if (color == CheckersColor.White)
                PieceColor = new SolidColorBrush(Color.FromArgb(255, 210, 210, 210));
            else
                PieceColor = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));

            if (isQueen)
            {
                PieceColorQueen = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
            else
            {
                PieceColorQueen = new SolidColorBrush(Color.FromArgb(a: 0, 0, 0, 0));
            }
        }

        public void SetTransparent()
        {
            PieceColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            PieceColorQueen = new SolidColorBrush(Color.FromArgb(a: 0, 0, 0, 0));
            PieceColorBorder = new SolidColorBrush(Color.FromArgb(a: 0, 0, 0, 0));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<FieldGUI> ButtonClicked;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(this, this);
        }
    }
}
