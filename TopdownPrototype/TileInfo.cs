using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        // maybe move elsewhere later??
        //public SoundEffect WalkingSound { get; set; }
        public int Priority { get; set; }

    }
}
