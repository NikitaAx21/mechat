using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chain
{
	/// <summary>
	/// Логика взаимодействия для VisualJoint.xaml
	/// </summary>
	public partial class VisualJoint : VisualObject
	{
		private Point _coordinate;

		public VisualJoint(Joint parent)
		{
			ParentObject = parent;
			InitializeComponent();
		}

		public Point Coordinate
		{
			get => _coordinate;
			set
			{
				_coordinate = value;
				SetPosition();
			}
		}

		public void SetPosition()
		{
			var halfWidth = Coordinate.Y - ActualWidth / 2;
			var halfHeight = Coordinate.X - ActualHeight / 2;
			Margin = new Thickness(halfWidth, halfHeight, halfWidth, halfHeight);
		}

		public void PutOnCenter()
		{
			var canvas = VisualParent as Canvas;
			if (canvas == null)
				return;

			var halfWidth = canvas.ActualWidth / 2;
			var halfHeight = canvas.ActualHeight / 2;
			Coordinate = new Point(halfHeight, halfWidth);
		}

		public override event Action<VisualObject> OnSelectedChanged;

		private void OnSelected(object sender, MouseButtonEventArgs e)
		{
			OnSelectedChanged?.Invoke(this);
		}
	}
}