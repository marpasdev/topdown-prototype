using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TopdownPrototype
{
    public class TopdownPrototypeGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // resolution independency and borderless window
        private RenderTarget2D renderTarget;
        // maybe too low - there are some visible off pixels
        //private const int NATIVE_WIDTH = 1280;
        //private const int NATIVE_HEIGHT = 720;
        // it seems modifying the resolution also modifies the max and min zoom level
        private const int NATIVE_WIDTH = 1920;
        private const int NATIVE_HEIGHT = 1080;
        private Rectangle renderDestination;
        private bool isResizing;
        private bool isBorderless;

        private KeyboardState previousKS;

        private Map map;

        private Player player;

        public TopdownPrototypeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = NATIVE_WIDTH;
            graphics.PreferredBackBufferHeight = NATIVE_HEIGHT;
            graphics.ApplyChanges();
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        protected override void Initialize()
        {
            Camera.ScreenWidth = graphics.PreferredBackBufferWidth;
            Camera.ScreenHeight = graphics.PreferredBackBufferHeight;

            TileRegistry.Content = Content;
            WorldObjectRegistry.Content = Content;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            renderTarget = new RenderTarget2D(GraphicsDevice, NATIVE_WIDTH, NATIVE_HEIGHT);
            CalculateRenderDestination();

            TileRegistry.Load();
            TileRegistry.LoadSlopes();
            WorldObjectRegistry.Load();

            map = new Map(100, 100, 1);

            player = new Player() {
                Texture = Content.Load<Texture2D>("player")
            };
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape))
                Exit();

            if (ks.IsKeyDown(Keys.F11) && !previousKS.IsKeyDown(Keys.F11))
            {
                ToggleBorderless();
            }

            previousKS = ks;

            player.Update(gameTime, map);

            Vector2 center = new Vector2(player.Position.X + player.Texture.Width / 2, 
                player.Position.Y + player.Texture.Height / 2);
            Camera.Update(gameTime, center, map);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, 
                BlendState.AlphaBlend, SamplerState.PointClamp,
                null, null, null, Camera.Transform);

            map.Draw(spriteBatch, player.Position, player);

            //player.Draw(spriteBatch);

            //map.DrawWorldObjects(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            spriteBatch.Draw(renderTarget, renderDestination, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CalculateRenderDestination()
        {
            Point size = GraphicsDevice.Viewport.Bounds.Size;

            float scaleX = (float)size.X / renderTarget.Width;
            float scaleY = (float)size.Y / renderTarget.Height;
            float scale = Math.Min(scaleX, scaleY);

            renderDestination.Width = (int)(renderTarget.Width * scale);
            renderDestination.Height = (int)(renderTarget.Height * scale);

            // pillar-boxing/letter-boxing
            renderDestination.X = (size.X - renderDestination.Width) / 2;
            renderDestination.Y = (size.Y - renderDestination.Height) / 2;
        }

        private void ToggleBorderless()
        {
            isBorderless = !isBorderless;

            DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            if (isBorderless)
            {
                Window.IsBorderless = true;
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferWidth = display.Width;
                graphics.PreferredBackBufferHeight = display.Height;
                Window.Position = Point.Zero;
            }
            else
            {
                Window.IsBorderless = false;
                graphics.IsFullScreen = false;
                // TODO: fix
                // currently out of order, needs to be fixed - native resolution is at fault
                // too big for a window when back in fullscreen/window mode
                graphics.PreferredBackBufferWidth = NATIVE_WIDTH;
                graphics.PreferredBackBufferHeight = NATIVE_HEIGHT;
                Window.Position = new Point((display.Width - NATIVE_WIDTH) / 2,
                    (display.Height - NATIVE_HEIGHT) / 2);
            }
            graphics.ApplyChanges();

            CalculateRenderDestination();
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0 && !isResizing)
            {
                isResizing = true;
                CalculateRenderDestination();
                isResizing = false;
            }
        }

    }
}
