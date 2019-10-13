using System.Windows;

namespace Chain
{
	public class Matrix
	{
		private double _dX;
		private double _dY;
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
		public Point LabCoord(TransformationMatrix tM, CoordinatesMatrix cM)
		{
			var res = new Point
			{
				X = tM.Matrix[0, 0] * cM.Vector[0] + tM.Matrix[0, 1] * cM.Vector[1] + tM.Matrix[0, 2] * cM.Vector[2],
				Y = tM.Matrix[1, 0] * cM.Vector[0] + tM.Matrix[1, 1] * cM.Vector[1] + tM.Matrix[1, 2] * cM.Vector[2]
			};
			tM.X = res.X;
			tM.Y = res.Y;
			return res;
		}
	}
}