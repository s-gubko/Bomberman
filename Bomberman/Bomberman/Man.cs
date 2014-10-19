using Bomberman.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Bomberman
{

    public class Man : MovingModel
    {
        int maxBombCount, bombCount;
        

        Keys up = Keys.Up, down = Keys.Down, left = Keys.Left, right = Keys.Right;

        public event Action ManDead, MeetDoor;
        public event Action<Vector2> MeetMonster, MeetBonus;

        public Man(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures, float speed = 2.5f, int maxBombCount = 1)
            : base(graphicsDevice, position, textures, speed)
        {
            this.maxBombCount = maxBombCount;
            bombCount = maxBombCount;
        }

        public void onManDead()
        {
            if (ManDead != null)
                ManDead();
        }

        public void onMeetDoor()
        {
            if (MeetDoor != null)
                MeetDoor();
        }

        public void onMeetBonus()
        {
            if (MeetBonus != null)
                MeetBonus(this.centerPosition);
        }

        public void onMeetMonster()
        {
            if (MeetMonster != null)
                MeetMonster(this.centerPosition);
        }


        public void Update(GameTime gameTime, Field field)
        {
            Vector2 temp = new Vector2();
            temp = field.Convert(centerPosition);

            if (field[temp].flag == flag.Fire)
            {
                onManDead();
            }

            if (field[temp].flag == flag.Monster || field[(int)temp.X, (int)temp.Y - 1].flag == flag.Monster || field[(int)temp.X+1, (int)temp.Y].flag == flag.Monster || field[(int)temp.X-1, (int)temp.Y].flag == flag.Monster || field[(int)temp.X, (int)temp.Y + 1].flag == flag.Monster)
            {
                onMeetMonster();
            }

            if (field[temp].flag == flag.Door)
            {
                onMeetDoor();
            }

            if (field[temp].flag == flag.Bonus)
            {
                onMeetBonus();
            }
            bool move = false;

            KeyboardState st = Keyboard.GetState();

            if (st.IsKeyDown(up))
            {
                move = MoveUp(field);
            }
            else if (st.IsKeyDown(down))
            {
                move = MoveDown(field);
            }
            else if (st.IsKeyDown(left))
            {
                move = MoveLeft(field);
            }
            else if (st.IsKeyDown(right))
            {
                move = MoveRight(field);
            }

            if (!move)
                texture =textures["baseTexture"];

            centerPosition.X = spritePosition.X + texture.frameWidth * texture.scale / 2;
            centerPosition.Y = spritePosition.Y + texture.frameHeight * texture.scale / 2;
            texture.FrameUpdate(gameTime);
        }

        public void PutBomb(ContentManager Content, BombCollection activeBombs, GameTime gameTime, Field field)
        {

            Vector2 temp = new Vector2();
            temp = field.Convert(centerPosition);
            if ((bombCount != 0) && (field[temp].IsEmpty))
            {
                TextureCollection texturesBomb = new TextureCollection();
                texturesBomb["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/bomb"), 4);
                activeBombs.Add(new Bomb(graphicsDevice, Field.Convert((int)temp.X,(int) temp.Y), texturesBomb, field));
                activeBombs[activeBombs.Count - 1].BombBoom += IncBomb;
                field[temp].flag=flag.Bomb;
                --bombCount;
            }

        }

        public void IncBomb(Bomberman.Models.Model bomb)
        {
            ++bombCount;
        }

        public void IncMaxBombCount()
        {
            ++maxBombCount;
            ++bombCount;
        }
    }
}
