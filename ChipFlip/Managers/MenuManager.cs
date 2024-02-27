using ChipFlip.Global;
using ChipFlip.Models;
using Microsoft.Xna.Framework;

namespace ChipFlip.Managers
{
    internal class MenuManager
    {
        private Sprite _menuPanel;
        private Vector2 _menuPanelPosition;
        private Vector2 _menuPanelPositionHide;
        private Sprite _winner;
        private Vector2 _winnerPosition;
        private Vector2 _winnerPositionHide;
        private Sprite _player1Winner;
        private Vector2 _player1Position;
        private Vector2 _player1PositionHide;
        private Sprite _player2Winner;
        private Vector2 _player2Position;
        private Vector2 _player2PositionHide;
        private Sprite _tie;
        private Vector2 _tiePosition;
        private Vector2 _tiePositionHide;
        private Sprite _theWinner;

        private Button _start;
        private Vector2 _startPosition;
        private Vector2 _startPositionHide;
        private Button _continue;
        private Vector2 _continuePosition;
        private Vector2 _continuePositionHide;
        private Button _playAgain;
        private Vector2 _playAgainPosition;
        private Vector2 _playAgainPositionHide;
        private Button _exit;
        private Vector2 _exitPosition;
        private Vector2 _exitPositionHide;
        private float _windowMidX;
        private float _windowMidY;

        public int Winner;

        public MenuManager()
        {
            _menuPanel = new Sprite("MenuPanel");
            _winner = new Sprite("Winner");
            _player1Winner = new Sprite("Player1");
            _player2Winner = new Sprite("Player2");
            _tie = new Sprite("Tie");
            _start = new Button("BtnStart", "BtnStart-Hover");
            _continue = new Button("BtnContinue", "BtnContinue-Hover");
            _playAgain = new Button("BtnPlayAgain", "BtnPlayAgain-Hover");
            _exit = new Button("BtnExit", "BtnExit-Hover");
            Winner = 0;
        }

        public void Load()
        {
            _windowMidX = Globals.WindowSize.X / 2;
            _windowMidY = Globals.WindowSize.Y / 2;

            //Panel
            _menuPanelPosition = new Vector2(_windowMidX - (_menuPanel.Texture.Width / 2), _windowMidY - (_menuPanel.Texture.Height / 2));
            _menuPanelPositionHide = new Vector2(0 - _menuPanel.Texture.Width, 0 - _menuPanel.Texture.Height);

            //Text
            _winnerPosition = new Vector2(_windowMidX - (_winner.Texture.Width / 2), _menuPanelPosition.Y + 150);
            _winnerPositionHide = new Vector2(0 - (_winner.Texture.Width / 2), 0 - (_winner.Texture.Height / 2));

            float whoWonYPos = _winnerPosition.Y + _winner.Texture.Height + 10;

            _player1Position = new Vector2(_windowMidX - (_player1Winner.Texture.Width / 2), whoWonYPos);
            _player1PositionHide = new Vector2(0 - _player1Winner.Texture.Width, 0 - _player1Winner.Texture.Height);
            _player2Position = new Vector2(_windowMidX - (_player2Winner.Texture.Width / 2), whoWonYPos);
            _player2PositionHide = new Vector2(0 - _player2Winner.Texture.Width, 0 - _player2Winner.Texture.Height);
            _tiePosition = new Vector2(_windowMidX - (_tie.Texture.Width / 2), whoWonYPos);
            _tiePositionHide = new Vector2(0 - _tie.Texture.Width, 0 - _tie.Texture.Height);

            //Buttons
            _startPosition = new Vector2(_windowMidX - (_start.Texture.Width / 2), _windowMidY - (_start.Texture.Height / 2));
            _startPositionHide = new Vector2(0 - _start.Texture.Width, 0 - _start.Texture.Height);
            _continuePosition = new Vector2(_windowMidX - (_continue.Texture.Width / 2), _windowMidY - (_continue.Texture.Height / 2));
            _continuePositionHide = new Vector2(0 - _continue.Texture.Width, 0 - _continue.Texture.Height);
            _playAgainPosition = new Vector2(_windowMidX - (_playAgain.Texture.Width / 2), _windowMidY - (_playAgain.Texture.Height / 2));
            _playAgainPositionHide = new Vector2(0 - _playAgain.Texture.Width, 0 - _playAgain.Texture.Height);
            _exitPosition = new Vector2(_windowMidX - (_exit.Texture.Width / 2), _windowMidY + (_exit.Texture.Height));
            _exitPositionHide = new Vector2(0 - _exit.Texture.Width, 0 - _exit.Texture.Height);

            HideMenu();
        }

