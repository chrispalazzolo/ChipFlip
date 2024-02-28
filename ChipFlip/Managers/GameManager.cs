using ChipFlip.Global;
using ChipFlip.Models;
using Microsoft.Xna.Framework;

namespace ChipFlip.Managers
{
    internal class GameManager
    {
        private readonly MenuManager _menu;
        private readonly BoardManager _board;
        private readonly Sprite _logo;
        private int PlayerTurn { get; set; } = 0;

        public GameManager()
        {
            _menu = new MenuManager();
            _board = new BoardManager();
            _board.AddTexture("tile");
            _logo = new Sprite("Logo");
        }

        public void Load()
        {
            Globals.GameState = GameState.MainMenu;
            _menu.Load();
            _board.Load();
            _logo.Position = new Vector2((Globals.WindowSize.Width / 2) - (_logo.Texture.Width/2), (_board.BoardOffset.Y / 2) - _logo.Texture.Height/2);
        }

        public void UpdatePlaying()
        {
            _board.Update();

            if (_board.TileClicked != null)
            {
                _board.SetChip(_board.TileClicked, PlayerTurn);
                _board.ScanBoard(_board.TileClicked.Column, _board.TileClicked.Row, PlayerTurn);
                PlayerTurn = PlayerTurn == 0 ? 1 : 0;
                _board.CurrentPlayer = PlayerTurn;
                _board.TileClicked = null;
            }

            if (_board.hasWinner)
            {
                Globals.GameState = GameState.DelayEnd;
                _menu.Winner = _board.playerWon;
                _menu.Update();
            }
        }

        public void Update()
        {
            InputManager.Update();

            if(Globals.GameState != GameState.Playing)
            {
                _menu.Update();
                Globals.GameState = _menu.GetMenuAction();

                if(Globals.GameState == GameState.Restart)
                {
                    _board.Reset();
                    Globals.GameState = GameState.Playing;
                }

                if(Globals.GameState == GameState.Pause && InputManager.IsEscKeyClicked)
                {
                    Globals.GameState = GameState.Playing;
                }
            }
            else
            {
                UpdatePlaying();

                if (InputManager.IsEscKeyClicked)
                {
                    Globals.GameState = GameState.Pause;
                }   
            }
        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();

            if(Globals.GameState == GameState.MainMenu)
            {
                _menu.Draw();
            }
            else
            {
                _logo.Draw();
                _board.Draw();

                //If the game is finished or paused, we still want the board to be drawn
                if (Globals.GameState == GameState.Completed || Globals.GameState == GameState.Pause)
                {
                    _menu.Draw();
                }
            }
                                    
            Globals.SpriteBatch.End();
        }
    }
}
