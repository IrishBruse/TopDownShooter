using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Player
{
    private Texture2D Sprite;

    private SpriteFont Font;

    private int Width;

    private int Height;

    private int WINDOW_WIDTH;

    private int WINDOW_HEIGHT;

    private Rectangle Rect = new Rectangle(0, 0, 0, 0);

    private GamePadState Controller = GamePad.GetState(PlayerIndex.One);

    public Rectangle CollisionRectangle => Rect;

    public int x
    {
        get
        {
            return Rect.X;
        }
        set
        {
            Rect.X = value;
        }
    }

    public int y
    {
        get
        {
            return Rect.Y;
        }
        set
        {
            Rect.Y = value;
        }
    }

    public Player(Texture2D sprite, int Width, int Height, int WINDOW_WIDTH, int WINDOW_HEIGHT, int X, int Y, SpriteFont Font)
    {
        this.Font = Font;
        Sprite = sprite;
        Rect.X = X;
        Rect.Y = Y;
        this.Width = Width;
        this.Height = Height;
        this.WINDOW_HEIGHT = WINDOW_HEIGHT;
        this.WINDOW_WIDTH = WINDOW_WIDTH;
    }

    public void Update(GameTime time)
    {
        Controller = GamePad.GetState(PlayerIndex.One);
        KeyboardState state = Keyboard.GetState();
        if(state.IsKeyDown(Keys.W))
        {
            Rect.Y -= 4;
        }
        if(state.IsKeyDown(Keys.S))
        {
            Rect.Y += 4;
        }
        if(state.IsKeyDown(Keys.A))
        {
            Rect.X -= 4;
        }
        if(state.IsKeyDown(Keys.D))
        {
            Rect.X += 4;
        }
        if(GamePad.GetState(PlayerIndex.One).IsConnected)
        {
            Rect.X += (int)(Controller.ThumbSticks.Left.X * 4f * 1f);
            Rect.Y += (int)(Controller.ThumbSticks.Left.Y * 4f * -1f);
        }
        if(Rect.X < 0)
        {
            Rect.X = 0;
        }
        if(Rect.X > WINDOW_WIDTH - Width)
        {
            Rect.X = WINDOW_WIDTH - Width;
        }
        if(Rect.Y < 0)
        {
            Rect.Y = 0;
        }
        if(Rect.Y > WINDOW_HEIGHT - Height)
        {
            Rect.Y = WINDOW_HEIGHT - Height;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rect.Width = Width;
        Rect.Height = Height;
        spriteBatch.Draw(Sprite, Rect, Color.White);
    }
}
