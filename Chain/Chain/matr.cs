using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    public static class DegreeToRad
    {
        
        public static double DegreeToRadian(double A)
        {
            return (Math.PI * A / 180.0);
        }
    }
    public class TransformationMatrix
    {
        public double[,] matr = new double[3, 3];
        double curAngle = 0;
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
            double rad = DegreeToRad.DegreeToRadian(curAngle); //Поменять везде
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
            vect[0] = S.Length * Math.Sin(DegreeToRad.DegreeToRadian(J.CurrentAngle));
            vect[1] = S.Length * Math.Cos(DegreeToRad.DegreeToRadian(J.CurrentAngle));
            vect[2] = 1;
        }
        

    }
    public class RotateMatrix
    {
        public double[,] matr = new double[3, 3];
        public RotateMatrix(double angle)
        {
            double rad = DegreeToRad.DegreeToRadian(angle);
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
