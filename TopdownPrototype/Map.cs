using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

            // assuming this defaults to false
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

            WorldObject stone = new WorldObject(new Point(1, 1));
            stone.Info = WorldObjectRegistry.GetInfo((int)WorldObjectType.StoneLarge);
            stone.GetPlaced(OccupancyGrid, WorldObjects);

        }

        // TODO: split into drawing for ground, world objects and terrain
        public void Draw(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            // render distance
            // TODO: remove hardcoded values, maybe evaluate it based on the screen resolution etc.
            int renderDistX = 3;
            int renderDistY = 3;
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

            // TODO: resolve drawing in order
            for (int i = 0; i < WorldObjects.Count; i++)
            {
                WorldObjects[i].Draw(spriteBatch, TileSize);
            }
        }
    }
}
