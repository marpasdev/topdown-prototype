using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TopdownPrototype
{
    // represents tile information associated with a certain tile type/id
    internal class TileInfo
    {
        //public int ID { get; set; }
        public Texture2D Texture { get; set; }
        //public string Name { get; set; }
        public bool Walkable { get; set; } = true;
        // maybe move elsewhere later??
        //public SoundEffect WalkingSound { get; set; }

    }
}
