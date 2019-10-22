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
			var x = Coordinate.X - 15.0 / 2;
			var y = Coordinate.Y - 15.0 / 2;
			Margin = new Thickness(x, y, x, y);
		}

		public void PutOnCenter()
		{
			var canvas = VisualParent as Canvas;
			if (canvas == null)
				return;

			var y = canvas.ActualHeight / 2;
			var x = canvas.ActualWidth / 2;
			Coordinate = new Point(x, y);
		}

		public Point GetCurrentCanvasParameters()
		{
			var res = new Point();
			if (!(VisualParent is Canvas canvas))
				return res;

			res.X = canvas.ActualWidth;
			res.Y = canvas.ActualHeight;

			return res;
		}

		public override event Action<VisualObject> OnSelectedChanged;

		private void OnSelected(object sender, MouseButtonEventArgs e)
		{
			OnSelectedChanged?.Invoke(this);
		}
	}
}