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

                       
            for (int i = 0; i < List.Count; i++)
            {
                X_coord += List[i].mass_center.x * List[i].mass;//единицы/координаты локальные или глобальные-перевод?

                Y_coord += List[i].mass_center.y * List[i].mass;//единицы/координаты локальные или глобальные-перевод?

                Mass += List[i].mass;
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
