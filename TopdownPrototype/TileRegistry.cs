using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TopdownPrototype
{
    internal static class TileRegistry
    {
        // maybe give it a better name?
        private static List<TileInfo> tileInfo;
        public static ContentManager Content { get; set; }

        static TileRegistry()
        {
            tileInfo = new List<TileInfo>();
        }

        public static void Load()
        {
            tileInfo.Add(new TileInfo()
            {
                Texture = null
            });
            tileInfo.Add(new TileInfo()
            {
                //Texture = Content.Load<Texture2D>("stone")
            });
            tileInfo.Add(new TileInfo()
            {
                //Texture = Content.Load<Texture2D>("dirt")
            });
            tileInfo.Add(new TileInfo()
            {
                Texture = Content.Load<Texture2D>("grass")
            });
            tileInfo.Add(new TileInfo()
            {
                //Texture = Content.Load<Texture2D>("gravel")
            });
            tileInfo.Add(new TileInfo()
            {
                //Texture = Content.Load<Texture2D>("sand")
            });
            tileInfo.Add(new TileInfo()
            {
                //Texture = Content.Load<Texture2D>("water")
            });
            tileInfo.Add(new TileInfo()
            {
                //Texture = Content.Load<Texture2D>("mud")
            });
            tileInfo.Add(new TileInfo()
            {
                //Texture = Content.Load<Texture2D>("path")
            });
        }

        public static TileInfo GetInfo(int tileID)
        {
            return tileInfo[tileID];
        }
    }
}
