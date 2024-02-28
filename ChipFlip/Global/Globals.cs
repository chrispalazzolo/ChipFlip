using ChipFlip.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ChipFlip.Global
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Pause,
        DelayEnd,
        Completed,
        Restart,
        Exit
    }

    internal class Globals
    {
        public static float Time { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static SpriteFont Font { get; set; }
        public static SpriteFont FontLarge { get; set; }
        public static Point WindowSize { get; set; }
        public static Point MapSize { get; set; }
        public static Point TileSize { get; set; }
        public static GameState GameState { get; set; }

        public static void Update(GameTime gt)
        {
            Time = (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
