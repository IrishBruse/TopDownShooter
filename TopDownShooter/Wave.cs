// TopDownShooter.Wave
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Wave
{
    private const int Wave1 = 2;

    private const int TimerConst = 120;

    private bool Restart;

    private int Zombies;

    private int wave;

    private int Timer;

    private int ZombiesToSpawn;

    public int AmountToSpawn
    {
        get
        {
            return Zombies;
        }
        set
        {
            Zombies = value;
        }
    }

    public int WaveNumber
    {
        get
        {
            return wave;
        }
        set
        {
            wave = value;
        }
    }

    public bool Reset
    {
        get
        {
            return Restart;
        }
        set
        {
            Restart = value;
        }
    }

    public Wave()
    {
        ZombiesToSpawn = 10;
        Zombies = 10;
    }

    public void Update(GameTime gameTime, int ZombiesAmount)
    {
        if (Restart)
        {
            Zombies = 0;
            ZombiesToSpawn = 2;
            wave = 0;
            Restart = false;
        }
        if (Zombies == -1 && Timer == 0)
        {
            ZombiesToSpawn += 4;
            Zombies = ZombiesToSpawn;
        }
        if (Timer > 0 && ZombiesAmount == 0)
        {
            Timer--;
        }
        else
        {
            Timer = 1;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont WaveFont, Vector2 WindowPos)
    {
        spriteBatch.DrawString(WaveFont, "Wave", new Vector2(WindowPos.X / 2f - 15f, 2f), Color.Red);
        if (wave > 0)
        {
            spriteBatch.DrawString(WaveFont, string.Concat(wave), new Vector2(WindowPos.X / 2f + 15f, 24f), Color.Red);
        }
    }
}
