using ChipFlip.Global;
using ChipFlip.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace ChipFlip.Managers
{
    enum ScanDirection
    {
        Up,
        Down,
        Left,
        Right,
        DiagUpRight,
        DiagUpLeft,
        DiagDownRight,
        DiagDownLeft
    }

    internal class BoardManager
    {
        public Size BoardSize;
        public Size TileSize;
        public List<Texture> textures = new List<Texture>();
        public List<Tile> tileSprites = new List<Tile>();
        public Tile[,] board;
        public Chip[,] chips;
        public int Chip1Ct { get; set; }
        public int Chip2Ct { get; set; }
        private readonly Text _player1ChipCtText;
        private readonly Text _player2ChipCtText;
        private Vector2 _p1ChipCtTextSingleNumPos;
        private Vector2 _p2ChipCtTextSingleNumPos;
        private Vector2 _p1ChipCtTextDoubleNumPos;
        private Vector2 _p2ChipCtTextDoubleNumPos;
        private Texture _chip1Texture;
        private Texture _chip2Texture;
        private Texture _chip1GuideTexture;
        private Texture _chip2GuideTexture;
        private Texture _blockedHighlightTexture;
        public Sprite playerMouseChip;
        private Sprite _boarder;
        private Sprite _player1Panel;
        private Sprite _player2Panel;
        public Grid BoardGrid {  get; set; }
        public Point MouseOnTile { get; set; }
        public Tile TileClicked { get; set; }
        public Offset BoardOffset { get; set; }
        public Winner WinnerIs { get; set; }
        public int CurrentPlayer { get; set; }
        

        public BoardManager()
        {
            BoardGrid = new Grid(8, 8);
            WinnerIs = Winner.None;
            CurrentPlayer = 0;
            float yPos = 606f;
            _p1ChipCtTextSingleNumPos = new Vector2(604f, yPos);
            _p1ChipCtTextDoubleNumPos = new Vector2(596f, yPos);
            _player1ChipCtText = new Text("", _p1ChipCtTextSingleNumPos);
            _player1ChipCtText.Size = TextSizes.Large;
            _p2ChipCtTextSingleNumPos = new Vector2(1300f, yPos);
            _p2ChipCtTextDoubleNumPos = new Vector2(1292f, yPos);
            _player2ChipCtText = new Text("", _p2ChipCtTextSingleNumPos);
            _player2ChipCtText.Size = TextSizes.Large;
        }

        private void LoadTextures()
        {
            _chip1GuideTexture = new Texture("chip1Mouse");
            _chip2GuideTexture = new Texture("chip2Mouse");
            _blockedHighlightTexture = new Texture("tile-highlight-red");
            _chip1Texture = new Texture("chip1");
            _chip2Texture = new Texture("chip2");
            _boarder = new Sprite("BoardBorder");
            _player1Panel = new Sprite("Player1Panel");
            _player2Panel = new Sprite("Player2Panel");
        }

        public void Load()
        {
            LoadTextures();
            
            playerMouseChip = new Sprite(_chip1GuideTexture, new Vector2(-100f, -100f));
            
            if (textures.Count > 0)
            {
                TileSize = new Size(textures[0].Width, textures[0].Height);
            }

            BoardSize = new Size(TileSize.Width * BoardGrid.Columns, TileSize.Height * BoardGrid.Rows);
            BoardOffset = new Offset((Globals.WindowSize.Width / 2) - (BoardSize.Width / 2), (Globals.WindowSize.Height / 2) - (BoardSize.Height / 2));
            _boarder.Position = new Vector2 (BoardOffset.X - ((_boarder.Texture.Width - BoardSize.Width) / 2), BoardOffset.Y - ((_boarder.Texture.Height - BoardSize.Height) / 2));
            _player1Panel.Position = new Vector2(BoardOffset.X - (_player1Panel.Texture.Width + 24), BoardOffset.Y - 24);
            _player2Panel.Position = new Vector2(_boarder.Position.X + _boarder.Texture.Width, BoardOffset.Y - 24);
            CreateBoard();
        }

        public void Init()
        {
            chips[3, 3] = new Chip(_chip1Texture, board[3, 3].Position, 3, 3);
            chips[4, 3] = new Chip(_chip2Texture, board[4, 3].Position, 4, 3);
            chips[3, 4] = new Chip(_chip2Texture, board[3, 4].Position, 3, 4);
            chips[4, 4] = new Chip(_chip1Texture, board[4, 4].Position, 4, 4);

            Chip1Ct = 2;
            Chip2Ct = 2;

            WinnerIs = Winner.None;
            _player1ChipCtText.Position = _p1ChipCtTextSingleNumPos;
            _player2ChipCtText.Position = _p2ChipCtTextSingleNumPos;
            CurrentPlayer = 0;
        }

        public void Reset()
        {
            for (int y = 0; y < BoardGrid.Rows; y++)
            {
                for (int x = 0; x < BoardGrid.Columns; x++)
                {
                    chips[x, y] = null;
                }
            }
            
            Init();
        }

        private void CreateBoard()
        {
            board = new Tile[BoardGrid.Columns, BoardGrid.Rows];
            chips = new Chip[BoardGrid.Columns, BoardGrid.Rows];
            Vector2 position = new Vector2();

            for (int y = 0; y < BoardGrid.Rows; y++)
            {
                for (int x = 0; x < BoardGrid.Columns; x++)
                {
                    position = new Vector2((x * TileSize.Width) + BoardOffset.X, (y * TileSize.Height) + BoardOffset.Y);
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
                TileSize.Width = tileSprites[0].Width;
                TileSize.Height = tileSprites[0].Height;
            }
        }

        public void SetChip(Tile tile, int whichChip)
        {
            Texture texture = whichChip == 0 ? _chip1Texture : _chip2Texture;
            chips[tile.Column, tile.Row] = new Chip(texture, tile.Position, tile.Column, tile.Row);

            if(whichChip == 0)
            {
                Chip1Ct++;
            }
            else
            {
                Chip2Ct++;
            }
        }

        private void ScanColumn(int startRow, int col, ScanDirection direction, Texture flipToTexture)
        {
            List<Chip> chipsToFlip = new List<Chip>();
            int y = direction == ScanDirection.Down ? startRow + 1 : startRow - 1;
            bool isColEnd = false;

            while (true)
            {
                isColEnd = direction == ScanDirection.Down ? y >= BoardGrid.Rows : y < 0;
                
                if (isColEnd || chips[col, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[col, y].TextureName == flipToTexture.TextureName)
                {
                    FlipChips(chipsToFlip, flipToTexture);

                    if (flipToTexture == _chip1Texture)
                    {
                        Chip1Ct += chipsToFlip.Count;
                        Chip2Ct -= chipsToFlip.Count;
                    }
                    else
                    {
                        Chip1Ct -= chipsToFlip.Count;
                        Chip2Ct += chipsToFlip.Count;
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[col, y]);
                    
                    y = direction == ScanDirection.Down ? y + 1  : y - 1;
                }
            }
        }

        private void ScanRow(int row, int startCol, ScanDirection direction, Texture flipToTexture)
        {
            List<Chip> chipsToFlip = new List<Chip>();
            int x = direction == ScanDirection.Right ? startCol + 1 : startCol - 1;
            bool isRowEnd = false;

            while (true)
            {
                isRowEnd = direction == ScanDirection.Right ? x >= BoardGrid.Rows : x < 0;
                
                if (isRowEnd || chips[x, row] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, row].TextureName == flipToTexture.TextureName)
                {
                    FlipChips(chipsToFlip, flipToTexture);
                    
                    if (flipToTexture == _chip1Texture)
                    {
                        Chip1Ct += chipsToFlip.Count;
                        Chip2Ct -= chipsToFlip.Count;
                    }
                    else
                    {
                        Chip1Ct -= chipsToFlip.Count;
                        Chip2Ct += chipsToFlip.Count;
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, row]);
                    x = direction == ScanDirection.Right ? x + 1 : x - 1;
                }
            }
        }

        private void ScanDiagonal(int startRow, int startCol, ScanDirection direction, Texture flipToTexture)
        {
            List<Chip> chipsToFlip = new List<Chip>();
            int x = direction == ScanDirection.DiagUpRight || direction == ScanDirection.DiagDownRight ? startCol + 1 : startCol - 1;
            int y = direction == ScanDirection.DiagDownRight || direction == ScanDirection.DiagDownLeft ? startRow + 1 : startRow - 1;

            bool isColEnd = false;
            bool isRowEnd = false;

            while (true)
            {
                isColEnd = direction == ScanDirection.DiagUpRight || direction == ScanDirection.DiagDownRight ? x >= BoardGrid.Columns : x < 0;
                isRowEnd = direction == ScanDirection.DiagDownRight || direction == ScanDirection.DiagDownLeft ? y >= BoardGrid.Columns : y < 0;
                
                if (isColEnd || isRowEnd || chips[x, y] == null)
                {
                    chipsToFlip.Clear();
                    break;
                }
                else if (chips[x, y].TextureName == flipToTexture.TextureName)
                {
                    FlipChips(chipsToFlip, flipToTexture);

                    if (flipToTexture == _chip1Texture)
                    {
                        Chip1Ct += chipsToFlip.Count();
                        Chip2Ct -= chipsToFlip.Count();
                    }
                    else
                    {
                        Chip1Ct -= chipsToFlip.Count();
                        Chip2Ct += chipsToFlip.Count();
                    }

                    break;
                }
                else
                {
                    chipsToFlip.Add(chips[x, y]);
                    x = direction == ScanDirection.DiagUpRight || direction == ScanDirection.DiagDownRight ? x + 1 : x - 1;
                    y = direction == ScanDirection.DiagDownRight || direction == ScanDirection.DiagDownLeft ? y + 1 : y - 1;
                }
            }
        }

        public void ScanBoard(int col, int row, int whichChip)
        {
            Texture chipTexture = whichChip == 0 ? _chip1Texture : _chip2Texture;
            
            //Scan Column Down
            ScanColumn(row, col, ScanDirection.Down, chipTexture);
            //Scan Column Up
            ScanColumn(row, col, ScanDirection.Up, chipTexture);
            //Scan Row Right
            ScanRow(row, col, ScanDirection.Right, chipTexture);
            //Scan Row Left
            ScanRow(row, col, ScanDirection.Left, chipTexture);
            //Scan Diagonal Up Right
            ScanDiagonal(row, col, ScanDirection.DiagUpRight, chipTexture);
            //Scan Diagonal Down Right
            ScanDiagonal(row, col, ScanDirection.DiagDownRight, chipTexture);
            //Scan Diagonal Down Left
            ScanDiagonal(row, col, ScanDirection.DiagDownLeft, chipTexture);
            //Scan Diagonal Up Left
            ScanDiagonal(row, col, ScanDirection.DiagUpLeft, chipTexture);
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

        private bool CheckMoves(int player, int outerLimit, int innerLimit)
        {
            

            return false;
        }

        public bool HasMovesLeft(int player)
        {
            Texture playerChip;
            Texture opponentChip;
            bool hasStart = false;
            int startIs = -1;
            bool possibleMove = false;

            if (player == 0)
            {
                playerChip = _chip1Texture;
                opponentChip = _chip2Texture;
            }
            else
            {
                playerChip = _chip2Texture;
                opponentChip = _chip1Texture;
            }

            //Look through rows for any moves
            for (int y = 0; y < BoardGrid.Rows; y++)
            {
                hasStart = false;
                startIs = -1;
                possibleMove = false;

                for (int x = 0; x < BoardGrid.Columns; x++)
                {
                    if (chips[x, y] == null)
                    {
                        if (possibleMove && startIs == 1)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 0;
                    }
                    else if (chips[x, y].Texture == opponentChip)
                    {
                        if (hasStart)
                        {
                            possibleMove = true;
                        }
                        else
                        {
                            possibleMove = false;
                            startIs = -1;
                        }
                    }
                    else if (chips[x, y].Texture == playerChip)
                    {
                        if (possibleMove && startIs == 0)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 1;
                    }
                }
            }

            //Look through Columns for any moves
            for (int x = 0; x < BoardGrid.Columns; x++)
            {
                hasStart = false;
                startIs = -1;
                possibleMove = false;

                for (int y = 0; y < BoardGrid.Rows; y++)
                {
                    if (chips[x, y] == null)
                    {
                        if (possibleMove && startIs == 1)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 0;
                    }
                    else if (chips[x, y].Texture == opponentChip)
                    {
                        if (hasStart)
                        {
                            possibleMove = true;
                        }
                        else
                        {
                            possibleMove = false;
                            startIs = -1;
                        }
                    }
                    else if (chips[x, y].Texture == playerChip)
                    {
                        if (possibleMove && startIs == 0)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 1;
                    }
                }
            }

            //Look for any Diagonal moves
            //start [2,0]
            for (int x = 2; x < 6; x++)
            {
                hasStart = false;
                startIs = -1;
                possibleMove = false;
                int diagX = x;
                int y = 0;

                while(diagX >= 0)
                {
                    if (chips[diagX, y] == null)
                    {
                        if (possibleMove && startIs == 1)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 0;
                    }
                    else if (chips[diagX, y].Texture == opponentChip)
                    {
                        if (hasStart)
                        {
                            possibleMove = true;
                        }
                        else
                        {
                            possibleMove = false;
                            startIs = -1;
                        }
                    }
                    else if (chips[diagX, y].Texture == playerChip)
                    {
                        if (possibleMove && startIs == 0)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 1;
                    }

                    y++;
                    diagX--;
                }
            }

            for (int x = 5; x > 1; x--)
            {
                hasStart = false;
                startIs = -1;
                possibleMove = false;
                int diagX = x;
                int y = 0;

                while (diagX < 8)
                {
                    if (chips[diagX, y] == null)
                    {
                        if (possibleMove && startIs == 1)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 0;
                    }
                    else if (chips[diagX, y].Texture == opponentChip)
                    {
                        if (hasStart)
                        {
                            possibleMove = true;
                        }
                        else
                        {
                            possibleMove = false;
                            startIs = -1;
                        }
                    }
                    else if (chips[diagX, y].Texture == playerChip)
                    {
                        if (possibleMove && startIs == 0)
                        {
                            return true;
                        }

                        hasStart = true;
                        startIs = 1;
                    }

                    y++;
                    diagX++;
                }
            }

            return false;
        }

        public Winner GetWinner()
        {
            if (Chip1Ct > Chip2Ct)
            {
                WinnerIs = Winner.Player1;
            }
            else if (Chip2Ct > Chip1Ct)
            {
                WinnerIs = Winner.Player2;
            }
            else if (Chip1Ct == Chip2Ct)
            {
                WinnerIs = Winner.Tie;
            }

            return WinnerIs;
        }

        public void Update()
        {
            MouseState mState = Mouse.GetState();

            if((mState.X > BoardOffset.X && mState.X < (BoardSize.Width + BoardOffset.X)) &&  (mState.Y > BoardOffset.Y && mState.Y < (BoardSize.Height + BoardOffset.Y)))
            {
                int col = (mState.X - BoardOffset.X) / TileSize.Width;
                int row = (mState.Y - BoardOffset.Y) / TileSize.Height;

                Tile hoveredTile = board[col, row];

                if (chips[col,row] != null)
                {
                    playerMouseChip.Texture = _blockedHighlightTexture;
                    playerMouseChip.Position = hoveredTile.Position;
                }
                else
                {
                    playerMouseChip.Texture = CurrentPlayer == 0 ? _chip1GuideTexture : _chip2GuideTexture;
                    playerMouseChip.Position = hoveredTile.Position;
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

            _player1ChipCtText.Position = Chip1Ct > 9 ? _p1ChipCtTextDoubleNumPos : _p1ChipCtTextSingleNumPos;
            _player2ChipCtText.Position = Chip2Ct > 9 ? _p2ChipCtTextDoubleNumPos : _p2ChipCtTextSingleNumPos;
            
            _player1ChipCtText.Update(Chip1Ct.ToString());
            _player2ChipCtText.Update(Chip2Ct.ToString());
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

            _player1ChipCtText.Draw();
            _player2ChipCtText.Draw();

            if(Globals.GameState != GameState.DelayEnd || Globals.GameState != GameState.Completed)
            {
                playerMouseChip.Draw();
            }
        }
    }
}
