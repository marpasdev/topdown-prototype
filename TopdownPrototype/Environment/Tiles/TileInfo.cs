using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace TopdownPrototype.Environment.Tiles
{
    public class TileInfo
    {
        public Point AtlasPosition { get; set; }
        public float Layer { get; set; }
        public int ConnectPriority { get; set; }
        public float Friction { get; set; } = 0.7f;
        public float WalkingSpeedModifier { get; set; } = 1.0f;
        public SoundEffect PlacementSound { get; set; }
    }
}
