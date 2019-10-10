using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
            LManager = new ListManager(ChainList, Panel);

            LManager.Add();
            var center = ChainList.Last() as Joint;
            if (center == null)
                return;
            Panel.SelectedObject = center;
            Canvas.Children.Add(center.Visual);

            SL = new SaveLoad();
        }

        public List<Object> ChainList = new List<Object>();
        public ListManager LManager;
        public SaveLoad SL;

        private void SaveList(object sender, RoutedEventArgs e)
        {
            SL.Save(ChainList);
        }

        private void LoadList(object sender, RoutedEventArgs e)
        {
            SL.Load(ChainList);
        }

        private void CenterCircleSetPosition(object sender = null, RoutedEventArgs e = null)
        {
            if (ChainList.FirstOrDefault() is Joint joint)
                joint.Visual.PutOnCenter();
        }

        private void DeleteObject(object sender, RoutedEventArgs e)
        {
            foreach (var o in ChainList.Where(o => o.Id >= Panel.SelectedObject.Id))
            {
                switch (o)
                {
                    case Joint a:
                        Canvas.Children.Remove(a.Visual);
                        break;
                    case Segment b:
                        Canvas.Children.Remove(b.Visual);
                        break;
                }
            }

            LManager.Delete(Panel.SelectedObject.Id);
            Panel.SelectedObject = ChainList.LastOrDefault();
        }

        private void AddObject(object sender, RoutedEventArgs e)
        {
            LManager.Add();
            var obj = ChainList.Last();

            switch (obj)
            {
                case Joint joint:
                    Canvas.Children.Add(joint.Visual);
                    break;
                case Segment segment:
                    Canvas.Children.Add(segment.Visual);
                    break;
            }

            if (ChainList.Count == 1)
                CenterCircleSetPosition();
        }
    }
}