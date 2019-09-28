using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chain
{
    /// <summary>
    /// Логика взаимодействия для VisualJoint.xaml
    /// </summary>
    public partial class VisualJoint : UserControl
    {
        public VisualJoint(Joint parent)
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

        public Joint ParentObject { get; set; }
        public void PutOnCenter()
        {
            var canvas = VisualParent as Canvas;
            if (canvas == null)
                return;

            var halfWidth = canvas.ActualWidth / 2;
            var halfHeight = canvas.ActualHeight / 2;
            SetPosition(halfHeight, halfWidth);
        }

        public event Action OnSelectedChanged;

        private void OnSelected(object sender, MouseButtonEventArgs e)
        {
            OnSelectedChanged?.Invoke();
        }
    }
}