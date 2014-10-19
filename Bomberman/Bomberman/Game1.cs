using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bomberman.Bonuses;
using Bomberman.Models;

namespace Bomberman
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public event Action RestartGame;

        GraphicsDeviceManager graphics;
        Man man;
        MonsterCollection monsters;
        BombCollection activeBombs;
        BrickCollection bricks, activeBricks;
        BonusCollection bonuses;
        FireSourceCollection fireSources;
        Door door;
        BonusCreator bonusCreator;
        Stone[,] stones;
        Field field;
        int fieldSize = 50;
        int maxBombCount = 2;
        float speed = 2.5f;
        Background background;
        bool gameover = false;
        Vector2 spawn1, spawn2, spawn3, spawn4, spawn5;

        KeyboardState oldState, newState;

        TextureCollection texturesMan, texturesStone, texturesFire, texturesBrick, texturesMonster, texturesDoor;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth =  1250;
            graphics.PreferredBackBufferHeight =  750;
            graphics.PreferMultiSampling = false;

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            background = new Background(graphics.GraphicsDevice, Window, Content.Load<Texture2D>("Textures/background"));

            field = new Field(Window, fieldSize);

            CreateSpawns();

            stones = new Stone[field.fieldWidth, field.fieldHeight];
            
            texturesMan = new TextureCollection();
            texturesMan["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/man"));
            texturesMan["leftTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/manleft"));
            texturesMan["rightTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/manright"));
            texturesMan["upTexture"] = new MyTexture(Content.Load<Texture2D>( "Textures/man"));
            texturesMan["downTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/man"));

            texturesStone = new TextureCollection();
            texturesStone["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/stone"));

            texturesFire = new TextureCollection();
            texturesFire["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/fire"));           

            texturesDoor = new TextureCollection();
            texturesDoor["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/doorclosed"));
            texturesDoor.Add("openTexture", new MyTexture(Content.Load<Texture2D>("Textures/dooropen")));

            activeBombs = new BombCollection();

            monsters = new MonsterCollection();
            monsters.MonstersDead += ( () => { door.Open(); } ) ;
            monsters.ManDead += Dead;

            fireSources=new FireSourceCollection();

            bricks = new BrickCollection();
            activeBricks = new BrickCollection();
            bonuses = new BonusCollection();

            BrickGenerator();

            man = new Man(graphics.GraphicsDevice, field[1, 2].position, texturesMan, speed, maxBombCount);
            man.ManDead += Dead;
            man.MeetDoor += ManMeetDoor;
            man.MeetMonster += monsters.ManMeetMonster;
            man.MeetBonus += bonuses.ManInBonus;

            bonusCreator = new BonusCreator(graphics.GraphicsDevice, Content, () => { man.IncMaxBombCount(); }, 10, () => { FireSource.incFireLength(); }, 10);
            

            CreateMonsters();

            CreateStones();
           
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if ((newState.IsKeyDown(Keys.Space)) && !(oldState.IsKeyDown(Keys.Space)) && !gameover)
            {
                man.PutBomb(Content, activeBombs, gameTime, field);
                activeBombs[activeBombs.Count - 1].BombBoom += NewFireSource;
            }

            if (!gameover)
                man.Update(gameTime, field);

            door.Update(gameTime, field);

            bricks.UpdateCollection(gameTime, field);
            activeBricks.UpdateCollection(gameTime);
            fireSources.UpdateCollection(gameTime, field);
            bonuses.UpdateCollection(gameTime, field);
            monsters.UpdateCollection(gameTime, field);
            activeBombs.UpdateCollection(gameTime, field);

            oldState = newState;
        }

        void DrawMenu()
        {
 
        }

        protected override void Draw(GameTime gameTime)
        {

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            background.Draw();

            //рамка
            for (int i = 0; i < field.fieldWidth; ++i)
            {
                stones[i, 0].Draw();
                stones[i, field.fieldHeight - 1].Draw();
            }

            for (int i = 0; i < field.fieldHeight; ++i)
            {
                stones[0, i].Draw();
                stones[field.fieldWidth - 1, i].Draw();
            }

            //столбы
            for (int i = 2; i < field.fieldWidth; i += 2)
                for (int j = 2; j < field.fieldHeight; j += 2)
                {
                    stones[i, j].Draw();
                }

            door.Draw();
            activeBombs.DrawCollection();

            bricks.DrawCollection();
            activeBricks.DrawCollection();

            fireSources.DrawCollection();

            bonuses.DrawCollection();

            monsters.DrawCollection();

            if (!gameover)
                man.Draw();

            
        }

        public void BrickGenerator()
        {
            Random a = new Random((int)DateTime.Now.Ticks);

            int z = a.Next(40, 61);

            int n = 0;
            for (int j = 1; j <= field.fieldHeight - 2; j += 2)
                for (int i = 1; i <= field.fieldWidth - 2; ++i)
                {
                    int b = a.Next(2);
                    if (b > 0)
                    {
                        if ((i == (int)spawn1.X && j == (int)spawn1.Y) || (i == (int)spawn2.X && j == (int)spawn2.Y) || (i == (int)spawn3.X && j == (int)spawn3.Y)
                            || (i == (int)spawn4.X && j == (int)spawn4.Y) || (i == (int)spawn5.X && j == (int)spawn5.Y) || (i == 1 && j == 1) || (i == 1 && j == 2) || (i == 2 && j == 1))
                            continue;
                        ++n;
                        texturesBrick = new TextureCollection();
                        texturesBrick["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/brick"));
                        texturesBrick.Add("crashTexture", new MyTexture(Content.Load<Texture2D>("Textures/brickcrash"), 10));
                        bricks.Add(new Brick(graphics.GraphicsDevice, Field.Convert(i, j), texturesBrick, field));
                        bricks[bricks.Count - 1].changeCollection += bricks.Remove;
                        bricks[bricks.Count - 1].changeCollection += ActiveBricksAdd;
                        bricks[bricks.Count - 1].BrickCrash += field.Free;
                        bricks[bricks.Count - 1].BrickCrash += NewBonus;
                        bricks[bricks.Count - 1].AfterBrickCrash += field.Free;

                        if (n == z)
                            door = new Door(graphics.GraphicsDevice, Field.Convert(i, j), texturesDoor, field);
                    }
                }
            for (int j = 2; j <= field.fieldHeight - 2; j += 2)
                for (int i = 1; i <= field.fieldWidth - 2; i += 2)
                {
                    int b = a.Next(2);
                    if (b > 0)
                    {
                        if ((i == (int)spawn1.X && j == (int)spawn1.Y) || (i == (int)spawn2.X && j == (int)spawn2.Y) || (i == (int)spawn3.X && j == (int)spawn3.Y)
                            || (i == (int)spawn4.X && j == (int)spawn4.Y) || (i == (int)spawn5.X && j == (int)spawn5.Y) || (i == 1 && j == 1) || (i == 1 && j == 2) || (i == 2 && j == 1))
                            continue;
                        ++n;
                        texturesBrick = new TextureCollection();
                        texturesBrick["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/brick"));
                        texturesBrick.Add("crashTexture", new MyTexture(Content.Load<Texture2D>("Textures/brickcrash"), 10));
                        bricks.Add(new Brick(graphics.GraphicsDevice, Field.Convert(i, j), texturesBrick, field));
                        bricks[bricks.Count - 1].changeCollection += bricks.Remove;
                        bricks[bricks.Count - 1].changeCollection += ActiveBricksAdd;
                        bricks[bricks.Count - 1].BrickCrash += field.Free;
                        bricks[bricks.Count - 1].BrickCrash += NewBonus;
                        bricks[bricks.Count - 1].AfterBrickCrash += field.Free;
                        if (n == z)
                            door = new Door(graphics.GraphicsDevice, Field.Convert(i, j), texturesDoor, field);
                    }
                }
        }

        public void CreateMonstersType1(int ii, int jj, int n)
        {
            float speed = 2.5f;
            for (int i = 1; i <= n; ++i)
            {
                texturesMonster = new TextureCollection();
                texturesMonster["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster"));
                texturesMonster["leftTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster1"), 10);
                texturesMonster["rightTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster1"), 10);
                texturesMonster["upTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster1"), 10);
                texturesMonster["downTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster1"), 10);
                monsters.Add(new Monster(graphics.GraphicsDevice, field[ii, jj].position, texturesMonster, field, speed));
            }
        }

        public void CreateMonstersType2(int ii, int jj, int n)
        {
            float speed = 5f;
            for (int i = 1; i <= n; ++i)
            {
                texturesMonster = new TextureCollection();
                texturesMonster["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster51"));
                texturesMonster["leftTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster5"), 10);
                texturesMonster["rightTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster5"), 10);
                texturesMonster["upTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster5"), 10);
                texturesMonster["downTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster5"), 10);
                monsters.Add(new Monster(graphics.GraphicsDevice, field[ii, jj].position, texturesMonster, field, speed));
            }
        }

        public void CreateMonstersType3(int ii, int jj, int n)
        {
            float speed = 1.25f;
            for (int i = 1; i <= n; ++i)
            {
                texturesMonster = new TextureCollection();
                texturesMonster["baseTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster61"));
                texturesMonster["leftTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster6"), 10);
                texturesMonster["rightTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster6"), 10);
                texturesMonster["upTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster6"), 10);
                texturesMonster["downTexture"] = new MyTexture(Content.Load<Texture2D>("Textures/monster6"), 10);
                monsters.Add(new Monster(graphics.GraphicsDevice, field[ii, jj].position, texturesMonster, field, speed));
            }
        }

        public void CreateMonsters()
        {

            CreateMonstersType1((int) spawn1.X, (int) spawn1.Y, 1);
            CreateMonstersType1((int) spawn2.X, (int) spawn2.Y, 1);
            CreateMonstersType1((int) spawn3.X, (int) spawn3.Y, 1);
            CreateMonstersType1((int) spawn4.X, (int) spawn4.Y, 2);
            CreateMonstersType1((int) spawn5.X, (int) spawn5.Y, 2);

            CreateMonstersType2((int)spawn1.X, (int)spawn1.Y, 1);
            CreateMonstersType2((int)spawn2.X, (int)spawn2.Y, 1);
            CreateMonstersType2((int)spawn3.X, (int)spawn3.Y, 1);


            CreateMonstersType3((int)spawn1.X, (int)spawn1.Y, 1);
            CreateMonstersType3((int)spawn2.X, (int)spawn2.Y, 1);
            CreateMonstersType3((int)spawn3.X, (int)spawn3.Y, 1);
            CreateMonstersType3((int)spawn4.X, (int)spawn4.Y, 1);
            CreateMonstersType3((int)spawn5.X, (int)spawn5.Y, 1);
        }

        public void CreateStones()
        {
            //рамка
            for (int i = 0; i < field.fieldWidth; ++i)
            {
                stones[i, 0] = new Stone(graphics.GraphicsDevice, field[i, 0].position, texturesStone);
                field.Fill(stones[i, 0].spritePosition, flag.Stone);
                stones[i, field.fieldHeight - 1] = new Stone(graphics.GraphicsDevice, field[i, field.fieldHeight - 1].position, texturesStone);
                field.Fill(stones[i, field.fieldHeight - 1].spritePosition, flag.Stone);
            }

            for (int i = 0; i < field.fieldHeight; ++i)
            {
                stones[0, i] = new Stone(graphics.GraphicsDevice, field[0, i].position, texturesStone);
                field.Fill(stones[0, i].spritePosition, flag.Stone);
                stones[field.fieldWidth - 1, i] = new Stone(graphics.GraphicsDevice, field[field.fieldWidth - 1, i].position, texturesStone);
                field.Fill(stones[field.fieldWidth - 1, i].spritePosition, flag.Stone);
            }


            //столбы
            for (int i = 2; i < field.fieldWidth; i += 2)
                for (int j = 2; j < field.fieldHeight; j += 2)
                {
                    stones[i, j] = new Stone(graphics.GraphicsDevice, field[i, j].position, texturesStone);
                    field.Fill(stones[i, j].spritePosition, flag.Stone);
                }
        }

        public void CreateSpawns()
        {
            spawn1 = new Vector2(3, field.fieldHeight - 4);
            spawn2 = new Vector2(field.fieldWidth - 4, field.fieldHeight - 4);
            spawn3 = new Vector2(field.fieldWidth - 4, 3);
            spawn4 = new Vector2(field.fieldWidth / 2 + 1, 3);
            spawn5 = new Vector2(field.fieldWidth / 2 + 1, field.fieldHeight - 4);
        }

        public void OpenTheDoor()
        {
            door.Open();
        }

        public void ManMeetDoor()
        {
            if (door.open)
            {
                gameover = true;
                MessageBox(new IntPtr(0), "Вы победили", "", 0);
            }
        }

        public void Dead()
        {
            gameover = true;
            man.ManDead -= Dead;
            MessageBox(new IntPtr(0), "Вы проиграли", "", 0);
        }

        public void onButtonEndClick()
        {
            if (RestartGame != null)
                RestartGame();
        }

        public void NewFireSource(Bomberman.Models.Model bomb)
        {
            fireSources.Add(new FireSource(GraphicsDevice, bomb.spritePosition, texturesFire, field));
            fireSources[fireSources.Count - 1].onFire += activeBombs.Boom;
            fireSources[fireSources.Count - 1].crashBrick += bricks.Crash;
        }

        public void NewBonus(Bomberman.Models.Model brick)
        {
            Bonus bonus;
            bonus = bonusCreator.GiveBonus(brick);
            if (bonus != null)
                bonuses.Add(bonus);
        }

        public void ActiveBricksAdd(Brick brick)
        {
            activeBricks.Add(brick);
            field.Fill(activeBricks[activeBricks.Count - 1].spritePosition, flag.Brick);
        }
    }
}
