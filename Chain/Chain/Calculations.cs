using System;
using System.Collections.Generic;
using System.Windows;

namespace Chain
{
	public class Calculations
	{
		private double _dX;
		private double _dY;
		public double[,] matr = new double[3, 3];
		public double[] vect = new double[3];
        public static List<Point> CoordMas = new List<Point>();

		/// <summary>
		/// Метод для вычисления координат в Лаб.С.К.
		///В метод передается матрица преобразования и вектор координат в собственной с.к.
		///Возвращается Point из двух элементов[x, y]
		/// </summary>
		/// <param name="tM"></param>
		/// <param name="cM"></param>
		/// <returns></returns>
		public Point Mass_center(List<Object> list)
		{
			double coordX = 0;
			double coordY = 0;
			double mass = 0;
			const double defaultX = 0;
			const double defaultY = 0;

			for (var i = 0; i < list.Count; i++)
			{
				mass += list[i].Mass;

				if (list[i] is Joint)
				{
					if (i == 0)
					{
						coordX += defaultX * list[i].Mass; //---
						coordY += defaultY * list[i].Mass; //---
					}
					else
					{
						coordX += Calculations.CoordMas[i].X * list[i].Mass;
						coordY += Calculations.CoordMas[i].Y * list[i].Mass;
					}
				}
				else
				{
					if (i == 1)
					{
						coordX += (Calculations.CoordMas[i].X + defaultX) / 2 * list[i].Mass; //---
						coordY += (Calculations.CoordMas[i].Y + defaultY) / 2 * list[i].Mass; //---
					}
					else
					{
						coordX += (Calculations.CoordMas[i].X + Calculations.CoordMas[i-2].X) / 2 * list[i].Mass;
						coordY += (Calculations.CoordMas[i].Y + Calculations.CoordMas[i-2].Y) / 2 * list[i].Mass;
					}
				}
			}

			coordX /= mass;
			coordY /= mass;

			var allMassCenter = new Point
			{
				X = coordX,
				Y = coordY
			};

			return allMassCenter;
		}

		public static double DegreeToRadian(double A)
		{
			return Math.PI * A / 180.0;
		}

        public static int GetScale()
        {
            return 1;
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

		public static int[] GetInetersectionElements(List<Object> ChainList)
		{
			int[] mas = new int[100];
            int k = 0;
			for (int i = 2; i < ChainList.Count - 1; i += 2)
			{
				for (int j = i + 4; j < ChainList.Count; j += 2)
				{
					Point a = Calculations.CoordMas[i];
					Point b = Calculations.CoordMas[i - 2];
					Point c = Calculations.CoordMas[j];
					Point d = Calculations.CoordMas[j - 2];
					if (IsIntersected(a, b, c, d))
					{
                        mas[k] = i - 1;
                        mas[k + 1] = j - 1;
                        k += 2;
					}
				}
			}

			return mas;
		}

		public static Point GetCoord(TransformationMatrix tM, CoordinatesMatrix cM)
		{
			var res = new Point
			{
				X = tM.TransMatrix[0, 0] * cM.Vector[0] + tM.TransMatrix[0, 1] * cM.Vector[1] +
					tM.TransMatrix[0, 2] * cM.Vector[2],
				Y = tM.TransMatrix[1, 0] * cM.Vector[0] + tM.TransMatrix[1, 1] * cM.Vector[1] +
					tM.TransMatrix[1, 2] * cM.Vector[2]
			};
			tM.X = res.X;
			tM.Y = res.Y;
            
			//CoordMas.Add(res);
            //CoordMas.Add(res);
            return res;
		}
	}

	public class TransformationMatrix
	{
		public double[,] TransMatrix = new double[3, 3];
		private double _curAngle = 0;
		public double X = 0; //для хранения координаты конца предыдущего сегмента
		public double Y = 0;

		public TransformationMatrix()
		{
			TransMatrix[0, 0] = 1;
			TransMatrix[0, 1] = 0;
			TransMatrix[0, 2] = 0;
			TransMatrix[1, 0] = 0;
			TransMatrix[1, 1] = 1;
			TransMatrix[1, 2] = 0;
			TransMatrix[2, 0] = 0;
			TransMatrix[2, 1] = 0;
			TransMatrix[2, 2] = 1;
		}

		//Матрица для поворота системы на угол "angle" начиная с элемента №"num" в коллекции ChainList
		public TransformationMatrix(double angle, List<Object> ChainList, int num = 0)
		{
			if (num == 0)
			{
				num = ChainList.Count;
			}
            angle = 0;
			for (int i = 1; i < num; i++)
			{
				var o = ChainList[i-1];
				switch (o)
				{
					case Joint j:
						angle += j.CurrentAngle;
						break;
				}
			}

			var rad = Calculations.DegreeToRadian(angle);
			TransMatrix[0, 0] = Math.Cos(rad);
            TransMatrix[0, 1] = Math.Sin(rad);
            TransMatrix[0, 2] = Calculations.CoordMas[num-1].X;
			TransMatrix[1, 0] = -Math.Sin(rad);
            TransMatrix[1, 1] = Math.Cos(rad);
            TransMatrix[1, 2] = Calculations.CoordMas[num-1].Y;
			TransMatrix[2, 0] = 0;
			TransMatrix[2, 1] = 0;
			TransMatrix[2, 2] = 1;
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
            
            Vector[0] = s.Length * Math.Sin(Calculations.DegreeToRadian(j.CurrentAngle));
            Vector[1] = s.Length * Math.Cos(Calculations.DegreeToRadian(j.CurrentAngle));
            Vector[2] = 1;
		}
	} 
}