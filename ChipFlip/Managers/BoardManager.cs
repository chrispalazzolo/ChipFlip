using ChipFlip.Global;
using ChipFlip.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace ChipFlip.Managers
{
    internal class BoardManager
    {
        public struct MapSize
        {
            public int Width;
            public int Height;
        }

        public struct TileSize
        {
            public int Width;
            public int Height;
        }

        public MapSize boardSize = new MapSize();
        public TileSize tileSize = new TileSize();
        public List<Texture> textures = new List<Texture>();
        public List<Tile> tileSprites = new List<Tile>();
        public Tile[,] board;
        public Sprite[,] chips;
        private Texture _chip1Texture;
        private Texture _chip2Texture;
        public Tile highlight;
        public Tile redHighlight;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Point MouseOnTile { get; set; }
        public Tile TileClicked { get; set; }
        public int offsetX;
        public int offsetY;

        public BoardManager()
        {
            Columns = 8;
            Rows = 8;
        }
        
        public void Load()
        {
            LoadTextures();
            boardSize.Width = tileSize.Width * Columns;
            boardSize.Height = tileSize.Height * Rows;
            offsetX = (Globals.WindowSize.X / 2) - (boardSize.Width / 2);
            offsetY = (Globals.WindowSize.Y / 2) - (boardSize.Height / 2);
            CreateBoard();
        }

        private void LoadTextures()
        {
            highlight = new Tile(new Texture("tile-highlight"), new Vector2(0f, 0f));
            redHighlight = new Tile(new Texture("tile-highlight-red"), new Vector2(0f, 0f));
            _chip1Texture = new Texture("chip1");
            _chip2Texture = new Texture("chip2");

            if(textures.Count > 0)
            {
                tileSize.Width = textures[0].Width;
                tileSize.Height = textures[0].Height;
            }
        }
        private void CreateBoard()
        {
            board = new Tile[Columns, Rows];
            chips = new Sprite[Columns, Rows];
            Vector2 position = new Vector2();

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    position = new Vector2((x * tileSize.Width) + offsetX, (y * tileSize.Height) + offsetY);
                    board[x, y] = new Tile(textures[0], position, x, y);
                }
            }

            chips[3, 3] = new Sprite(_chip1Texture, board[3, 3].Position);
            chips[4, 3] = new Sprite(_chip2Texture, board[4, 3].Position);
            chips[3, 4] = new Sprite(_chip2Texture, board[3, 4].Position);
            chips[4, 4] = new Sprite(_chip1Texture, board[4, 4].Position);
        }

        public void AddTexture(string textureName)
        {
            if(textureName != string.Empty)
            {
                textures.Add(new Texture(textureName));
            }
        }

        public void SetTileSize()
        {
            if (!tileSprites.Any())
            {
                tileSize.Width = tileSprites[0].Width;
                tileSize.Height = tileSprites[0].Height;
            }
        }

        public void SetChip(Tile tile, int whichChip)
        {
            Texture texture = whichChip == 0 ? _chip1Texture : _chip2Texture;
            chips[tile.Column, tile.Row] = new Sprite(texture, tile.Position);
            FlipChips(tile.Column, tile.Row);
        }

        public void FlipChips(int col, int row)
        {

        }

        public void Update()
        {
            MouseState mState = Mouse.GetState();

            if((mState.X > offsetX && mState.X < (boardSize.Width + offsetX)) &&  (mState.Y > offsetY && mState.Y < (boardSize.Height + offsetY)))
            {
                int col = (mState.X - offsetX) / tileSize.Width;
                int row = (mState.Y - offsetY) / tileSize.Height;

                Tile hoveredTile = board[col, row];

                highlight.Position = hoveredTile.Position;

                if (chips[col,row] != null)
                {
                    redHighlight.Position = hoveredTile.Position;
                }
                else
                {
                    redHighlight.Position = new Vector2(0 - tileSize.Width, 0f);
                }

                MouseOnTile = new Point(col, row);

                if (InputManager.IsMouseClicked)
                {
                    if (chips[col, row] == null)
                    {
                        TileClicked = hoveredTile;
                    }
                    else
                    {
                        TileClicked = null;
                    }
                }
            }
        }

        public void Draw()
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    board[x, y].Draw();

                    if (chips[x,y] != null)
                    {
                        chips[x,y].Draw();
                    }
                }
            }

            highlight.Draw();
            redHighlight.Draw();
        }
    }
}
