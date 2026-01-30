using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        static TileRegistry()
        {
            tileInfo = new List<TileInfo>();
            slopeInfo = new List<SlopeInfo>();    
        }

        public static void Load()
        {
            tileInfo.Add(new TileInfo()
            {
                Texture = null
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("stone")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("dirt")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("grass")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("gravel")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("sand")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("water")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("mud")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("path")
            });
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
