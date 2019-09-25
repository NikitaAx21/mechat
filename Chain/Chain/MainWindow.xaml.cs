using System.Windows;
using System.Windows.Input;

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
            Panel.SelectedObject = new Joint();
        }
    }
}