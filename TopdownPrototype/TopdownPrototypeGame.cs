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

        private RenderTarget2D renderTarget;
        private const int NATIVE_WIDTH = 1920;
        private const int NATIVE_HEIGHT = 1080;
        private Point windowedSize = new Point(1280, 720);
        private Rectangle renderDestination;
        private bool isResizing;
        private bool isBorderless;
        private Rectangle windowBounds;

        private KeyboardState previousKS;
        private Map map;
        private Player player;

        public TopdownPrototypeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = windowedSize.X;
            graphics.PreferredBackBufferHeight = windowedSize.Y;
            graphics.ApplyChanges();
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnResize;
        }

        protected override void Initialize()
        {
            Camera.ScreenWidth = windowedSize.X;
            Camera.ScreenHeight = windowedSize.Y;
            Camera.NativeScreenWidth = NATIVE_WIDTH;
            Camera.NativeScreenHeight = NATIVE_HEIGHT;

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

            player = new Player(Content.Load<Texture2D>("player"),
                Content.Load<Texture2D>("player_anim_move_testpng"));
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

            player.Update(gameTime, map, new Point(renderDestination.X, renderDestination.Y));

            Vector2 center = new Vector2(player.Position.X + player.Width / 2,
                player.Position.Y + player.Height / 2);

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
                windowBounds = Window.ClientBounds;
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
                graphics.PreferredBackBufferWidth = windowBounds.Width;
                graphics.PreferredBackBufferHeight = windowBounds.Height;
                Window.Position = new Point(windowBounds.X, windowBounds.Y);
            }
            Camera.ScreenWidth = GraphicsDevice.Viewport.Width;
            Camera.ScreenHeight = GraphicsDevice.Viewport.Height;
            graphics.ApplyChanges();
            //CenterWindow();

            CalculateRenderDestination();
        }

        private void CenterWindow()
        {
            DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;

            Window.Position = new Point(
                (display.Width - GraphicsDevice.Viewport.Width) / 2,
                (display.Height - GraphicsDevice.Viewport.Height) / 2
                );
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0 && !isResizing)
            {
                isResizing = true;
                CalculateRenderDestination();
                isResizing = false;
                Camera.ScreenWidth = GraphicsDevice.Viewport.Width;
                Camera.ScreenHeight = GraphicsDevice.Viewport.Height;
            }
        }
    }
}
