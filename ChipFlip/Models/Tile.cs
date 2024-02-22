using Microsoft.Xna.Framework;

namespace ChipFlip.Models
{
    internal class Tile : Sprite
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Column {  get; set; }
        public int Row { get; set; }

        public Tile(string textureName) : base(textureName)
        {
            Init();
        }

        public Tile(string spriteName, Vector2 position) : base(spriteName, position)
        {
            Init();
        }

        public Tile(Texture texture, Vector2 position) : base(texture, position)
        {
            Init();
        }

        public Tile(Texture texture, Vector2 position, int col, int row) : base(texture, position)
        {
            Init();
            Column = col;
            Row = row;
        }

        private void Init()
        {
            Width = _texture.Width;
            Height = _texture.Height;
        }
    }
}
