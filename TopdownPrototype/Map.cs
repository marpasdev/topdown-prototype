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
        // 32 pixelart looks stunning, maybe go with that in the future?
        public int TileSize { get; set; } = 16;
        // maybe rename to Tiles or TileGrid?
        public TileType[,] Grid { get; set; }
        //***** elevation - slopes and surfaces *****
        // format: [level, x, y]
        // would changing it to [x, y, z] be a reasonable thing?
        // maximum number of elevation levels
        public int MaxLevels { get; set; }
        // maybe merge with TileGrid later? Or possibly let it be this way, to be compatible with slopes
        public TileType[,,] SurfaceGrid { get; set; }
        // grid storing the type of elevated slope
        public SlopeType[,,] SlopeGrid { get; set; }
        // stores the top elevation for each coordinate
        public int[,] Elevation { get; private set; }
        public WorldObject[,] OccupancyGrid { get; set; }
        public List<WorldObject> WorldObjects { get; set; }
        // rename this
        public GroundRenderGrid TerrainRenderGrid { get; set; }

        public Map(int width, int height, int maxElevationLevels)
        {
            Width = width;
            Height = height;
            Grid = new TileType[width, height];
            TerrainRenderGrid = new GroundRenderGrid(width, height);

            // replace the hardcoded number at the end with something else
            maxElevationLevels = Math.Clamp(maxElevationLevels, 0, 10);
            MaxLevels = maxElevationLevels;

            SurfaceGrid = new TileType[MaxLevels, Width, Height];
            SlopeGrid = new SlopeType[MaxLevels, Width, Height];
            Elevation = new int[Width, Height];

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

            // make seed variable
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            noise.SetFractalOctaves(4);
            noise.SetFrequency(0.02f);

            float[,] noiseMap = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float value = noise.GetNoise(x, y);
                    // converting to [0, 1] instead of [-1, 1]
                    noiseMap[x, y] = (value + 1) / 2;
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float value = noiseMap[x, y];
                    if (value <= 0.25f)
                    {
                        Grid[x, y] = TileType.Water;
                    }
                    else if (value > 0.25f && value <= 0.3f)
                    {
                        Grid[x, y] = TileType.Sand;
                    }
                    else if (value > 0.3f && value <= 0.8f)
                    {
                        Grid[x, y] = TileType.Grass;
                    }
                    else if (value > 0.8f && value <= 1.0f)
                    {
                        Grid[x, y] = TileType.Stone;
                        SlopeGrid[0, x, y] = SlopeType.Stone;
                        Elevation[x, y] = 1;
                        SurfaceGrid[0, x, y] = TileType.Gravel;
                    }
                }
            }

            Random random = new Random();

            for (int y = 10; y < Height - 10; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Grid[x, y] == TileType.Water) { continue; }
                    if (Grid[x, y] == TileType.Stone) { continue; }

                    int randInt = random.Next(40);
                    if (randInt >= 1 && randInt <= 5)
                    {
                        if (Grid[x, y] == TileType.Sand) { continue; }
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

        // TODO: get rid of playerPosition param
        public void Draw(SpriteBatch spriteBatch, Vector2 playerPosition, Player player)
        {
            // render distance
            // TODO: remove hardcoded values, maybe evaluate it based on the screen resolution etc.
            // funny how this ended up being 16 by 9, I guess it's the aspect ratio after all
            // this needs to update the values based on zoom level and the player's position
            // if the player is in the world's corner, the radius needs to be bigger
            int renderDistX = 32;
            int renderDistY = 20;
            Vector2 maxBound = new Vector2(Width, Height);
            Vector2 start = Vector2.Clamp(new Vector2(playerPosition.X / TileSize
                - renderDistX, playerPosition.Y / TileSize - renderDistX),
                Vector2.Zero, maxBound);
            Vector2 end = Vector2.Clamp(new Vector2(playerPosition.X / TileSize
                + renderDistX, playerPosition.Y / TileSize + renderDistY),
                Vector2.Zero, maxBound);
            Point playerPoint = new Point((int)(player.Feet.X / TileSize), (int)(player.Feet.Y / TileSize));
            playerPoint.X = Math.Clamp(playerPoint.X, 0, Width - 1);
            playerPoint.Y = Math.Clamp(playerPoint.Y, 0, Height - 1);
            for (int y = (int)start.Y; y < (int)end.Y; y++)
            {
                for (int x = (int)start.X; x < (int)end.X; x++)
                {
                    if (Elevation[x, y] > 0) 
                    {
                        // draw surface
                        spriteBatch.Draw(TileRegistry.GetInfo((int)SurfaceGrid[Elevation[x, y] - 1, x, y]).Texture
                            , TileSize * new Vector2(x, y - 1), Color.White);

                        // draw slope
                        if (y != Height - 1)
                        {
                            if (Elevation[x, y + 1] < Elevation[x, y])
                            {
                                spriteBatch.Draw(TileRegistry.GetSlopeInfo((int)SlopeGrid[
                                    Elevation[x, y] - 1, x, y])
                                    .Texture, TileSize * new Vector2(x, y), Color.White);
                            }
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(TileRegistry.GetInfo((int)Grid[x, y]).Texture
                            , TileSize * new Vector2(x, y), Color.White);
                        // world object
                        // TODO: needs to be moved alongside elevation, otherwise it will be drawn on top of
                        WorldObject obj = OccupancyGrid[x, y];
                        if (obj != null)
                        {
                            if (obj.LeftmostTile == new Point(x, y))
                            {
                                obj.Draw(spriteBatch, TileSize);
                            }
                        } 
                    }

                    if (playerPoint == new Point(x - 1, y))
                    {
                        player.Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
