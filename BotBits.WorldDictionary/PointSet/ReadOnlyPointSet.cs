using System.Collections;
using System.Collections.Generic;

namespace BotBits.WorldDictionary
{
    internal class ReadOnlyPointSet : IPointSet
    {
        private readonly List<Point> _list;

        public ReadOnlyPointSet()
        {
            this._list = new List<Point>();
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Point point)
        {
            this._list.Add(point);
        }

        public bool Remove(Point point)
        {
            return this._list.Remove(point);
        }

        public int Count => this._list.Count;
        public bool Contains(Point point)
        {
            return this._list.Contains(point);
        }
    }
}