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
        private readonly Sprite _playerTurn;
        private Texture _player1Turn;
        private Texture _player2Turn;
        private Texture _player1NoMoves;
        private Texture _player2NoMoves;

        private int CurrentPlayer { get; set; } = 0;

        public GameManager()
        {
            _menu = new MenuManager();
            _board = new BoardManager();
            _board.AddTexture("tile");
            _logo = new Sprite("Logo");
            _player1Turn = new Texture("Player1Turn");
            _player2Turn = new Texture("Player2Turn");
            _player1NoMoves = new Texture("Player1NoMoves");
            _player2NoMoves = new Texture("Player2NoMoves");

            _playerTurn = new Sprite(_player1Turn);
        }

        public void Load()
        {
            Globals.GameState = GameState.MainMenu;
            _menu.Load();
            _board.Load();
            _logo.Position = new Vector2((Globals.WindowSize.Width / 2) - (_logo.Texture.Width/2), (_board.BoardOffset.Y / 2) - _logo.Texture.Height / 2);
            _playerTurn.Position = new Vector2((Globals.WindowSize.Width / 2) - (_playerTurn.Texture.Width / 2), (_logo.Position.Y + _logo.Texture.Height) + 5);
        }

        public void UpdatePlaying()
        {
            if (_board.TileClicked != null)
            {
                _board.SetChip(_board.TileClicked, CurrentPlayer);
                _board.ScanBoard(_board.TileClicked.Column, _board.TileClicked.Row, CurrentPlayer);
                
                CurrentPlayer = CurrentPlayer == 0 ? 1 : 0;

                if (!_board.HasMovesLeft(CurrentPlayer))
                {
                    CurrentPlayer = CurrentPlayer == 0 ? 1 : 0;

                    if (!_board.HasMovesLeft(CurrentPlayer))
                    {
                        Globals.GameState = GameState.DelayEnd;
                        _menu.whoWon = _board.GetWinner();
                        _menu.Update();
                    }
                }

                _playerTurn.Texture = CurrentPlayer == 0 ? _player1Turn : _player2Turn;
                _board.CurrentPlayer = CurrentPlayer;
                _board.TileClicked = null;
            }

            _board.Update();
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
                    CurrentPlayer = 0;
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
                _playerTurn.Draw();
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
