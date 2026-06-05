using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TopdownPrototype
{
    // represents tile information associated with a certain tile type/id
    internal class TileInfo
    {
        //public int ID { get; set; }
        // deprecated
        public Texture2D Texture { get; set; }
        // keep this instead
        public Point AtlasPosition { get; set; }
        //public string Name { get; set; }
        public bool Walkable { get; set; } = true;
        // later add placing, breaking and walking sound
        public SoundEffect PlacingSound { get; set; }
        public int Priority { get; set; }
        public float Layer { get; set; }

    }
}
