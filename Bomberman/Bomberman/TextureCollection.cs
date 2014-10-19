using System.Collections.Generic;

namespace Bomberman
{
    public class TextureCollection
    {
        private Dictionary<string, MyTexture> textures;

        public TextureCollection()
        {
            textures=new Dictionary<string,MyTexture>();
        }

        public void Add(string name, MyTexture texture)
        {
            textures.Add(name, texture);
        }

        public MyTexture Get(string name)
        {
            return textures[name];
        }

        public MyTexture this[string name]
        {
            get { return Get(name); }
            set { textures[name] = value; }
        }

    }
}
