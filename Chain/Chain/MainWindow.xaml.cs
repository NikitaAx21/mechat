using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chain
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            IsPanelVisible = true;
            InitializeComponent();
        }

        public bool IsPanelVisible { get ; set; }

        public Object SelectedObject
        {
            get => new Object(); //костыль. пока не будет нормальный выбор элементов
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

        private void ShowHideMenu(object sender, RoutedEventArgs e)
        {
            if (SelectedObject == null)
                return;

            else
            {
                IsPanelVisible = !IsPanelVisible;
                OnPropertyChanged($"IsPanelVisible");
                var button = sender as Button;
                if (button == null)
                    return;
                button.Content = IsPanelVisible ? ">" : "<";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}