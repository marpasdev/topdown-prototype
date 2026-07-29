using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace TopdownPrototype.Environment.Tiles
{
    public class TileInfo
    {
        public Point AtlasPosition { get; set; }
        public bool Walkable { get; set; } = true;
        public SoundEffect PlacingSound { get; set; }
        public SoundEffect BreakingSounds { get; set; }
        public SoundEffect WalkingSounds { get; set; }
        public int Priority { get; set; }
        public float Layer { get; set; }

    }
}
