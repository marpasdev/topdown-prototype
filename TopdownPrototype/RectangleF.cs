using System.Numerics;

namespace TopdownPrototype
{
    // add other method that regular Rectangle class contains
    internal struct RectangleF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Intersects(RectangleF other)
        {
            if (X < other.Right && Right > other.X &&
                Y < other.Bottom && Bottom > other.Y)
            {
                return true;
            }
            return false;
        }
    }
}
