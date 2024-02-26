using ChipFlip.Managers;

namespace ChipFlip.Models
{
    internal class Button : Sprite
    {
        private Texture _textureHover;
        private Texture _textureBase;
        public bool IsPressed { get; private set; }

        public Button(string texture, string textureHover) : base(texture)
        {
            _textureBase = base.Texture;
            _textureHover = new Texture(textureHover);
            IsPressed = false;
        }

        public new void Update()
        {
            base.Update();

            if (isHovered)
            {
                base.Texture = _textureHover;
                
                if (InputManager.IsMouseClicked)
                {
                    IsPressed = true;
                }
                else
                {
                    IsPressed = false;
                }
            }
            else
            {
                IsPressed = false;
                base.Texture = _textureBase;
            }
        }
    }
}
