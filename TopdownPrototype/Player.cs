using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        public Vector2 Feet => new Vector2(Position.X + 6, Position.Y + 27);
        // elevation of the player 
        private int elevation = 0;
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
        private MouseState previousMouseState;
        public Texture2D Texture { get; set; }
        private AnimationManager animations;
        private PlayerState previousState;
        private PlayerState state;
        public int Width { get; } = 14;
        public int Height { get; } = 28;


        public Player(Texture2D texture, Texture2D walkingDownTexture)
        {
            Texture = texture;
            Collider = new RectangleF(Position.X, 14 + Position.Y, 12, 14);
            animations = new();
            animations.Animations.Add("Idle", new Animation(Texture, 1, 5));
            animations.Animations.Add("WalkingDown", new Animation(walkingDownTexture, 4, 0.09f));
            animations.SwitchAnimation("Idle");
            state = PlayerState.Idle;
            previousState = PlayerState.Idle;

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

            previousState = state;
            if (direction.Y > 0)
            {
                state = PlayerState.WalkingDown;
            }
            else
            {
                state = PlayerState.Idle;
            }
        }

        private void Zoom(float deltaTime)
        {
            MouseState ms = Mouse.GetState();

            if (ms.ScrollWheelValue > previousScrollValue)
            {
                Camera.TargetZoom *= 1.1f;
            }
            else if (ms.ScrollWheelValue < previousScrollValue)
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
                    // will this certainly not cause any strange behaviour regarding null here??
                    if (map.Elevation[x, y] != elevation || (map.OccupancyGrid[x, y] != null &&
                        !map.OccupancyGrid[x, y].Info.Walkable))
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
        }

        public void Update(GameTime gameTime, Map map, Point renderDestination)
        {
            if (previousState == PlayerState.Idle && state == PlayerState.WalkingDown)
            {
                animations.SwitchAnimation("WalkingDown");
            }
            else if (previousState == PlayerState.WalkingDown && state == PlayerState.Idle)
            {
                animations.SwitchAnimation("Idle");
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Move(deltaTime, map);
            animations.Update(gameTime);
            Texture = animations.CurrentAnimation.Texture;
            Zoom(deltaTime);
            BoundPosition(map);
            Interact(map, renderDestination);
            previousMouseState = Mouse.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture is not null)
            {
                spriteBatch.Draw(animations.CurrentAnimation.Texture, Position, animations.CurrentAnimation.SourceRectangle, Color.White);
            }
        }

        // testing purposes - rework later
        public void Interact(Map map, Point renderDestination)
        {
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                //Vector2 screenPosition = ms.Position.ToVector2() - new Vector2(renderDestination.X, renderDestination.Y);
                //Vector2 screenPosition = Vector2.Transform(ms.Position.ToVector2(),
                //    Matrix.CreateScale((float)Camera.NativeScreenWidth / Camera.ScreenWidth,
                //    (float)Camera.NativeScreenHeight / Camera.ScreenHeight, 1));
                //Vector2 worldPosition = Vector2.Transform(screenPosition, Matrix.Invert(Camera.Transform));

                //Point gridPosition = new Point
                //(
                //    MathHelper.Clamp((int)(worldPosition.X / map.TileSize), 0, map.Width - 1),
                //    MathHelper.Clamp((int)(worldPosition.Y / map.TileSize), 0, map.Height - 1)
                //);
                Vector2? worldPosition = Camera.ConvertMouseToWorld(ms.Position);
                if (worldPosition is null) { return; }

                Point gridPosition = Camera.ConvertWorldToTile(worldPosition ?? Vector2.Zero);

                if (map.Grid[gridPosition.X, gridPosition.Y] == TileType.Grass)
                {
                    TileRegistry.GetInfo((int)TileType.Sand).PlacingSound.Play();
                    map.Grid[gridPosition.X, gridPosition.Y] = TileType.Sand;
                    WorldGenerator.Autotile(map);
                }
                else if (map.Grid[gridPosition.X, gridPosition.Y] == TileType.Sand)
                {
                    TileRegistry.GetInfo((int)TileType.Grass).PlacingSound.Play();
                    map.Grid[gridPosition.X, gridPosition.Y] = TileType.Grass;
                    WorldGenerator.Autotile(map);
                }
            }

        }

    }

    internal enum PlayerState
    {
        Idle,
        WalkingDown
    }

}
