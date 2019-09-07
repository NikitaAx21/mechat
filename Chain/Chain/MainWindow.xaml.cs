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

namespace Chain
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CenterCircleSetPosition(object sender, RoutedEventArgs e)
        {
            var halfWidth = Canvas.ActualWidth / 2 - CenterCircle.ActualWidth / 2;
            var halfHeight = Canvas.ActualHeight / 2 - CenterCircle.ActualHeight / 2;
            CenterCircle.Margin = new Thickness(halfWidth, halfHeight, halfWidth, halfHeight);
        }

        private void OnCenterClicked(object sender, MouseButtonEventArgs e)
        {
        }
    }
}