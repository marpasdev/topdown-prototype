using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TopdownPrototype
{
    internal static class WorldObjectRegistry
    {
        // maybe a better name??
        private static List<WorldObjectInfo> worldObjectInfo;
        public static ContentManager Content { get; set; }

        static WorldObjectRegistry()
        {
            worldObjectInfo = new List<WorldObjectInfo>();
        }

        public static void Load()
        {
            // serves as invisible barrier
            worldObjectInfo.Add(new WorldObjectInfo(0)
            {
                Texture = null
            });
            WorldObjectInfo stoneLarge = new WorldObjectInfo(1)
            {
                Texture = Content.Load<Texture2D>("stone_large"),
                DrawOffset = new Vector2(0, -1),
                LeftmostTile = new Point(1, 0)
            };
            stoneLarge.OccupiedTiles.Add(new Point(1, 0));
            worldObjectInfo.Add(stoneLarge);

            WorldObjectInfo spruce = new WorldObjectInfo(2)
            {
                Texture = Content.Load<Texture2D>("spruce"),
                DrawOffset = new Vector2(-1, -4),
                LeftmostTile = new Point(0, 0)
            };
            worldObjectInfo.Add(spruce);
        }

        public static WorldObjectInfo GetInfo(int worldObjectID)
        {
            return worldObjectInfo[worldObjectID];
        }
    }
}
