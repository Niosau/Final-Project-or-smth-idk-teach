using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
enum Screen
{
    Title,
    Play,
    TowerPick,
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
        Texture2D temp, bg, titleScreen, map, playButton, easyButton, normalButton, hardButton, scout, inventory;
        Rectangle playRec, easyRec, normalRec, hardRec, window, scoutRec, inventoryRec;
        float opacity = 0f;
        int sizeChange = 2, coordChange = 1;
        int smalldown = 2, smallcoord = 1;
        Vector2 position = new Vector2(200, 300);
        float scale = 0.4f;
        const float NormalScale = 0.4f;
        const float HoverScale = 0.5f;
        const float LerpSpeed = 0.15f;
        Button btnEasy;
        Button btnNormal;
        Button btnHard;
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
            inventoryRec = new Rectangle(400, 400, 200, 100);
            easyRec = new Rectangle(10, 10, 10, 10);
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
            inventory = Content.Load<Texture2D>("inventoryTemp");
            easyButton = Content.Load<Texture2D>("easyMode");
            normalButton = Content.Load<Texture2D>("moltenMode");
            hardButton = Content.Load<Texture2D>("fallenMode");
            map = Content.Load<Texture2D>("crossroadsUnfinished2");
            temp = Content.Load<Texture2D>("tempImage");
            scout = Content.Load<Texture2D>("scout");
            bg = temp;


            btnEasy = new Button(easyButton, new Vector2(200, 300));
            btnNormal = new Button(normalButton, new Vector2(500, 300));
            btnHard = new Button(hardButton, new Vector2(800, 300));
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            // Update window title with the FRESH mouse position
            this.Window.Title = mouseState.Position.ToString();

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
                btnEasy.Update(mouseState, prevMouseState);
                btnNormal.Update(mouseState, prevMouseState);
                btnHard.Update(mouseState, prevMouseState);
                if (mouseState.LeftButton == ButtonState.Pressed && inventoryRec.Contains(mouseState.Position))
                {
                    screen = Screen.TowerPick;
                }

                if (btnEasy.IsClicked) screen = Screen.Easy;
                if (btnNormal.IsClicked) screen = Screen.Normal;
                if (btnHard.IsClicked) screen = Screen.Hard;
            }
            else if (screen == Screen.TowerPick)
            {
                bg = temp;

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
                Vector2 easyOrigin = new Vector2(easyButton.Width / 2f, easyButton.Height / 2f);
                _spriteBatch.Draw(bg, window, Color.White);
                 btnEasy.Draw(_spriteBatch);
    btnNormal.Draw(_spriteBatch);
    btnHard.Draw(_spriteBatch);
                _spriteBatch.Draw(inventory, inventoryRec, Color.White);
                //REMEMBER TO MAKE THE DIFFUICULTY BUTTONS GET BIGGER WHEN HOVERED OVER YOU BUM




            }
            else if (screen == Screen.TowerPick)
            {
                _spriteBatch.Draw(bg, window, Color.White);

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

        // This was way to complicated for little ol me and prob unoptimised but it lowkey worked so idc
        public class Button
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _scale;
        
        private const float NormalScale = 0.4f;
        private const float HoverScale = 0.6f;
        private const float LerpSpeed = 0.15f;

        // Public properties to access state from Game1
        public Rectangle Hitbox { get; private set; }
        public bool IsClicked { get; private set; }

        public Button(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
            _scale = NormalScale;
        }

        public void Update(MouseState mouseState, MouseState prevMouseState)
        {
            
            int scaledWidth = (int)(_texture.Width * _scale);
            int scaledHeight = (int)(_texture.Height * _scale);

            
            Hitbox = new Rectangle(
                (int)_position.X - (scaledWidth / 2),
                (int)_position.Y - (scaledHeight / 2),
                scaledWidth,
                scaledHeight
            );

            // 3. Hover scale interpolation
            if (Hitbox.Contains(mouseState.Position))
            {
                _scale = MathHelper.Lerp(_scale, HoverScale, LerpSpeed);
            }
            else
            {
                _scale = MathHelper.Lerp(_scale, NormalScale, LerpSpeed);
            }

           
            IsClicked = Hitbox.Contains(mouseState.Position) && 
                        mouseState.LeftButton == ButtonState.Pressed && 
                        prevMouseState.LeftButton == ButtonState.Released;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            Vector2 origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, origin, _scale, SpriteEffects.None, 0f);
        }
    }
}
    }

