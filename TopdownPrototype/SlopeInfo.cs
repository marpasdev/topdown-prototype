using Microsoft.Xna.Framework.Graphics;

namespace TopdownPrototype
{
    internal class SlopeInfo
    {
        public int ID { get; }
        public Texture2D Texture { get; set; }

        public SlopeInfo(int id)
        {
            ID = id;
        }
    }
}
