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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PieceColor"));
            }
        }
        Brush _pieceColor;

        public FieldGUI()
        {
            InitializeComponent();
            DataContext = this;
            PieceColor = new SolidColorBrush(Colors.Gray);
        }

        public FieldGUI(Color color, Field fieldRef) : this()
        {
            border.Background = new SolidColorBrush(color);
            FieldRef = fieldRef;
            FieldRef.PieceChanged += FieldRef_PieceChanged;
            FieldRef_PieceChanged(null, FieldRef.Piece);
        }

        private void FieldRef_PieceChanged(object? sender, Piece? e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Color == CheckersColor.White)
                        PieceColor = new SolidColorBrush(Color.FromArgb(255, 210, 210, 210));
                    else
                        PieceColor = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
                }
                else
                    PieceColor = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Error");
            }
           
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PieceColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }
    }
}
