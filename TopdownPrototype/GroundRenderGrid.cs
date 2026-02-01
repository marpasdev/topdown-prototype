using System;

namespace TopdownPrototype
{
    internal class GroundRenderGrid
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public RenderTile[,] TopLeft { get; set; }
        public RenderTile[,] TopRight { get; set; }
        public RenderTile[,] BottomLeft { get; set; }
        public RenderTile[,] BottomRight { get; set; }

        public GroundRenderGrid(int width, int height)
        {
            Width = width;
            Height = height;
            TopLeft = new RenderTile[Width, Height];
            TopRight = new RenderTile[Width, Height];
            BottomLeft = new RenderTile[Width, Height];
            BottomRight = new RenderTile[Width, Height];
        }

        public struct RenderTile
        {
            public TileType Type { get; set; }
            public int Variation { get; set; }
        
            public RenderTile(TileType type, int variation)
            {
                Type = type;
                Variation = variation;
            }
        }
    }

}
