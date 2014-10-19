using Bomberman.Models;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    public class Cell
    {
        public Vector2 position;
        public Vector2 centerPosition;
        public flag flag;

        public Cell(int i, int j, int cellSize, flag flag = flag.Empty)
        {
            position = new Vector2(i * cellSize, j * cellSize);
            centerPosition = new Vector2(position.X + cellSize / 2, position.Y + cellSize / 2);
            this.flag = flag;
        }

        public bool IsEmpty
        {
            get
            {
                if ((flag == flag.Empty) || (flag == flag.Monster) || (flag == flag.Fire) || (flag == flag.Door) || (flag == flag.Bonus))
                    return true;
                return false;
            }
        }
    }

    public enum flag
    {
        Empty, Man, Monster, Stone, Brick, Bomb, Fire, Door, Bonus
    }
    
    

    public class Field
    {
        private Cell[,] cells;

        public int fieldWidth, fieldHeight;
        public static  int cellSize;

        public Field(GameWindow window, int cellSize)
        {
            Field.cellSize = cellSize;
            fieldWidth = window.ClientBounds.Width / cellSize;
            fieldHeight = window.ClientBounds.Height / cellSize;
            cells = new Cell [fieldWidth, fieldHeight];
            for (int i = 0; i < fieldWidth; ++i)
                for (int j = 0; j < fieldHeight; ++j)
                {
                    cells[i, j] = new Cell(i, j, cellSize);             
                 }
        }

        public static Vector2 Convert(int i, int j)
        {
            Vector2 temp = new Vector2(i * cellSize, j * cellSize);
            return temp;
        }

        public Vector2 Convert(Vector2 centerPosition, out bool down, out bool right, out bool up, out bool left)
        {
            down = false;
            right = false;
            up = false;
            left = false;
            Vector2 temp = new Vector2(centerPosition.X, centerPosition.Y);
            temp.X = (int)temp.X / cellSize;
            temp.Y = (int)temp.Y / cellSize;


            if (centerPosition.X - cells[(int)temp.X, (int)temp.Y].centerPosition.X > 0)
                right = true;
            if (centerPosition.X - cells[(int)temp.X, (int)temp.Y].centerPosition.X < 0)
                left = true;
            if (centerPosition.Y - cells[(int)temp.X, (int)temp.Y].centerPosition.Y > 0)
                down = true;
            if (centerPosition.Y - cells[(int)temp.X, (int)temp.Y].centerPosition.Y < 0)
                up = true;
            return temp;
        }

        public Vector2 Convert(Vector2 spritePosition)
        {
            Vector2 temp = new Vector2(spritePosition.X, spritePosition.Y);
            temp.X = (int)temp.X / cellSize;
            temp.Y = (int)temp.Y / cellSize;
            return temp;
        }



        public void Fill (Vector2 spritePosition, flag flag)
        {
            Vector2 temp=new Vector2();
            temp = Convert(spritePosition);
            cells[(int)temp.X, (int)temp.Y].flag = flag;
        }



        public void Free(Vector2 spritePosition)
        {
            Vector2 temp = new Vector2();
            temp = Convert(spritePosition);
            cells[(int)temp.X, (int)temp.Y].flag = flag.Empty;
        }

        public void Free(Model model)
        {
            Vector2 temp = new Vector2();
            temp = Convert(model.spritePosition);
            cells[(int)temp.X, (int)temp.Y].flag = flag.Empty;
        }

        public Cell this[int i, int j]
        {
            get
            {
                return cells[i,j];
            }
            set
            {
                cells[i, j] = value;
            }
        }

        public Cell this[Vector2 V]
        {

            get
            {
                return cells[(int)V.X, (int)V.Y];
            }
            set
            {
                cells[(int)V.X, (int)V.Y] = value;
            }
        }
    }
}
