using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TopdownPrototype.Environment.WorldObjects
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
                DrawOffset = new Vector2(-1, -5),
                LeftmostTile = new Point(0, 0)
            };
            worldObjectInfo.Add(spruce);

            WorldObjectInfo porcino = new WorldObjectInfo(3)
            {
                Texture = Content.Load<Texture2D>("porcino_mushroom"),
                Walkable = true
            };
            worldObjectInfo.Add(porcino);

            WorldObjectInfo flyAmanita = new WorldObjectInfo(4)
            {
                Texture = Content.Load<Texture2D>("flyamanita"),
                Walkable = true
            };
            worldObjectInfo.Add(flyAmanita);

            WorldObjectInfo oak = new WorldObjectInfo(5)
            {
                Texture = Content.Load<Texture2D>("oak"),
                Walkable = false,
                DrawOffset = new Vector2(-1, -3),
                LeftmostTile = new Point(0, 0)
            };
            worldObjectInfo.Add(oak);

            WorldObjectInfo birch = new WorldObjectInfo(6)
            {
                Texture = Content.Load<Texture2D>("birch"),
                Walkable = false,
                DrawOffset = new Vector2(-1, -4),
                LeftmostTile = new Point(0, 0)
            };
            worldObjectInfo.Add(birch);

            WorldObjectInfo spruceStump = new WorldObjectInfo(7)
            {
                Texture = Content.Load<Texture2D>("spruce_stump"),
                Walkable = false
            };

            worldObjectInfo.Add(spruceStump);

            WorldObjectInfo stoneSmall = new WorldObjectInfo(8)
            {
                Texture = Content.Load<Texture2D>("stone_small"),
                Walkable = false
            };

            worldObjectInfo.Add(stoneSmall);

            WorldObjectInfo daisies = new WorldObjectInfo(9)
            {
                Texture = Content.Load<Texture2D>("daisies"),
                Walkable = true
            };

            worldObjectInfo.Add(daisies);
        }

        public static WorldObjectInfo GetInfo(int worldObjectID)
        {
            return worldObjectInfo[worldObjectID];
        }
    }
}
