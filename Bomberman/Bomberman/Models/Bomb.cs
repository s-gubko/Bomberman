using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bomberman;

namespace Bomberman.Models
{
    public class Bomb:Model
    {
        public event Action<Model> BombBoom;

        double time;
        double lifeTime;

        public void onBombBoom()
        {
            if (BombBoom != null)
                BombBoom(this);
        }

        public Bomb(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures,  Field field, float lifeTime = 3)
            : base(graphicsDevice, position,textures)
        {
            BombBoom += field.Free;
            this.time = 0;
            this.lifeTime = lifeTime;
        }

        public void Update(GameTime gameTime, BombCollection activeBombs, Field field)
        {
            field.Fill(centerPosition, flag.Bomb);
            time += gameTime.ElapsedGameTime.TotalSeconds;
            if (time >= lifeTime)
                Boom(activeBombs);
            this.texture.FrameUpdate(gameTime);
        }

        public void Boom(BombCollection activeBombs)
        {
            activeBombs.Remove(this);
            onBombBoom();
        }
    }
}
