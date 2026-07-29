using Microsoft.Xna.Framework;

namespace TopdownPrototype.Utils
{
    public class Point3D
    {
        public int X;
        public int Y;
        public int Z;

        public Point3D(int value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
