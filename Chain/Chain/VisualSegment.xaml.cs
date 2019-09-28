using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chain
{
    /// <summary>
    /// Логика взаимодействия для VisualJoint.xaml
    /// </summary>
    public partial class VisualSegment : UserControl
    {
        public VisualSegment(Segment parent)
        {
            ParentObject = parent;
            InitializeComponent();
        }

        public void SetPosition(double height, double width)
        {
            var halfWidth = width - ActualWidth / 2;
            var halfHeight = height - ActualHeight / 2;
            Margin = new Thickness(halfWidth, halfHeight, halfWidth, halfHeight);
        }

        public Segment ParentObject { get; set; }
        public event Action OnSelectedChanged;

        private void OnSelected(object sender, MouseButtonEventArgs e)
        {
            OnSelectedChanged?.Invoke();
        }
    }
}