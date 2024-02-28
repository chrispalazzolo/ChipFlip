using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChipFlip.Managers
{
    internal class InputManager
    {
        private static Vector2 _clickedPoint;
        public static Vector2 ClickedPoint => _clickedPoint;
        private static bool _isMouseLeftClicked = false;
        private static bool _isMouseClicked = false;
        public static bool IsLeftMouseClicked => _isMouseLeftClicked;
        public static bool IsMouseClicked => _isMouseClicked;
        private static bool _isEscKeyPressed = false;
        public static bool IsEscKeyPressed => _isEscKeyPressed;
        private static bool _isEscKeyClicked = false;
        public static bool IsEscKeyClicked => _isEscKeyClicked;
        public static MouseState mouseState;
        public static KeyboardState keyboardState;

        public static void Update()
        {
            MouseState mouseState = Mouse.GetState();
            _isMouseClicked = false;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                _isMouseLeftClicked = true;
                _isMouseClicked = false;
                _clickedPoint.X = -1;
                _clickedPoint.Y = -1;
            }

            if (mouseState.LeftButton == ButtonState.Released && _isMouseLeftClicked)
            {
                _clickedPoint.X = mouseState.X;
                _clickedPoint.Y = mouseState.Y;

                _isMouseLeftClicked = false;
                _isMouseClicked = true;
            }

            keyboardState = Keyboard.GetState();
            _isEscKeyClicked = false;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            {
                _isEscKeyPressed = true;
            }

            if (keyboardState.IsKeyUp(Keys.Escape) && _isEscKeyPressed)
            {
                _isEscKeyPressed = false;
                _isEscKeyClicked = true;
            }
        }
    }
}
