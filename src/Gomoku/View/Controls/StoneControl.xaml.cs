using Model.Gomoku;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View.Controls
{
    /// <summary>
    /// Interaction logic for StoneControl.xaml
    /// </summary>
    public partial class StoneControl : UserControl
    {
        public StoneControl()
        {
            InitializeComponent();
        }

        public Stone Owner
        {
            get { return (Stone)GetValue(OwnerProperty); }
            set { SetValue(OwnerProperty, value); }
        }

        public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register("Owner", typeof(Stone), typeof(StoneControl), new PropertyMetadata(null, OnOwnerChange));

        private static void OnOwnerChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((StoneControl)obj).Update();
        }

        public Brush WhiteBrush
        {
            get { return (Brush)GetValue(WhiteBrushProperty); }
            set { SetValue(WhiteBrushProperty, value); }
        }

        public static readonly DependencyProperty WhiteBrushProperty = DependencyProperty.Register("WhiteBrush", typeof(Brush), typeof(StoneControl), new PropertyMetadata(null, OnWhiteBrushChange));

        private static void OnWhiteBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((StoneControl)obj).Update();
        }

        public Brush BlackBrush
        {
            get { return (Brush)GetValue(BlackBrushProperty); }
            set { SetValue(BlackBrushProperty, value); }
        }

        public static readonly DependencyProperty BlackBrushProperty = DependencyProperty.Register("BlackBrush", typeof(Brush), typeof(StoneControl), new PropertyMetadata(null, OnBlackBrushChange));

        private static void OnBlackBrushChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((StoneControl)obj).Update();
        }

        private void Update()
        {
            if ( this.Owner == null )
            {
                ellipse.Visibility = Visibility.Hidden;
            }
            else if ( this.Owner == Stone.BLACK)
            {
                ellipse.Visibility = Visibility.Visible;
                ellipse.Fill = BlackBrush;
            }
            else
            {
                ellipse.Visibility = Visibility.Visible;
                ellipse.Fill = WhiteBrush;
            }
        }
    }
}
