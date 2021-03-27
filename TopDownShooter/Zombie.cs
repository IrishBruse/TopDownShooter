// TopDownShooter.zombie
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Zombie
{
    private const int speed = 2;

    private Texture2D Sprite1;

    private Texture2D Sprite2;

    private Rectangle Rect;

    private bool Destroys;

    private int Type;

    public Rectangle CollisionRectangle
    {
        get
        {
            return Rect;
        }
    }

    public bool Destroy
    {
        get
        {
            return Destroys;
        }
        set
        {
            Destroys = value;
        }
    }

    public Zombie(Texture2D Sprite1, Texture2D Sprite2, int X, int Y)
    {
        this.Sprite1 = Sprite1;
        this.Sprite2 = Sprite2;
        Rect = new Rectangle(0, 0, 22, 22);
        Rect.X = X;
        Rect.Y = Y;
    }

    public void Update(GameTime gameTime, int PlayerX, int PlayerY)
    {
        if (Destroys)
        {
            return;
        }
        Vector2 vector = new Vector2(PlayerX, PlayerY);
        if ((Type == 0 && Rect.X != PlayerX) || Rect.Y != PlayerY)
        {
            if ((float)Rect.X > vector.X)
            {
                Rect.X -= 2;
            }
            else if ((float)Rect.X < vector.X)
            {
                Rect.X += 2;
            }
            if ((float)Rect.Y > vector.Y)
            {
                Rect.Y -= 2;
            }
            else if ((float)Rect.Y < vector.Y)
            {
                Rect.Y += 2;
            }
        }
    }

    public void Draw(SpriteBatch spritebatch)
    {
        if (!Destroys)
        {
            spritebatch.Draw(Sprite1, Rect, Color.White);
        }
    }

    public void ZombieType(int Type)
    {
        this.Type = Type;
    }
}
