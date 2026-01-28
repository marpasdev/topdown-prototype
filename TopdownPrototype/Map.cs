using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace TopdownPrototype
{
    internal class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        // maybe rename to Tiles or TileGrid?
        public TileType[,] Grid { get; set; }
        // 32 pixelart looks stunning, maybe go with that in the future?
        public int TileSize { get; set; } = 16;
        public WorldObject[,] OccupancyGrid { get; set; }
        public List<WorldObject> WorldObjects { get; set; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new TileType[width, height];

            // assuming this defaults to null
            OccupancyGrid = new WorldObject[width, height];

            WorldObjects = new List<WorldObject>();

            // TODO: this is basic map generating - later move it to its standalone class
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Grid[x, y] = TileType.Grass;
                }
            }

            Random random = new Random();

            for (int y = 10; y < Height - 10; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int randInt = random.Next(40);
                    if (randInt >= 1 && randInt <= 5)
                    {
                        WorldObject tree = new WorldObject(new Point(x, y));
                        tree.Info = WorldObjectRegistry.GetInfo((int)WorldObjectType.SpruceTree);
                        tree.GetPlaced(OccupancyGrid, WorldObjects);
                    }
                    else if (randInt == 10)
                    {
                        WorldObject stone = new WorldObject(new Point(x, y));
                        stone.Info = WorldObjectRegistry.GetInfo((int)WorldObjectType.StoneLarge);
                        stone.GetPlaced(OccupancyGrid, WorldObjects);
                    }
                }
            }

            WorldObjects.Sort(new RenderOrderComparer());

        }

        // TODO: split into drawing for ground, world objects and terrain
        public void Draw(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            // render distance
            // TODO: remove hardcoded values, maybe evaluate it based on the screen resolution etc.
            int renderDistX = 10;
            int renderDistY = 7;
            Vector2 maxBound = new Vector2(Width, Height);
            Vector2 start = Vector2.Clamp(new Vector2(playerPosition.X / TileSize
                - renderDistX, playerPosition.Y / TileSize - renderDistX),
                Vector2.Zero, maxBound);
            Vector2 end = Vector2.Clamp(new Vector2(playerPosition.X / TileSize
                + renderDistX, playerPosition.Y / TileSize + renderDistY),
                Vector2.Zero, maxBound);
            for (int y = (int)start.Y; y < (int)end.Y; y++)
            {
                for (int x = (int)start.X; x < (int)end.X; x++)
                {
                    spriteBatch.Draw(TileRegistry.GetInfo((int)Grid[x, y]).Texture
                        , TileSize * new Vector2(x, y), Color.White);
                }
            }

            for (int i = 0; i < WorldObjects.Count; i++)
            {
                WorldObjects[i].Draw(spriteBatch, TileSize);
            }
        }
    }
}
