using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TopdownPrototype.Environment.Tiles
{
    // merge with worldobject registry?
    public static class TileRegistry
    {
        // maybe give it a better name?
        private static List<TileInfo> tileInfo;
        public static ContentManager Content { get; set; }
        public static Texture2D Atlas { get; set; }

        static TileRegistry()
        {
            tileInfo = new List<TileInfo>();
        }

        public static void Load()
        {
            Atlas = Content.Load<Texture2D>("tile_atlas");

            tileInfo.Add(new TileInfo());
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = Point.Zero,
                Layer = 1f
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(64, 0)
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(128, 0),
                PlacingSound = Content.Load<SoundEffect>("place"),
                Priority = 0,
                Layer = 4f
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(0, 80)
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(64, 80),
                PlacingSound = Content.Load<SoundEffect>("place"),
                Priority = 1,
                Layer = 2f
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(128, 80),
                Layer = 3f
            });
        }

        public static TileInfo GetInfo(int tileID)
        {
            return tileInfo[tileID];
        }
    }
}
