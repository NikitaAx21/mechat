using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{



    class AddDelete
    {
        public void Add(List<Object> List, Object Obj)
        {
            Obj.Id = List.Count;
            List.Add(Obj);
        }

        public void Delete(List<Object> List, int id)
        {
            List.RemoveRange(id, List.Count-1);

        }



    }
}
