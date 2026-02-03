using System;
using Microsoft.Xna.Framework;

namespace TopdownPrototype
{
    internal static class WorldGenerator
    {

        public static void Generate(Map map)
        {  
            // TODO: this is basic map generating - will be divided and improved later
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    map.Grid[x, y] = TileType.Grass;
                }
            }

            // make seed variable
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
            noise.SetFractalOctaves(4);
            noise.SetFrequency(0.02f);

            float[,] noiseMap = new float[map.Width, map.Height];

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    float value = noise.GetNoise(x, y);
                    // converting to [0, 1] from [-1, 1]
                    noiseMap[x, y] = (value + 1) / 2;
                }
            }

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    float value = noiseMap[x, y];
                    if (value <= 0.25f)
                    {
                        map.Grid[x, y] = TileType.Water;
                    }
                    else if (value > 0.25f && value <= 0.3f)
                    {
                        map.Grid[x, y] = TileType.Sand;
                    }
                    else if (value > 0.3f && value <= 0.8f)
                    {
                        map.Grid[x, y] = TileType.Grass;
                    }
                    else if (value > 0.8f && value <= 1.0f)
                    {
                        map.Grid[x, y] = TileType.Stone;
                        map.SlopeGrid[0, x, y] = SlopeType.Stone;
                        map.Elevation[x, y] = 1;
                        map.SurfaceGrid[0, x, y] = TileType.Gravel;
                    }
                }
            }

            Random random = new Random();

            for (int y = 10; y < map.Height - 10; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map.Grid[x, y] == TileType.Water) { continue; }
                    if (map.Grid[x, y] == TileType.Stone) { continue; }

                    int randInt = random.Next(40);
                    if (randInt >= 1 && randInt <= 5)
                    {
                        if (map.Grid[x, y] == TileType.Sand) { continue; }
                        WorldObject tree = new WorldObject(new Point(x, y));
                        tree.Info = WorldObjectRegistry.GetInfo((int)WorldObjectType.SpruceTree);
                        tree.GetPlaced(map.OccupancyGrid, map.WorldObjects);
                    }
                    else if (randInt == 10)
                    {
                        WorldObject stone = new WorldObject(new Point(x, y));
                        stone.Info = WorldObjectRegistry.GetInfo((int)WorldObjectType.StoneLarge);
                        stone.GetPlaced(map.OccupancyGrid, map.WorldObjects);
                    }
                    else if (randInt == 11)
                    {
                        WorldObject porcino = new WorldObject(new Point(x, y));
                        porcino.Info = WorldObjectRegistry.GetInfo((int)WorldObjectType.Porcino);
                        porcino.GetPlaced(map.OccupancyGrid, map.WorldObjects);
                    }
                    else if (randInt == 12)
                    {
                        WorldObject flyAmanita = new WorldObject(new Point(x, y));
                        flyAmanita.Info = WorldObjectRegistry.GetInfo((int)WorldObjectType.FlyAmanita);
                        flyAmanita.GetPlaced(map.OccupancyGrid, map.WorldObjects);
                    }
                }
            }


        }

        private static byte GetNeighborMask(Map map, int x, int y)
        {
            byte mask = 0;

            

            return mask;
        }

        public static void Autotile(Map map)
        {
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    // determining neighbors
                    bool n = false, s = false, w = false, e = false, nw = false, ne = false, sw = false, se = false;
                    if (y > 0 && map.Grid[x, y - 1] == map.Grid[x, y])
                    {
                        n = true;
                    }
                    if (x > 0 && map.Grid[x - 1, y] == map.Grid[x, y])
                    {
                        w = true;
                    }
                    if (y > 0 && x > 0 && map.Grid[x - 1, y - 1] == map.Grid[x, y])
                    {
                        nw = true;
                    }
                    if (y > 0 && x < map.Width - 1 && map.Grid[x + 1, y - 1] == map.Grid[x, y])
                    {
                        ne = true;
                    }
                    if (x > 0 && y < map.Height - 1 && map.Grid[x - 1, y + 1] == map.Grid[x, y])
                    {
                        sw = true;
                    }
                    if (y < map.Height - 1 && map.Grid[x, y + 1] == map.Grid[x, y])
                    {
                        s = true;
                    }
                    if (x < map.Width - 1 && map.Grid[x + 1, y] == map.Grid[x, y])
                    {
                        e = true;
                    }
                    if (x < map.Width - 1 && y < map.Height - 1 && map.Grid[x + 1, y + 1] == map.Grid[x, y])
                    {
                        se = true;
                    }

                    // handling edges
                    if (x == 0)
                    {
                        w = true;
                        if (y > 0)
                        {
                            if (map.Grid[x, y - 1] == map.Grid[x, y])
                            {
                                nw = true;
                            }
                        }

                        if (y < map.Height - 1)
                        {
                            if (map.Grid[x, y + 1] == map.Grid[x, y])
                            {
                                sw = true;
                            }

                        }
                        if (y == map.Height - 1)
                        {
                            sw = true;
                        }
                        if (y == 0)
                        {
                            nw = true;
                        }
                    }
                    if (y == 0)
                    {
                        n = true;
                        if (x > 0)
                        {
                            if (map.Grid[x - 1, y] == map.Grid[x, y])
                            {
                                nw = true;
                            }
                        }
                        if (x < map.Width - 1)
                        {
                            if (map.Grid[x + 1, y] == map.Grid[x, y])
                            {
                                ne = true;
                            }
                        }
                    }

                    if (x == map.Width - 1)
                    {
                        e = true;
                        if (y > 0)
                        {
                            if (map.Grid[x, y - 1] == map.Grid[x, y])
                            {
                                ne = true;
                            }
                        }
                        if (y < map.Height - 1)
                        {
                            if (map.Grid[x, y + 1] == map.Grid[x, y])
                            {
                                se = true;
                            }

                        }
                        if (y == map.Height - 1)
                        {
                            se = true;
                        }
                        if (y == 0)
                        {
                            ne = true;
                        }
                    }

                    if (y == map.Height - 1)
                    {
                        s = true;
                        if (x > 0)
                        {
                            if (map.Grid[x - 1, y] == map.Grid[x, y])
                            {
                                sw = true;
                            }
                        }
                        if (x < map.Width - 1)
                        {
                            if (map.Grid[x + 1, y] == map.Grid[x, y])
                            {
                                se = true;
                            }
                        }
                    }

                    // isn't this a bit stupid?
                    map.TerrainRenderGrid.TopLeft[x, y].Type = map.Grid[x, y];
                    map.TerrainRenderGrid.TopRight[x, y].Type = map.Grid[x, y];
                    map.TerrainRenderGrid.BottomLeft[x, y].Type = map.Grid[x, y];
                    map.TerrainRenderGrid.BottomRight[x, y].Type = map.Grid[x, y];

                    // assigning variations based on neighbors

                    // top left
                    if (!n && !nw && !w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 13;
                    }
                    else if (n && !nw && !w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 1;
                    }
                    else if (!n && nw && !w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 4;

                        if (map.Grid[x - 1, y] == map.Grid[x, y - 1] &&
                            TileRegistry.GetInfo((int)map.Grid[x, y - 1]).Priority > TileRegistry.GetInfo((int)map.Grid[x, y]).Priority)
                        {
                            map.TerrainRenderGrid.TopLeft[x, y].Variation = 13;
                        }
                    }
                    else if (!n && !nw && w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 3;
                    }
                    else if (n && nw && !w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 10;
                    }
                    else if (n && !nw && w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 5;
                    }
                    else if (!n && nw && w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 2;
                    }
                    else if (n && nw && w)
                    {
                        map.TerrainRenderGrid.TopLeft[x, y].Variation = 6;
                    }

                    // top right
                    if (!n && !ne && !e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 0;
                    }
                    else if (n && !ne && !e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 11;
                    }
                    else if (!n && ne && !e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 14;

                        if (map.Grid[x, y - 1] == map.Grid[x + 1, y] &&
                            TileRegistry.GetInfo((int)map.Grid[x, y - 1]).Priority > TileRegistry.GetInfo((int)map.Grid[x, y]).Priority)
                        {
                            map.TerrainRenderGrid.TopRight[x, y].Variation = 0;
                        }
                    }
                    else if (!n && !ne && e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 3;
                    }
                    else if (n && ne && !e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 7;
                    }
                    else if (n && !ne && e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 2;
                    }
                    else if (!n && ne && e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 5;
                    }
                    else if (n && ne && e)
                    {
                        map.TerrainRenderGrid.TopRight[x, y].Variation = 6;
                    }

                    // bottom left
                    if (!s && !sw && !w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 8;
                    }
                    else if (s && !sw && !w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 1;
                    }
                    else if (!s && sw && !w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 14;
 
                        if (map.Grid[x, y + 1] == map.Grid[x - 1, y] &&
                            TileRegistry.GetInfo((int)map.Grid[x, y + 1]).Priority > TileRegistry.GetInfo((int)map.Grid[x, y]).Priority)
                        {
                            map.TerrainRenderGrid.BottomLeft[x, y].Variation = 8;
                        }
                    }
                    else if (!s && !sw && w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 9;
                    }
                    else if (s && sw && !w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 5;
                    }
                    else if (s && !sw && w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 10;
                    }
                    else if (!s && sw && w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 7;
                    }
                    else if (s && sw && w)
                    {
                        map.TerrainRenderGrid.BottomLeft[x, y].Variation = 6;
                    }

                    // bottom right
                    if (!s && !se && !e)
                    {
                        map.TerrainRenderGrid.BottomRight[x, y].Variation = 15;
                    }
                    else if (s && !se && !e)
                    {
                    map.TerrainRenderGrid.BottomRight[x, y].Variation = 11;
                    }
                    else if (!s && se && !e)
                    {
                        map.TerrainRenderGrid.BottomRight[x, y].Variation = 4;

                        if (map.Grid[x, y + 1] == map.Grid[x + 1, y] &&
                            TileRegistry.GetInfo((int)map.Grid[x, y + 1]).Priority > TileRegistry.GetInfo((int)map.Grid[x, y]).Priority)
                        {
                            map.TerrainRenderGrid.BottomRight[x, y].Variation = 15;
                        }
                    }
                    else if (!s && !se && e)
                    {
                        map.TerrainRenderGrid.BottomRight[x, y].Variation = 9;
                    }
                    else if (s && se && !e)
                    {
                        map.TerrainRenderGrid.BottomRight[x, y].Variation = 2;
                    }
                    else if (s && !se && e)
                    {
                        map.TerrainRenderGrid.BottomRight[x, y].Variation = 7;
                    }
                    else if (!s && se && e)
                    {
                        map.TerrainRenderGrid.BottomRight[x, y].Variation = 10;
                    }
                    else if (s && se && e)
                    {
                        map.TerrainRenderGrid.BottomRight[x, y].Variation = 6;
                    }
                }
            }
        }
    }
}
