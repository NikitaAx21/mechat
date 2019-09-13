using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    class Object
    {

        public bool mass_center;
        public int mass;
        
        public Object()
        {

            mass_center = false;
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
        public int angle_restriction1;
        public int angle_restriction2;

        public Joint()
        {
            angle_restriction = false;

            angle_restriction1=0;
            angle_restriction2=0;

        }

    }
}
