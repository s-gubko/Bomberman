using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman.Models
{
    class Fire:Model
    {
        double time;
        double burnTime;

        public event Action<Vector2> TellToField;

        public void onTellToField()
        {
            if (TellToField != null)
                TellToField(this.spritePosition);
        }

        public Fire(GraphicsDevice graphicsDevice, Vector2 position,TextureCollection textures, Field field, double burnTime=0.5)
            : base(graphicsDevice, position,textures)
        {
            TellToField += field.Free;
            time = 0;
            this.burnTime = burnTime;
            field.Fill(centerPosition, flag.Fire);
        }

        public void Update(GameTime gameTime, List<Fire> fires, Field field)
        {
            field.Fill(centerPosition, flag.Fire);
            time += gameTime.ElapsedGameTime.TotalSeconds;
            if (time >= burnTime)
            {
                onTellToField();
                fires.Remove(this);
            }
            this.texture.FrameUpdate(gameTime);
        }


    }
}
