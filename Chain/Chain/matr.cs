using System;
using System.Windows;

namespace Chain
{
    public static class Calc
    {
        public static double DegreeToRadian(double A)
        {
            return Math.PI * A / 180.0;
        }
        public static bool IsIntersected(Point a, Point b, Point c, Point d)
        {
            double common = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            if (common == 0) return false; //они параллельны

            double rH = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
            double sH = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);
            double r = rH / common;
            double s = sH / common;

            if (r >= 0 && r <= 1 && s >= 0 && s <= 1)
                return true;
            else
                return false;
        }
    }

    public class TransformationMatrix
    {
        public double[,] matr = new double[3, 3];
        private double curAngle = 0;
        public double X = 0; //для хранения координаты конца предыдущего сегмента
        public double Y = 0;

        public TransformationMatrix()
        {
            matr[0, 0] = 0;
            matr[0, 1] = 0;
            matr[0, 2] = 1;
            matr[1, 0] = 0;
            matr[1, 1] = 0;
            matr[1, 2] = 1;
            matr[2, 0] = 0;
            matr[2, 1] = 0;
            matr[2, 2] = 1;
        }

        public TransformationMatrix(double angle)
        {
            curAngle = curAngle + angle;
            double rad = Calc.DegreeToRadian(curAngle); //Поменять везде
            matr[0, 0] = Math.Cos(rad);
            matr[0, 1] = Math.Sin(rad);
            matr[0, 2] = X; //исправить на length 
            matr[1, 0] = -Math.Sin(rad);
            matr[1, 1] = Math.Cos(rad);
            matr[1, 2] = Y;
            matr[2, 0] = 0;
            matr[2, 1] = 0;
            matr[2, 2] = 1;
        }

    }
    public class CoordMatrix
    {
        public double[] vect = new double[3];
        public CoordMatrix()
        {
            vect[0] = 1;
            vect[1] = 1;
            vect[2] = 1;
        }
        public CoordMatrix(Segment S, Joint J)
        {
            vect[0] = S.Length * Math.Sin(Calc.DegreeToRadian(J.CurrentAngle));
            vect[1] = S.Length * Math.Cos(Calc.DegreeToRadian(J.CurrentAngle));
            vect[2] = 1;
        }


    }
    public class RotateMatrix
    {
        public double[,] matr = new double[3, 3];
        public RotateMatrix(double angle)
        {
            double rad = Calc.DegreeToRadian(angle);
            matr[0, 0] = Math.Cos(rad);
            matr[0, 1] = Math.Sin(rad);
            matr[0, 2] = 0;
            matr[1, 0] = -Math.Sin(rad);
            matr[1, 1] = Math.Cos(rad);
            matr[1, 2] = 0;
            matr[2, 0] = 0;
            matr[2, 1] = 0;
            matr[2, 2] = 1;
        }

    }

}
