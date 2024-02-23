using Microsoft.Xna.Framework;

namespace ChipFlip.Models
{
    internal class Chip : Sprite
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public Chip(string textureName) : base(textureName)
        {
            Init();
        }

        public Chip(string spriteName, Vector2 position) : base(spriteName, position)
        {
            Init();
        }

        public Chip(Texture texture, Vector2 position) : base(texture, position)
        {
            Init();
        }

        public Chip(Texture texture, Vector2 position, int col, int row) : base(texture, position)
        {
            Column = col;
            Row = row;
        }

        private void Init()
        {
            Column = 0;
            Row = 0;
        }
    }
}
