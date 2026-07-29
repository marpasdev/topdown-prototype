using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TopdownPrototype.Entities;
using TopdownPrototype.Environment.Tiles;
using TopdownPrototype.Environment.WorldObjects;
using TopdownPrototype.Rendering;

namespace TopdownPrototype.Environment
{
    internal class Map
    {
        public int Width { get; set; } = 1024;
        public int Length { get; set; } = 1024;
        public int Height { get; set; } = 32;
        public int TileSize { get; set; } = 16;
        // add third dimension
        public TileType[,] Grid { get; set; }
        // stores the top elevation for each coordinate
        public int[,] Elevation { get; private set; }
        public WorldObject[,] OccupancyGrid { get; set; }
        public List<WorldObject> WorldObjects { get; set; }
        // rename this
        public GroundRenderGrid TerrainRenderGrid { get; set; }

        public Map(int width, int length, int maxElevationLevels)
        {
            Width = width;
            Length = length;
            Grid = new TileType[width, length];
            TerrainRenderGrid = new GroundRenderGrid(width, length);

            // assuming this defaults to null
            OccupancyGrid = new WorldObject[width, length];

            WorldObjects = new List<WorldObject>();

            WorldGenerator.Generate(this);

            WorldObjects.Sort(new RenderOrderComparer());

            WorldGenerator.Autotile(this);
        }

        // works with dual grid
        private void DrawTile(SpriteBatch spriteBatch, int x, int y)
        {
            TileInfo info = TileRegistry.GetInfo((int)Grid[x, y]);
            Vector2 offset;
            Vector2 leftCorner;     // left corner in the tileset

            // top left
            offset = new Vector2(-TileSize / 2);
            leftCorner = TileSize * new Vector2(TerrainRenderGrid.TopLeft[x, y].Variation % 4,
                TerrainRenderGrid.TopLeft[x, y].Variation / 4);
            //leftCorner = Vector2.Zero;
            spriteBatch.Draw(TileRegistry.Atlas, new Vector2(x, y) * TileSize + offset,
                new Rectangle(info.AtlasPosition.X + (int)leftCorner.X, info.AtlasPosition.Y + (int)leftCorner.Y,
                TileSize, TileSize), Color.White);


            // top right
            offset = new Vector2(TileSize / 2, -TileSize / 2);
            leftCorner = TileSize * new Vector2(TerrainRenderGrid.TopRight[x, y].Variation % 4,
                TerrainRenderGrid.TopRight[x, y].Variation / 4);
            spriteBatch.Draw(TileRegistry.Atlas, new Vector2(x, y) * TileSize + offset,
                new Rectangle(info.AtlasPosition.X + (int)leftCorner.X, info.AtlasPosition.Y + (int)leftCorner.Y,
                TileSize, TileSize), Color.White);

            // bottom left
            offset = new Vector2(-TileSize / 2, TileSize / 2);
            leftCorner = TileSize * new Vector2(TerrainRenderGrid.BottomLeft[x, y].Variation % 4,
                TerrainRenderGrid.BottomLeft[x, y].Variation / 4);
            spriteBatch.Draw(TileRegistry.Atlas, new Vector2(x, y) * TileSize + offset,
                new Rectangle(info.AtlasPosition.X + (int)leftCorner.X, info.AtlasPosition.Y + (int)leftCorner.Y,
                TileSize, TileSize), Color.White);

            // bottom right
            offset = new Vector2(TileSize / 2);
            leftCorner = TileSize * new Vector2(TerrainRenderGrid.BottomRight[x, y].Variation % 4,
                TerrainRenderGrid.BottomRight[x, y].Variation / 4);
            spriteBatch.Draw(TileRegistry.Atlas, new Vector2(x, y) * TileSize + offset,
                new Rectangle(info.AtlasPosition.X + (int)leftCorner.X, info.AtlasPosition.Y + (int)leftCorner.Y,
                TileSize, TileSize), Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 playerPosition, Player player)
        {
            int renderDistX = (int)(Camera.NativeScreenWidth / (TileSize * Camera.Zoom)) / 2 + 10;
            int renderDistY = (int)(Camera.NativeScreenHeight / (TileSize * Camera.Zoom)) / 2 + 10;
            // TODO: add render distance increment in corners

            Vector2 maxBound = new Vector2(Width, Length);
            Vector2 start = Vector2.Clamp(new Vector2(playerPosition.X / TileSize
                - renderDistX, playerPosition.Y / TileSize - renderDistX),
                Vector2.Zero, maxBound);
            Vector2 end = Vector2.Clamp(new Vector2(playerPosition.X / TileSize
                + renderDistX, playerPosition.Y / TileSize + renderDistY),
                Vector2.Zero, maxBound);
            Point playerPoint = new Point((int)(player.Feet.X / TileSize), (int)(player.Feet.Y / TileSize));
            playerPoint.X = Math.Clamp(playerPoint.X, 0, Width - 1);
            playerPoint.Y = Math.Clamp(playerPoint.Y, 0, Length - 1);

            var floorDrawQueue = new List<DrawTile>();

            // moving floor tiles to draw queue
            for (int y = (int)start.Y; y < (int)end.Y; y++)
            {
                for (int x = (int)start.X; x < (int)end.X; x++)
                {
                    DrawTile(spriteBatch, x, y);
                    floorDrawQueue.Add(new DrawTile()
                    {
                        Position = new Point(x, y),
                        Layer = TileRegistry.GetInfo((int)Grid[x, y]).Layer
                    });
                }
            }

            floorDrawQueue.Sort((a, b) =>
            {
                return a.Layer.CompareTo(b.Layer);
            });

            for (int i = 0; i < floorDrawQueue.Count; i++)
            {
                DrawTile(spriteBatch, floorDrawQueue[i].Position.X, floorDrawQueue[i].Position.Y);
            }

            for (int y = (int)start.Y; y < (int)end.Y; y++)
            {
                for (int x = (int)start.X; x < (int)end.X; x++)
                {
                    WorldObject obj = OccupancyGrid[x, y];
                    if (obj != null)
                    {
                        if (obj.LeftmostTile == new Point(x, y))
                        {
                            obj.Draw(spriteBatch, TileSize);
                        }
                    }

                    playerPoint = new Point((int)(player.Feet.X / TileSize), (int)((player.Feet.Y - TileSize / 2) / TileSize));
                    if (playerPoint == new Point(x - 1, y))
                    {
                        player.Draw(spriteBatch);
                    }
                }
            }
        }
    }

    internal class DrawTile
    {
        public Point Position;
        public float Layer;
    }
}
