using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace Chain
{
    /// <summary>
    /// Логика взаимодействия для AngleSlider.xaml
    /// </summary>
    public partial class AngleSlider : UserControl, INotifyPropertyChanged
    {
        public AngleSlider()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(double),
            typeof(AngleSlider),
            new UIPropertyMetadata(-180d));

        public double Minimum
        {
            get => (double) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(double),
            typeof(AngleSlider),
            new UIPropertyMetadata(180d));

        public double Maximum
        {
            get => (double) GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty IsAngleRestrictedProperty = DependencyProperty.Register(
            "IsAngleRestricted",
            typeof(bool),
            typeof(AngleSlider),
            new UIPropertyMetadata(false));


        public bool IsAngleRestricted
        {
            get => (bool) GetValue(IsAngleRestrictedProperty);
            set => SetValue(IsAngleRestrictedProperty, value);
        }

        #region LowRestriction

        public static readonly DependencyProperty LowRestrictionProperty = DependencyProperty.Register(
            "LowRestriction",
            typeof(double),
            typeof(AngleSlider),
            new UIPropertyMetadata(-180d, LowRestrictionChanged));

        public double LowRestriction
        {
            get => (double) GetValue(LowRestrictionProperty);
            set => SetValue(LowRestrictionProperty, value);
        }

        private static void LowRestrictionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var a = d as AngleSlider;
            var value = e.NewValue as double?;
            if (a == null || value == null)
                return;

            if (value > a.UpRestriction || value > a.Value)
                a.LowRestriction = (double) e.OldValue;
        }

        #endregion

        #region UpRestriction

        public static readonly DependencyProperty UpRestrictionProperty = DependencyProperty.Register(
            "UpRestriction",
            typeof(double),
            typeof(AngleSlider),
            new UIPropertyMetadata(180d, UpRestrictionChanged));

        public double UpRestriction
        {
            get => (double) GetValue(UpRestrictionProperty);
            set => SetValue(UpRestrictionProperty, value);
        }

        private static void UpRestrictionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var a = d as AngleSlider;
            var value = e.NewValue as double?;
            if (a == null || value == null)
                return;

            if (value < a.LowRestriction || value < a.Value)
                a.UpRestriction = (double) e.OldValue;
        }

        #endregion

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(AngleSlider),
            new UIPropertyMetadata(0d, ValueChanged));

        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public int ValueAsInt => (int) Value;

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var a = d as AngleSlider;
            var value = e.NewValue as double?;
            if (a == null || value == null)
                return;

            if (value < a.LowRestriction || value > a.UpRestriction)
                a.Value = (double) e.OldValue;
            a.NotifyPropertyChanged(() => a.ValueAsInt);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                string propertyName = ((MemberExpression) property.Body).Member.Name;
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}