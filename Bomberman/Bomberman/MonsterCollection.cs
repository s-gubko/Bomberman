using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Bomberman
{


    class MonsterCollection:MyCollection<Monster>
    {
        public MonsterCollection(): base()
        {

        }

        public event Action MonstersDead, ManDead;

        public override void UpdateCollection(GameTime gameTime, Field field)
        {
            for (int i = 0; i < List.Count(); ++i)
                List[i].Update(gameTime, field, this);
            if (List.Count() == 0)
                onMonstersDead();
        }

        public void onMonstersDead()
        {
            if (MonstersDead != null)
                MonstersDead();
        }

        public void onManDead()
        {
            if (ManDead != null)
                ManDead();
        }

        public double Distance(Vector2 centerPosition1, Vector2 centerPosition2)
        {
            return (Math.Sqrt(Math.Pow(centerPosition1.X - centerPosition2.X, 2) + Math.Pow(centerPosition1.Y - centerPosition2.Y, 2)));
        }

        public void ManMeetMonster(Vector2 manPosition)
        {
            foreach (Monster monster in List)
                if (Distance(manPosition, monster.centerPosition) < (Field.cellSize / 2))
                    onManDead();
        }
    }
}
