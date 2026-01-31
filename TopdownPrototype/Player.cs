using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopdownPrototype
{
    internal class Player
    {
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                collider = new RectangleF(position.X, 14 + position.Y, collider.Width, collider.Height);
            }
        }
        private Vector2 previousPosition;
        // elevation of the player 
        private int elevation = 0;
        public required Texture2D Texture { get; set; }
        private RectangleF collider;
        public RectangleF Collider
        {
            get
            {
                return collider;
            }
            set
            {
                collider = value;
                position = new Vector2(collider.X, collider.Y - 14);
            }
        }
        public float Speed { get; set; } = 50f;
        public float RunMultiplier { get; set; } = 1.5f;
        private int previousScrollValue = 0;

        public Player()
        {
            Collider = new RectangleF(Position.X, 14 + Position.Y, 12, 14);
        }

        public void Move(float deltaTime, Map map)
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

            previousPosition = Position;
            Position += new Vector2(direction.X * speed * deltaTime, 0);
            HandleWorldCollision(map, 'x');

            previousPosition = Position;
            Position += new Vector2(0, direction.Y * speed * deltaTime);
            HandleWorldCollision(map, 'y');
        }

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

        private void HandleWorldCollision(Map map, char axis)
        {
            Point colliderTile = new Point((int)(Collider.X / map.TileSize),
                (int)(Collider.Y / map.TileSize));

            int radius = 2;
            Point start = new Point(Math.Clamp(colliderTile.X - radius, 0, map.Width - 1),
                Math.Clamp(colliderTile.Y - radius, 0, map.Height - 1));
            Point end = new Point(Math.Clamp(colliderTile.X + radius, 0, map.Width - 1),
                Math.Clamp(colliderTile.Y + radius, 0, map.Height - 1));

            Vector2 velocity = Position - previousPosition;

            // elevation
            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int x = start.X; x <= end.X; x++)
                {
                    if (map.Elevation[x, y] != elevation)
                    {
                        RectangleF tileRect = new RectangleF(x * map.TileSize, y * map.TileSize,
                            map.TileSize, map.TileSize);

                        if (Collider.Intersects(tileRect) && axis == 'x')
                        {
                            if (velocity.X > 0)
                            {
                                Collider = new RectangleF(tileRect.Left - Collider.Width, Collider.Y, Collider.Width,
                                         Collider.Height);
                            }
                            else if (velocity.X < 0)
                            {
                                Collider = new RectangleF(tileRect.Right, Collider.Y, Collider.Width,
                                         Collider.Height);
                            }
                            return;
                        }
                        if (Collider.Intersects(tileRect) && axis == 'y') 
                        {
                            if (velocity.Y > 0)
                            {
                                Collider = new RectangleF(Collider.X, tileRect.Top - Collider.Height, Collider.Width,
                                         Collider.Height);
                            }
                            else if (velocity.Y < 0)
                            {
                                Collider = new RectangleF(Collider.X, tileRect.Bottom, Collider.Width,
                                    Collider.Height);
                            }
                            return;
                        }
                    }
                }
            }
            // world objects
        }
        
        public void Update(GameTime gameTime, Map map)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Move(deltaTime, map);
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
