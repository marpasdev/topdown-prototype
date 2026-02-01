using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TopdownPrototype
{
    internal class WorldObject
    {
        public Point AnchorTile { get; set; }
        public WorldObjectInfo Info { get; set; }
        public Point LeftmostTile
        {
            get
            {
                return AnchorTile + Info.LeftmostTile;
            }
        }

        public WorldObject(Point anchorTile)
        {
            AnchorTile = anchorTile;
        }

        // I am wondering where this is the correct approach. Should each WorldObject hold
        // its placing logic or should this instead be handled in a different class?
        public bool CanBePlaced(WorldObject[,] occupancyGrid)
        {
            foreach (Point pRel in Info.OccupiedTiles)
            {
                Point pAbs = AnchorTile + pRel;
                if (pAbs.X < 0 || pAbs.Y < 0 || pAbs.X >= occupancyGrid.GetLength(0) ||
                    pAbs.Y >= occupancyGrid.GetLength(1))
                {
                    return false;
                }
                
                if (occupancyGrid[pAbs.X, pAbs.Y] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetPlaced(WorldObject[,] occupancyGrid, List<WorldObject> objects)
        {
            if (CanBePlaced(occupancyGrid))
            {
                foreach (Point pRel in Info.OccupiedTiles)
                {
                    Point pAbs = AnchorTile + pRel;
                    occupancyGrid[pAbs.X, pAbs.Y] = this;
                }
                objects.Add(this);

                return true;
            } 
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int tileSize)
        {
            if (Info.Texture == null) { return; }
            // specifying the layer depth was supposed to fix an issue, instead it broke it even more
            spriteBatch.Draw(Info.Texture, tileSize * new Vector2((float)AnchorTile.X + Info.DrawOffset.X,
                (float)AnchorTile.Y + Info.DrawOffset.Y), null, Color.White, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, -0.0f);
        }
    }
}
