using System.Collections.Generic;
using TopdownPrototype.Utils;

namespace TopdownPrototype.Environment
{
    public class World
    {
        public readonly int Width = 1024;
        public readonly int Length = 1024;
        public const int Height = Constants.WorldHeight;
        private Dictionary<Point3D, Chunk> chunks = new();
        public readonly bool IsWrapping = false;

        public World() { }

        public World(int width, int length)
        {
            Width = width;
            Length = length;
        }

        public Chunk GetChunk(Point3D coord)
        {
            return chunks.TryGetValue(coord, out Chunk c) ? c : null;
        }

        public Chunk GetOrCreateChunk(Point3D coord)
        {
            if (!chunks.TryGetValue(coord, out Chunk chunk))
            {
                chunk = new Chunk(coord);
                chunks[coord] = chunk;
            }
            return chunk;
        }

        public void RegisterChunk(Chunk chunk)
        {
            chunks[chunk.Coord] = chunk;
        }

        public void UnloadChunk(Point3D coord)
        {
            chunks.Remove(coord);
        }

        public IEnumerable<Chunk> GetLoadedChunks() => chunks.Values;

        public IEnumerable<Point3D> GetLoadedChunkCoords() => chunks.Keys;

        public int LoadedChunkCount => chunks.Count;

        public ushort GetBlock(int wx, int wy, int wz)
        {
            Point3D chunkCoord = WorldCoords.ToChunkCoord(wx, wy, wz);
            Point3D localCoord = WorldCoords.ToLocalCoord(wx, wy, wz);
            return chunks[chunkCoord].Get(localCoord.X, localCoord.Y, localCoord.Z);
        }

        public void SetBlock(int wx, int wy, int wz, ushort blockId)
        {
            Point3D chunkCoord = WorldCoords.ToChunkCoord(wx, wy, wz);
            Point3D localCoord = WorldCoords.ToLocalCoord(wx, wy, wz);
            chunks[chunkCoord].Set(localCoord.X, localCoord.Y, localCoord.Z, blockId);
        }

        public int GetSurvaceZ(int wx, int wy)
        {
            return -1;
        }

    }
}
