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
        public static MouseState mouseState;

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
        }
    }
}
