using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bomberman.Models
{
    public abstract class Model
    {
        protected TextureCollection textures;
        public MyTexture texture;

        public SpriteBatch spriteBatch;

        public Vector2 spritePosition;
        public Vector2 centerPosition;

        public GraphicsDevice graphicsDevice;

        public Model(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures)
        {
            this.graphicsDevice = graphicsDevice;
            this.textures = textures;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            texture = textures["baseTexture"];
            this.spritePosition = position;
            centerPosition.X = spritePosition.X + texture.frameWidth * texture.scale / 2;
            centerPosition.Y = spritePosition.Y + texture.frameHeight * texture.scale / 2;
        }

        public Model()
        {

        }

        public void Draw()
        {           
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(texture.texture, spritePosition, texture.Rectangles[texture.frame], Color.White, 0, new Vector2(0, 0), texture.scale, 0, 0);
            spriteBatch.End();
        }



        public void AddTexture(string textureName, MyTexture texture)
        {
            textures.Add(textureName, texture);
        }
    }
}

