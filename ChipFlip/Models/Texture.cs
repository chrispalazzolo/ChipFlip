using Microsoft.Xna.Framework.Graphics;
using ChipFlip.Global;

namespace ChipFlip.Models
{
    internal class Texture
    {
        private Texture2D _texture;
        public string TextureName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Texture(string textureName)
        {
            TextureName = textureName;
            _texture = Globals.Content.Load<Texture2D>(TextureName);
            Width = _texture.Width;
            Height = _texture.Height;
        }

        public Texture2D GetTexture() { return _texture; }
    }
}
