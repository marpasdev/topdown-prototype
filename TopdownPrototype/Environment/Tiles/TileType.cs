namespace TopdownPrototype.Environment.Tiles
{
    internal enum TileType : ushort
    {
        Void = 0,
        Stone = 1,
        Dirt = 2,
        Grass = 3,
        Gravel = 4,
        Sand = 5,
        Water = 6
        // Snow, Ice, Mud, Wooden floor, path, lava
        // transitions: grass -> path -> dirt -> stone
    }
}
