using TopdownPrototype.Utils;

namespace TopdownPrototype.Environment
{
    public static class WorldCoords
    {
        public static Point3D ToChunkCoord(int wx, int wy, int wz)
        {
            return new Point3D(
                wx / Constants.ChunkWidth,
                wy / Constants.ChunkLength,
                wz / Constants.ChunkHeight
                );
        }

        public static Point3D ToLocalCoord(int wx, int wy, int wz)
        {
            return new Point3D(
                wx % Constants.ChunkWidth,
                wy % Constants.ChunkLength,
                wz % Constants.ChunkHeight
                );
        }

        public static Point3D ToWorldCoord(Point3D chunkCoord, Point3D localCoord)
        {
            return new Point3D(
                chunkCoord.X * Constants.ChunkWidth + localCoord.X,
                chunkCoord.Y * Constants.ChunkLength + localCoord.Y,
                chunkCoord.Z * Constants.ChunkHeight + localCoord.Z
                );
        }
    }
}
