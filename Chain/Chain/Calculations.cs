using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    

    class Calculations
    {

        public Point Mass_center(List<Object> List)
        {
            int X_coord=0;
            int Y_coord=0;
            int Mass=0;




            Mass += List[0].mass;



            for (int i = 1; i < List.Count; i++)
            {

                Mass += List[i].mass;

                Segment S = List[i] as Segment;
                if (S != null)
                {
                    if (i != 1)
                    {
                        Segment S0 = List[i-1] as Segment;


                        X_coord += (S.vector.x + S0.vector.x) / 2 * List[i].mass;//---
                        Y_coord += (S.vector.y + S0.vector.y) / 2 * List[i].mass;//---
                    }
                    else
                    {
                        X_coord += (S.vector.x + Default_x) / 2 * List[i].mass;//---
                        Y_coord += (S.vector.y + Default_y) / 2 * List[i].mass;//---

                    }

                   
                }
                else
                {
                    S = List[i-1] as Segment;

                    X_coord += S.vector.x * List[i].mass;
                    Y_coord += S.vector.y * List[i].mass;

                }

              
               


            }



            X_coord = X_coord / Mass;
            Y_coord = Y_coord / Mass;

            Point all_mass_center;

            all_mass_center.x = X_coord;
            all_mass_center.y = Y_coord;

            return all_mass_center;
        }


    }
}
