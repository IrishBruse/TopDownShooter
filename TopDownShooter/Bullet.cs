using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class Bullet
{
    private const int speed = 12;

    private Texture2D Sprite;

    private Rectangle Rect;

    private bool destroy = false;

    private int Dir;

    private Vector2 PlayerPos;

    public bool Destroy
    {
        get
        {
            return destroy;
        }
        set
        {
            destroy = value;
        }
    }

    public Rectangle CollisionRectangle
    {
        get
        {
            return Rect;
        }
    }

    public Bullet(Texture2D Bullet, Vector2 PlayerPos, int Dir)
    {
        Sprite = Bullet;
        this.Dir = Dir;
        this.PlayerPos = PlayerPos;
        Rect = new Rectangle((int)PlayerPos.X + 9, (int)PlayerPos.Y + 9, 6, 6);
    }

    public void Draws(SpriteBatch spritebatch)
    {
        if(!destroy)
        {
            if(Dir == 1)
            {
                Rect.Y -= 12;
            }
            if(Dir == 2)
            {
                Rect.Y += 12;
            }
            if(Dir == 3)
            {
                Rect.X -= 12;
            }
            if(Dir == 4)
            {
                Rect.X += 12;
            }
            spritebatch.Draw(Sprite, Rect, Color.Blue);
        }
        else
        {
            Rect.Location = Point.Zero;
        }
    }
}
