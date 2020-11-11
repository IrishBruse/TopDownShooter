// TopDownShooter.DoublePoints
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class DoublePoints
{
    private Texture2D Sprite;

    private Rectangle Rect;

    private Vector2 WindowPos;

    private bool active = false;

    private bool Animate = false;

    private int timer = -1;

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

    public DoublePoints(Texture2D Bullet, Vector2 WindowPos)
    {
        Sprite = Bullet;
        Rect = new Rectangle((int)WindowPos.X / 2 - 30, (int)WindowPos.Y - 40, 32, 32);
        this.WindowPos = WindowPos;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if(timer > 0)
        {
            timer--;
        }
        else if(timer == -1)
        {
            timer = 1020;
        }
        if(timer == 200)
        {
            Animate = true;
        }
        if(timer == 180)
        {
            Animate = false;
        }
        if(timer == 160)
        {
            Animate = true;
        }
        if(timer == 140)
        {
            Animate = false;
        }
        if(timer == 120)
        {
            Animate = true;
        }
        if(timer == 100)
        {
            Animate = false;
        }
        if(timer == 80)
        {
            Animate = true;
        }
        if(timer == 60)
        {
            Animate = false;
        }
        if(timer == 40)
        {
            Animate = true;
        }
        if(timer == 20)
        {
            Animate = false;
        }
        if(timer == 0)
        {
            active = false;
        }
        if(!active && timer == 0)
        {
            timer = -1;
        }
        if(active && !Animate)
        {
            spriteBatch.Draw(Sprite, Rect, Color.White);
        }
    }
}
