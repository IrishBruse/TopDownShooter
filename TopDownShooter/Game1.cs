using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Game1 : Game
{
    private const string LIVES_PREFIX = "Lives: ";

    private const string KILLS_PREFIX = "Score: ";

    private GraphicsDeviceManager graphics;

    private SpriteBatch spriteBatch;

    private Player Player;

    private Texture2D BallSprite;

    private Texture2D ZombieSprite1;

    private Texture2D ZombieSprite2;

    private Texture2D Pixel;

    private int WINDOW_WIDTH = 1280;

    private int WINDOW_HEIGHT = 720;

    private Wave WaveController;

    private Pause Pause;

    private MainMenu menu;

    private Zombie zombie;

    private List<Zombie> Zombies = new List<Zombie>();

    private Bullet bullet;

    private List<Bullet> bullets = new List<Bullet>();

    private ButtonState PreviousButtonState = ButtonState.Released;

    private int HP = 100;

    private int Score;

    private int MaxZombie = 1000000;

    private int X;

    private int Y;

    private Vector2 KillsPos;

    private int CoolDown = 0;

    private DoublePoints PowerUp1;

    private int HighScore;

    private int Direction = 0;

    private SpriteFont Font;

    private SpriteFont WaveFont;

    private GameTime gametime = new GameTime();

    private Random RandomNumberGenerator = new Random();

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
    }

    protected override void Initialize()
    {
        KillsPos = new Vector2(WINDOW_WIDTH - 80, 0f);
        Zombies = new List<Zombie>(MaxZombie);
        WaveController = new Wave();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(base.GraphicsDevice);
        BallSprite = base.Content.Load<Texture2D>("Ball");
        Pixel = base.Content.Load<Texture2D>("Pixel");
        Player = new Player(base.Content.Load<Texture2D>("Player"), 24, 24, WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2, Font);
        ZombieSprite1 = base.Content.Load<Texture2D>("Zombie");
        Font = base.Content.Load<SpriteFont>("Font");
        WaveFont = base.Content.Load<SpriteFont>("WaveFont");
        menu = new MainMenu(base.Content.Load<Texture2D>("Button"), new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT), Font);
        PowerUp1 = new DoublePoints(base.Content.Load<Texture2D>("DoublePoints"), new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT));
        Pause = new Pause(base.Content.Load<Texture2D>("Pixel"), WINDOW_WIDTH, WINDOW_HEIGHT, Font);
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        MouseState state2 = Mouse.GetState();
        if(state.IsKeyDown(Keys.F1))
        {
            Exit();
        }
        if(HP <= 0)
        {
            HP = 100;
            Player.x = WINDOW_WIDTH / 2;
            Player.y = WINDOW_HEIGHT / 2;
            WaveController.Reset = true;
            for(int num = Zombies.Count - 1; num >= 0; num--)
            {
                Zombies.RemoveAt(num);
            }
            Score = 0;
            menu.Active = true;
        }
        if(menu.State == 2)
        {
            graphics.ToggleFullScreen();
        }
        if(menu.State == 3)
        {
            Exit();
        }
        if(!menu.Active)
        {
            base.IsMouseVisible = true;
            Pause.Update(gametime);
            if(!Pause.Active)
            {
                Player.Update(gameTime);
                if(CoolDown > 0)
                {
                    CoolDown--;
                }
                int coolDown = 60;
                if(state.IsKeyDown(Keys.Up) && CoolDown == 0)
                {
                    Direction = 1;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                if(state.IsKeyDown(Keys.Down) && CoolDown == 0)
                {
                    Direction = 2;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                if(state.IsKeyDown(Keys.Left) && CoolDown == 0)
                {
                    Direction = 3;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                if(state.IsKeyDown(Keys.Right) && CoolDown == 0)
                {
                    Direction = 4;
                    CreateBullet();
                    CoolDown = coolDown;
                }
                int num2 = 2;
                WaveController.Update(gametime, Zombies.Count);
                if(WaveController.AmountToSpawn > 0 && num2 == 2)
                {
                    num2 = 1000;
                    SpawnZombie();
                    WaveController.AmountToSpawn--;
                }
                else if(WaveController.AmountToSpawn == 0)
                {
                    WaveController.AmountToSpawn = -1;
                    WaveController.WaveNumber++;
                }
                for(int num = Zombies.Count - 1; num >= 0; num--)
                {
                    if(Zombies[num].Destroy)
                    {
                        Zombies.RemoveAt(num);
                        Score++;
                        if(PowerUp1.Active)
                        {
                            Score++;
                        }
                    }
                }
                foreach(Zombie zombie in Zombies)
                {
                    zombie.Update(gameTime, Player.CollisionRectangle.Center.X, Player.CollisionRectangle.Center.Y);
                    foreach(Bullet bullet2 in bullets)
                    {
                        if(!zombie.Destroy && zombie.CollisionRectangle.Intersects(bullet2.CollisionRectangle))
                        {
                            if(RandomNumberGenerator.Next(1, 18) == 1)
                            {
                                PowerUp1.Active = true;
                            }
                            zombie.Destroy = true;
                            bullet.Destroy = true;
                        }
                    }
                }
                foreach(Zombie zombie2 in Zombies)
                {
                    if(!zombie2.Destroy && zombie2.CollisionRectangle.Intersects(Player.CollisionRectangle))
                    {
                        zombie2.Destroy = true;
                        HP--;
                    }
                }
                if(state.IsKeyDown(Keys.K))
                {
                    HP -= 3;
                }
                if(state.IsKeyDown(Keys.L))
                {
                    HP = 100;
                }
                Zombies.Capacity += 100;
                PreviousButtonState = state2.LeftButton;
            }
        }
        else
        {
            base.IsMouseVisible = true;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GamePadState state = GamePad.GetState(PlayerIndex.One);
        base.GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        KeyboardState state2 = Keyboard.GetState();
        MouseState state3 = Mouse.GetState();
        Player.Draw(spriteBatch);
        menu.Draw(spriteBatch);
        foreach(Zombie zombie in Zombies)
        {
            zombie.Draw(spriteBatch);
        }
        PowerUp1.Draw(spriteBatch);
        if(!menu.Active || !Pause.Active)
        {
            foreach(Bullet bullet2 in bullets)
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
        switch(RandomNumberGenerator.Next(1, 5))
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
        }
        zombie = new Zombie(ZombieSprite1, ZombieSprite2, X, Y);
        Zombies.Add(zombie);
    }

    public void CreateBullet()
    {
        bullet = new Bullet(BallSprite, new Vector2(Player.x, Player.y), Direction);
        bullets.Add(bullet);
    }

    private string DrawKills(int kills)
    {
        return "Score: " + kills;
    }
}
