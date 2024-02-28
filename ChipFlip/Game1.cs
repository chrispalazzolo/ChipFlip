using ChipFlip.Global;
using ChipFlip.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

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
            Globals.WindowSize = new Size(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            _graphics.PreferredBackBufferWidth = Globals.WindowSize.Width;
            _graphics.PreferredBackBufferHeight = Globals.WindowSize.Height;
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

            if (Globals.GameState == GameState.DelayEnd)
            {
                //Poor way to delay
                Thread.Sleep(1000);
                Globals.GameState = GameState.Completed;
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
