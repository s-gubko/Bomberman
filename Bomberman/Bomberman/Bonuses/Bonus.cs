using Bomberman.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Bomberman.Bonuses
{
    public class Bonus : Bomberman.Models.Model
    {
        public event Action BonusAction;

        public Bonus(GraphicsDevice graphicsDevice, Vector2 position, Bomberman.TextureCollection textures, Action bonusAction) 
            : base(graphicsDevice, position, textures)
        {
            BonusAction += bonusAction;
        }

        public Bonus()
        {

        }

        public void onBonusAction()
        {
            if (BonusAction != null)
                BonusAction();
        }

        public void Update(GameTime gameTime, Field field)
        {
            field.Fill(centerPosition, flag.Bonus);
        }

        
    }
}
