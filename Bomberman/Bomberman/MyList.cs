using System.Collections.Generic;
using System.Linq;

namespace Bomberman
{
    class MyList<T>
    {
        private List<T> myList;

        public void Add(T item)
        {
            myList.Add(item);
        }

        public void Remove(T item)
        {
            myList.Remove(item);
        }

        public T Get(int i)
        {
            return myList[i];
        }

        public int Count
        {
            get { return myList.Count(); }
        }

        public T this[int i]
        {
            get { return Get(i); }
            set { myList[i] = value; }
        }
    }
}
