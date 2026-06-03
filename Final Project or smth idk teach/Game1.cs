using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
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
        Rectangle sidebarRect;
        private Tower _focusedTower = null;
        bool isPlacingTower = false;
        int sidebarWidth = 200;
        int baseHealth;
        int gold;
        SpriteFont gameFont;
        int towerCost = 100;
        Screen screen;
        MouseState mouseState, prevMouseState;
        private Song Menu;
        Texture2D temp, bg, titleScreen, map, playButton, easyButton, normalButton, hardButton, scout, sniper, inventory, enemyTexture, fastEnemyTexture, tankEnemyTexture, rangeCircle, upgradeButton;
        Rectangle playRec, easyRec, normalRec, hardRec, window, scoutRec, inventoryRec, upgradeRec;
        float opacity = 0f;
        int sizeChange = 2, coordChange = 1;
        int smalldown = 2, smallcoord = 1;
        Vector2 position = new Vector2(200, 300);
        private SpriteFont _font;
        bool clickedTower = false;

        public enum TowerType { None, Basic, Sniper }
        private TowerType _selectedTower = TowerType.None;

        private Button _btnBasicTower;
        private Button _btnSniperTower;
        List<Tower> activeTowers;
        List<Projectile> activeProjectiles = new List<Projectile>();
        Texture2D bulletTexture;
        List<Vector2> easyPath = new List<Vector2>
{
             new Vector2(90, 380),
             new Vector2(370, 380),
             new Vector2(370, 310),
             new Vector2(535, 310),
             new Vector2(535, 450),
             new Vector2(365, 450),
             new Vector2(365, 300),
             new Vector2(535, 300),
             new Vector2(535, 380),
             new Vector2(785, 380),
             new Vector2(785, 100),
             new Vector2(450, 100),
             new Vector2(450, 310),
             new Vector2(535, 310),
             new Vector2(535, 450),
             new Vector2(365, 450),
             new Vector2(365, 310),
             new Vector2(535, 310),
             new Vector2(535, 450),
             new Vector2(450, 450),
             new Vector2(450, 750)
        };
        List<Vector2> level1Path;
        List<Enemy> activeEnemies;
        List<Rectangle> pathHitboxes;
        WaveManager waveManager;
        float scale = 0.2f;
        const float NormalScale = 0.4f;
        const float HoverScale = 0.5f;
        const float LerpSpeed = 0.15f;
        Button btnEasy;
        Button btnNormal;
        Button btnHard;
        private Texture2D CreateCircleTexture(int radius)
        {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radiusSquared = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= radiusSquared)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
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
            inventoryRec = new Rectangle(750, 650, 200, 100);
            easyRec = new Rectangle(10, 10, 10, 10);
            normalRec = new Rectangle(400, 200, 200, 200);
            hardRec = new Rectangle(700, 200, 200, 200);
            sidebarRect = new Rectangle(GraphicsDevice.Viewport.Width - sidebarWidth, 0, sidebarWidth, GraphicsDevice.Viewport.Height);
            _graphics.PreferredBackBufferWidth = window.Width;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = window.Height;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Textures Below
            upgradeButton = Content.Load<Texture2D>("rectangle");
            _font = Content.Load<SpriteFont>("minesFont");
            Texture2D basicTex = Content.Load<Texture2D>("scoutImgTEMP");
            Texture2D sniperTex = Content.Load<Texture2D>("sniperImgTEMP");
            enemyTexture = Content.Load<Texture2D>("enemyTemp");
            fastEnemyTexture = Content.Load<Texture2D>("enemyTemp");
            tankEnemyTexture = Content.Load<Texture2D>("enemyTemp");
            playButton = Content.Load<Texture2D>("PlayButton");
            inventory = Content.Load<Texture2D>("inventoryTemp");
            easyButton = Content.Load<Texture2D>("easyMode");
            normalButton = Content.Load<Texture2D>("moltenMode");
            hardButton = Content.Load<Texture2D>("fallenMode");
            map = Content.Load<Texture2D>("crossroadsUnfinished2");
            temp = Content.Load<Texture2D>("tempImage");
            scout = Content.Load<Texture2D>("scoutImgTEMP");
            sniper = Content.Load<Texture2D>("sniperImgTEMP");


            bg = temp;
            gameFont = Content.Load<SpriteFont>("minesFont");
            bulletTexture = Content.Load<Texture2D>("bullet");

            baseHealth = 20;
            gold = 300;
            rangeCircle = CreateCircleTexture(100);

            _btnBasicTower = new Button(basicTex, new Vector2(860, 200), 3f, 3.13f);
            _btnSniperTower = new Button(sniperTex, new Vector2(860, 400), 0.1f, 0.13f);

            // Difficulty Buttons (Default or custom size)
            btnEasy = new Button(easyButton, new Vector2(200, 300), 0.4f, 0.5f);
            btnNormal = new Button(normalButton, new Vector2(500, 300), 0.4f, 0.5f);
            btnHard = new Button(hardButton, new Vector2(800, 300), 0.4f, 0.5f);
            level1Path = new List<Vector2> {
             new Vector2(90, 380),
             new Vector2(370, 380),
             new Vector2(370, 310),
             new Vector2(535, 310),
             new Vector2(535, 450),
             new Vector2(365, 450),
             new Vector2(365, 300),
             new Vector2(535, 300),
             new Vector2(535, 380),
             new Vector2(785, 380),
             new Vector2(785, 100),
             new Vector2(450, 100),
             new Vector2(450, 310),
             new Vector2(535, 310),
             new Vector2(535, 450),
             new Vector2(365, 450),
             new Vector2(365, 310),
             new Vector2(535, 310),
             new Vector2(535, 450),
             new Vector2(450, 450),
             new Vector2(450, 750)
                };
            pathHitboxes = new List<Rectangle>
            {
                // The X, Y, Width, and Height of the invisible boxes
                new Rectangle(0, 350, 350, 100),   // Left horizontal path
                new Rectangle(350, 300, 200, 200), // Center intersection
                new Rectangle(400, 500, 100, 300), // Bottom vertical path
               
            };
            activeTowers = new List<Tower>();


            activeEnemies = new List<Enemy>();
            waveManager = new WaveManager(level1Path, enemyTexture, fastEnemyTexture, tankEnemyTexture);
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
                if (mouseState.LeftButton == ButtonState.Pressed && playRec.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)
                {
                    screen = Screen.Play;
                }
            }
            else if (screen == Screen.Play)
            {
                btnEasy.Update(mouseState, prevMouseState);
                btnNormal.Update(mouseState, prevMouseState);
                btnHard.Update(mouseState, prevMouseState);
                if (mouseState.LeftButton == ButtonState.Pressed && inventoryRec.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)
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

                _btnBasicTower.Update(mouseState, prevMouseState);
                _btnSniperTower.Update(mouseState, prevMouseState);
                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    _selectedTower = TowerType.Basic;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    _selectedTower = TowerType.Sniper;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D0))
                {
                    gold += 1000;
                }

                if (_btnBasicTower.IsClicked) _selectedTower = TowerType.Basic;
                if (_btnSniperTower.IsClicked) _selectedTower = TowerType.Sniper;


                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Point mousePos = new Point(mouseState.X, mouseState.Y);


                    // Check if we clicked on an existing tower
                    foreach (Tower tower in activeTowers)
                    {

                        int tw = (int)(tower.Texture.Width * tower.Scale);
                        int th = (int)(tower.Texture.Height * tower.Scale);
                        Rectangle towerRect = new Rectangle((int)tower.Position.X - tw / 2, (int)tower.Position.Y - th / 2, tw, th);

                        if (towerRect.Contains(mousePos))
                        {
                            _focusedTower = tower;
                            clickedTower = true;
                            break;

                        }
                        if (upgradeRec.Contains(mousePos) && gold >= 100 && clickedTower)
                        {
                            gold -= 100;
                            tower.Damage += 10;

                        }
                        if (!upgradeRec.Contains(mousePos) && clickedTower)
                        {

                            clickedTower = false;
                        }
                    }


                    if (!clickedTower && _selectedTower == TowerType.None && !upgradeRec.Contains(mousePos))
                    {
                        _focusedTower = null;

                    }

                }






                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Point mousePos = new Point(mouseState.X, mouseState.Y);
                    Vector2 clickPosition = new Vector2(mouseState.X, mouseState.Y);


                    if (!_btnBasicTower.Hitbox.Contains(mousePos) && !_btnSniperTower.Hitbox.Contains(mousePos))
                    {
                        bool canPlace = true;

                        // Path collision check
                        foreach (Rectangle rect in pathHitboxes)
                        {
                            if (rect.Contains(mousePos)) { canPlace = false; break; }
                        }

                        // Placement logic based on selection
                        if (canPlace)
                        {
                            float minimumDistance = 40f; // Adjust this based on how fat tower sprites are
                            bool tooClose = false;


                            foreach (Tower tower in activeTowers)
                            {
                                if (Vector2.Distance(tower.Position, clickPosition) < minimumDistance)
                                {
                                    tooClose = true;
                                    break;
                                }
                            }


                            if (!tooClose)
                            {
                                if (_selectedTower == TowerType.Basic && gold >= 50)
                                {
                                    activeTowers.Add(new Tower(scout, clickPosition, 64f, 200f, 1, 1.025f));
                                    gold -= 50;
                                    _selectedTower = TowerType.None;
                                }
                                else if (_selectedTower == TowerType.Sniper && gold >= 100)
                                {
                                    activeTowers.Add(new Tower(sniper, clickPosition, 64f, 500f, 25, 5.025f));
                                    gold -= 100;
                                    _selectedTower = TowerType.None;
                                }
                            }
                        }
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !waveManager.IsWaveActive)
                {
                    waveManager.StartNextWave();
                }


                waveManager.Update(gameTime, activeEnemies);


                for (int i = activeEnemies.Count - 1; i >= 0; i--)
                {
                    activeEnemies[i].Update(gameTime);

                    // Check if the enemy died
                    if (activeEnemies[i].IsDead)
                    {

                        gold += activeEnemies[i].GoldReward;

                        // Remove them from the game
                        activeEnemies.RemoveAt(i);
                    }
                    // Check if they reached the end
                    else if (activeEnemies[i].ReachedEnd)
                    {
                        baseHealth--;
                        activeEnemies.RemoveAt(i);
                    }
                }
                foreach (Tower tower in activeTowers)
                {
                    tower.Update(gameTime, activeEnemies, activeProjectiles, bulletTexture);
                }

                // 2. Update projectiles and clean up inactive ones
                for (int i = activeProjectiles.Count - 1; i >= 0; i--)
                {
                    activeProjectiles[i].Update();
                    if (!activeProjectiles[i].IsActive)
                    {
                        activeProjectiles.RemoveAt(i);
                    }
                }

                // 4. TOWER PLACEMENT (Only the click logic goes in here)
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Point mousePos = new Point(mouseState.X, mouseState.Y);
                    Vector2 clickPosition = new Vector2(mouseState.X, mouseState.Y);
                    bool canPlace = true;

                    foreach (Rectangle rect in pathHitboxes)
                    {
                        if (rect.Contains(mousePos))
                        {
                            canPlace = false;
                            break;
                        }
                    }

                    float minimumDistance = 50f;
                    foreach (Tower tower in activeTowers)
                    {
                        if (Vector2.Distance(tower.Position, clickPosition) < minimumDistance)
                        {
                            canPlace = false;
                            break;
                        }
                    }
                }


                // Game Over Check
                if (baseHealth <= 0)
                {
                    screen = Screen.Title;
                    Initialize();
                }
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





            }
            else if (screen == Screen.TowerPick)
            {
                _spriteBatch.Draw(bg, window, Color.White);

            }
            else if (screen == Screen.Easy)
            {
                _spriteBatch.Draw(bg, window, Color.White);

                _spriteBatch.DrawString(_font, $"Health: {baseHealth}", new Vector2(10, 10), Color.Red);
                _spriteBatch.DrawString(_font, $"Gold: {gold}", new Vector2(10, 35), Color.Gold);
                _spriteBatch.DrawString(_font, $"Selected: {_selectedTower}", new Vector2(10, 60), Color.White);
                _spriteBatch.DrawString(_font, $"Enemies: {activeEnemies.Count}", new Vector2(10, 85), Color.White);
                _spriteBatch.DrawString(_font, $"Towers: {activeTowers.Count}", new Vector2(10, 110), Color.White);
                _btnBasicTower.Draw(_spriteBatch);
                _btnSniperTower.Draw(_spriteBatch);

                foreach (Tower tower in activeTowers)
                {
                    tower.Draw(_spriteBatch);
                }
                foreach (Enemy enemy in activeEnemies)
                {
                    enemy.Draw(_spriteBatch);
                }
                foreach (Projectile p in activeProjectiles)
                {
                    p.Draw(_spriteBatch);
                }

                if (_selectedTower != TowerType.None)
                {
                    if (_selectedTower != TowerType.None)
                    {
                        Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);


                        Texture2D previewTexture = (_selectedTower == TowerType.Basic) ? scout : sniper;
                        float previewRange = (_selectedTower == TowerType.Basic) ? 200f : 500f;
                        int currentPrice = (_selectedTower == TowerType.Basic) ? 50 : 100;


                        bool invalidSpot = false;

                        // Check if mouse is over the path
                        foreach (Rectangle rect in pathHitboxes)
                        {
                            if (rect.Contains(mouseState.Position)) { invalidSpot = true; break; }
                        }

                        // Check if mouse is too close to other towers
                        foreach (Tower tower in activeTowers)
                        {
                            if (Vector2.Distance(tower.Position, mousePos) < 40f) { invalidSpot = true; break; }
                        }

                        // Check if can afford it
                        bool canAfford = gold >= currentPrice;


                        Color circleColor = (invalidSpot || !canAfford) ? Color.Red * 0.4f : Color.White * 0.3f;

                        float rangeScale = previewRange / 100f;
                        Vector2 rangeOrigin = new Vector2(rangeCircle.Width / 2f, rangeCircle.Height / 2f);
                        _spriteBatch.Draw(rangeCircle, mousePos, null, circleColor, 0f, rangeOrigin, rangeScale, SpriteEffects.None, 0f);


                        float ghostScale = 64f / previewTexture.Width;
                        Vector2 towerOrigin = new Vector2(previewTexture.Width / 2f, previewTexture.Height / 2f);
                        _spriteBatch.Draw(previewTexture, mousePos, null, Color.White * 0.5f, 0f, towerOrigin, ghostScale, SpriteEffects.None, 0f);


                        if (!canAfford)
                        {
                            _spriteBatch.DrawString(gameFont, "NOT ENOUGH GOLD", new Vector2(mouseState.X, mouseState.Y - 40), Color.Red);
                        }
                        else if (invalidSpot)
                        {
                            _spriteBatch.DrawString(gameFont, "CANNOT PLACE HERE", new Vector2(mouseState.X, mouseState.Y - 40), Color.Orange);
                        }
                    }
                    if (_selectedTower != TowerType.None)
                    {
                        // Determine the cost based on what is in the player's "hand"
                        int currentPrice = (_selectedTower == TowerType.Basic) ? 50 : 100;

                        if (gold < currentPrice)
                        {
                            // Draw the text slightly above the mouse cursor
                            Vector2 textPos = new Vector2(mouseState.X, mouseState.Y - 30);


                            _spriteBatch.DrawString(gameFont, "NOT ENOUGH GOLD", textPos, Color.Red);
                        }
                    }
                }
                foreach (Tower tower in activeTowers)
                {
                    tower.Draw(_spriteBatch);
                }


                if (_focusedTower != null)
                {
                    float rangeScale = _focusedTower.Range / 100f; // Scale based on the tower's unique range

                    upgradeRec = new Rectangle(_focusedTower.Position.ToPoint().X - 50, _focusedTower.Position.ToPoint().Y - 70, 100, 50);
                    Vector2 origin = new Vector2(rangeCircle.Width / 2f, rangeCircle.Height / 2f);

                    _spriteBatch.Draw(upgradeButton, upgradeRec, Color.White);
                    int dam = _focusedTower.Damage;
                    string currentDamage = dam.ToString();
                    _spriteBatch.Draw(rangeCircle, _focusedTower.Position, null, Color.Yellow * 0.4f, 0f, origin, rangeScale, SpriteEffects.None, 0f);
                    _spriteBatch.DrawString(_font, currentDamage, new Vector2(100, 100), Color.White);
                }
                foreach (Enemy enemy in activeEnemies)
                {
                    enemy.Draw(_spriteBatch);

                    // Check if the mouse is hovering over this specific enemy
                    if (enemy.GetBounds().Contains(mouseState.Position))
                    {
                        string hpText = $"HP: {enemy.Health}";


                        Vector2 textSize = gameFont.MeasureString(hpText);
                        Vector2 textPos = new Vector2(
                            enemy.Position.X - (textSize.X / 2),
                            enemy.Position.Y - (enemy.Texture.Height * enemy.Scale / 2) - 20
                        );


                        _spriteBatch.DrawString(gameFont, hpText, textPos + new Vector2(1, 1), Color.Black);
                        // Draw the actual HP in white or green
                        _spriteBatch.DrawString(gameFont, hpText, textPos, Color.GreenYellow);
                    }
                }
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
            base.Draw(gameTime);
        }
    }
}