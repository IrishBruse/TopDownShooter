using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Pause
{
    private Color Colour = new Color(0, 0, 0, 125);

    private Texture2D Texture;

    private Rectangle Rect;

    private int Width;

    private int Height;

    private SpriteFont Font;

    private bool PreviousPressed;

    private bool active;

    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
        }
    }

    public Pause(Texture2D Texture, int Width, int Height, SpriteFont spritefont)
    {
        this.Texture = Texture;
        Font = spritefont;
        this.Width = Width;
        this.Height = Height;
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Escape) && !PreviousPressed)
        {
            active = !active;
        }
        if (state.IsKeyUp(Keys.Escape))
        {
            PreviousPressed = false;
        }
        else
        {
            PreviousPressed = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        MouseState state = Mouse.GetState();
        if (active)
        {
            spriteBatch.DrawString(Font, "Paused", new Vector2(Width / 2, Height / 2 - 300), Color.Black);
            Rect = new Rectangle(0, 0, Width, Height);
            spriteBatch.Draw(Texture, Rect, Colour);
        }
    }
}
