using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.Models
{
    public class Stone:Bomberman.Models.Model
    {
        public Stone(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures)
            : base(graphicsDevice, position, textures)
        {

        }
    }
}
