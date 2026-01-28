using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopdownPrototype
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;

        private Map map;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player();

            Camera.ScreenWidth = _graphics.PreferredBackBufferWidth;
            Camera.ScreenHeight = _graphics.PreferredBackBufferHeight;

            TileRegistry.Content = Content;
            WorldObjectRegistry.Content = Content;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TileRegistry.Load();
            WorldObjectRegistry.Load();

            map = new Map(100, 100);

            player.Texture = Content.Load<Texture2D>("player");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            Vector2 center = new Vector2(player.Position.X + player.Texture.Width / 2, 
                player.Position.Y + player.Texture.Height / 2);
            Camera.Update(gameTime, center);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, 
                BlendState.AlphaBlend, SamplerState.PointClamp,
                null, null, null, Camera.Transform);

            map.Draw(_spriteBatch, player.Position);

            player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
