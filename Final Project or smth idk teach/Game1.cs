using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
enum Screen
{
    Title,
    Play,
    Easy,
    Normal,
    Hard
}
namespace Final_Project_or_smth_idk_teach
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Screen screen;
        MouseState mouseState, prevMouseState;
        private Song Menu;
        Texture2D temp, bg, titleScreen, map, playButton, easyButton, normalButton, hardButton, scout;
        Rectangle playRec, easyRec, normalRec, hardRec, window;

        public Game1()  
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = Screen.Title;
            // Rectangles Below
            window = new Rectangle(0, 0, 1000, 800);
            playRec = new Rectangle(400, 400, 200, 100);
            easyRec = new Rectangle(100, 200, 200, 200);
            normalRec = new Rectangle(400, 200, 200, 200);
            hardRec = new Rectangle(700, 200, 200, 200);
            _graphics.PreferredBackBufferWidth = window.Width;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = window.Height;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Textures Below
            playButton = Content.Load<Texture2D>("PlayButton");
            easyButton = Content.Load<Texture2D>("easyMode");
            normalButton = Content.Load<Texture2D>("moltenMode");
            hardButton = Content.Load<Texture2D>("fallenMode");
            map = Content.Load<Texture2D>("crossroadsUnfinished2");
            temp = Content.Load<Texture2D>("tempImage");
            bg = temp;
        }

        protected override void Update(GameTime gameTime)
        {
            this.Window.Title = mouseState.Position.ToString();
            mouseState = Mouse.GetState();
            prevMouseState = mouseState;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (screen == Screen.Title)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && playRec.Contains(mouseState.Position))
                {
                    screen = Screen.Play;

                    

                }






            }
            else if (screen == Screen.Play)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && easyRec.Contains(mouseState.Position))
                {
                    screen = Screen.Easy;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && normalRec.Contains(mouseState.Position))
                {
                    screen = Screen.Normal;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && hardRec.Contains(mouseState.Position))
                {
                    screen = Screen.Hard;
                }
            }
            else if (screen == Screen.Easy)
            {
                bg = map;

            }
            else if (screen == Screen.Normal)
            {
                bg = map;

            }
            else if (screen == Screen.Hard)
            {
                bg = map;

            }
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            
            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(bg, window, Color.White);
                _spriteBatch.Draw(playButton, playRec, Color.White);






            }
            else if (screen == Screen.Play)
            {
                _spriteBatch.Draw(bg, window, Color.White);
                _spriteBatch.Draw(easyButton, easyRec, Color.White);
                _spriteBatch.Draw(normalButton, normalRec, Color.White);
                _spriteBatch.Draw(hardButton, hardRec, Color.White);

                //REMEMBER TO MAKE THE DIFFUICULTY BUTTONS GET BIGGER WHEN HOVERED OVER YOU BUM




            }
            else if (screen == Screen.Easy)
            {
                _spriteBatch.Draw(bg, window, Color.White);






            }
            else if (screen == Screen.Normal)
            {
                _spriteBatch.Draw(bg, window, Color.White);






            }
            else if (screen == Screen.Hard)
            {
                _spriteBatch.Draw(bg, window, Color.White);






            }




            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
