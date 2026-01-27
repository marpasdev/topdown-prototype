using System.Collections.Generic;

namespace TopdownPrototype
{
    internal class RenderOrderComparer : IComparer<WorldObject>
    {
        public int Compare(WorldObject obj1, WorldObject obj2)
        {
            int yComparison = obj1.AnchorTile.X.CompareTo(obj2.AnchorTile.X);
            if (yComparison == 0)
            {
                return obj1.AnchorTile.Y.CompareTo(obj2.AnchorTile.Y);
            } else
            {
                return yComparison;
            }
        }
    }
}
