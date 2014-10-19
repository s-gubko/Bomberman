using System.Linq;
using Microsoft.Xna.Framework;
using Bomberman.Models;

namespace Bomberman
{ 
    class FireCollection:MyCollection<Fire>
    {
        
        public FireCollection():base()
        {

        }

        public override void UpdateCollection(GameTime gameTime, Field field)
        {
            for (int i = 0; i < List.Count(); ++i)
                List[i].Update(gameTime, List, field);
        }

    }
}
