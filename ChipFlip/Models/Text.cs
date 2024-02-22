using ChipFlip.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChipFlip.Models
{
    internal class Text
    {
        public string _text;
        public Vector2 Position { get; protected set; }
        public Vector2 Origin { get; protected set; }

        public Text(string text, Vector2 position)
        {
            _text = text;
            Position = position;
            Origin = new Vector2(Globals.WindowSize.X / 2, Globals.WindowSize.Y / 2);
        }

        public void Update(string text)
        {
            _text = text;
        }
        public void Draw()
        {
            Vector2 middlePoint = Globals.Font.MeasureString(_text);

            Globals.SpriteBatch.DrawString(Globals.Font, _text, Position, Color.Black, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0.5f);
        }
    }
}
