using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomberman.Bonuses
{
    public class BonusCreator
    {
        GraphicsDevice graphicsDevice;
        ContentManager Content;

        private event Action GiveBomb;
        private event Action GiveFireLength;

        private int giveBombProb;

        private int GiveBombProb
        {
            get { return giveBombProb; }
            set 
            { 
                if (value>=0 && value<=100)
                    giveBombProb = value; 
            }
        }

        private int giveFireLengthProb;

        private int GiveFireLengthProb
        {
            get { return giveFireLengthProb; }
            set 
            {
                if (value >= 0 && value <= 100)
                    giveFireLengthProb = value; 
            }
        }


        public BonusCreator(GraphicsDevice _graphicsDevice, ContentManager _Content, Action _GiveBomb, int _giveBombProb, Action _GiveFireLength, int _giveFireLengthProb)
        {
            graphicsDevice = _graphicsDevice;
            GiveBomb = _GiveBomb;
            GiveBombProb = _giveBombProb;
            GiveFireLength = _GiveFireLength;
            GiveFireLengthProb = _giveFireLengthProb;
            Content = _Content;
        }

        public Bonus GiveBonus(Bomberman.Models.Model model)
        {
            Random r = new Random();
            int p = r.Next(0, 100);
            if (p > 0 && p <= GiveBombProb)
            {
                //give = true;
                TextureCollection texturesBonus = new TextureCollection();
                texturesBonus["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/bonusBomb"));
                return new Bonus(graphicsDevice, model.spritePosition, texturesBonus, GiveBomb);
            }
            else
            {
                if (p > GiveBombProb && p <= GiveBombProb + GiveFireLengthProb)
                {
                    //give = true;
                    TextureCollection texturesBonus = new TextureCollection();
                    texturesBonus["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/bonusFireLength"));
                    return new Bonus(graphicsDevice, model.spritePosition, texturesBonus, GiveFireLength);
                }
            }
            return null;
        }

    }
}
