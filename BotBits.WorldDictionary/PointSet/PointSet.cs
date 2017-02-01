using System.Collections;
using System.Collections.Generic;

namespace BotBits.WorldDictionary
{
    internal class PointSet : IPointSet
    {
        private readonly Dictionary<Point, bool> _set;

        public PointSet()
        {
            this._set = new Dictionary<Point, bool>();
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return this._set.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Point point)
        {
            this._set.Add(point, default(bool));
        }

        public bool Remove(Point point)
        {
            return this._set.Remove(point);
        }

        public int Count => this._set.Count;
        public bool Contains(Point point)
        {
            return this._set.ContainsKey(point);
        }
    }
}