using System.Collections.Generic;
using System.Windows;

namespace Chain
{
	public class Calculations
	{
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
						var S = list[i - 1] as Segment;

						coordX += S.Vector.X * list[i].Mass;
						coordY += S.Vector.Y * list[i].Mass;
					}
				}
				else
				{
					var s = list[i] as Segment;

					if (i == 1)
					{
						coordX += (s.Vector.X + defaultX) / 2 * list[i].Mass; //---
						coordY += (s.Vector.Y + defaultY) / 2 * list[i].Mass; //---
					}
					else
					{
						var S0 = list[i - 2] as Segment;

						coordX += (s.Vector.X + S0.Vector.X) / 2 * list[i].Mass;
						coordY += (s.Vector.Y + S0.Vector.Y) / 2 * list[i].Mass;
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
	}
}