using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Bomberman
{
    public class MyTexture
    {
        public Texture2D texture { get; set; }

        public readonly Rectangle[] Rectangles;

        public int frameCount{get; set;}

        public bool Loop { get; set; }

        private double timeFrame;
        public int frame {get; set;}
        private double totalElapsed;
        public float scale{get; set;}

        public readonly int frameWidth, frameHeight;
        public readonly int width, height;

        public MyTexture(Texture2D texture, int framePerSec=24, int cellSize=50, bool loop=true)
        {
            this.texture = texture;
            timeFrame = 1f / framePerSec;
            frame = 0;
            totalElapsed = 0;
            Loop = loop;

            int frameSize = 150;

            if (texture.Width > texture.Height)
            {
                frameCount = texture.Width / frameSize;
                Rectangles = new Rectangle[frameCount];
                scale = (float)cellSize / (texture.Height / frameCount);
                frameWidth = texture.Width / frameCount;
                frameHeight = texture.Height;

                for (int i = 0; i < frameCount; ++i)
                    Rectangles[i] = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
            }
            else
            {
                frameCount = texture.Height / frameSize;
                Rectangles = new Rectangle[frameCount];
                scale = (float)cellSize / (texture.Height / frameCount);
                frameWidth = texture.Width;
                frameHeight = texture.Height / frameCount;

                for (int i = 0; i < frameCount; ++i)
                    Rectangles[i] = new Rectangle(0, i * frameHeight, frameWidth, frameHeight);
            }
            width = (int)(frameWidth * scale);
            height = (int)(frameHeight * scale);
        }

        public void FrameUpdate(GameTime gameTime, bool loop = true)
        {
            totalElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (totalElapsed >= timeFrame)
            {
                ++frame;
                if ((frame > this.frameCount - 1) && loop)
                    frame = 0;
                totalElapsed -= timeFrame;
            }
        }

        public Texture2D Load(ContentManager content, string textureName)
        {
            return content.Load<Texture2D>(textureName);
        }
    }
}
