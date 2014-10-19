using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Bomberman.Models;

namespace Bomberman
{
    class StopFlags
    {
        public bool left, right, up, down;
        public StopFlags()
        {
            left=right=up=down=true;
        }
    }

    class FireSource
    {
        static int fireLength=3;
        int length;
        Dictionary<string, FireCollection> fireCollection;
        StopFlags stopFlags;
        Vector2 position;
        GraphicsDevice graphicsDevice;
        TextureCollection textures;

        public event Action<int, int> onFire, crashBrick;

        public FireSource(GraphicsDevice graphicsDevice, Vector2 position, TextureCollection textures, Field field)
        {
            this.textures = textures;
            length = 0;
            fireCollection = new Dictionary<string, FireCollection>();
            fireCollection.Add("centerFire", new FireCollection());
            fireCollection.Add("leftFires", new FireCollection());
            fireCollection.Add("rightFires", new FireCollection());
            fireCollection.Add("upFires", new FireCollection());
            fireCollection.Add("downFires", new FireCollection());

            fireCollection["centerFire"].Add(new Fire(graphicsDevice, position, textures, field));

            this.position = position;
            stopFlags = new StopFlags();
            this.graphicsDevice = graphicsDevice;
        }

        void Burn(Field field)
        {
            if (length < fireLength)
                MakeFire(field);
        }

        void MakeFire(Field field)
        {
            ++length;
            Vector2 cellPosition = new Vector2();
            cellPosition = field.Convert(position);
            if (stopFlags.left)
            {
                stopFlags.left=false;
                if (field[(int)cellPosition.X - length, (int)cellPosition.Y].flag == flag.Bomb)
                {
                    if (onFire != null)
                        onFire((int)cellPosition.X - length, (int)cellPosition.Y);
                }
                else if (field[(int)cellPosition.X - length, (int)cellPosition.Y].flag == flag.Brick)
                {
                    if (crashBrick != null)
                        crashBrick((int)cellPosition.X - length, (int)cellPosition.Y);
                }
                else if (field[(int)cellPosition.X - length, (int)cellPosition.Y].IsEmpty)
                {
                    Vector2 temp = new Vector2(position.X - Field.cellSize*length, position.Y);
                    fireCollection["leftFires"].Add(new Fire(graphicsDevice, temp, textures, field));
                    
                    stopFlags.left = true;
                }
                
            }

            if (stopFlags.right)
            {
                stopFlags.right = false;
                if (field[(int)cellPosition.X + length, (int)cellPosition.Y].flag == flag.Bomb)
                {
                    if (onFire != null)
                        onFire((int)cellPosition.X + length, (int)cellPosition.Y);
                }
                else if (field[(int)cellPosition.X + length, (int)cellPosition.Y].flag == flag.Brick)
                {
                    if (crashBrick != null)
                        crashBrick((int)cellPosition.X + length, (int)cellPosition.Y);
                }
                else if (field[(int)cellPosition.X + length, (int)cellPosition.Y].IsEmpty)
                {
                    Vector2 temp = new Vector2(position.X + Field.cellSize * length, position.Y);
                    fireCollection["rightFires"].Add(new Fire(graphicsDevice, temp, textures, field));
                    stopFlags.right = true;
                }
            }

            if (stopFlags.up)
            {
                stopFlags.up = false;
                if (field[(int)cellPosition.X, (int)cellPosition.Y - length].flag == flag.Bomb)
                {
                    if (onFire != null)
                        onFire((int)cellPosition.X, (int)cellPosition.Y - length);
                }
                else if (field[(int)cellPosition.X, (int)cellPosition.Y - length].flag == flag.Brick)
                {
                    if (crashBrick != null)
                        crashBrick((int)cellPosition.X, (int)cellPosition.Y - length);
                }

                else if (field[(int)cellPosition.X, (int)cellPosition.Y - length].IsEmpty)
                {
                    Vector2 temp = new Vector2(position.X, position.Y - Field.cellSize * length);
                    fireCollection["upFires"].Add(new Fire(graphicsDevice, temp, textures, field));
                    stopFlags.up = true;
                }           
            }

            if (stopFlags.down)
            {
                stopFlags.down = false;
                if (field[(int)cellPosition.X, (int)cellPosition.Y + length].flag == flag.Bomb)
                {
                    if (onFire != null)
                        onFire((int)cellPosition.X, (int)cellPosition.Y + length);
                }
                else if (field[(int)cellPosition.X, (int)cellPosition.Y + length].flag == flag.Brick)
                {
                    if (crashBrick != null)
                        crashBrick((int)cellPosition.X, (int)cellPosition.Y + length);
                }
                else if (field[(int)cellPosition.X, (int)cellPosition.Y + length].IsEmpty)
                {
                    Vector2 temp = new Vector2(position.X, position.Y + Field.cellSize * length);
                    fireCollection["downFires"].Add(new Fire(graphicsDevice, temp, textures, field));
                    stopFlags.down = true;
                }
            }

        }

        public void Update(GameTime gameTime, List<FireSource> List, Field field)
        {
            bool isEmpty = true;
            Burn(field);
            foreach (KeyValuePair<string, FireCollection> collection in fireCollection)
            {
                collection.Value.UpdateCollection(gameTime, field);
                if (!collection.Value.IsEmpty && isEmpty)
                    isEmpty = false;
            }
            if (isEmpty)
                List.Remove(this);        
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, FireCollection> collection in fireCollection)
                collection.Value.DrawCollection();
        }

        public static void incFireLength()
        {
            ++fireLength;
        }

    }
}
