using System.Collections.Generic;
using System.Windows;

namespace Chain
{
    public class Calculations
    {
        public Point Mass_center(List<Object> List)
        {
            double coordX = 0;
            double CoordY = 0;
            double Mass = 0;

            double defaultX = 0;
            double defaultY = 0;


            for (int i = 0; i < List.Count; i++)
            {
                Mass += List[i].Mass;

                var J = List[i] as Joint;
                if (J != null)
                {
                    if (i == 0)
                    {
                        coordX += defaultX * List[i].Mass; //---
                        CoordY += defaultY * List[i].Mass; //---
                    }
                    else
                    {
                        var S = List[i - 1] as Segment;

                        coordX += S.Vector.X * List[i].Mass;
                        CoordY += S.Vector.Y * List[i].Mass;
                    }
                }
                else
                {
                    Segment S = List[i] as Segment;

                    if (i == 1)
                    {
                        coordX += (S.Vector.X + defaultX) / 2 * List[i].Mass; //---
                        CoordY += (S.Vector.Y + defaultY) / 2 * List[i].Mass; //---
                    }
                    else
                    {
                        Segment S0 = List[i - 2] as Segment;

                        coordX += (S.Vector.X + S0.Vector.X) / 2 * List[i].Mass;
                        CoordY += (S.Vector.Y + S0.Vector.Y) / 2 * List[i].Mass;
                    }
                }
            }


            coordX = coordX / Mass;
            CoordY = CoordY / Mass;

            var allMassCenter = new Point
            {
                X = coordX,
                Y = CoordY
            };


            return allMassCenter;
        }
    }
}