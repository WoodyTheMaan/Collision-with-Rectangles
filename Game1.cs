using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Collision_with_Rectangles
{
    public class Game1 : Game
    {
        KeyboardState keyboardState;
        MouseState mouseState;
        // Textures
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacUpTexture;
        Texture2D pacDownTexture;
        Texture2D currentPacTexture; // Current Pacman texture to draw
        Rectangle pacRect; // This rectangle will track where Pacman is and his size
        Texture2D exitTexture;
        Rectangle exitRect;
        Texture2D barrierTexture;
        Rectangle barrierRect1, barrierRect2;
        Texture2D coinTexture;
        
        List<Rectangle> coins;
        Random rand;
        int pacSpeed;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            pacSpeed = 15;
            pacRect = new Rectangle(10, 10, 50, 50);
            barrierRect1 = new Rectangle(0, 250, 330, 75);
            barrierRect2 = new Rectangle(450, 250, 350, 75);
            exitRect = new Rectangle(700, 380, 100, 100);
            rand = new Random();
            base.Initialize();
            coins = new List<Rectangle>()
            {
                new Rectangle(400, 50, coinTexture.Width, coinTexture.Height),
                new Rectangle(100, 50, coinTexture.Width, coinTexture.Height),
                new Rectangle(50, 400, coinTexture.Width, coinTexture.Height),
                new Rectangle(50, 100, coinTexture.Width, coinTexture.Height),
                
                new Rectangle(12, 16, coinTexture.Width, coinTexture.Height),
                new Rectangle(5, 99, coinTexture.Width, coinTexture.Height),
                new Rectangle(600, 23, coinTexture.Width, coinTexture.Height),
            };
            for (int i = 0; i < rand.Next(90,500); i++)
            {
                coins.Add(new Rectangle(rand.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width), rand.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height), coinTexture.Width, coinTexture.Height));
            }
        }

        protected override void LoadContent()
        {
            //pacman
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pacDownTexture = Content.Load<Texture2D>("pac_down");
            pacUpTexture = Content.Load<Texture2D>("pac_up");
            pacRightTexture = Content.Load<Texture2D>("pac_right");
            pacLeftTexture = Content.Load<Texture2D>("pac_left");
            currentPacTexture = pacRightTexture;
            //Barrier
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
            // Exit
            exitTexture = Content.Load<Texture2D>("hobbit_door");
            // Coin
            coinTexture = Content.Load<Texture2D>("coin");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            this.Window.Title = $"Coins on Screen: {coins.Count}";
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;
            }
            if (pacRect.Intersects(barrierRect1))
            {
                pacRect.X = barrierRect1.Right;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;
            }
            if (pacRect.Intersects(barrierRect1))
            {
                pacRect.Y = barrierRect1.Bottom;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;
            }            
            if (pacRect.Intersects(barrierRect1))
            {
                pacRect.Y = barrierRect1.Top - pacRect.Height;
            }
            if (mouseState.LeftButton == ButtonState.Pressed & exitRect.Contains(mouseState.X, mouseState.Y))
                Exit();
            if (exitRect.Contains(pacRect))
                Exit();


            bool didSpawn = false;
            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                    if (!didSpawn)
                        coins.Add(new Rectangle(rand.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width), rand.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height), coinTexture.Width, coinTexture.Height));
                    didSpawn = true;
                }
            }
            
            if (pacRect.Right > _graphics.PreferredBackBufferWidth)
            {
                pacRect.X = _graphics.PreferredBackBufferWidth - pacRect.Width;
            }
            else if (pacRect.Left < 0)
            {
                pacRect.X = 0;
            }
            if(pacRect.Bottom > _graphics.PreferredBackBufferHeight)
            {
                pacRect.Y = _graphics.PreferredBackBufferHeight - pacRect.Height;
            }
            else if( pacRect.Top < 0)
            {
                pacRect.Y = 0;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(barrierTexture, barrierRect1, Color.White);
            _spriteBatch.Draw(barrierTexture, barrierRect2, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}