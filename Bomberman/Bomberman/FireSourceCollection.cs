using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomberman
{
    class FireSourceCollection : MyCollection<FireSource>
    {

        public FireSourceCollection()
            : base()
        {

        }

        public override void UpdateCollection(GameTime gameTime, Field field)
        {
            for (int i = 0; i < List.Count(); ++i)
                List[i].Update(gameTime, List, field);
        }

        public override void DrawCollection()
        {
            foreach (FireSource fireSource in List)
            {
                fireSource.Draw();
            }
        }

    }
}
