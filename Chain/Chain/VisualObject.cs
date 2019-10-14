using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Controls;

namespace Chain
{
	public abstract class VisualObject : UserControl, INotifyPropertyChanged
	{
		private bool _isSelected;
		public virtual event Action<VisualObject> OnSelectedChanged;

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				NotifyPropertyChanged(() => IsSelected);
			}
		}

		public bool Interseced { get; set; }

		public Object ParentObject { get; set; }

		public VisualObject()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				string propertyName = ((MemberExpression)property.Body).Member.Name;
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}