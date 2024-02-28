using ChipFlip.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChipFlip.Models
{
    public enum TextSizes
    {
        Small,
        Large
    }

    internal class Text
    {
        public string _text;
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; protected set; }
        public TextSizes Size { get; set; }

        public Text(string text, Vector2 position)
        {
            _text = text;
            Position = position;
            Origin = new Vector2(Globals.WindowSize.Width / 2, Globals.WindowSize.Height / 2);
            Size = TextSizes.Small;
        }

        public void Update(string text)
        {
            _text = text;
        }
        public void Draw()
        {
            Vector2 middlePoint = Globals.Font.MeasureString(_text);
            SpriteFont Font = Size == TextSizes.Large ? Globals.FontLarge : Globals.Font;
            Globals.SpriteBatch.DrawString(Font, _text, Position, Color.Black, 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0.5f);
        }
    }
}
