using System.Windows;

namespace Chain
{
    class Matrix
    {
        private double dX;
        private double dY;
        public double[,] matr = new double[3, 3];
        public double[] vect = new double[3];

        /// <summary>
        /// Метод для вычисления координат в Лаб.С.К.
        ///В метод передается матрица преобразования и вектор координат в собственной с.к.
        ///Возвращается Point из двух элементов[x, y]
        /// </summary>
        /// <param name="tM"></param>
        /// <param name="cM"></param>
        /// <returns></returns>
        public Point LabCoord(TransformationMatrix tM, CoordMatrix cM)
        {
            Point res = new Point();
            res.X = tM.matr[0, 0] * cM.vect[0] + tM.matr[0, 1] * cM.vect[1] + tM.matr[0, 2] * cM.vect[2];
            res.Y = tM.matr[1, 0] * cM.vect[0] + tM.matr[1, 1] * cM.vect[1] + tM.matr[1, 2] * cM.vect[2];
            tM.X = res.X;
            tM.Y = res.Y;
            return res;
        }

    }

}
