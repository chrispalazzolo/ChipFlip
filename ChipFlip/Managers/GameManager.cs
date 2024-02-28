using ChipFlip.Global;
using ChipFlip.Models;
using Microsoft.Xna.Framework;
using System.Threading;

namespace ChipFlip.Managers
{
    internal class GameManager
    {
        private readonly MenuManager _menu;
        private readonly BoardManager _board;
        private readonly Sprite _logo;
        private int whosTurn;

        public GameManager()
        {
            _menu = new MenuManager();
            _board = new BoardManager();
            _board.AddTexture("tile");
            _logo = new Sprite("Logo");
            whosTurn = 0;
        }

        public void Load()
        {
            Globals.GameState = GameState.MainMenu;
            _menu.Load();
            _board.Load();
            Globals.MapSize = new Point(_board.boardSize.Width, _board.boardSize.Height);
            Globals.TileSize = new Point(_board.tileSize.Width, _board.tileSize.Height);
            _logo.Position = new Vector2((Globals.WindowSize.X / 2) - (_logo.Texture.Width/2), (_board.offsetY / 2) - _logo.Texture.Height/2);
        }

        public void UpdatePlaying()
        {
            _board.Update();

            if (_board.TileClicked != null)
            {
                _board.SetChip(_board.TileClicked, whosTurn);
                _board.ScanBoard(_board.TileClicked.Column, _board.TileClicked.Row, whosTurn);
                whosTurn = whosTurn == 0 ? 1 : 0;
                _board.currentPlayer = whosTurn;
                _board.TileClicked = null;
            }

            if (_board.isEnd)
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

        private void DrawGameRunning()
        {
            _logo.Draw();
            _board.Draw();
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
                DrawGameRunning();

                if (Globals.GameState == GameState.Completed || Globals.GameState == GameState.Pause)
                {
                    _menu.Draw();
                }
            }
                                    
            Globals.SpriteBatch.End();
        }
    }
}
