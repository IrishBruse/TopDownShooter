using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager graphics;

    private SpriteBatch spriteBatch;

    private Player Player;

    private Texture2D BallSprite;

    private Texture2D ZombieSprite1;

    private Texture2D Pixel;

    private readonly int WINDOW_WIDTH = 1280;

    private readonly int WINDOW_HEIGHT = 720;

    private Wave WaveController;

    private Pause Pause;

    private MainMenu menu;

    private Zombie zombie;

    private List<Zombie> Zombies = new();

    private Bullet bullet;

    private readonly List<Bullet> bullets = new();

    private ButtonState PreviousButtonState = ButtonState.Released;

    private int HP = 100;

    private int Score;

    private readonly int MaxZombie = 1000000;

    private int X;

    private int Y;

    private Vector2 KillsPos;

    private int CoolDown;

    private DoublePoints PowerUp1;

    private int Direction;

    private SpriteFont Font;

    private SpriteFont WaveFont;

    private readonly GameTime gametime = new();

    private readonly Random RandomNumberGenerator = new();

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        KillsPos = new Vector2(WINDOW_WIDTH - 80, 0f);
        Zombies = new List<Zombie>(MaxZombie);
        WaveController = new Wave();

        graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        BallSprite = Content.Load<Texture2D>("Ball");
        Pixel = Content.Load<Texture2D>("Pixel");
        Player = new Player(Content.Load<Texture2D>("Player"), 24, 24, WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2, Font);
        ZombieSprite1 = Content.Load<Texture2D>("Zombie");
        Font = Content.Load<SpriteFont>("Font");
        WaveFont = Content.Load<SpriteFont>("WaveFont");
        menu = new MainMenu(Content.Load<Texture2D>("Button"), new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT), Font);
        PowerUp1 = new DoublePoints(Content.Load<Texture2D>("DoublePoints"), new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT));
        Pause = new Pause(Content.Load<Texture2D>("Pixel"), WINDOW_WIDTH, WINDOW_HEIGHT, Font);
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        MouseState state2 = Mouse.GetState();
        if (state.IsKeyDown(Keys.F1))
        {
            Exit();
        }
        if (HP <= 0)
        {
            HP = 100;
            Player.x = WINDOW_WIDTH / 2;
            Player.y = WINDOW_HEIGHT / 2;
            WaveController.Reset = true;
            for (int num = Zombies.Count - 1; num >= 0; num--)
            {
                Zombies.RemoveAt(num);
            }
            Score = 0;
            menu.Active = true;
        }
        if (menu.State == 2)
        {
            graphics.ToggleFullScreen();
        }
        if (menu.State == 3)
        {
            Exit();
        }
        if (!menu.Active)
        {
            IsMouseVisible = true;
            Pause.Update(gametime);
            if (!Pause.Active)
            {
                Player.Update(gameTime);
                if (CoolDown > 0)
                {
                    CoolDown--;
                }
                int coolDown = 60;
                if (state.IsKeyDown(Keys.Up) && CoolDown == 0)
                {
                    Direction = 1;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                if (state.IsKeyDown(Keys.Down) && CoolDown == 0)
                {
                    Direction = 2;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                if (state.IsKeyDown(Keys.Left) && CoolDown == 0)
                {
                    Direction = 3;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                if (state.IsKeyDown(Keys.Right) && CoolDown == 0)
                {
                    Direction = 4;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                int num2 = 2;
                WaveController.Update(gametime, Zombies.Count);
                if (WaveController.AmountToSpawn > 0 && num2 == 2)
                {
                    SpawnZombie();
                    WaveController.AmountToSpawn--;
                }
                else if (WaveController.AmountToSpawn == 0)
                {
                    WaveController.AmountToSpawn = -1;
                    WaveController.WaveNumber++;
                }
                for (int num = Zombies.Count - 1; num >= 0; num--)
                {
                    if (Zombies[num].Destroy)
                    {
                        Zombies.RemoveAt(num);
                        Score++;
                        if (PowerUp1.Active)
                        {
                            Score++;
                        }
                    }
                }
                foreach (Zombie zombie in Zombies)
                {
                    zombie.Update(gameTime, Player.CollisionRectangle.Center.X, Player.CollisionRectangle.Center.Y);
                    foreach (Bullet bullet2 in bullets)
                    {
                        if (!zombie.Destroy && zombie.CollisionRectangle.Intersects(bullet2.CollisionRectangle))
                        {
                            if (RandomNumberGenerator.Next(1, 18) == 1)
                            {
                                PowerUp1.Active = true;
                            }
                            zombie.Destroy = true;
                            bullet.Destroy = true;
                        }
                    }
                }
                foreach (Zombie zombie2 in Zombies)
                {
                    if (!zombie2.Destroy && zombie2.CollisionRectangle.Intersects(Player.CollisionRectangle))
                    {
                        zombie2.Destroy = true;
                        HP--;
                    }
                }
                if (state.IsKeyDown(Keys.K))
                {
                    HP -= 3;
                }
                if (state.IsKeyDown(Keys.L))
                {
                    HP = 100;
                }
                Zombies.Capacity += 100;
                PreviousButtonState = state2.LeftButton;
            }
        }
        else
        {
            IsMouseVisible = true;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GamePadState state = GamePad.GetState(PlayerIndex.One);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        KeyboardState state2 = Keyboard.GetState();
        MouseState state3 = Mouse.GetState();
        Player.Draw(spriteBatch);
        menu.Draw(spriteBatch);
        foreach (Zombie zombie in Zombies)
        {
            zombie.Draw(spriteBatch);
        }
        PowerUp1.Draw(spriteBatch);
        if (!menu.Active || !Pause.Active)
        {
            foreach (Bullet bullet2 in bullets)
            {
                bullet2.Draws(spriteBatch);
            }
            spriteBatch.DrawString(Font, DrawKills(Score), new Vector2(WINDOW_WIDTH - 175, 0f), Color.Red);
            spriteBatch.DrawString(Font, string.Concat(HP), new Vector2(30f, 50f), Color.Red);
            Pause.Draw(spriteBatch);
            WaveController.Draw(spriteBatch, WaveFont, new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT));
        }
        spriteBatch.End();
        base.Draw(gameTime);
    }

    public void SpawnZombie()
    {
        X = 0;
        Y = 0;
        switch (RandomNumberGenerator.Next(1, 5))
        {
            case 1:
                X = RandomNumberGenerator.Next(0, WINDOW_WIDTH);
                break;
            case 2:
                X = RandomNumberGenerator.Next(0, WINDOW_WIDTH + 32);
                Y = WINDOW_HEIGHT - 20;
                break;
            case 3:
                X = WINDOW_WIDTH - 20;
                Y = RandomNumberGenerator.Next(0, WINDOW_HEIGHT + 32);
                break;
            case 4:
                Y = RandomNumberGenerator.Next(0, WINDOW_HEIGHT);
                break;
            default:
                break;
        }
        zombie = new Zombie(ZombieSprite1, null, X, Y);
        Zombies.Add(zombie);
    }

    public void CreateBullet()
    {
        bullet = new Bullet(BallSprite, new Vector2(Player.x, Player.y), Direction);
        bullets.Add(bullet);
    }

    private static string DrawKills(int kills)
    {
        return "Score: " + kills;
    }
}
