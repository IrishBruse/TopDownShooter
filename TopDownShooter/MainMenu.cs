using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class MainMenu
{
    private const int Width = 240;

    private const int Height = 40;

    private Color Hover = new Color(255, 230, 102, 255);

    private Texture2D Texture;

    private Rectangle Rect1;

    private Rectangle Rect2;

    private Rectangle Rect3;

    private SpriteFont Font;

    private ButtonState PreviousClicked = ButtonState.Released;

    private bool PreviousPressed;

    private int Choice;

    private bool active = true;

    public int State => Choice;

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

    public MainMenu(Texture2D Texture, Vector2 WindowRatio, SpriteFont spritefont)
    {
        this.Texture = Texture;
        Font = spritefont;
        Rect1 = new Rectangle((int)WindowRatio.X / 2 - 120, (int)WindowRatio.Y / 2, 240, 40);
        Rect2 = new Rectangle((int)WindowRatio.X / 2 - 120, (int)WindowRatio.Y / 2 + 45, 240, 40);
        Rect3 = new Rectangle((int)WindowRatio.X / 2 - 120, (int)WindowRatio.Y / 2 + 110, 240, 40);
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.Escape) && !PreviousPressed)
        {
            active = !active;
            if (!active)
            {
                Choice = 1;
            }
            else
            {
                Choice = 0;
            }
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
        if (Choice == 2)
        {
            Choice = 0;
        }
        if (Rect1.Contains(state.X, state.Y) && active)
        {
            spriteBatch.Draw(Texture, Rect1, Hover);
            if (state.LeftButton == ButtonState.Pressed && PreviousClicked == ButtonState.Released)
            {
                Choice = 1;
                active = false;
            }
        }
        else if (active)
        {
            spriteBatch.Draw(Texture, Rect1, Color.White);
        }
        if (Rect2.Contains(state.X, state.Y) && active)
        {
            spriteBatch.Draw(Texture, Rect2, Hover);
            if (state.LeftButton == ButtonState.Pressed && PreviousClicked == ButtonState.Released)
            {
                Choice = 2;
            }
        }
        else if (active)
        {
            spriteBatch.Draw(Texture, Rect2, Color.White);
        }
        if (Rect3.Contains(state.X, state.Y) && active)
        {
            spriteBatch.Draw(Texture, Rect3, Hover);
            if (state.LeftButton == ButtonState.Pressed && PreviousClicked == ButtonState.Released)
            {
                Choice = 3;
            }
        }
        else if (active)
        {
            spriteBatch.Draw(Texture, Rect3, Color.White);
        }
        if (active)
        {
            spriteBatch.DrawString(Font, "Play", new Vector2(Rect1.X + 10, Rect1.Center.Y - 11), Color.Black);
            spriteBatch.DrawString(Font, "FullScreen", new Vector2(Rect2.X + 10, Rect2.Center.Y - 11), Color.Black);
            spriteBatch.DrawString(Font, "Exit", new Vector2(Rect3.X + 10, Rect3.Center.Y - 11), Color.Black);
        }
        PreviousClicked = state.LeftButton;
    }
}
