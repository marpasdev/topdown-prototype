using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TopdownPrototype
{
    internal class WorldObjectInfo
    {
        public int ID { get; private set; }
        public Texture2D Texture { get; set; }
        public Vector2 DrawOffset { get; set; }
        public bool Walkable { get; set; } = false;
        public List<Point> OccupiedTiles { get; set; }

        public WorldObjectInfo(int id)
        {
            OccupiedTiles = new List<Point>();
            OccupiedTiles.Add(Point.Zero);
            ID = id;
        }
    }
}
