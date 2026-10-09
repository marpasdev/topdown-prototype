using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TopdownPrototype.Environment.Tiles
{
    public static class TileRegistry
    {
        // TODO: better name
        private static List<TileInfo> tileInfo;
        public static ContentManager Content { get; set; }
        // restructure
        public static Texture2D Atlas { get; set; }

        static TileRegistry()
        {
            tileInfo = new List<TileInfo>();
        }

        // TODO: load from file?
        public static void Load()
        {
            Atlas = Content.Load<Texture2D>("tile_atlas");

            // void
            tileInfo.Add(new TileInfo());
            // currently no way to identify the tile
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = Point.Zero,
                Layer = 5f,
                // move to sound bank, reuse shared sounds
                PlacementSound = Content.Load<SoundEffect>("stone_place")
                // include priority everywhere
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(64, 0),
                PlacementSound = Content.Load<SoundEffect>("place"),
                Layer = 6f
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(128, 0),
                PlacementSound = Content.Load<SoundEffect>("place"),
                ConnectPriority = 0,
                Layer = 7f
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(0, 80),
                PlacementSound = Content.Load<SoundEffect>("gravel_place"),
                Layer = 2f
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(64, 80),
                PlacementSound = Content.Load<SoundEffect>("place"),
                ConnectPriority = 1,
                Layer = 3f
            });
            tileInfo.Add(new TileInfo()
            {
                AtlasPosition = new Point(128, 80),
                Layer = 4f,
                PlacementSound = Content.Load<SoundEffect>("water_splash")
            });
        }

        // use TileType instead
        public static TileInfo GetInfo(int tileID)
        {
            return tileInfo[tileID];
        }
    }
}
