using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace Chain
{
	public abstract class Object : INotifyPropertyChanged
	{
		public int Id;
		private bool _isMassCenterVisible;
		private double _mass;

		protected Object()
		{
			IsMassCenterVisible = false;
			Mass = 10;
			Id = 0;
		}

		public bool IsMassCenterVisible
		{
			get => _isMassCenterVisible;
			set
			{
				_isMassCenterVisible = value;
				NotifyPropertyChanged(() => IsMassCenterVisible);
			}
		}

		public VisualObject Visual { get; set; }

		public double Mass
		{
			get => _mass;
			set
			{
				_mass = value;
				NotifyPropertyChanged(() => Mass);
				OnObjectChanged();
			}
		}

		public void OnObjectChanged()
		{
			ObjectChanged?.Invoke(this);
		}

		public event Action<Object> ObjectChanged;
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

	public class Segment : Object
	{
		private double _length;
		private bool _visibility;
		private bool _efemerik;

		public Segment()
		{
			Visual = new VisualSegment(this);

			Length = 20;

			Visibility = true;
			Efemerik = false;
		}

		public double Length
		{
			get => _length;
			set
			{
				_length = value;
				NotifyPropertyChanged(() => Length);
				OnObjectChanged();
			}
		}

		public bool Visibility
		{
			get => _visibility;
			set
			{
				_visibility = value;
				NotifyPropertyChanged(() => Visibility);
			}
		}

		public bool Efemerik
		{
			get => _efemerik;
			set
			{
				_efemerik = value;
				NotifyPropertyChanged(() => Efemerik);
			}
		}

		//public override event Action<Object> ObjectChanged;
	}

	public class Joint : Object
	{
		private double _angleRestrictionLeft;
		private double _angleRestrictionRight;
		private double _currentAngle;
		private bool _isAngleRestricted;

		public Joint()
		{
			Visual = new VisualJoint(this);

			CurrentAngle = 0;
			IsAngleRestricted = false;
		}

		public double CurrentAngle
		{
			get => _currentAngle;
			set
			{
				_currentAngle = value;
				NotifyPropertyChanged(() => CurrentAngle);
				OnObjectChanged();
			}
		}

		public bool IsAngleRestricted
		{
			get => _isAngleRestricted;
			set
			{
				_isAngleRestricted = value;
				if (!_isAngleRestricted)
				{
					AngleRestrictionLeft = -180d;
					AngleRestrictionRight = 180d;
				}

				NotifyPropertyChanged(() => IsAngleRestricted);
			}
		}

		public double AngleRestrictionLeft
		{
			get => _angleRestrictionLeft;
			set
			{
				_angleRestrictionLeft = value;
				NotifyPropertyChanged(() => AngleRestrictionLeft);
			}
		}

		public double AngleRestrictionRight
		{
			get => _angleRestrictionRight;
			set
			{
				_angleRestrictionRight = value;
				NotifyPropertyChanged(() => AngleRestrictionRight);
			}
		}
	}
}