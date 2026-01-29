using System;
using Microsoft.Xna.Framework;

namespace TopdownPrototype
{
    internal static class Camera
    {
        public static Vector2 Center { get; set; }
        public static Matrix Transform { get; set; }
        public static float MinZoom { get; set; } = 3;
        public static float MaxZoom { get; set; } = 7;
        public static float Zoom { get; private set; } = (MaxZoom + MinZoom) / 2;
        private static float targetZoom = Zoom;
        public static float TargetZoom
        {
            get => targetZoom;
            set
            {
                targetZoom = MathHelper.Clamp(value, MinZoom, MaxZoom);
            }
        }
        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }
        private static float zoomSpeed = 100;

        private static Matrix CreateTranslation(Vector2 center)
        {
            float dx = ((ScreenWidth / 2) - center.X * Zoom);
            float dy = ((ScreenHeight / 2) - center.Y * Zoom);
            return Matrix.CreateTranslation(new Vector3(dx, dy, 0));
        }

        private static Matrix CreateZoom()
        {
            return Matrix.CreateScale(Zoom, Zoom, 1.0f);
        }

        private static void UpdateZoom(float deltaTime)
        {
            Zoom = MathHelper.Lerp(Zoom, targetZoom, 0.1f * zoomSpeed * deltaTime);   
        }

        private static Vector2 Clamp(Vector2 center, Map map)
        {
            Vector2 halfScreen = new Vector2(ScreenWidth / 2 / Zoom, ScreenHeight / 2 / Zoom);
            Vector2 bound = new Vector2(map.Width * map.TileSize, map.Height * map.TileSize) - halfScreen;
            return Vector2.Clamp(center, halfScreen,
                bound);
        }

        public static void Update(GameTime gameTime, Vector2 center, Map map)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateZoom(deltaTime);
            Transform = CreateZoom() * CreateTranslation(Clamp(center, map));
        }
    }
}
