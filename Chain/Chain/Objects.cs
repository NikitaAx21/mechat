using System.Windows;

namespace Chain
{
    public abstract class Object
    {
        public bool IsMassCenterVisible { get; set; }
        public double Mass { get; set; }

        protected Object()
        {
            IsMassCenterVisible = false;
            Mass = 10;
        }
    }

    public class Segment : Object
    {
        public double Length { get; set; }

        public Point Vector; //пересчёт из длины и угла предыдущего сустава

        public bool Visibility { get; set; }
        public bool Efemerik { get; set; }

        public Segment()
        {
            Vector.X = 0;
            Vector.Y = 2;
            Length = 1;

            Visibility = true;
            Efemerik = false;
        }
    }

    public class Joint : Object
    {
        public double CurrentAngle { get; set; }

        public bool IsAngleRestricted { get; set; }
        public double AngleRestrictionLeft { get; set; }
        public double AngleRestrictionRight { get; set; }

        public Joint()
        {
            CurrentAngle = 0;

            IsAngleRestricted = false;

            AngleRestrictionLeft = 0;
            AngleRestrictionRight = 0;
        }
    }
}