using System.Collections.Generic;
using System.Linq;

namespace Chain
{
    public class ListManager
    {
        public ListManager(List<Object> list, Panel panel)
        {
            _list = list;
            _panel = panel;
        }

        private readonly List<Object> _list;
        private readonly Panel _panel;

        public void Add()
        {
            var isNeedToCreateSegment = _list.Count != 0 && _list.Last() is Joint;
            var obj = isNeedToCreateSegment ? (Object) new Segment() : new Joint();

            obj.Id = _list.Count;
            obj.OnSelectedChanged += Select;
            _list.Add(obj);
        }

        private void Select(Object obj)
        {
            _panel.SelectedObject = obj;
        }

        public void Delete(int id)
        {
            _list.RemoveAll(o => o.Id >= id);
        }
    }
}