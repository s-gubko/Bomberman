using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Bomberman
{
    public enum Direction
    {
        Right, Left, Up, Down, None
    }

    public abstract class MovingModel : Bomberman.Models.Model
    {
        protected float speed;
        protected Direction moveDirection;

        public MovingModel(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures, float speed)
            : base(graphicsDevice, position, textures)
        {
            this.speed = speed;
        }

        protected void MoveRight()
        {
            texture = textures["rightTexture"];
            spritePosition.X += speed;
            moveDirection = Direction.Right;
        }

        protected void MoveLeft()
        {
            texture = textures["leftTexture"];
            spritePosition.X -= speed;
            moveDirection = Direction.Left;
        }

        protected void MoveUp()
        {
            texture = textures["upTexture"];
            spritePosition.Y -= speed;
            moveDirection = Direction.Up;
        }

        protected void MoveDown()
        {
            texture = textures["downTexture"];
            spritePosition.Y += speed;
            moveDirection = Direction.Down;
        }

        protected bool CanMove(Direction dir, Field field)
        {
            Vector2 temp = new Vector2();
            bool flagRight = false, flagDown = false, flagLeft = false, flagUp = false;
            temp = field.Convert(centerPosition, out flagDown, out flagRight, out flagUp, out flagLeft);
            switch (dir)
            {
                case (Direction.Right):
                    {
                        temp = field.Convert(new Vector2(centerPosition.X, centerPosition.Y), out flagDown, out flagRight, out flagUp, out flagLeft);
                        if (((field[(int)temp.X + 1, (int)temp.Y].IsEmpty) || (Math.Abs(centerPosition.X - field[(int)temp.X + 1, (int)temp.Y].centerPosition.X) > Field.cellSize)))
                        {
                            if (((flagDown) && (!field[(int)temp.X + 1, (int)temp.Y + 1].IsEmpty)) || ((flagUp) && (!field[(int)temp.X + 1, (int)temp.Y - 1].IsEmpty)))
                                return false;
                            return true;
                        }
                        return false;
                    }
                case (Direction.Left):
                    {
                        temp = field.Convert(new Vector2(centerPosition.X - speed, centerPosition.Y), out flagDown, out flagRight, out flagUp, out flagLeft);
                        if (((field[(int)temp.X - 1, (int)temp.Y].IsEmpty) || (Math.Abs(centerPosition.X - field[(int)temp.X - 1, (int)temp.Y].centerPosition.X) > Field.cellSize)))
                        {
                            if (((flagDown) && (!field[(int)temp.X - 1, (int)temp.Y + 1].IsEmpty)) || ((flagUp) && (!field[(int)temp.X - 1, (int)temp.Y - 1].IsEmpty)))
                                return false;
                            return true;
                        }
                        return false;
                    }
                case (Direction.Up):
                    {
                        temp = field.Convert(new Vector2(centerPosition.X , centerPosition.Y - speed), out flagDown, out flagRight, out flagUp, out flagLeft);
                        if (((field[(int)temp.X, (int)temp.Y - 1].IsEmpty) || (Math.Abs(centerPosition.Y - field[(int)temp.X, (int)temp.Y - 1].centerPosition.Y) > Field.cellSize)))
                        {
                            if (((flagLeft) && (!field[(int)temp.X - 1, (int)temp.Y - 1].IsEmpty)) || ((flagRight) && (!field[(int)temp.X + 1, (int)temp.Y - 1].IsEmpty)))
                                return false;
                            return true;
                        }
                        return false;
                    }
                case (Direction.Down):
                    {
                        temp = field.Convert(new Vector2(centerPosition.X, centerPosition.Y), out flagDown, out flagRight, out flagUp, out flagLeft);
                        if (((field[(int)temp.X, (int)temp.Y + 1].IsEmpty)|| (Math.Abs(centerPosition.Y - field[(int)temp.X, (int)temp.Y + 1].centerPosition.Y) > Field.cellSize)))
                        {
                            if (((flagLeft) && (!field[(int)temp.X - 1, (int)temp.Y + 1].IsEmpty)) || ((flagRight) && (!field[(int)temp.X + 1, (int)temp.Y + 1].IsEmpty)))
                                return false;
                            return true;
                        }
                        return false;
                    }

            }

            return false;
        }

        public bool MoveUp(Field field)
        {
            Vector2 temp = new Vector2();
            bool flagRight = false, flagDown = false, flagLeft = false, flagUp = false;
            temp = field.Convert(centerPosition, out flagDown, out flagRight, out flagUp, out flagLeft);
            if (CanMove(Direction.Up, field))
            {
                MoveUp();
                return true;
            }
            if (field[(int)temp.X, (int)temp.Y - 1].IsEmpty)
            {
                if (flagLeft)
                {
                    MoveRight();
                    MoveUp();
                    return true;
                }
                if (flagRight)
                {
                    MoveLeft();
                    MoveUp();
                    return true;
                }
            }
            return false;
        }

        public bool MoveDown(Field field)
        {
            Vector2 temp = new Vector2();
            bool flagRight = false, flagDown = false, flagLeft = false, flagUp = false;
            temp = field.Convert(centerPosition, out flagDown, out flagRight, out flagUp, out flagLeft);
            if (CanMove(Direction.Down, field))
            {
                MoveDown();
                return true;
            }
            if (field[(int)temp.X, (int)temp.Y + 1].IsEmpty)
            {
                if (flagLeft)
                {
                    MoveRight();
                    MoveDown();
                    return true;
                }
                if (flagRight)
                {
                    MoveLeft();
                    MoveDown();
                    return true;
                }
            }
            return false;
        }

        public bool MoveLeft(Field field)
        {
            Vector2 temp = new Vector2();
            bool flagRight = false, flagDown = false, flagLeft = false, flagUp = false;
            temp = field.Convert(centerPosition, out flagDown, out flagRight, out flagUp, out flagLeft);
            if (CanMove(Direction.Left, field))
            {
                MoveLeft();
                return true;
            }
            if (field[(int)temp.X - 1, (int)temp.Y].IsEmpty)
            {
                if (flagUp)
                {
                    MoveDown();
                    MoveLeft();
                    return true;
                }
                if (flagDown)
                {
                    MoveUp();
                    MoveLeft();
                    return true;
                }
            }
            return false;
        }

        public bool MoveRight(Field field)
        {
            Vector2 temp = new Vector2();
            bool flagRight = false, flagDown = false, flagLeft = false, flagUp = false;
            temp = field.Convert(centerPosition, out flagDown, out flagRight, out flagUp, out flagLeft);
            if (CanMove(Direction.Right, field))
            {
                MoveRight();
                return true;
            }
            if (field[(int)temp.X + 1, (int)temp.Y].IsEmpty)
            {
                if (flagUp)
                {
                    MoveDown();
                    MoveRight();
                    return true;
                }
                if (flagDown)
                {
                    MoveUp();
                    MoveRight();
                    return true;
                }
            }
            return false;
        }
    }
}
