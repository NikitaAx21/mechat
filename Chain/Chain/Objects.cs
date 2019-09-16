using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{

    struct Point
    {
        public int x;
        public int y;
    }

    class Object
    {
        public bool mass_center_visible;
        public int mass;

        public Point mass_center;

        public Object()
        {
            mass_center_visible = false;
            mass=0;            
        }                
    }
       
    class Segment : Object
    {        
        public int length;

        public bool visibility;
        public bool efemerik;

        public Segment()
        {
            length=2;

            visibility = true;
            efemerik = false;
        }
    }

    class Joint : Object
    {      
        public bool angle_restriction;
        public int current_angle;
        public int angle_restriction_left;
        public int angle_restriction_right;

        public Joint()
        {
            current_angle = 0;

            angle_restriction = false;

            angle_restriction_left = 0;
            angle_restriction_right = 0;
        }
    }
}
