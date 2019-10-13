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
			var common = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

			if (common == 0) return false; //они параллельны

			var rH = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
			var sH = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);
			var r = rH / common;
			var s = sH / common;

			return r >= 0 && r <= 1 && s >= 0 && s <= 1;
		}
	}

	public class TransformationMatrix
	{
		public double[,] Matrix = new double[3, 3];
		private double _curAngle = 0;
		public double X = 0; //для хранения координаты конца предыдущего сегмента
		public double Y = 0;

		public TransformationMatrix()
		{
			Matrix[0, 0] = 0;
			Matrix[0, 1] = 0;
			Matrix[0, 2] = 1;
			Matrix[1, 0] = 0;
			Matrix[1, 1] = 0;
			Matrix[1, 2] = 1;
			Matrix[2, 0] = 0;
			Matrix[2, 1] = 0;
			Matrix[2, 2] = 1;
		}

		public TransformationMatrix(double angle)
		{
			_curAngle += angle;
			var rad = Calc.DegreeToRadian(_curAngle); //Поменять везде
			Matrix[0, 0] = Math.Cos(rad);
			Matrix[0, 1] = Math.Sin(rad);
			Matrix[0, 2] = X; //исправить на length 
			Matrix[1, 0] = -Math.Sin(rad);
			Matrix[1, 1] = Math.Cos(rad);
			Matrix[1, 2] = Y;
			Matrix[2, 0] = 0;
			Matrix[2, 1] = 0;
			Matrix[2, 2] = 1;
		}
	}

	public class CoordinatesMatrix
	{
		public double[] Vector = new double[3];

		public CoordinatesMatrix()
		{
			Vector[0] = 1;
			Vector[1] = 1;
			Vector[2] = 1;
		}

		public CoordinatesMatrix(Segment s, Joint j)
		{
			Vector[0] = s.Length * Math.Sin(Calc.DegreeToRadian(j.CurrentAngle));
			Vector[1] = s.Length * Math.Cos(Calc.DegreeToRadian(j.CurrentAngle));
			Vector[2] = 1;
		}
	}

	public class RotateMatrix
	{
		public double[,] Matrix = new double[3, 3];

		public RotateMatrix(double angle)
		{
			var rad = Calc.DegreeToRadian(angle);
			Matrix[0, 0] = Math.Cos(rad);
			Matrix[0, 1] = Math.Sin(rad);
			Matrix[0, 2] = 0;
			Matrix[1, 0] = -Math.Sin(rad);
			Matrix[1, 1] = Math.Cos(rad);
			Matrix[1, 2] = 0;
			Matrix[2, 0] = 0;
			Matrix[2, 1] = 0;
			Matrix[2, 2] = 1;
		}
	}
}