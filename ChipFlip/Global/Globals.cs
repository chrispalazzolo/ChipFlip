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

    public class Size
    {
        public int Width { get; set; }
        public int Height {  get; set; }

        public Size() { }
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
     
    public class Grid
    {
        public int Columns { get; set; }
        public int Rows { get; set; }

        public Grid() { }
        public Grid(int cols, int rows)
        {
            Columns = cols;
            Rows = rows;
        }
    }

    public class Offset
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Offset() { }
        public Offset(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    internal class Globals
    {
        public static float Time { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static SpriteFont Font { get; set; }
        public static SpriteFont FontLarge { get; set; }
        public static Size WindowSize { get; set; }
        public static GameState GameState { get; set; }

        public static void Update(GameTime gt)
        {
            Time = (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
