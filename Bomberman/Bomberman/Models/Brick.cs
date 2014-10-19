using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Bomberman.Models
{

    public class Brick : Model
    {
        public event Action<Model> BrickCrash, AfterBrickCrash;

        public event Action<Brick> changeCollection;


        public Brick(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures, Field field)
            : base(graphicsDevice, position, textures)
        {
            field.Fill(spritePosition, flag.Brick);
        }

        public void onBrickCrash()
        {
            if (BrickCrash != null)
                BrickCrash(this);
            if (changeCollection != null)
                changeCollection(this);
        }

        public void onAfterBrickCrash()
        {
            if (AfterBrickCrash != null)
                AfterBrickCrash(this);
        }

        public void Update(GameTime gameTime, BrickCollection activeBricks)
        {
            if (texture.frame < texture.frameCount - 1)
                this.texture.FrameUpdate(gameTime, false);
            else
            {
                activeBricks.Remove(this);
                onAfterBrickCrash();
            }
        }

        public void Update(GameTime gameTime, Field field)
        {
            field.Fill(centerPosition, flag.Brick);
        }

        public void Crash()
        {
            onBrickCrash();
            texture = textures["crashTexture"];
        }

    }
}
