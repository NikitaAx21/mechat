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
			}
		}

		protected Object()
		{
			IsMassCenterVisible = false;
			Mass = 10;
			Id = 0;
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

	public class Segment : Object
	{
		public double Length
		{
			get => _length;
			set
			{
				_length = value;
				NotifyPropertyChanged(() => Length);
			}
		}

		public Point Vector; //пересчёт из длины и угла предыдущего сустава
		private double _length;
		private bool _visibility;
		private bool _efemerik;

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

		public Segment()
		{
			Visual = new VisualSegment(this);

			Vector.X = 0;
			Vector.Y = 2;
			Length = 20;

			Visibility = true;
			Efemerik = false;
		}
	}

	public class Joint : Object
	{
		private double _angleRestrictionLeft;
		private double _angleRestrictionRight;
		private double _currentAngle;
		private bool _isAngleRestricted;

		public double CurrentAngle
		{
			get => _currentAngle;
			set
			{
				_currentAngle = value;
				NotifyPropertyChanged(() => CurrentAngle);
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
		public Joint()
		{
			Visual = new VisualJoint(this);
			CurrentAngle = 0;

			IsAngleRestricted = false;

			AngleRestrictionLeft = -180;
			AngleRestrictionRight = 180;
		}
	}
}