using System.Linq;
using Microsoft.Xna.Framework;
using Bomberman.Models;


namespace Bomberman
{

    public class BombCollection:MyCollection<Bomb>
    {
        public BombCollection():base()
        {
        }


        public override void UpdateCollection(GameTime gameTime, Field field)
        {
            for (int i = 0; i < List.Count(); ++i)
                List[i].Update(gameTime, this, field);
        }

        public void Boom(int x, int y)
        {
            Vector2 temp = new Vector2();
            temp = Field.Convert(x, y);
            for (int i = 0; i < List.Count(); ++i)
            {
                if (List[i].spritePosition == temp)
                    List[i].Boom(this);
            }
        }
    }
}
