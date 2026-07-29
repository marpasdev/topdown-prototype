using TopdownPrototype.Utils;

namespace TopdownPrototype.Environment
{
    public sealed class Chunk
    {
        public static int CHUNK_WIDTH = 16;
        public static int CHUNK_LENGTH = 16;
        public static int CHUNK_HEIGHT = 32;
        public Point3D Coord;
        public ushort[] Blocks;

        public bool IsDirty { get; set; }
        public bool IsGenerated { get; set; }

        public Chunk(Point3D coord)
        {
            Coord = coord;
            Blocks = new ushort[CHUNK_WIDTH * CHUNK_HEIGHT * CHUNK_LENGTH];
            IsDirty = false;
            IsGenerated = false;
        }

        private int Index(int lx, int ly, int lz)
        {
            return lx + ly * CHUNK_WIDTH + lz * CHUNK_WIDTH * CHUNK_LENGTH;
        }

        public ushort Get(int lx, int ly, int lz)
        {
            return Blocks[Index(lx, ly, lz)];
        }

        public void Set(int lx, int ly, int lz, ushort id)
        {
            Blocks[Index(lx, ly, lz)] = id;
            IsDirty = true;
        }

        public void CleanDirty() => IsDirty = false;
    }
}
