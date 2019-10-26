using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace Chain
{
	/// <summary>
	/// Логика взаимодействия для Panel.xaml
	/// </summary>
	public partial class Panel : UserControl, INotifyPropertyChanged
	{
		public Panel()
		{
			InitializeComponent();
		}

		private bool _isPanelVisible;

		public bool IsPanelVisible
		{
			get => _isPanelVisible;
			set
			{
				_isPanelVisible = value;
				NotifyPropertyChanged(() => IsPanelVisible);
			}
		}

		public Joint SelectedJoint => SelectedObject as Joint;
		public Segment SelectedSegment => SelectedObject as Segment;
		private Object _selectedObject;

		public Object SelectedObject
		{
			get => _selectedObject;
			set
			{
				if (_selectedObject != null)
					_selectedObject.Visual.IsSelected = false;
				_selectedObject = value;
				if (_selectedObject != null)
					_selectedObject.Visual.IsSelected = true;
				IsPanelVisible = value != null;
				NotifyPropertyChanged(() => SelectedObject);
				NotifyPropertyChanged(() => SelectedSegment);
				NotifyPropertyChanged(() => SelectedJoint);
			}
		}

		private void ShowHideMenu(object sender, RoutedEventArgs e)
		{
			if (SelectedObject == null)
				return;

			IsPanelVisible = !IsPanelVisible;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
		{
			var propertyName = ((MemberExpression)property.Body).Member.Name;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}