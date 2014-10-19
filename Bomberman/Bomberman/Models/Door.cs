using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.Models
{
    class Door:Model
    {
        public bool open;

        public Door(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures, Field field)
            : base(graphicsDevice, position, textures)
        {
            open = false;
        }

        public void Open()
        {
            open = true;
            texture = textures["openTexture"];
        }

        public void Update(GameTime gameTime, Field field)
        {
            field.Fill(spritePosition, flag.Door);
        }
    }
}
