using System.Windows;

namespace Chain
{
    public class Object
    {
        public bool IsMassCenterVisible;
        public double Mass;

        public Object()
        {
            IsMassCenterVisible = false;
            Mass = 0;
        }
    }

    public class Segment : Object
    {
        //public int length;

        public Point Vector; //пересчёт из длины и угла предыдущего сустава


        public bool Visibility;
        public bool Efemerik;

        public Segment()
        {
            //length=2;
            Vector.X = 0;
            Vector.Y = 2;

            Visibility = true;
            Efemerik = false;
        }
    }

    internal class Joint : Object
    {
        public double CurrentAngle;

        public bool IsAngleRestricted;
        public double AngleRestrictionLeft;
        public double AngleRestrictionRight;

        public Joint()
        {
            CurrentAngle = 0;

            IsAngleRestricted = false;

            AngleRestrictionLeft = 0;
            AngleRestrictionRight = 0;
        }
    }
}