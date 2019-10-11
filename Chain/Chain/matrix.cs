using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chain
{
    class Matrix
    {
        double dX;
        double dY;
        public double[,] matr = new double[3, 3];
        public double[] vect= new double[3];
        
        /*Метод для вычисления координат в Лаб.С.К.
          В метод передается матрица преобразования и вектор координат в собственной с.к.
          Возвращается вектор из двух элементов [x,y]
        */
        //Поменять на Point
        public Point LabCoord(TransformationMatrix tM, CoordMatrix cM)
        {
            Point res = new Point();
            res.X = tM.matr[0, 0] * cM.vect[0] + tM.matr[0, 1] * cM.vect[1] + tM.matr[0, 2] * cM.vect[2];
            res.Y = tM.matr[1, 0] * cM.vect[0] + tM.matr[1, 1] * cM.vect[1] + tM.matr[1, 2] * cM.vect[2];
            tM.X = res.X;
            tM.Y = res.Y;
            return res;
        }
        public Matrix(double x, double y)
        {
            dX = x;
            dY = y;
        }

        /*Метод для поворота всех элементов на заданный угол a.
          Считает координату (x,y)
          Метод пока ничего не возвращает 
        */
        public void Rotate(List<Object> list, double a)  
        {
            RotateMatrix rM;
            rM = new RotateMatrix(a);
            for (int i = 0; i < list.Count; i++)
            {
                var S = list[i] as Segment;
                if (S != null)
                {
                    S.Vector.X = S.Vector.X * rM.matr[0, 0] + S.Vector.Y * rM.matr[0, 1];
                    S.Vector.Y = S.Vector.X * rM.matr[1, 0] + S.Vector.Y * rM.matr[1, 1];
                }
            }
        }

    }

}
