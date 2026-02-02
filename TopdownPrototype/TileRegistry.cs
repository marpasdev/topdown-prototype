using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TopdownPrototype
{
    // merge with worldobject registry?
    internal static class TileRegistry
    {
        // maybe give it a better name?
        private static List<TileInfo> tileInfo;
        private static List<SlopeInfo> slopeInfo;
        public static ContentManager Content { get; set; }
        public static Texture2D Atlas { get; set; }

        static TileRegistry()
        {
            tileInfo = new List<TileInfo>();
            slopeInfo = new List<SlopeInfo>();    
        }

        public static void Load()
        {
            Atlas = Content.Load<Texture2D>("tile_atlas");

            tileInfo.Add(new TileInfo()
            {
                Texture = null
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("stone"),
                AtlasPosition = Point.Zero
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("dirt"),
                AtlasPosition = new Point(64, 0)
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("grass"),
                AtlasPosition = new Point(128, 0)
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("gravel"),
                AtlasPosition = new Point(0, 80)
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("sand"),
                AtlasPosition = new Point(64, 80)
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("water"),
                AtlasPosition = new Point(128, 80)
            });
            /*
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("mud")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("path")
            });*/
        }

        public static void LoadSlopes()
        {
            slopeInfo.Add(new SlopeInfo(0));
            slopeInfo.Add(new SlopeInfo(1)
            {
                Texture = Content.Load<Texture2D>("stone_slope")
            });
            slopeInfo.Add(new SlopeInfo(2)
            {
                Texture = Content.Load<Texture2D>("dirt_slope")
            });
        }

        public static TileInfo GetInfo(int tileID)
        {
            return tileInfo[tileID];
        }

        public static SlopeInfo GetSlopeInfo(int slopeID)
        {
            return slopeInfo[slopeID];
        }
    }
}
