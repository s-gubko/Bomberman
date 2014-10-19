using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Bomberman.Models;

namespace Bomberman
{
    public abstract class MyCollection<T>:IEnumerable<T>
    {
        protected List<T> List;

        public MyCollection()
        {
            List = new List<T>();
        }

        public void Add(T item)
        {
            List.Add(item);
        }

        public void Remove(T item)
        {
            List.Remove(item);
        }

        public T Get(int i)
        {
            return List[i];
        }

        public T this[int i]
        {
            get { return Get(i); }
            set { Add(value); }
        }

        public int Count
        {
            get {return List.Count;}
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public abstract void UpdateCollection(GameTime gameTime, Field field);

        public virtual void DrawCollection()
        {
            foreach (T m in List)
            {
                Model mm = m as Model;
                if (mm!=null)
                    mm.Draw();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

    }
}
