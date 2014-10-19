using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    class Background
    {
        Rectangle viewPortRectangle;
        Texture2D background;
        SpriteBatch spriteBatch;

        public Background(GraphicsDevice graphicsDevice, GameWindow window, Texture2D background)
        {
            viewPortRectangle = new Rectangle(0, 0, window.ClientBounds.Width, window.ClientBounds.Height);
            this.background = background;
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(background, viewPortRectangle, Color.White);
            spriteBatch.End();
        }
        
    }
}