        private void HideMenu()
        {
            _menuPanel.Position = _menuPanelPositionHide;
            _start.Position = _startPositionHide;
            _continue.Position = _continuePositionHide;
            _playAgain.Position = _playAgainPositionHide;
            _exit.Position = _exitPositionHide;
            _winner.Position = _winnerPositionHide;
            _player1Winner.Position = _player1PositionHide;
            _player2Winner.Position = _player2PositionHide;
            _tie.Position = _tiePositionHide;
        }

        public void Update()
        {
            if (Globals.GameState != GameState.Playing)
            {
                _menuPanel.Position = _menuPanelPosition;

                switch (Globals.GameState)
                {
                    case GameState.MainMenu:
                        _start.Position = _startPosition;
                        _exit.Position = _exitPosition;
                        _continue.Position = _continuePositionHide;
                        _playAgain.Position = _playAgainPositionHide;
                        break;
                    case GameState.Pause:
                        _continue.Position = _continuePosition;
                        _exit.Position = _exitPosition;
                        _start.Position = _startPositionHide;
                        _playAgain.Position = _playAgainPositionHide;
                        break;
                    case GameState.Completed:
                        _winner.Position = _winnerPosition;

                        switch (Winner)
                        {
                            case 1:
                                _player1Winner.Position = _player1Position;
                                _player2Winner.Position = _player2PositionHide;
                                _tie.Position = _tiePositionHide;
                                _theWinner = _player1Winner;
                                break;
                            case 2:
                                _player2Winner.Position= _player2Position;
                                _player1Winner.Position = _player1PositionHide;
                                _tie.Position= _tiePositionHide;
                                _theWinner = _player2Winner;
                                break;
                            case 3:
                                _tie.Position= _tiePosition;
                                _player1Winner.Position = _player1PositionHide;
                                _player2Winner.Position = _player2PositionHide;
                                _theWinner = _tie;
                                break;
                        }

                        _playAgain.Position = new Vector2(_playAgainPosition.X, _playAgainPosition.Y + 100);
                        _exit.Position = new Vector2(_exitPosition.X, _exitPosition.Y + 100);
                        _start.Position = _startPositionHide;
                        _continue.Position = _continuePositionHide;
                        break;
                }

                _start.Update();
                _continue.Update();
                _playAgain.Update();
                _exit.Update();

                if (_start.IsPressed || _continue.IsPressed)
                {
                    Globals.GameState = GameState.Playing;
                }

                if (_playAgain.IsPressed)
                {
                    Globals.GameState = GameState.Restart;
                }

                if (_exit.IsPressed)
                {
                    Globals.GameState = GameState.Exit;
                }
            }
            else
            {
                HideMenu();
            }
        }

        public void Draw()
        {
            if(Globals.GameState != GameState.Playing)
            {
                _menuPanel.Draw();

                switch (Globals.GameState)
                {
                    case GameState.MainMenu:
                        _start.Draw();
                        _exit.Draw();
                        break;
                    case GameState.Pause:
                        _continue.Draw();
                        _exit.Draw();
                        break;
                    case GameState.Completed:
                        _winner.Draw();
                        _theWinner.Draw();
                        _playAgain.Draw();
                        _exit.Draw();
                        break;
                }
            }
        }
    }
}
