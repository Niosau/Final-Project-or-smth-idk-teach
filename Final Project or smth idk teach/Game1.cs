using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public enum TowerType { None, Basic, Sniper, Minigunner }

    public enum Screen
    {
        Title,
        Play,
        TowerPick,
        Easy,
        Normal,
        Hard
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Tower _focusedTower = null;
        int baseHealth;
        SpriteFont gameFont;
        Screen screen;
        MouseState mouseState, prevMouseState;
        Texture2D temp, bg, map, playButton, easyButton, normalButton, hardButton, scout, sniper, inventory, enemyTexture, fastEnemyTexture, tankEnemyTexture, rangeCircle, upgradeButton;
        Rectangle playRec, easyRec, normalRec, hardRec, window, inventoryRec, upgradeRec, hudRec,upgradeIconRec;
        Rectangle victoryPanelRec, victoryMenuRec;
        Texture2D HUD, titleScreenTds, upgradeIcon, scoutUpgrade1, scoutUpgrade2, scoutUpgrade3, scoutUpgrade4, sniperUpgrade1, sniperUpgrade2, sniperUpgrade3, sniperUpgrade4;
        private SpriteFont _font;
        bool clickedTower = false;
        private const int MaxEquippedTowers = 5;

        private TowerType _selectedTower = TowerType.None;
        private readonly List<TowerType> _availableTowerTypes = new List<TowerType> { TowerType.Basic, TowerType.Sniper, TowerType.Minigunner };
        private readonly List<TowerType> _ownedTowerTypes = new List<TowerType> { TowerType.Basic, TowerType.Sniper };
        private readonly List<TowerType> _equippedTowers = new List<TowerType> { TowerType.Basic, TowerType.Sniper };

        private Button _btnBasicTower;
        private Button _btnSniperTower;
        private Button _btnMinigunnerTower;
        private Button _inventoryBasicTower;
        private Button _inventorySniperTower;
        private Button _inventoryMinigunnerTower;
        private Texture2D _pixel;
        bool victoryPopupOpen = false;
        bool victoryRewardGiven = false;
        List<Tower> activeTowers;
        List<Projectile> activeProjectiles = new List<Projectile>();
        Texture2D bulletTexture;
        List<Vector2> level1Path;
        List<Enemy> activeEnemies;
        List<Rectangle> pathHitboxes;
        WaveManager waveManager;
        Dictionary<Screen, GameModeConfig> gameModes;
        GameModeConfig currentGameMode;
        Dictionary<EnemyType, Texture2D> enemyTextures;
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
            hudRec = new Rectangle(600, 130, 400, 500);
            victoryPanelRec = new Rectangle(250, 220, 500, 310);
            victoryMenuRec = new Rectangle(365, 425, 270, 70);
            _graphics.PreferredBackBufferWidth = window.Width;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = window.Height;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Textures Below
            upgradeButton = Content.Load<Texture2D>("upgradeButton");
            titleScreenTds = Content.Load<Texture2D>("titleScreenTDS");
            _font = Content.Load<SpriteFont>("minesFont");
            Texture2D basicTex = Content.Load<Texture2D>("scoutNew");
            Texture2D sniperTex = Content.Load<Texture2D>("sniperNEW");
            enemyTexture = Content.Load<Texture2D>("normal");
            fastEnemyTexture = Content.Load<Texture2D>("speedy");
            tankEnemyTexture = Content.Load<Texture2D>("slow");
            playButton = Content.Load<Texture2D>("PlayButton");
            inventory = Content.Load<Texture2D>("inventoryTemp");
            easyButton = Content.Load<Texture2D>("easyMode");
            normalButton = Content.Load<Texture2D>("moltenMode");
            hardButton = Content.Load<Texture2D>("fallenMode");
            map = Content.Load<Texture2D>("crossroadsUnfinished2");
            temp = Content.Load<Texture2D>("tempImage");
            scout = Content.Load<Texture2D>("scoutNew");
            sniper = Content.Load<Texture2D>("sniperNEW");
            HUD = Content.Load<Texture2D>("upgradeHUD");
            // -------------------Upgrades----------------------------------
            

            scoutUpgrade1 = Content.Load<Texture2D>("Scout1");
            scoutUpgrade2 = Content.Load<Texture2D>("upgradeHUD");
            scoutUpgrade3 = Content.Load<Texture2D>("upgradeHUD");
            scoutUpgrade4 = Content.Load<Texture2D>("upgradeHUD");
            

            sniperUpgrade1 = Content.Load<Texture2D>("upgradeHUD");
            sniperUpgrade2 = Content.Load<Texture2D>("upgradeHUD");
            sniperUpgrade3 = Content.Load<Texture2D>("upgradeHUD");
            sniperUpgrade4 = Content.Load<Texture2D>("upgradeHUD");
            











            bg = titleScreenTds;
            gameFont = Content.Load<SpriteFont>("minesFont");
            bulletTexture = Content.Load<Texture2D>("bullet");
            enemyTextures = new Dictionary<EnemyType, Texture2D>
            {
                { EnemyType.Basic, enemyTexture },
                { EnemyType.Fast, fastEnemyTexture },
                { EnemyType.Tank, tankEnemyTexture }
            };

            baseHealth = 20;
            Gamedata.gold = 300;
            rangeCircle = CreateCircleTexture(100);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _btnBasicTower = new Button(basicTex, new Vector2(860, 200), 3f, 3.13f);
            _btnSniperTower = new Button(sniperTex, new Vector2(860, 400), 3f, 3.13f);
            _btnMinigunnerTower = new Button(basicTex, new Vector2(860, 600), 3f, 3.13f);
            _inventoryBasicTower = new Button(basicTex, new Vector2(350, 300), 3f, 3.13f);
            _inventorySniperTower = new Button(sniperTex, new Vector2(550, 300), 3f, 3.13f);
            _inventoryMinigunnerTower = new Button(basicTex, new Vector2(750, 300), 3f, 3.13f);
            
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
            gameModes = CreateGameModes();
            currentGameMode = gameModes[Screen.Easy];
            waveManager = new WaveManager(currentGameMode.Path, enemyTextures, currentGameMode.Waves, currentGameMode.SpawnInterval);
        }

        private Dictionary<Screen, GameModeConfig> CreateGameModes()
        {
            return new Dictionary<Screen, GameModeConfig>
            {
                {
                    Screen.Easy,
                    new GameModeConfig(
                        Screen.Easy,
                        "Easy",
                        map,
                        25,
                        350,
                        level1Path,
                        pathHitboxes,
                        new List<WaveDefinition>
                        {
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 5, 2f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 4, 2f, 4), new WaveEnemyGroup(EnemyType.Fast, 2, 5f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 10, 2f, 4), new WaveEnemyGroup(EnemyType.Fast, 4, 2f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Tank, 3, 1f, 14), new WaveEnemyGroup(EnemyType.Basic, 6, 2f, 4), new WaveEnemyGroup(EnemyType.Fast, 4, 5f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Tank, 6, 2f, 4), new WaveEnemyGroup(EnemyType.Basic, 5, 2f, 4))
                        },
                        0.8f,
                        50)
                },
                {
                    Screen.Normal,
                    new GameModeConfig(
                        Screen.Normal,
                        "Normal",
                        map,
                        20,
                        300,
                        level1Path,
                        pathHitboxes,
                        new List<WaveDefinition>
                        {
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 8, 2f, 5)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 8, 2f, 6), new WaveEnemyGroup(EnemyType.Fast, 4, 5f, 5)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Tank, 4, 1f, 18), new WaveEnemyGroup(EnemyType.Basic, 8, 2f, 7)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Fast, 10, 5f, 6), new WaveEnemyGroup(EnemyType.Tank, 5, 1f, 20)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 12, 2f, 10), new WaveEnemyGroup(EnemyType.Tank, 8, 1f, 24))
                        },
                        0.7f,
                        100)
                },
                {
                    Screen.Hard,
                    new GameModeConfig(
                        Screen.Hard,
                        "Fallen",
                        map,
                        15,
                        250,
                        level1Path,
                        pathHitboxes,
                        new List<WaveDefinition>
                        {
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 10, 2.5f, 6), new WaveEnemyGroup(EnemyType.Fast, 4, 5.5f, 5)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 12, 2.5f, 8), new WaveEnemyGroup(EnemyType.Fast, 8, 5.5f, 6)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Tank, 8, 1.2f, 22), new WaveEnemyGroup(EnemyType.Fast, 8, 5.5f, 8)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 16, 2.5f, 12), new WaveEnemyGroup(EnemyType.Tank, 10, 1.2f, 28)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Fast, 18, 5.5f, 10), new WaveEnemyGroup(EnemyType.Tank, 12, 1.2f, 34))
                        },
                        0.6f,
                        175)
                }
            };
        }

        private bool IsGameplayScreen(Screen gameScreen)
        {
            return gameScreen == Screen.Easy || gameScreen == Screen.Normal || gameScreen == Screen.Hard;
        }

        private void StartGameMode(Screen gameScreen)
        {
            currentGameMode = gameModes[gameScreen];
            screen = gameScreen;
            bg = currentGameMode.MapTexture;
            baseHealth = currentGameMode.StartingHealth;
            Gamedata.gold = currentGameMode.StartingGold;
            pathHitboxes = currentGameMode.PathHitboxes;
            activeTowers.Clear();
            activeEnemies.Clear();
            activeProjectiles.Clear();
            _selectedTower = TowerType.None;
            _focusedTower = null;
            clickedTower = false;
            victoryPopupOpen = false;
            victoryRewardGiven = false;
            waveManager = new WaveManager(currentGameMode.Path, enemyTextures, currentGameMode.Waves, currentGameMode.SpawnInterval);
        }

        private void ReturnToMainMenu()
        {
            screen = Screen.Title;
            bg = titleScreenTds;
            activeTowers.Clear();
            activeEnemies.Clear();
            activeProjectiles.Clear();
            _selectedTower = TowerType.None;
            _focusedTower = null;
            clickedTower = false;
            victoryPopupOpen = false;
        }

        private void WinCurrentGameMode()
        {
            if (!victoryRewardGiven)
            {
                Gamedata.coins += currentGameMode.CoinReward;
                victoryRewardGiven = true;
            }

            victoryPopupOpen = true;
            _selectedTower = TowerType.None;
            _focusedTower = null;
        }

        private bool IsTowerOwned(TowerType towerType)
        {
            return _ownedTowerTypes.Contains(towerType);
        }

        private bool IsTowerEquipped(TowerType towerType)
        {
            return _equippedTowers.Contains(towerType);
        }

        private Button GetGameTowerButton(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return _btnBasicTower;
            if (towerType == TowerType.Sniper) return _btnSniperTower;
            if (towerType == TowerType.Minigunner) return _btnMinigunnerTower;
            return null;
        }

        private Button GetInventoryTowerButton(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return _inventoryBasicTower;
            if (towerType == TowerType.Sniper) return _inventorySniperTower;
            if (towerType == TowerType.Minigunner) return _inventoryMinigunnerTower;
            return null;
        }

        private Texture2D GetTowerTexture(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return scout;
            if (towerType == TowerType.Sniper) return sniper;
            if (towerType == TowerType.Minigunner) return scout;
            return scout;
        }

        private int GetTowerCost(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return 50;
            if (towerType == TowerType.Sniper) return 150;
            if (towerType == TowerType.Minigunner) return 350;
            return 0;
        }

        private int GetTowerUnlockCost(TowerType towerType)
        {
            if (towerType == TowerType.Minigunner) return 250;
            return 0;
        }

        private float GetTowerRange(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return 200f;
            if (towerType == TowerType.Sniper) return 500f;
            if (towerType == TowerType.Minigunner) return 175f;
            return 0f;
        }

        private int GetTowerDamage(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return 1;
            if (towerType == TowerType.Sniper) return 25;
            if (towerType == TowerType.Minigunner) return 2;
            return 0;
        }

        private float GetTowerFireRate(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return 1.025f;
            if (towerType == TowerType.Sniper) return 5.025f;
            if (towerType == TowerType.Minigunner) return 0.18f;
            return 1f;
        }

        private Vector2 GetGameTowerPricePosition(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return new Vector2(830, 285);
            if (towerType == TowerType.Sniper) return new Vector2(830, 485);
            if (towerType == TowerType.Minigunner) return new Vector2(830, 685);
            return Vector2.Zero;
        }

        private void ToggleTowerLoadout(TowerType towerType)
        {
            if (!IsTowerOwned(towerType))
            {
                int unlockCost = GetTowerUnlockCost(towerType);
                if (Gamedata.coins >= unlockCost)
                {
                    Gamedata.coins -= unlockCost;
                    _ownedTowerTypes.Add(towerType);
                }
                else
                {
                    return;
                }
            }

            if (_equippedTowers.Contains(towerType))
            {
                _equippedTowers.Remove(towerType);
                if (_selectedTower == towerType)
                {
                    _selectedTower = TowerType.None;
                }
            }
            else if (_equippedTowers.Count < MaxEquippedTowers)
            {
                _equippedTowers.Add(towerType);
            }
        }

        private bool MouseIsOverTowerButton(Point mousePos)
        {
            foreach (TowerType towerType in _equippedTowers)
            {
                Button button = GetGameTowerButton(towerType);
                if (button != null && button.Hitbox.Contains(mousePos))
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawRectangle(Rectangle rectangle, Color color)
        {
            _spriteBatch.Draw(_pixel, rectangle, color);
        }

        private void DrawRectangleOutline(Rectangle rectangle, Color color, int thickness)
        {
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            DrawRectangle(new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            DrawRectangle(new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
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
                bg = temp;
                btnEasy.Update(mouseState, prevMouseState);
                btnNormal.Update(mouseState, prevMouseState);
                btnHard.Update(mouseState, prevMouseState);
                if (mouseState.LeftButton == ButtonState.Pressed && inventoryRec.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)
                {
                    screen = Screen.TowerPick;
                }

                if (btnEasy.IsClicked) StartGameMode(Screen.Easy);
                if (btnNormal.IsClicked) StartGameMode(Screen.Normal);
                if (btnHard.IsClicked) StartGameMode(Screen.Hard);
            }
            else if (screen == Screen.TowerPick)
            {
                bg = temp;
                _inventoryBasicTower.Update(mouseState, prevMouseState);
                _inventorySniperTower.Update(mouseState, prevMouseState);
                _inventoryMinigunnerTower.Update(mouseState, prevMouseState);

                foreach (TowerType towerType in _availableTowerTypes)
                {
                    Button button = GetInventoryTowerButton(towerType);
                    if (button != null && button.IsClicked)
                    {
                        ToggleTowerLoadout(towerType);
                    }
                }

                if (mouseState.LeftButton == ButtonState.Pressed && inventoryRec.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)
                {
                    screen = Screen.Play;
                }

            }
            else if (IsGameplayScreen(screen))
            {
                bg = currentGameMode.MapTexture;

                if (victoryPopupOpen)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && victoryMenuRec.Contains(mouseState.Position))
                    {
                        ReturnToMainMenu();
                    }

                    base.Update(gameTime);
                    return;
                }

                foreach (TowerType towerType in _equippedTowers)
                {
                    Button button = GetGameTowerButton(towerType);
                    if (button != null)
                    {
                        button.Update(mouseState, prevMouseState);
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D1) && IsTowerEquipped(TowerType.Basic))
                {
                    _selectedTower = TowerType.Basic;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D2) && IsTowerEquipped(TowerType.Sniper))
                {
                    _selectedTower = TowerType.Sniper;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D3) && IsTowerEquipped(TowerType.Minigunner))
                {
                    _selectedTower = TowerType.Minigunner;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D0))
                {
                    Gamedata.gold += 1000;
                }

                if (IsTowerEquipped(TowerType.Basic) && _btnBasicTower.IsClicked) _selectedTower = TowerType.Basic;
                if (IsTowerEquipped(TowerType.Sniper) && _btnSniperTower.IsClicked) _selectedTower = TowerType.Sniper;
                if (IsTowerEquipped(TowerType.Minigunner) && _btnMinigunnerTower.IsClicked) _selectedTower = TowerType.Minigunner;

                

                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Point mousePos = new Point(mouseState.X, mouseState.Y);

                    if (upgradeRec.Contains(mousePos) && _focusedTower != null)
                    {
                        _focusedTower.Upgrade();
                    }
                    else
                    {
                        clickedTower = false;

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
                            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                            {

                                activeTowers.Remove(_focusedTower);
                                Exit();
                            }
                        }

                        if (!clickedTower && _selectedTower == TowerType.None)
                        {
                            _focusedTower = null;

                        }
                    }
                    
                }






                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Point mousePos = new Point(mouseState.X, mouseState.Y);
                    Vector2 clickPosition = new Vector2(mouseState.X, mouseState.Y);


                    if (!MouseIsOverTowerButton(mousePos))
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
                                if (_selectedTower != TowerType.None && IsTowerEquipped(_selectedTower) && Gamedata.gold >= GetTowerCost(_selectedTower))
                                {
                                    activeTowers.Add(new Tower(
                                        GetTowerTexture(_selectedTower),
                                        clickPosition,
                                        64f,
                                        GetTowerRange(_selectedTower),
                                        GetTowerDamage(_selectedTower),
                                        GetTowerFireRate(_selectedTower),
                                        _selectedTower,
                                        false,
                                        GetTowerCost(_selectedTower)));
                                    Gamedata.gold -= GetTowerCost(_selectedTower);
                                    _selectedTower = TowerType.None;
                                }
                                
                            }
                        }
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !waveManager.IsWaveActive && waveManager.HasMoreWaves)
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

                        Gamedata.gold += activeEnemies[i].GoldReward;

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

                if (!waveManager.IsWaveActive && !waveManager.HasMoreWaves && activeEnemies.Count == 0)
                {
                    WinCurrentGameMode();
                }

                // Game Over Check
                if (baseHealth <= 0)
                {
                    screen = Screen.Title;
                    Initialize();
                }
                
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
                _spriteBatch.DrawString(_font, "Inventory", new Vector2(390, 90), Color.White);
                _spriteBatch.DrawString(_font, $"Pick up to {MaxEquippedTowers}: {_equippedTowers.Count}/{MaxEquippedTowers}", new Vector2(310, 135), Color.White);

                foreach (TowerType towerType in _availableTowerTypes)
                {
                    Button button = GetInventoryTowerButton(towerType);
                    if (button != null)
                    {
                        button.Draw(_spriteBatch);

                        Rectangle selectedRect = button.Hitbox;
                        selectedRect.Inflate(12, 12);
                        Color outlineColor = IsTowerEquipped(towerType) ? Color.LimeGreen : Color.DarkGray;
                        DrawRectangleOutline(selectedRect, outlineColor, 4);

                        Vector2 labelPos = new Vector2(selectedRect.X, selectedRect.Bottom + 10);
                        _spriteBatch.DrawString(_font, $"{towerType} ${GetTowerCost(towerType)}", labelPos, Color.White);
                    }
                }

                _spriteBatch.Draw(inventory, inventoryRec, Color.White);
                _spriteBatch.DrawString(_font, "Done", new Vector2(815, 685), Color.White);

            }
            else if (IsGameplayScreen(screen))
            {
                _spriteBatch.Draw(bg, window, Color.White);

                _spriteBatch.DrawString(_font, $"Mode: {currentGameMode.Name}", new Vector2(10, 135), Color.White);
                _spriteBatch.DrawString(_font, $"Wave: {waveManager.WaveNumber}/{currentGameMode.Waves.Count}", new Vector2(10, 160), Color.White);
                _spriteBatch.DrawString(_font, $"Health: {baseHealth}", new Vector2(10, 10), Color.Red);
                _spriteBatch.DrawString(_font, $"Gold: {Gamedata.gold}", new Vector2(10, 35), Color.Gold);
                _spriteBatch.DrawString(_font, $"Selected: {_selectedTower}", new Vector2(10, 60), Color.White);
                _spriteBatch.DrawString(_font, $"Enemies: {activeEnemies.Count}", new Vector2(10, 85), Color.White);
                _spriteBatch.DrawString(_font, $"Towers: {activeTowers.Count}", new Vector2(10, 110), Color.White);
                foreach (TowerType towerType in _equippedTowers)
                {
                    Button button = GetGameTowerButton(towerType);
                    if (button != null)
                    {
                        button.Draw(_spriteBatch);
                        Vector2 pricePos = towerType == TowerType.Basic ? new Vector2(830, 285) : new Vector2(830, 485);
                        _spriteBatch.DrawString(_font, $"${GetTowerCost(towerType)}", pricePos, Color.Gold);
                    }
                }

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


                        Texture2D previewTexture = GetTowerTexture(_selectedTower);
                        float previewRange = GetTowerRange(_selectedTower);
                        int currentPrice = GetTowerCost(_selectedTower);


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
                        bool canAfford = Gamedata.gold >= currentPrice;


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
                        
                        int currentPrice = GetTowerCost(_selectedTower);

                        if (Gamedata.gold < currentPrice)
                        {
                            
                            Vector2 textPos = new Vector2(mouseState.X, mouseState.Y - 30);


                            _spriteBatch.DrawString(gameFont, "NOT ENOUGH CASH", textPos, Color.Red);
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
                    
                    upgradeRec = new Rectangle(669, 535, 267, 67);
                    upgradeIconRec = new Rectangle(616, 165, 200, 200);
                    Vector2 origin = new Vector2(rangeCircle.Width / 2f, rangeCircle.Height / 2f);

                    upgradeIcon = scoutUpgrade1;
                    
                    _spriteBatch.Draw(HUD, hudRec, Color.White);
                    _spriteBatch.Draw(rangeCircle, _focusedTower.Position, null, Color.Yellow * 0.4f, 0f, origin, rangeScale, SpriteEffects.None, 0f);
                    _spriteBatch.Draw(upgradeButton, upgradeRec, Color.White);
                    _spriteBatch.Draw(upgradeIcon, upgradeIconRec, Color.White);
                    _spriteBatch.DrawString(_font, $"Rng: {_focusedTower.StatRange}", new Vector2(900, 222), Color.White);
                    _spriteBatch.DrawString(_font, $"Dmg: {_focusedTower.Damage}", new Vector2(900, 185), Color.White);
                    _spriteBatch.DrawString(_font, $"Spd: {_focusedTower.FireRate}", new Vector2(900, 265), Color.White);
                    _spriteBatch.DrawString(_font, $"Upgrade", new Vector2(760, 558), Color.White);
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
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
