using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopdownPrototype
{
    internal class Player
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public float Speed { get; set; } = 50f;
        public float RunMultiplier { get; set; } = 1.5f;
        private int previousScrollValue = 0;
        private MouseState previousMouseState; 

        public void Move(float deltaTime)
        {
            KeyboardState ks = Keyboard.GetState();

            Vector2 direction = Vector2.Zero;

            if (ks.IsKeyDown(Keys.W))
            {
                direction.Y -= 1;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                direction.Y += 1;
            }
            if (ks.IsKeyDown(Keys.A))
            {
                direction.X -= 1;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                direction.X += 1;
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            float speed = Speed;

            if (ks.IsKeyDown(Keys.LeftShift))
            {
                speed *= RunMultiplier;   
            }
            
            Position += direction * speed * deltaTime;
        }

        // TODO FIX: currently breaks render distance
        private void Zoom(float deltaTime)
        {
            MouseState ms = Mouse.GetState();

            if (ms.ScrollWheelValue > previousScrollValue)
            {
                Camera.TargetZoom *= 1.1f;
            } else if (ms.ScrollWheelValue < previousScrollValue)
            {
                Camera.TargetZoom /= 1.1f;
            }

            previousScrollValue = ms.ScrollWheelValue;
        }

        private void BoundPosition(Map map)
        {
            Position = Vector2.Clamp(Position, Vector2.Zero, map.TileSize * new Vector2(
                map.Width, map.Height) - new Vector2(Texture.Width, Texture.Height));
        }
        
        public void Update(GameTime gameTime, Map map)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Move(deltaTime);
            Zoom(deltaTime);
            BoundPosition(map);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}
