﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChipFlip.Global;

namespace ChipFlip.Models
{
    internal class Sprite
    {
        public string SpriteName { get; set; }
        protected readonly Texture2D _texture;
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects Effect { get; set; }
        public bool isHovered;
        public Sprite(string spriteName) : this(spriteName, new Vector2(0f, 0f)) { }
        public Sprite(string spriteName, Vector2 position)
        {
            SpriteName = spriteName;
            Effect = SpriteEffects.None;
            _texture = Globals.Content.Load<Texture2D>(SpriteName);
            Position = position;
            Origin = Origin = new Vector2(0f, 0f);//new Vector2(_texture.Width / 2, _texture.Height / 2);
            isHovered = false;
        }
        public Sprite(Texture texture, Vector2 position) : this(texture, position, SpriteEffects.None) { }
        public Sprite(Texture texture, Vector2 position, SpriteEffects effect)
        {
            _texture = texture.GetTexture();
            Position = position;
            Effect = effect;
            Origin = new Vector2(0f, 0f);//new Vector2(texture.Width / 2, texture.Height / 2);
            isHovered = false;
        }

        public void Update()
        {
            MouseState mState = Mouse.GetState();
            Point mPoint = new Point(mState.X, mState.Y);
            Rectangle rectangle = new Rectangle(mPoint.X, mPoint.Y, _texture.Width, _texture.Height);

            if (rectangle.Contains(mPoint))
            {
                isHovered = true;
            }
            else
            {
                isHovered = false;
            }
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_texture, Position, null, Color.White, 0f, Origin, 1f, Effect, 0f);
        }
    }
}