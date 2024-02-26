using ChipFlip.Global;
using ChipFlip.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChipFlip
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Globals.WindowSize = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
            _graphics.ApplyChanges();

            Globals.Content = Content;
            _gameManager = new GameManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Font = Content.Load<SpriteFont>("Font");
            Globals.FontLarge = Content.Load<SpriteFont>("FontLarge"); 
            _gameManager.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Globals.GameState == GameState.Exit)
            {
                Exit();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Globals.GameState = GameState.Pause;
            }
            
            Globals.Update(gameTime);
            _gameManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            _gameManager.Draw();

            base.Draw(gameTime);
        }
    }
}
