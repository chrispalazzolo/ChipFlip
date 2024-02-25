using ChipFlip.Global;
using ChipFlip.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChipFlip.Managers
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Pause,
        Completed
    }

    internal class GameManager
    {
        private GameState _gameState;
        private readonly BoardManager _board;
        private readonly Player _player1;
        private readonly Player _player2;
        private readonly Text _mouseText;
        private readonly Text _infoText;
        private readonly Text _highlightText;
        private readonly Text _player1ChipCtText;
        private readonly Text _player2ChipCtText;
        private readonly Sprite _logo;
        private int whosTurn;

        public GameManager()
        {
            _board = new BoardManager();
            _board.AddTexture("tile");//Switch to get tiles from a file.
            _player1 = new Player();
            _logo = new Sprite("Logo");
            _mouseText = new Text("My Text Test", new Vector2(10f, Globals.WindowSize.Y - 70));
            _infoText = new Text("", new Vector2(10f, Globals.WindowSize.Y - 50));
            _highlightText = new Text("", new Vector2(10f, Globals.WindowSize.Y - 90));
            _player1ChipCtText = new Text("", new Vector2(595f, 607f));
            _player1ChipCtText.Size = Size.Large;
            _player2ChipCtText = new Text("", new Vector2(1291, 607f));
            _player2ChipCtText.Size = Size.Large;
            whosTurn = 0;
        }

        public void Load()
        {
            _gameState = GameState.Playing;
            _board.Load();
            Globals.MapSize = new Point(_board.boardSize.Width, _board.boardSize.Height);
            Globals.TileSize = new Point(_board.tileSize.Width, _board.tileSize.Height);
            _logo.Position = new Vector2((Globals.WindowSize.X / 2) - (_logo.Texture.Width/2), (_board.offsetY / 2) - _logo.Texture.Height/2);
        }

        public void UpdatePlaying()
        {
            if (_board.TileClicked != null)
            {
                _board.SetChip(_board.TileClicked, whosTurn);
                _board.ScanBoard(_board.TileClicked.Column, _board.TileClicked.Row, whosTurn);
                whosTurn = whosTurn == 0 ? 1 : 0;
                _board.TileClicked = null;
            }

            _mouseText.Update("Mouse: X: " + Mouse.GetState().X + ", Y: " + Mouse.GetState().Y + " | Clicked At: X: " + InputManager.ClickedPoint.X + ", Y: " + InputManager.ClickedPoint.Y);
            _infoText.Update("Window: Width: " + Globals.WindowSize.X + " Height: " + Globals.WindowSize.Y + " | Map: Width: " + _board.boardSize.Width + " Height: " + _board.boardSize.Height + " | Tile: Width: " + _board.tileSize.Width + " Height: " + _board.tileSize.Height + " | Cols: " + _board.Columns + ", Rows: " + _board.Rows);
            _highlightText.Update("Hightlight Tile Pos: " + _board.highlight.Position + " | Mouse on: Column: " + (Mouse.GetState().X / _board.Columns) + ", Row: " + (Mouse.GetState().Y / _board.Rows) + " | IsLeftMouseClicked = " + InputManager.IsLeftMouseClicked + " | IsMouseClicked: " + InputManager.IsMouseClicked);
            _player1ChipCtText.Update(_board.chip1Ct.ToString());
            _player2ChipCtText.Update(_board.chip2Ct.ToString());
        }

        public void Update()
        {
            InputManager.Update();

            switch (_gameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.Playing:
                    UpdatePlaying();
                    break;
                case GameState.Pause:
                    break;
                case GameState.Completed:
                    break;
            }
            _board.Update();

            
        }

        private void DrawMainMenu()
        {

        }

        private void DrawGameRunning()
        {
            _board.Draw();
            _logo.Draw();
            _mouseText.Draw();
            _infoText.Draw();
            _highlightText.Draw();
            _player1ChipCtText.Draw();
            _player2ChipCtText.Draw();
        }

        private void DrawPause()
        {
            //button to unpause
            //button to return to main menu
            //button to exit
        }

        private void DrawGameCompleted()
        {

        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();
            switch (_gameState)
            {
                case GameState.MainMenu:
                    DrawMainMenu();
                    break;
                case GameState.Playing:
                    DrawGameRunning();
                    break;
                case GameState.Pause:
                    DrawPause();
                    break;
                case GameState.Completed:
                    DrawGameCompleted();
                    break;
            }
                        
            Globals.SpriteBatch.End();
        }
    }
}
