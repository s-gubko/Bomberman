using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bomberman.Models;

namespace Bomberman.Bonuses
{
    public class BonusCollection : MyCollection<Bonus>
    {
        public BonusCollection()
            : base()
        {
        }

        public override void UpdateCollection(GameTime gameTime, Field field)
        {
            for (int i = 0; i < List.Count(); ++i)
                List[i].Update(gameTime, field);
        }

        public void ManInBonus(Vector2 manPosition)
        {
            bool f = false;
            Bonus b = new Bonus();
            foreach (Bonus bonus in List)
                if (Distance(manPosition, bonus.centerPosition) < (Field.cellSize / 2))
                {
                    bonus.onBonusAction();
                    b=bonus;
                    f = true;
                }
            if (f)
                List.Remove(b);
        }

        public double Distance(Vector2 centerPosition1, Vector2 centerPosition2)
        {
            return (Math.Sqrt(Math.Pow(centerPosition1.X - centerPosition2.X, 2) + Math.Pow(centerPosition1.Y - centerPosition2.Y, 2)));
        }
    }
}
