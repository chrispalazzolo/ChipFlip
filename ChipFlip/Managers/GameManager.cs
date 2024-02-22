﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ChipFlip.Models;
using ChipFlip.Global;
using System.Transactions;

namespace ChipFlip.Managers
{
    internal class GameManager
    {
        private readonly BoardManager _board;
        private readonly Player _player1;
        private readonly Player _player2;
        private readonly Text _mouseText;
        private readonly Text _infoText;
        private readonly Text _highlightText;
        private int whosTurn;

        public GameManager()
        {
            _board = new BoardManager();
            _board.AddTexture("tile");//Switch to get tiles from a file.
            _player1 = new Player();
            _mouseText = new Text("My Text Test", new Vector2(10f, Globals.WindowSize.Y - 70));
            _infoText = new Text("", new Vector2(10f, Globals.WindowSize.Y - 50));
            _highlightText = new Text("", new Vector2(10f, Globals.WindowSize.Y - 90));
            whosTurn = 0;
        }

        public void Load()
        {
            _board.Load();
            Globals.MapSize = new Point(_board.boardSize.Width, _board.boardSize.Height);
            Globals.TileSize = new Point(_board.tileSize.Width, _board.tileSize.Height);
        }

        public void Update()
        {
            InputManager.Update();
            _board.Update();

            if (_board.TileClicked != null)
            {
                _board.SetChip(_board.TileClicked, whosTurn);
                whosTurn = whosTurn == 0 ? 1 : 0;
                _board.TileClicked = null;
            }
            
            _mouseText.Update("Mouse: X: " + Mouse.GetState().X + ", Y: " + Mouse.GetState().Y + " | Clicked At: X: " + InputManager.ClickedPoint.X + ", Y: " + InputManager.ClickedPoint.Y);
            _infoText.Update("Window: Width: " + Globals.WindowSize.X + " Height: " + Globals.WindowSize.Y + " | Map: Width: " + _board.boardSize.Width + " Height: " + _board.boardSize.Height + " | Tile: Width: " + _board.tileSize.Width + " Height: " + _board.tileSize.Height + " | Cols: " + _board.Columns + ", Rows: " + _board.Rows);
            _highlightText.Update("Hightlight Tile Pos: " + _board.highlight.Position + " | Mouse on: Column: " + (Mouse.GetState().X / _board.Columns) + ", Row: " + (Mouse.GetState().Y / _board.Rows) + " | IsLeftMouseClicked = " + InputManager.IsLeftMouseClicked + " | IsMouseClicked: " + InputManager.IsMouseClicked);
        }

        public void Draw()
        {
            Globals.SpriteBatch.Begin();
            _board.Draw();
            _mouseText.Draw();
            _infoText.Draw();
            _highlightText.Draw();
            Globals.SpriteBatch.End();
        }
    }
}