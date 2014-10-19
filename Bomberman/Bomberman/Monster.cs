using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bomberman
{
    class Monster:MovingModel
    {
        Random a;
        public event Action<Bomberman.Models.Model> MonsterDead;

        public Monster(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures, Field field, float speed = 2.5f)
            : base(graphicsDevice, position, textures, speed)
        {
            a = new Random((int)DateTime.Now.Ticks);
            int b = a.Next(4);
            if (b == 0)
                moveDirection = Direction.Up;
            else if (b == 1)
                moveDirection = Direction.Right;
            else if (b == 2)
                moveDirection = Direction.Down;
            else if (b == 3)
                moveDirection = Direction.Left;
            MonsterDead += field.Free;
        }

        public void Update(GameTime gameTime, Field field, MonsterCollection monsterCollection)
        {
            Vector2 temp = new Vector2();
            temp = field.Convert(centerPosition);
            if (field[temp].flag == flag.Fire)
            {
                onMonsterDead();
                monsterCollection.Remove(this);
            }

            field.Free(spritePosition);
            bool move = false;

            int prob1=5, prob2=3, prob3=4; //5 3 4 

            if (moveDirection==Direction.Up)
            {   
                move = MoveUp(field);
                int b = a.Next(prob1);  
                if ((CanMove(Direction.Left, field) && (b == prob2)))
                    moveDirection = Direction.Left;
                else if ((CanMove(Direction.Right, field) && (b == prob3)))
                    moveDirection = Direction.Right;
            }
            else if (moveDirection == Direction.Down)
            {
                move = MoveDown(field);
                int b = a.Next(prob1);
                if ((CanMove(Direction.Left, field) && (b == prob2)))
                    moveDirection = Direction.Left;
                else if ((CanMove(Direction.Right, field) && (b == prob3)))
                    moveDirection = Direction.Right;
            }
            else if (moveDirection == Direction.Left)
            {
                move = MoveLeft(field);
                int b = a.Next(prob1);
                if ((CanMove(Direction.Up, field) && (b == prob2)))
                    moveDirection = Direction.Up;
                else if ((CanMove(Direction.Down, field) && (b == prob3)))
                    moveDirection = Direction.Down;
            }
            else if (moveDirection == Direction.Right)
            {
                move = MoveRight(field);
                int b = a.Next(prob1);
                if ((CanMove(Direction.Up, field) && (b == prob2)))
                    moveDirection = Direction.Up;
                else if ((CanMove(Direction.Down, field) && (b == prob3)))
                    moveDirection = Direction.Down;
            }

            if (!move)
            {
                
                int b = a.Next(4);
                if (b == 0)
                    moveDirection = Direction.Up;
                else if (b == 1)
                    moveDirection = Direction.Right;
                else if (b == 2)
                    moveDirection = Direction.Down;
                else if (b == 3)
                    moveDirection = Direction.Left;
            }   

            centerPosition.X = spritePosition.X + texture.frameWidth * texture.scale / 2;
            centerPosition.Y = spritePosition.Y + texture.frameHeight * texture.scale / 2;
            temp = field.Convert(centerPosition);
            
            if (field[temp].IsEmpty)
                field.Fill(spritePosition, flag.Monster);
            
            texture.FrameUpdate(gameTime);
            
        }

        public void onMonsterDead()
        {
            if (MonsterDead != null)
                MonsterDead(this);
        }

        public void Dead()
        {
            onMonsterDead();
        }
    }
}
