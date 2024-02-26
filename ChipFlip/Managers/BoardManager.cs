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
        public Chip[,] chips;
        public int chip1Ct = 0;
        public int chip2Ct = 0;
        private Texture _chip1Texture;
        private Texture _chip2Texture;
        private Sprite _boarder;
        private Sprite _player1Panel;
        private Sprite _player2Panel;
        public Tile highlight;
        public Tile redHighlight;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Point MouseOnTile { get; set; }
        public Tile TileClicked { get; set; }
        public int offsetX;
        public int offsetY;
        public bool isEnd;
        public int playerWon;

        public BoardManager()
        {
            Columns = 8;
            Rows = 8;
            isEnd = false;
            playerWon = 0;
        }

        private void LoadTextures()
        {
            highlight = new Tile(new Texture("tile-highlight"), new Vector2(0f, 0f));
            redHighlight = new Tile(new Texture("tile-highlight-red"), new Vector2(0f, 0f));
            _chip1Texture = new Texture("chip1");
            _chip2Texture = new Texture("chip2");
            _boarder = new Sprite("BoardBorder");
            _player1Panel = new Sprite("Player1Panel");
            _player2Panel = new Sprite("Player2Panel");

            if (textures.Count > 0)
            {
                tileSize.Width = textures[0].Width;
                tileSize.Height = textures[0].Height;
            }
        }

        public void Load()
        {
            LoadTextures();
            boardSize.Width = tileSize.Width * Columns;
            boardSize.Height = tileSize.Height * Rows;
            offsetX = (Globals.WindowSize.X / 2) - (boardSize.Width / 2);
            offsetY = (Globals.WindowSize.Y / 2) - (boardSize.Height / 2);
            _boarder.Position = new Vector2 (offsetX - ((_boarder.Texture.Width - boardSize.Width) / 2), offsetY - ((_boarder.Texture.Height - boardSize.Height) / 2));
            _player1Panel.Position = new Vector2(offsetX - (_player1Panel.Texture.Width + 24), offsetY - 24);
            _player2Panel.Position = new Vector2(_boarder.Position.X + _boarder.Texture.Width, offsetY - 24);
            CreateBoard();
        }

        public void Init()
        {
            chips[3, 3] = new Chip(_chip1Texture, board[3, 3].Position, 3, 3);
            chips[4, 3] = new Chip(_chip2Texture, board[4, 3].Position, 4, 3);
            chips[3, 4] = new Chip(_chip2Texture, board[3, 4].Position, 3, 4);
            chips[4, 4] = new Chip(_chip1Texture, board[4, 4].Position, 4, 4);

            chip1Ct = 2;
            chip2Ct = 2;

            isEnd = false;
            playerWon = 0;
        }

        public void Reset()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    chips[x, y] = null;
                }
            }
            
            Init();
        }

        private void CreateBoard()
        {
            board = new Tile[Columns, Rows];
            chips = new Chip[Columns, Rows];
            Vector2 position = new Vector2();

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    position = new Vector2((x * tileSize.Width) + offsetX, (y * tileSize.Height) + offsetY);
                    board[x, y] = new Tile(textures[0], position, x, y);
                }
            }

            Init();
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
            chips[tile.Column, tile.Row] = new Chip(texture, tile.Position, tile.Column, tile.Row);

            if(whichChip == 0)
            {
                chip1Ct++;
            }
            else
            {
                chip2Ct++;
            }

            if((chip1Ct + chip2Ct) >= 64)
            {
                isEnd = true;

                if(chip1Ct > chip2Ct)
                {
                    playerWon = 1;
                }
                else if(chip2Ct > chip1Ct)
                {
                    playerWon = 2;
                }
                else
                {
                    playerWon = 3;
                }

                Globals.GameState = GameState.Completed;
            }
        }

        public void ScanBoard(int col, int row, int whichChip)
        {
            Texture chipTexture = whichChip == 0 ? _chip1Texture : _chip2Texture;
            List<Chip> chipsToFlip = new List<Chip>();

            //Scan Down
            int y = row + 1;
            while (true)
            {
                if (y >= Rows || chips[col, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if(chips[col, y].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if(whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[col, y]);
                    y++;
                }
            }
            
            chipsToFlip.Clear();

            //Scan Up
            y = row - 1;
            while (true)
            {
                if (y < 0 || chips[col, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if(chips[col, y].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if (whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[col, y]);
                    y--;
                }
            }
            
            chipsToFlip.Clear();
            
            //Scan Right
            int x = col + 1;
            while (true)
            {
                if (x >= Columns || chips[x, row] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, row].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if (whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, row]);
                    x++;
                }
            }
            
            chipsToFlip.Clear();

            //Scan Left
            x = col - 1;
            while (true)
            {
                if (x < 0 || chips[x, row] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, row].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if (whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, row]);
                    x--;
                }
            }
            
            chipsToFlip.Clear();

            //Scan Diagonal Up Right
            x = col + 1;
            y = row - 1;

            while (true)
            {
                if (x >= Columns || y < 0 || chips[x, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, y].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if (whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, y]);
                    x++;
                    y--;
                }
            }

            chipsToFlip.Clear();

            //Scan Diagonal Down Right
            x = col + 1;
            y = row + 1;

            while (true)
            {
                if (x >= Columns || y >= Rows || chips[x, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, y].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if (whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, y]);
                    x++;
                    y++;
                }
            }

            chipsToFlip.Clear();

            //Scan Diagonal Down Left
            x = col - 1;
            y = row + 1;

            while (true)
            {
                if (x < 0 || y >= Rows || chips[x, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, y].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if (whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, y]);
                    x--;
                    y++;
                }
            }

            chipsToFlip.Clear();

            //Scan Diagonal Up Left
            x = col - 1;
            y = row - 1;

            while (true)
            {
                if (x < 0 || y < 0 || chips[x, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, y].TextureName == chipTexture.TextureName)
                {
                    FlipChips(chipsToFlip, chipTexture);

                    if (whichChip == 0)
                    {
                        chip1Ct += chipsToFlip.Count();
                        chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        chip1Ct -= chipsToFlip.Count();
                        chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, y]);
                    x--;
                    y--;
                }
            }

            chipsToFlip.Clear();
        }

        private void FlipChips(List<Chip> chipsToFlip, Texture changeTo)
        {
            if (chipsToFlip.Count > 0)
            {
                foreach (Chip chip in chipsToFlip)
                {
                    chips[chip.Column, chip.Row].Texture = changeTo;
                }
            }
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
            _boarder.Draw();
            _player1Panel.Draw();
            _player2Panel.Draw();

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
