using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public enum TowerType { None, Basic, Sniper, Minigunner, DJ, Freezer, Farm, Commander, Accel, Soldier }

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
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState;
        SpriteFont gameFont;
        Screen screen;
        MouseState mouseState, prevMouseState;
        Texture2D temp, bg, map, playButton, easyButton, normalButton, hardButton, scout, sniper, inventory, enemyTexture, fastEnemyTexture, tankEnemyTexture, rangeCircle, upgradeButton;
        Rectangle playRec, easyRec, normalRec, hardRec, window, inventoryRec, upgradeRec, hudRec,upgradeIconRec;
        Rectangle victoryPanelRec, victoryMenuRec, hotbarRec;
        Texture2D HUD, titleScreenTds, upgradeIcon, scoutUpgrade1, scoutUpgrade2, scoutUpgrade3, scoutUpgrade4, sniperUpgrade1, sniperUpgrade2, sniperUpgrade3, sniperUpgrade4;
        Texture2D hotbar, loadoutScreen;
        Texture2D minigunner, accel, freezer, soldier, DJ, commander, Farm;
        private SpriteFont _font;
        private Song _easyModeSong;
        private Song _moltenSong;
        private Song _fallenSong;
        private Song _moltenBossSong;
        private Song _fallenKingSong;
        private bool _bossMusicStarted = false;
        bool clickedTower = false;
        private const int MaxEquippedTowers = 5;
        bool pauseMenuOpen = false;
        bool helpPopupOpen = false;
        private TowerType _selectedTower = TowerType.None;
        private readonly List<TowerType> _availableTowerTypes = new List<TowerType> { TowerType.Basic, TowerType.Sniper, TowerType.Minigunner, TowerType.DJ, TowerType.Freezer, TowerType.Farm, TowerType.Commander, TowerType.Accel};
        private readonly List<TowerType> _ownedTowerTypes = new List<TowerType> { TowerType.Basic, TowerType.Sniper };
        private readonly List<TowerType> _equippedTowers = new List<TowerType> { TowerType.Basic, TowerType.Sniper };
        Rectangle pausePanelRec, resumeRec, mainMenuRec;
        Rectangle helpButtonRec, helpPanelRec, helpCloseRec;
        private Dictionary<TowerType, Button> _gameTowerButtons;
        private Dictionary<TowerType, Button> _inventoryTowerButtons;
        private Texture2D _pixel;
        private Dictionary<TowerType, Texture2D[]> _upgradePreviews;
        bool victoryPopupOpen = false;
        bool victoryRewardGiven = false;
        private float nextWaveTimer = 0f;
        private bool waitingForNextWave = false;
        private bool wave1Started = false;
        private int _lastFarmPayoutWave = 0;
        List<Tower> activeTowers;
        List<Projectile> activeProjectiles = new List<Projectile>();
        Texture2D bulletTexture;
        SoundEffect _basicTowerShootSound;
        SoundEffect _freezerTowerShootSound;
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
            pausePanelRec = new Rectangle(300, 200, 400, 300);
            resumeRec = new Rectangle(350, 275, 300, 75);
            mainMenuRec = new Rectangle(350, 375, 300, 75);
            helpButtonRec = new Rectangle(20, 735, 45, 45);
            helpPanelRec = new Rectangle(190, 130, 620, 500);
            helpCloseRec = new Rectangle(helpPanelRec.Right - 45, helpPanelRec.Y + 15, 30, 30);
            window = new Rectangle(0, 0, 1000, 800);
            playRec = new Rectangle(400, 400, 200, 100);
            inventoryRec = new Rectangle(750, 650, 200, 100);
            easyRec = new Rectangle(10, 10, 10, 10);
            normalRec = new Rectangle(400, 200, 200, 200);
            hardRec = new Rectangle(700, 200, 200, 200);
            hudRec = new Rectangle(600, 130, 400, 500);
            upgradeRec = new Rectangle(669, 535, 267, 67);
            upgradeIconRec = new Rectangle(616, 165, 200, 200);
            victoryPanelRec = new Rectangle(250, 220, 500, 310);
            victoryMenuRec = new Rectangle(365, 425, 270, 70);
            hotbarRec = new Rectangle(200, 695, 500, 100);
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
            temp = Content.Load<Texture2D>("playBackground");
            scout = Content.Load<Texture2D>("scoutNew");
            sniper = Content.Load<Texture2D>("sniperNEW");
            minigunner = Content.Load<Texture2D>("minigunner");
            soldier = Content.Load<Texture2D>("soldier");
            freezer = Content.Load<Texture2D>("freezer");
            accel = Content.Load<Texture2D>("accel");
            DJ = Content.Load<Texture2D>("DJ");
             Farm = Content.Load<Texture2D>("farm");
            commander = Content.Load<Texture2D>("commander");
            HUD = Content.Load<Texture2D>("upgradeHUD");
            hotbar = Content.Load<Texture2D>("hotbarFull");
            loadoutScreen = Content.Load<Texture2D>("spritepaint");
            _basicTowerShootSound = Content.Load<SoundEffect>("towerShoot");
            _freezerTowerShootSound = Content.Load<SoundEffect>("freezerShoot");
            _easyModeSong = Content.Load<Song>("easyModeOST");
            _moltenSong = Content.Load<Song>("moltenOST");
            _fallenSong = Content.Load<Song>("fallenOST");
            _moltenBossSong = Content.Load<Song>("moltenBossOST");
            _fallenKingSong = Content.Load<Song>("fallenKingOST");
            // -------------------Upgrades----------------------------------


            scoutUpgrade1 = Content.Load<Texture2D>("Scout1");
            scoutUpgrade2 = Content.Load<Texture2D>("upgradeHUD");
            scoutUpgrade3 = Content.Load<Texture2D>("upgradeHUD");
            scoutUpgrade4 = Content.Load<Texture2D>("upgradeHUD");
            

            sniperUpgrade1 = Content.Load<Texture2D>("upgradeHUD");
            sniperUpgrade2 = Content.Load<Texture2D>("upgradeHUD");
            sniperUpgrade3 = Content.Load<Texture2D>("upgradeHUD");
            sniperUpgrade4 = Content.Load<Texture2D>("upgradeHUD");











            // Towers
            towerStats = new Dictionary<TowerType, TowerData>();

            towerStats[TowerType.Basic] = new TowerData
            {
                Texture = scout,
                Cost = 50,
                UnlockCost = 0,
                Damage = 1,
                Range = 200,
                FireRate = 1f
            };

            towerStats[TowerType.Sniper] = new TowerData
            {
                Texture = sniper,
                Cost = 150,
                UnlockCost = 0,
                Damage = 25,
                Range = 500,
                FireRate = 5f
            };

            towerStats[TowerType.Minigunner] = new TowerData
            {
                Texture = minigunner,
                Cost = 350,
                UnlockCost = 250,
                Damage = 2,
                Range = 175,
                FireRate = 0.18f
            };
            towerStats[TowerType.DJ] = new TowerData
            {
                Texture = DJ,
                Cost = 100,
                UnlockCost = 100,
                Damage = 0,
                Range = 200,
                FireRate = 100f // Support tower - doesn't shoot
            };
            towerStats[TowerType.Freezer] = new TowerData
            {
                Texture = freezer,
                Cost = 120,
                UnlockCost = 200,
                Damage = 0,
                Range = 220,
                FireRate = 1.5f
            };
            towerStats[TowerType.Farm] = new TowerData
            {
                Texture = Farm,
                Cost = 100,
                UnlockCost = 200,
                Damage = 0,
                Range = 0,
                FireRate = 10000f,
                FarmIncomePerWave = 20
            };
            towerStats[TowerType.Commander] = new TowerData
            {
                Texture = commander,
                Cost = 100,
                UnlockCost = 100,
                Damage = 0,
                Range = 200,
                FireRate = 100f // Support tower - doesn't shoot
            };
            towerStats[TowerType.Accel] = new TowerData
            {
                Texture = accel,
                Cost = 4000,
                UnlockCost = 5000,
                Damage = 50,
                Range = 200,
                FireRate = 0.5f
            };
            towerStats[TowerType.Soldier] = new TowerData
            {
                Texture = soldier,
                Cost = 100,
                UnlockCost = 100,
                Damage = 4,
                Range = 200,
                FireRate = 1.5f
            };

            // Setup upgrade preview textures per tower type/level (fallbacks used where assets missing)
            _upgradePreviews = new Dictionary<TowerType, Texture2D[]>();
            _upgradePreviews[TowerType.Basic] = new Texture2D[] { scout ?? scout, scout ?? scout, scout ?? scout, scout ?? scout };
            _upgradePreviews[TowerType.Sniper] = new Texture2D[] { sniper ?? sniper, sniper ?? sniper, sniper ?? sniper, sniper ?? sniper };
            _upgradePreviews[TowerType.Minigunner] = new Texture2D[] { minigunner, minigunner, minigunner, minigunner };
            _upgradePreviews[TowerType.DJ] = new Texture2D[] { DJ, DJ, DJ, DJ };
            _upgradePreviews[TowerType.Freezer] = new Texture2D[] { freezer, freezer, freezer, freezer };
            _upgradePreviews[TowerType.Farm] = new Texture2D[] { Farm, Farm, Farm, Farm };
            _upgradePreviews[TowerType.Commander] = new Texture2D[] { commander, commander, commander, commander };
            _upgradePreviews[TowerType.Accel] = new Texture2D[] { accel, accel, accel, accel };
            _upgradePreviews[TowerType.Soldier] = new Texture2D[] { soldier, soldier, soldier, soldier };














            bg = titleScreenTds;
            gameFont = Content.Load<SpriteFont>("minesFont");
            bulletTexture = Content.Load<Texture2D>("bullet");
            enemyTextures = new Dictionary<EnemyType, Texture2D>
            {
                { EnemyType.Basic, enemyTexture },
                { EnemyType.Fast, fastEnemyTexture },
                { EnemyType.Tank, tankEnemyTexture },
                { EnemyType.Abnormal, enemyTexture },
                { EnemyType.Quick, fastEnemyTexture },
                { EnemyType.FallenSkeleton, tankEnemyTexture },
                { EnemyType.FallenDreg, tankEnemyTexture },
                { EnemyType.FallenSquire, tankEnemyTexture },
                { EnemyType.Breaker2, tankEnemyTexture },
                { EnemyType.FallenSoul, fastEnemyTexture },
                { EnemyType.FallenHazmat, fastEnemyTexture },
                { EnemyType.Fallen, tankEnemyTexture },
                { EnemyType.FallenGiant, tankEnemyTexture },
                { EnemyType.PossessedArmor, tankEnemyTexture },
                { EnemyType.CorruptedFallen, tankEnemyTexture },
                { EnemyType.FallenSeraph, fastEnemyTexture },
                { EnemyType.FallenRusher, fastEnemyTexture },
                { EnemyType.FallenHero, tankEnemyTexture },
                { EnemyType.FallenShield, tankEnemyTexture },
                { EnemyType.FallenJester, tankEnemyTexture },
                { EnemyType.NecroticSkeleton, tankEnemyTexture },
                { EnemyType.FallenNecromancer, fastEnemyTexture },
                { EnemyType.FallenAngel, tankEnemyTexture },
                { EnemyType.FallenGuardian, tankEnemyTexture },
                { EnemyType.FallenTank, tankEnemyTexture },
                { EnemyType.Breaker4, tankEnemyTexture },
                { EnemyType.FallenSummoner, tankEnemyTexture },
                { EnemyType.FallenHonorGuard, tankEnemyTexture },
                { EnemyType.FallenKing, tankEnemyTexture },
                { EnemyType.Heavy, tankEnemyTexture },
                { EnemyType.EliteAbnormal, tankEnemyTexture },
                { EnemyType.Molten, tankEnemyTexture },
                { EnemyType.MoltenDemon, fastEnemyTexture },
                { EnemyType.EliteHazmat, fastEnemyTexture },
                { EnemyType.EliteBoomer, tankEnemyTexture },
                { EnemyType.MoltenHound, fastEnemyTexture },
                { EnemyType.MoltenMech, tankEnemyTexture },
                { EnemyType.HiddenBoss, tankEnemyTexture },
                { EnemyType.MoltenNecromancer, fastEnemyTexture },
                { EnemyType.Bulwark, tankEnemyTexture },
                { EnemyType.Breaker3, tankEnemyTexture },
                { EnemyType.MoltenExecutioner, tankEnemyTexture },
                { EnemyType.Tanker, tankEnemyTexture },
                { EnemyType.EliteMolten, fastEnemyTexture },
                { EnemyType.MoltenSummoner, tankEnemyTexture },
                { EnemyType.MoltenTitan, tankEnemyTexture },
                { EnemyType.MoltenWarlord, tankEnemyTexture }
            };

            baseHealth = 20;
            Gamedata.gold = 300;
            rangeCircle = CreateCircleTexture(100);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _gameTowerButtons = new Dictionary<TowerType, Button>();
            {
             CreateHotbarButtons();
            }

            _inventoryTowerButtons = new Dictionary<TowerType, Button>()
            {
                { TowerType.Basic, new Button(basicTex, new Vector2(200, 300), 3f, 3.13f) },
                { TowerType.Sniper, new Button(sniperTex, new Vector2(400, 300), 3f, 3.13f) },
                { TowerType.Minigunner, new Button(basicTex, new Vector2(600, 300), 3f, 3.13f) },
                { TowerType.DJ, new Button(DJ, new Vector2(800, 300), 3f, 3.13f) },
                { TowerType.Farm, new Button(Farm, new Vector2(200, 500), 3f, 3.13f) },
                { TowerType.Commander, new Button(commander, new Vector2(400, 500), 3f, 3.13f) },
                { TowerType.Accel, new Button(accel, new Vector2(600, 500), 3f, 3.13f) },
                { TowerType.Freezer, new Button(freezer, new Vector2(800, 500), 3f, 3.13f) }
            };

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
                new Rectangle(322, 277, 270, 270), // Center intersection
                new Rectangle(400, 500, 100, 300), // Bottom vertical path
                new Rectangle(410, 70, 450, 100), // Top horizontal path
                new Rectangle(750, 100, 100, 300),  // Right vertical path
                new Rectangle(400, 70, 100, 300), // Top vertical path
                new Rectangle(580, 350, 200, 100),   // Right horizontal path
               
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
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 4, 2f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 7, 2f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Quick, 2, 7f, 4), new WaveEnemyGroup(EnemyType.Basic, 6, 2f, 4), new WaveEnemyGroup(EnemyType.Quick, 2, 7f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Quick, 5, 7f, 4), new WaveEnemyGroup(EnemyType.Basic, 8, 2f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Heavy, 4, 1f, 18, 3f), new WaveEnemyGroup(EnemyType.Basic, 10, 2f, 4), new WaveEnemyGroup(EnemyType.Heavy, 4, 1f, 18, 3f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Heavy, 8, 1f, 18, 3f), new WaveEnemyGroup(EnemyType.Quick, 10, 7f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Basic, 6, 2f, 4), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.5f), new WaveEnemyGroup(EnemyType.Quick, 4, 7f, 4)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Quick, 12, 7f, 4), new WaveEnemyGroup(EnemyType.Heavy, 10, 0.9f, 24, 3.2f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Quick, 4, 7f, 4, 3.2f), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.5f), new WaveEnemyGroup(EnemyType.Heavy, 7, 0.9f, 24, 3.2f), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.5f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.HiddenBoss, 2, 1f, 30, 3.2f), new WaveEnemyGroup(EnemyType.Basic, 10, 2f, 4, 2.5f, true)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Quick, 10, 7f, 4, 3.2f), new WaveEnemyGroup(EnemyType.Abnormal, 6, 6f, 8), new WaveEnemyGroup(EnemyType.HiddenBoss, 3, 1f, 30, 3f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.5f), new WaveEnemyGroup(EnemyType.Tank, 6, 1f, 18), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.5f), new WaveEnemyGroup(EnemyType.Heavy, 8, 0.9f, 24, 3.2f), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.5f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Abnormal, 4, 6f, 8), new WaveEnemyGroup(EnemyType.Breaker2, 6, 1f, 20), new WaveEnemyGroup(EnemyType.HiddenBoss, 2, 1f, 30, 3.2f), new WaveEnemyGroup(EnemyType.Tank, 6, 1f, 18), new WaveEnemyGroup(EnemyType.Abnormal, 4, 6f, 8)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Abnormal, 5, 6f, 8, 2.8f, true), new WaveEnemyGroup(EnemyType.Breaker2, 8, 1f, 20), new WaveEnemyGroup(EnemyType.Tank, 6, 1f, 18), new WaveEnemyGroup(EnemyType.HiddenBoss, 2, 1f, 30, 3.2f), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.5f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Abnormal, 4, 6f, 8), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1f, 30, 3.2f), new WaveEnemyGroup(EnemyType.Tank, 5, 1f, 18), new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11), new WaveEnemyGroup(EnemyType.Breaker2, 6, 1f, 20)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Heavy, 20, 3.5f, 24, 3.3f), new WaveEnemyGroup(EnemyType.Tank, 10, 1f, 18), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 0.9f, 36, 3.5f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1.2f, 30, 3.2f, true), new WaveEnemyGroup(EnemyType.Breaker2, 8, 1f, 20), new WaveEnemyGroup(EnemyType.Tank, 10, 1f, 18), new WaveEnemyGroup(EnemyType.Basic, 10, 4.5f, 4, 3f, true), new WaveEnemyGroup(EnemyType.FallenHazmat, 5, 5f, 8), new WaveEnemyGroup(EnemyType.FallenNecromancer, 2, 5.5f, 11)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 7f, 25, 3f), new WaveEnemyGroup(EnemyType.Abnormal, 2, 4.5f, 10, 3.2f), new WaveEnemyGroup(EnemyType.Tank, 10, 1f, 20, 3.2f), new WaveEnemyGroup(EnemyType.HiddenBoss, 4, 1f, 30, 3.5f), new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11, 3.2f)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Breaker2, 6, 1f, 20), new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 0.9f, 36, 3.5f), new WaveEnemyGroup(EnemyType.FallenHazmat, 4, 5f, 8), new WaveEnemyGroup(EnemyType.HiddenBoss, 3, 1f, 30, 3f), new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11)),
                           
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
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Abnormal, 4, 2.5f, 6)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Abnormal, 8, 2.5f, 6)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Quick, 3, 6f, 5),
                                new WaveEnemyGroup(EnemyType.Abnormal, 5, 2.5f, 6)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Abnormal, 3, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Quick, 5, 6f, 5),
                                new WaveEnemyGroup(EnemyType.Abnormal, 3, 2.5f, 6)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Quick, 12, 6f, 5)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Heavy, 4, 1.2f, 14)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Heavy, 3, 1.2f, 14),
                                new WaveEnemyGroup(EnemyType.Quick, 5, 6f, 5),
                                new WaveEnemyGroup(EnemyType.Heavy, 3, 1.2f, 14),
                                new WaveEnemyGroup(EnemyType.Quick, 5, 6f, 5)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 1, 2.2f, 16),
                                new WaveEnemyGroup(EnemyType.Abnormal, 5, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 1, 2.2f, 16),
                                new WaveEnemyGroup(EnemyType.Abnormal, 7, 2.5f, 6)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Heavy, 9, 1.2f, 14)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Molten, 3, 2.2f, 12),
                                new WaveEnemyGroup(EnemyType.Heavy, 5, 1.2f, 14),
                                new WaveEnemyGroup(EnemyType.Abnormal, 6, 2.5f, 8),
                                new WaveEnemyGroup(EnemyType.Quick, 6, 6.5f, 5)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Heavy, 5, 1.2f, 14),
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 1, 2.2f, 16),
                                new WaveEnemyGroup(EnemyType.Abnormal, 9, 2.5f, 6)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Heavy, 6, 1.2f, 14),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 2, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 1, 2.2f, 16),
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 1, 2.2f, 18)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 1, 2.2f, 16),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 2, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 1, 2.2f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 4, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.Abnormal, 6, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Heavy, 5, 1.2f, 14)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Breaker3, 3, 1f, 18)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Molten, 5, 2.2f, 12),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 3, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.EliteAbnormal, 2, 2.2f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Molten, 4, 2.2f, 12),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 2, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.Molten, 3, 2.2f, 12),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 2, 5.2f, 8)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 1, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1.1f, 24, 2.5f, true),
                                new WaveEnemyGroup(EnemyType.Breaker3, 5, 1f, 18),
                                new WaveEnemyGroup(EnemyType.HiddenBoss, 1, 1.1f, 24, 2.5f, true),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 3, 5.2f, 8)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Molten, 8, 2.2f, 12),
                                new WaveEnemyGroup(EnemyType.MoltenNecromancer, 1, 5.5f, 9, 2.5f, true),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 4, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.MoltenNecromancer, 1, 5.5f, 9, 2.5f, true),
                                new WaveEnemyGroup(EnemyType.Heavy, 6, 1.2f, 14)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.MoltenGolem, 1, 0.9f, 24),
                                new WaveEnemyGroup(EnemyType.EliteHazmat, 5, 5f, 12),
                                new WaveEnemyGroup(EnemyType.EliteBoomer, 1, 0.9f, 18),
                                new WaveEnemyGroup(EnemyType.Breaker3, 5, 1f, 18)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.MoltenHound, 4, 7f, 6),
                                new WaveEnemyGroup(EnemyType.EliteHazmat, 5, 5f, 12),
                                new WaveEnemyGroup(EnemyType.MoltenDemon, 3, 5.2f, 8),
                                new WaveEnemyGroup(EnemyType.MoltenNecromancer, 1, 5.5f, 9)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Molten, 6, 2.2f, 12),
                                new WaveEnemyGroup(EnemyType.EliteHazmat, 5, 5f, 12),
                                new WaveEnemyGroup(EnemyType.MoltenHound, 8, 7f, 6),
                                new WaveEnemyGroup(EnemyType.MoltenNecromancer, 1, 5.5f, 9),
                                new WaveEnemyGroup(EnemyType.Fallen, 0, 0f, 0))
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
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Abnormal, 5, 2.5f, 6)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.Abnormal, 8, 2.5f, 6)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Quick, 1, 6f, 5),
                                new WaveEnemyGroup(EnemyType.Abnormal, 2, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Quick, 1, 6f, 5),
                                new WaveEnemyGroup(EnemyType.Abnormal, 2, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Quick, 1, 6f, 5),
                                new WaveEnemyGroup(EnemyType.Abnormal, 2, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Quick, 1, 6f, 5)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Quick, 8, 6f, 5),
                                new WaveEnemyGroup(EnemyType.Abnormal, 8, 2.5f, 6)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 3, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.Abnormal, 5, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Quick, 5, 6f, 5)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.FallenSkeleton, 5, 2.2f, 10)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12),
                                new WaveEnemyGroup(EnemyType.Abnormal, 9, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Quick, 3, 6.5f, 5)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 2, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 2, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.Abnormal, 3, 2.5f, 6),
                                new WaveEnemyGroup(EnemyType.Abnormal, 3, 2.2f, 8),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 2, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 2, 2f, 12)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12),
                                new WaveEnemyGroup(EnemyType.Breaker2, 1, 1f, 20),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 2, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12),
                                new WaveEnemyGroup(EnemyType.Breaker2, 1, 1f, 20),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 2, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 3, 2f, 12),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 3, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.Breaker2, 3, 1f, 20)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.FallenSoul, 11, 5.8f, 4)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 5, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 2, 2f, 12),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 1, 2f, 12),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Breaker2, 6, 1f, 20),
                                new WaveEnemyGroup(EnemyType.FallenSoul, 6, 5.8f, 4),
                                new WaveEnemyGroup(EnemyType.Fallen, 3, 1.2f, 18)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 8, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.Breaker2, 4, 1f, 20),
                                new WaveEnemyGroup(EnemyType.Fallen, 1, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenDreg, 5, 2f, 12),
                                new WaveEnemyGroup(EnemyType.FallenSoul, 4, 5.8f, 4),
                                new WaveEnemyGroup(EnemyType.Fallen, 3, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 2, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 1, 0.8f, 28)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 3, 5f, 8),
                                new WaveEnemyGroup(EnemyType.Fallen, 1, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 3, 5f, 8),
                                new WaveEnemyGroup(EnemyType.Fallen, 1, 1.2f, 18)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.FallenGiant, 1, 0.8f, 28), new WaveEnemyGroup(EnemyType.FallenHazmat, 7, 5f, 8)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSquire, 3, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenDreg, 10, 2f, 12),
                                new WaveEnemyGroup(EnemyType.Fallen, 4, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenSoul, 8, 5.8f, 4),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 1, 1f, 18)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.Fallen, 6, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 5, 5f, 8),
                                new WaveEnemyGroup(EnemyType.FallenSkeleton, 8, 2.2f, 10),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 3, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 1, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.Breaker2, 5, 1f, 20),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 3, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenSoul, 8, 5.8f, 4),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 2, 1f, 18),
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 1, 1.5f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 3, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 2, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenSeraph, 5, 6f, 9)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenSeraph, 5, 6f, 9),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.FallenSquire, 1, 1.6f, 16),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 5, 5f, 8)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenSoul, 7, 5.8f, 4),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 1, 1f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 4, 5f, 8),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 1, 1f, 18),
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 2, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 1, 1f, 18)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 1, 1f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 6, 5f, 8),
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 3, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 3, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenJester, 1, 1f, 20)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 1, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 2, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 1, 0.8f, 30),
                                new WaveEnemyGroup(EnemyType.Breaker4, 8, 0.9f, 26)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenGiant, 1, 0.8f, 30),
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 3, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 3, 1f, 18),
                                new WaveEnemyGroup(EnemyType.FallenSeraph, 8, 6f, 9),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.FallenRusher, 7, 7f, 4)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenRusher, 1, 7f, 4),
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 4, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.Breaker4, 10, 0.9f, 26),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 8, 5f, 8),
                                new WaveEnemyGroup(EnemyType.FallenHero, 1, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenSeraph, 9, 6f, 9)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenRusher, 3, 7f, 4),
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 5, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 4, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.FallenShield, 1, 1.2f, 22),
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 5, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenSummoner, 1, 1f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenHero, 2, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 1, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenHero, 2, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 1, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenHero, 2, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 1, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 7, 5f, 8),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 3, 1f, 18),
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 4, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 2, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.FallenSummoner, 1, 1f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenShield, 1, 1.2f, 22),
                                new WaveEnemyGroup(EnemyType.FallenRusher, 5, 7f, 4),
                                new WaveEnemyGroup(EnemyType.Breaker4, 6, 0.9f, 26),
                                new WaveEnemyGroup(EnemyType.FallenJester, 2, 1f, 20),
                                new WaveEnemyGroup(EnemyType.FallenHero, 4, 1.4f, 20)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 1, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 7, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 3, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 3, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.FallenSeraph, 18, 6f, 9),
                                new WaveEnemyGroup(EnemyType.FallenAngel, 1, 1.5f, 24)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenShield, 1, 1.2f, 22),
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 1, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHero, 2, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenHero, 1, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenRusher, 3, 7f, 4),
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 1, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHero, 2, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenHero, 1, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenRusher, 3, 7f, 4),
                                new WaveEnemyGroup(EnemyType.FallenSummoner, 1, 1f, 16)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 1, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.Breaker4, 9, 0.9f, 26),
                                new WaveEnemyGroup(EnemyType.FallenHero, 5, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 3, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 2, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.FallenHonorGuard, 1, 1f, 28)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.FallenGiant, 4, 0.8f, 28), new WaveEnemyGroup(EnemyType.CorruptedFallen, 8, 1.5f, 16), new WaveEnemyGroup(EnemyType.FallenHero, 10, 1.4f, 20), new WaveEnemyGroup(EnemyType.FallenTank, 1, 0.7f, 32)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.CorruptedFallen, 10, 1.5f, 16),
                                new WaveEnemyGroup(EnemyType.FallenHero, 4, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 5, 1f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHero, 4, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 5, 1f, 18),
                                new WaveEnemyGroup(EnemyType.FallenHero, 4, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenNecromancer, 1, 5.5f, 11),
                                new WaveEnemyGroup(EnemyType.PossessedArmor, 5, 1f, 18),
                                new WaveEnemyGroup(EnemyType.FallenGiant, 3, 0.8f, 28),
                                new WaveEnemyGroup(EnemyType.FallenAngel, 2, 1.5f, 24),
                                new WaveEnemyGroup(EnemyType.FallenAngel, 1, 1.5f, 24)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenShield, 1, 1.2f, 22),
                                new WaveEnemyGroup(EnemyType.FallenRusher, 5, 7f, 4),
                                new WaveEnemyGroup(EnemyType.Breaker4, 6, 0.9f, 26),
                                new WaveEnemyGroup(EnemyType.FallenJester, 2, 1f, 20),
                                new WaveEnemyGroup(EnemyType.FallenHero, 4, 1.4f, 20)),
                            new WaveDefinition(
                                new WaveEnemyGroup(EnemyType.FallenShield, 1, 1.2f, 22),
                                new WaveEnemyGroup(EnemyType.FallenRusher, 4, 7f, 4),
                                new WaveEnemyGroup(EnemyType.NecroticSkeleton, 3, 1.2f, 18),
                                new WaveEnemyGroup(EnemyType.Breaker4, 10, 0.9f, 26),
                                new WaveEnemyGroup(EnemyType.FallenHazmat, 8, 5f, 8),
                                new WaveEnemyGroup(EnemyType.FallenHero, 1, 1.4f, 20),
                                new WaveEnemyGroup(EnemyType.FallenGuardian, 1, 1.3f, 26),
                                new WaveEnemyGroup(EnemyType.FallenSummoner, 1, 1f, 16)),
                            new WaveDefinition(new WaveEnemyGroup(EnemyType.FallenShield, 1, 1.2f, 22), new WaveEnemyGroup(EnemyType.FallenRusher, 4, 7f, 4), new WaveEnemyGroup(EnemyType.NecroticSkeleton, 3, 1.2f, 18), new WaveEnemyGroup(EnemyType.FallenRusher, 4, 7f, 4), new WaveEnemyGroup(EnemyType.FallenTank, 1, 0.7f, 32), new WaveEnemyGroup(EnemyType.PossessedArmor, 6, 1f, 18), new WaveEnemyGroup(EnemyType.FallenHero, 5, 1.4f, 20), new WaveEnemyGroup(EnemyType.FallenGiant, 3, 0.8f, 28), new WaveEnemyGroup(EnemyType.FallenAngel, 3, 1.5f, 24), new WaveEnemyGroup(EnemyType.FallenSummoner, 1, 1f, 16), new WaveEnemyGroup(EnemyType.FallenGuardian, 2, 1.3f, 26), new WaveEnemyGroup(EnemyType.FallenKing, 1, 0.9f, 35), new WaveEnemyGroup(EnemyType.FallenSummoner, 1, 1f, 16))
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
            helpPopupOpen = false;
            pauseMenuOpen = false;
            victoryPopupOpen = false;
            victoryRewardGiven = false;
            _lastFarmPayoutWave = 0;
            waveManager = new WaveManager(currentGameMode.Path, enemyTextures, currentGameMode.Waves, currentGameMode.SpawnInterval);
            PlayMusicForMode(gameScreen);
            _bossMusicStarted = false;
            wave1Started = false;
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
            helpPopupOpen = false;
            pauseMenuOpen = false;
            victoryPopupOpen = false;
            _lastFarmPayoutWave = 0;
            PlayMusicForMode(Screen.Title);
        }

        private void PlayMusicForMode(Screen gameScreen)
        {
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.IsRepeating = true;

            if (gameScreen == Screen.Easy)
            {
                MediaPlayer.Play(_easyModeSong);
            }
            else if (gameScreen == Screen.Normal)
            {
                MediaPlayer.Play(_moltenSong);
            }
            else if (gameScreen == Screen.Hard)
            {
                MediaPlayer.Play(_fallenSong);
            }
            else
            {
                MediaPlayer.Stop();
            }

            _bossMusicStarted = false;
        }

        private void PlayBossThemeForMode()
        {
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.IsRepeating = true;

            if (screen == Screen.Normal)
            {
                MediaPlayer.Play(_moltenBossSong);
            }
            else if (screen == Screen.Hard)
            {
                MediaPlayer.Play(_fallenKingSong);
            }
        }

        private SoundEffect GetTowerShootSound(TowerType towerType)
        {
            return towerType == TowerType.Freezer ? _freezerTowerShootSound : _basicTowerShootSound;
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

        public class TowerData
        {
            public Texture2D Texture;
            public int Cost;
            public int UnlockCost;
            public int Damage;
            public float Range;
            public float FireRate;
            public int FarmIncomePerWave;
        }
        Dictionary<TowerType, TowerData> towerStats;
        private Texture2D GetTowerTexture(TowerType type)
        {
            return towerStats[type].Texture;
        }

        private int GetTowerCost(TowerType type)
        {
            return towerStats[type].Cost;
        }

        private int GetTowerUnlockCost(TowerType type)
        {
            return towerStats[type].UnlockCost;
        }

        private int GetTowerDamage(TowerType type)
        {
            return towerStats[type].Damage;
        }

        private float GetTowerRange(TowerType type)
        {
            return towerStats[type].Range;
        }

        private float GetTowerFireRate(TowerType type)
        {
            return towerStats[type].FireRate;
        }

        private int GetTowerFarmIncomePerWave(TowerType type)
        {
            return towerStats[type].FarmIncomePerWave;
        }

        private string GetTowerDisplayName(TowerType type)
        {
            return type switch
            {
                TowerType.Basic => "Basic",
                TowerType.Sniper => "Sniper",
                TowerType.Minigunner => "Minigunner",
                TowerType.DJ => "DJ",
                TowerType.Freezer => "Freezer",
                TowerType.Farm => "Farm",
                TowerType.Commander => "Commander",
                TowerType.Accel => "Accel",
                TowerType.Soldier => "Soldier",
                _ => "Unknown"
            };
        }

        private int CalculateFarmWaveIncome()
        {
            int total = 0;
            foreach (Tower tower in activeTowers)
            {
                if (tower.type == TowerType.Farm)
                {
                    total += tower.FarmIncomePerWave;
                }
            }
            return total;
        }

        private Vector2 GetGameTowerPricePosition(TowerType towerType)
        {
            if (towerType == TowerType.Basic) return new Vector2(830, 285);
            if (towerType == TowerType.Sniper) return new Vector2(830, 485);
            if (towerType == TowerType.Minigunner) return new Vector2(830, 685);
            return Vector2.Zero;
        }
        private Texture2D TextureOrDefault(Texture2D lvl0, Texture2D lvl1, Texture2D lvl2, Texture2D lvl3, int level)
        {
            if (level <= 0) return lvl0 ?? lvl1 ?? lvl2 ?? lvl3;
            if (level == 1) return lvl1 ?? lvl0 ?? lvl2 ?? lvl3;
            if (level == 2) return lvl2 ?? lvl1 ?? lvl0 ?? lvl3;
            return lvl3 ?? lvl2 ?? lvl1 ?? lvl0;
        }

        private Texture2D GetUpgradePreview(TowerType type, int level)
        {
            if (_upgradePreviews != null && _upgradePreviews.ContainsKey(type))
            {
                Texture2D[] arr = _upgradePreviews[type];
                if (arr.Length == 0) return null;
                int idx = MathHelper.Clamp(level, 0, arr.Length - 1);
                return arr[idx] ?? arr[0];
            }

            return null;
        }
        private void CreateHotbarButtons()
        {
            _gameTowerButtons.Clear();

            for (int i = 0; i < _equippedTowers.Count; i++)
            {
                TowerType towerType = _equippedTowers[i];

                _gameTowerButtons[towerType] = new Button(
                    GetTowerTexture(towerType),
                    _hotbarSlots[i],
                    3f,
                    3.13f);
            }
        }
        private void ToggleTowerLoadout(TowerType towerType)
        {
            // Unlock if not owned
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

            // Already equipped? Unequip it.
            if (_equippedTowers.Contains(towerType))
            {
                _equippedTowers.Remove(towerType);

                if (_selectedTower == towerType)
                    _selectedTower = TowerType.None;
                CreateHotbarButtons();
                return;
            }

            // Equip if space available
            if (_equippedTowers.Count < MaxEquippedTowers)
            {
                _equippedTowers.Add(towerType);

                CreateHotbarButtons();
            }
            
        }
        private bool MouseIsOverTowerButton(Point mousePos)
        {
            foreach (TowerType towerType in _equippedTowers)
            {
                _gameTowerButtons[towerType].Update(mouseState, prevMouseState);
            }

            return false;
        }
        private readonly List<Vector2> _hotbarSlots = new List<Vector2>()
            {
                new Vector2(250, 745),
                new Vector2(350, 745),
                new Vector2(450, 745),
                new Vector2(550, 745),
                new Vector2(650, 745)
            };
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

        private void DrawCenteredString(SpriteFont font, string text, Rectangle area, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 position = new Vector2(
                area.Center.X - size.X / 2,
                area.Center.Y - size.Y / 2);
            _spriteBatch.DrawString(font, text, position, color);
        }

        private void DrawModeInfo(Screen mode, Vector2 centerPosition)
        {
            if (!gameModes.TryGetValue(mode, out GameModeConfig modeConfig))
                return;

            string nameText = modeConfig.Name;
            string waveText = $"{modeConfig.Waves.Count} Waves";

            Vector2 nameSize = _font.MeasureString(nameText);
            Vector2 waveSize = _font.MeasureString(waveText);

            _spriteBatch.DrawString(_font, nameText, new Vector2(centerPosition.X - nameSize.X / 2, centerPosition.Y), Color.White);
            _spriteBatch.DrawString(_font, waveText, new Vector2(centerPosition.X - waveSize.X / 2, centerPosition.Y + 28), Color.LightGray);
        }

        private void DrawHelpButton()
        {
            Color buttonColor = helpButtonRec.Contains(mouseState.Position) ? Color.LightGoldenrodYellow : Color.White;
            DrawRectangle(helpButtonRec, Color.Black * 0.75f);
            DrawRectangleOutline(helpButtonRec, buttonColor, 3);
            DrawCenteredString(_font, "?", helpButtonRec, buttonColor);
        }

        private void DrawHelpPopup()
        {
            DrawRectangle(window, Color.Black * 0.45f);
            DrawRectangle(helpPanelRec, Color.Black * 0.9f);
            DrawRectangleOutline(helpPanelRec, Color.White, 3);

            _spriteBatch.DrawString(_font, "Controls", new Vector2(helpPanelRec.X + 35, helpPanelRec.Y + 30), Color.Gold);

            string[] controls =
            {
                "Space: start the first wave",
                "1-5: select towers from your hotbar",
                "Left Click: place, select, and upgrade towers",
                "X: cancel selected tower",
                "Q: sell selected tower",
                "Tab: pause menu"
            };

            for (int i = 0; i < controls.Length; i++)
            {
                _spriteBatch.DrawString(_font, controls[i], new Vector2(helpPanelRec.X + 45, helpPanelRec.Y + 85 + i * 30), Color.White);
            }

            _spriteBatch.DrawString(_font, "How To Play", new Vector2(helpPanelRec.X + 35, helpPanelRec.Y + 285), Color.Gold);

            string[] rundown =
            {
                "Pick a mode, place towers near the path, and survive every wave.",
                "Enemies give gold when defeated. Use gold to buy and upgrade towers.",
                "Support towers boost nearby towers. Farms pay out after waves.",
                "If health hits 0, you lose."
            };

            for (int i = 0; i < rundown.Length; i++)
            {
                _spriteBatch.DrawString(_font, rundown[i], new Vector2(helpPanelRec.X + 45, helpPanelRec.Y + 335 + i * 28), Color.LightGray);
            }

            DrawRectangle(helpCloseRec, Color.DarkRed);
            DrawRectangleOutline(helpCloseRec, Color.White, 2);
            DrawCenteredString(_font, "X", helpCloseRec, Color.White);
        }

        protected override void Update(GameTime gameTime)
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
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
               
                bg = loadoutScreen;                           
                foreach (Button button in _inventoryTowerButtons.Values)
                {
                    button.Update(mouseState, prevMouseState);
                }
                foreach (TowerType towerType in _availableTowerTypes)
                {
                    if (_inventoryTowerButtons[towerType].IsClicked)
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
                if (keyboardState.IsKeyDown(Keys.Tab) && previousKeyboardState.IsKeyUp(Keys.Tab))
                {
                    pauseMenuOpen = !pauseMenuOpen;
                }
                if (mouseState.LeftButton == ButtonState.Pressed &&
                    prevMouseState.LeftButton == ButtonState.Released &&
                    helpButtonRec.Contains(mouseState.Position))
                {
                    helpPopupOpen = true;
                    pauseMenuOpen = false;

                    base.Update(gameTime);
                    return;
                }
                if (helpPopupOpen)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (helpCloseRec.Contains(mouseState.Position) || helpButtonRec.Contains(mouseState.Position))
                        {
                            helpPopupOpen = false;
                        }
                    }

                    base.Update(gameTime);
                    return;
                }
                if (pauseMenuOpen)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        if (resumeRec.Contains(mouseState.Position))
                        {
                            pauseMenuOpen = false;
                        }

                        if (mainMenuRec.Contains(mouseState.Position))
                        {
                            ReturnToMainMenu();

                            // Forfeit rewards
                            victoryRewardGiven = true;
                            pauseMenuOpen = false;
                        }
                    }

                    base.Update(gameTime);
                    return;
                }
                if (!wave1Started && keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))
                {
                    wave1Started = true;
                    waveManager.StartNextWave();
                }

                if (wave1Started && waveManager.IsWaveActive && waveManager.WaveNumber == currentGameMode.Waves.Count && !_bossMusicStarted)
                {
                    PlayBossThemeForMode();
                    _bossMusicStarted = true;
                }

                if (wave1Started && waveManager.IsWaveActive && waveManager.WaveNumber < currentGameMode.Waves.Count && _bossMusicStarted)
                {
                    _bossMusicStarted = false;
                }

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
                    _gameTowerButtons[towerType].Update(mouseState, prevMouseState);
                }
                if (keyboardState.IsKeyDown(Keys.D1) &&
                    previousKeyboardState.IsKeyUp(Keys.D1) &&
                    _equippedTowers.Count > 0)
                {
                    _selectedTower = _equippedTowers[0];
                }

                if (keyboardState.IsKeyDown(Keys.D2) &&
                    previousKeyboardState.IsKeyUp(Keys.D2) &&
                    _equippedTowers.Count > 1)
                {
                    _selectedTower = _equippedTowers[1];
                }

                if (keyboardState.IsKeyDown(Keys.D3) &&
                    previousKeyboardState.IsKeyUp(Keys.D3) &&
                    _equippedTowers.Count > 2)
                {
                    _selectedTower = _equippedTowers[2];
                }

                if (keyboardState.IsKeyDown(Keys.D4) &&
                    previousKeyboardState.IsKeyUp(Keys.D4) &&
                    _equippedTowers.Count > 3)
                {
                    _selectedTower = _equippedTowers[3];
                }

                if (keyboardState.IsKeyDown(Keys.D5) &&
                    previousKeyboardState.IsKeyUp(Keys.D5) &&
                    _equippedTowers.Count > 4)
                {
                    _selectedTower = _equippedTowers[4];
                }
                foreach (TowerType towerType in _equippedTowers)
                {
                    if (_gameTowerButtons[towerType].IsClicked)
                    {
                        _selectedTower = towerType;
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Q) &&
                    previousKeyboardState.IsKeyUp(Keys.Q) &&
                     _focusedTower != null)
                {
                    Gamedata.gold += _focusedTower.TotalCost / 2;
                    Gamedata.gold += _focusedTower.TowerCost / 2;

                    activeTowers.Remove(_focusedTower);
                    _focusedTower = null;
                }
                if (keyboardState.IsKeyDown(Keys.X) &&
                   previousKeyboardState.IsKeyUp(Keys.X) &&
                    _selectedTower != TowerType.None)
                {
                    _selectedTower = TowerType.None;

                }
                if (keyboardState.IsKeyDown(Keys.Tab) &&
                   previousKeyboardState.IsKeyUp(Keys.Tab))
                {
                    

                }

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
                            if (rect.Contains(mousePos))
                            {
                                canPlace = false;
                                break;
                            }
                        }

                        // Hotbar collision check
                        if (hotbarRec.Contains(mousePos))
                        {
                            canPlace = false;
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
                                        GetTowerCost(_selectedTower),
                                        GetTowerFarmIncomePerWave(_selectedTower),
                                        GetTowerShootSound(_selectedTower)));
                                    Gamedata.gold -= GetTowerCost(_selectedTower);
                                    _selectedTower = TowerType.None;
                                }
                                
                            }
                        }
                    }
                }
                foreach (Tower tower in activeTowers)
                {
                    tower.RangeMultiplier = 1f;
                    tower.FireRateMultiplier = 1f;
                    tower.UpgradeDiscount = 0f;
                }
                foreach (Tower tower in activeTowers)
                {
                    if (tower.type == TowerType.DJ)
                    {
                        foreach (Tower other in activeTowers)
                        {
                            if (other == tower || other.type == TowerType.DJ)
                                continue;

                            float dist = Vector2.Distance(
                                tower.Position,
                                other.Position);

                            if (dist <= tower.Range)
                            {
                                other.RangeMultiplier += tower.DJRangeBuff;
                                other.UpgradeDiscount += tower.DJDiscountBuff;
                            }
                        }
                    }
                    if (tower.type == TowerType.Commander)
                    {
                        foreach (Tower other in activeTowers)
                        {
                            if (other == tower || other.type == TowerType.Commander)
                                continue;

                            float dist = Vector2.Distance(
                                tower.Position,
                                other.Position);

                            if (dist <= tower.Range)
                            {
                                other.FireRateMultiplier *= tower.CommanderFireRateBuff;
                            }
                        }
                    }
                }

                waveManager.Update(gameTime, activeEnemies);
                // Farm payout on wave completion
                if (wave1Started && !waveManager.IsWaveActive && activeEnemies.Count == 0 && waveManager.WaveNumber > _lastFarmPayoutWave)
                {
                    int farmIncome = CalculateFarmWaveIncome();
                    if (farmIncome > 0)
                    {
                        Gamedata.gold += farmIncome;
                    }
                    _lastFarmPayoutWave = waveManager.WaveNumber;
                }

                // Wave finished and enemies cleared
                if (wave1Started && !waveManager.IsWaveActive && activeEnemies.Count == 0 && waveManager.HasMoreWaves)
                {
                    if (!waitingForNextWave)
                    {
                        waitingForNextWave = true;
                        nextWaveTimer = 5f; // 5 seconds
                    }
                }

                // Count down until next wave
                if (waitingForNextWave)
                {
                    nextWaveTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (nextWaveTimer <= 0f)
                    {
                        waitingForNextWave = false;
                        waveManager.StartNextWave();
                    }
                }




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
                DrawModeInfo(Screen.Easy, new Vector2(200, 430));
                DrawModeInfo(Screen.Normal, new Vector2(500, 430));
                DrawModeInfo(Screen.Hard, new Vector2(800, 430));
                _spriteBatch.Draw(inventory, inventoryRec, Color.White);
                _spriteBatch.DrawString(_font, $"Coins: {Gamedata.coins}", new Vector2(10, 105), Color.White, 0f, Vector2.Zero, 2, SpriteEffects.None, 1f);




            }
            else if (screen == Screen.TowerPick)
            {
                _spriteBatch.Draw(bg, window, Color.White);
                _spriteBatch.DrawString(_font, $"Pick up to {MaxEquippedTowers}: {_equippedTowers.Count}/{MaxEquippedTowers}", new Vector2(310, 135), Color.White);
                _spriteBatch.DrawString(_font, $"Coins: {Gamedata.coins}", new Vector2(10, 105), Color.White, 0f, Vector2.Zero, 2, SpriteEffects.None, 1f);

                for (int i = 0; i < _equippedTowers.Count; i++)
                {
                    _spriteBatch.DrawString(
                        _font,
                        $"{i + 1}. {_equippedTowers[i]}",
                        new Vector2(10, 150 + i * 22),
                        Color.LimeGreen);
                }

                foreach (TowerType towerType in _availableTowerTypes)
                {
                    Button button = _inventoryTowerButtons[towerType];
                    if (button != null)
                    {
                        button.Draw(_spriteBatch);

                        Rectangle selectedRect = button.Hitbox;
                        selectedRect.Inflate(12, 12);
                        Color outlineColor = IsTowerEquipped(towerType) ? Color.LimeGreen : Color.DarkGray;
                        DrawRectangleOutline(selectedRect, outlineColor, 4);

                        string towerName = GetTowerDisplayName(towerType);
                        Vector2 nameSize = _font.MeasureString(towerName);
                        Vector2 namePos = new Vector2(selectedRect.Center.X - nameSize.X / 2, selectedRect.Top - nameSize.Y - 8);
                        _spriteBatch.DrawString(_font, towerName, namePos, Color.White);

                        Vector2 tooltipPos = new Vector2(selectedRect.Center.X - nameSize.X / 2, selectedRect.Bottom + 8);
                        

                        Vector2 labelPos = new Vector2(selectedRect.Center.X, selectedRect.Bottom + 26);

                        if (!IsTowerOwned(towerType))
                        {
                            int unlockCost = GetTowerUnlockCost(towerType);

                            Vector2 statusSize = _font.MeasureString($"LOCKED ({unlockCost} Coins)");
                            _spriteBatch.DrawString(
                                _font,
                                $"LOCKED ({unlockCost} Coins)",
                                new Vector2(labelPos.X - statusSize.X / 2, labelPos.Y),
                                Color.Red);
                        }
                        else if (IsTowerEquipped(towerType))
                        {
                            Vector2 equippedSize = _font.MeasureString("EQUIPPED");
                            _spriteBatch.DrawString(
                                _font,
                                "EQUIPPED",
                                new Vector2(labelPos.X - equippedSize.X / 2, labelPos.Y),
                                Color.LimeGreen);
                        }
                        else
                        {
                            string statusText = _equippedTowers.Count >= MaxEquippedTowers ? "OWNED (FULL)" : "OWNED";
                            Vector2 statusSize = _font.MeasureString(statusText);
                            _spriteBatch.DrawString(
                                _font,
                                statusText,
                                new Vector2(labelPos.X - statusSize.X / 2, labelPos.Y),
                                _equippedTowers.Count >= MaxEquippedTowers ? Color.Yellow : Color.White);
                        }

                    }
                }

                _spriteBatch.DrawString(_font, "Done", new Vector2(815, 685), Color.White);

            }
            else if (IsGameplayScreen(screen))
            {
                _spriteBatch.Draw(bg, window, Color.White);
                _spriteBatch.Draw(hotbar, hotbarRec, Color.White);
                _spriteBatch.DrawString(_font, $"Mode: {currentGameMode.Name}", new Vector2(10, 135), Color.White);
                _spriteBatch.DrawString(_font, $"Wave: {waveManager.WaveNumber}/{currentGameMode.Waves.Count}", new Vector2(10, 160), Color.White);
                _spriteBatch.DrawString(_font, $"Health: {baseHealth}", new Vector2(10, 10), Color.Red);
                _spriteBatch.DrawString(_font, $"Gold: {Gamedata.gold}", new Vector2(10, 35), Color.Gold);
                _spriteBatch.DrawString(_font, $"Selected: {_selectedTower}", new Vector2(10, 60), Color.White);
                _spriteBatch.DrawString(_font, $"Enemies: {activeEnemies.Count}", new Vector2(10, 85), Color.White);
                _spriteBatch.DrawString(_font, $"Towers: {activeTowers.Count}", new Vector2(10, 110), Color.White);
                
                foreach (TowerType towerType in _equippedTowers)
                {
                    Button button = _gameTowerButtons[towerType];

                    if (button != null)
                    {
                        button.Draw(_spriteBatch);

                        string costText = $"${GetTowerCost(towerType)}";
                        Vector2 textSize = _font.MeasureString(costText);

                        Vector2 pricePos = new Vector2(
                            button.Hitbox.Center.X - textSize.X / 2,
                            button.Hitbox.Top - 25
                        );

                        _spriteBatch.DrawString(
                            _font,
                            costText,
                            pricePos,
                            Color.GhostWhite);
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
                        if (hotbarRec.Contains(mouseState.Position))
                        {
                            invalidSpot = true;
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


                        
                        if (invalidSpot)
                        {
                            _spriteBatch.DrawString(gameFont, "CANNOT PLACE HERE", new Vector2(mouseState.X, mouseState.Y - 40), Color.Orange);
                        }
                        else if (!canAfford)
                        {
                            Vector2 textPos = new Vector2(mouseState.X, mouseState.Y - 30);
                            _spriteBatch.DrawString(gameFont, "NOT ENOUGH CASH", textPos, Color.Red);
                        }
                }
                if (_focusedTower != null)
                {
                    float rangeScale = _focusedTower.Range / 100f; // Scale based on the tower's unique range
                    float effectiveFireRate = _focusedTower.FireRate * _focusedTower.FireRateMultiplier;
                    float effectiveRange = _focusedTower.Range * _focusedTower.RangeMultiplier;
                    float effectiveStatRange = effectiveRange / 10;
                    
                    upgradeRec = new Rectangle(669, 535, 267, 67);
                    upgradeIconRec = new Rectangle(616, 165, 200, 200);
                    Vector2 origin = new Vector2(rangeCircle.Width / 2f, rangeCircle.Height / 2f);

                    // pick upgrade preview image based on tower type and level
                    upgradeIcon = GetUpgradePreview(_focusedTower.type, _focusedTower.Level);
                    
                    _spriteBatch.Draw(HUD, hudRec, Color.White);
                    _spriteBatch.Draw(rangeCircle, _focusedTower.Position, null, Color.Yellow * 0.4f, 0f, origin, rangeScale, SpriteEffects.None, 0f);

                    // Draw header with level
                    _spriteBatch.DrawString(_font, $"Level: {_focusedTower.Level}", new Vector2(670, 150), Color.White);

                    // Next upgrade preview & description
                    string nextDesc = _focusedTower.GetNextUpgradeDescription();
                    int nextBaseCost = _focusedTower.GetNextUpgradeBaseCost();
                    int nextCost = nextBaseCost == 0 ? 0 : _focusedTower.GetDiscountedUpgradeCost(nextBaseCost);
                    int discountPercent = (int)System.Math.Round(_focusedTower.UpgradeDiscount * 100f);

                    // Draw preview image
                    _spriteBatch.Draw(upgradeIcon, upgradeIconRec, Color.White);

                    // Draw description box
                    Rectangle descBox = new Rectangle(616, 370, 320, 140);
                    DrawRectangle(descBox, Color.Black * 0.8f);
                    _spriteBatch.DrawString(_font, "Next Upgrade:", new Vector2(626, 380), Color.White);
                    _spriteBatch.DrawString(_font, nextDesc, new Vector2(626, 405), Color.LightGray);
                    _spriteBatch.DrawString(_font, $"Cost: ${nextCost}", new Vector2(626, 440), Color.Gold);
                    _spriteBatch.DrawString(_font, $"Discount: {discountPercent}%", new Vector2(626, 465), Color.LightGreen);

                    // Draw effective stats with multiplier info
                    Color rangeColor = _focusedTower.RangeMultiplier > 1f ? Color.LimeGreen : Color.White;
                    Color speedColor = _focusedTower.FireRateMultiplier < 1f ? Color.LimeGreen : Color.White;
                    
                    _spriteBatch.DrawString(_font, $"Rng: {effectiveStatRange:F1}", new Vector2(900, 222), rangeColor);
                    _spriteBatch.DrawString(_font, $"Dmg: {_focusedTower.Damage}", new Vector2(900, 185), Color.White);
                    _spriteBatch.DrawString(_font, $"Spd: {effectiveFireRate:F2}", new Vector2(900, 265), speedColor);

                    // Show buff multipliers if active
                    if (_focusedTower.RangeMultiplier > 1f || _focusedTower.FireRateMultiplier < 1f)
                    {
                        _spriteBatch.DrawString(_font, $"Range x{_focusedTower.RangeMultiplier:F2}", new Vector2(900, 300), Color.LimeGreen);
                        _spriteBatch.DrawString(_font, $"Speed x{_focusedTower.FireRateMultiplier:F2}", new Vector2(900, 325), Color.LimeGreen);
                    }

                    // Draw upgrade button state
                    bool max = _focusedTower.IsMaxLevel();
                    bool affordable = nextCost > 0 && Gamedata.gold >= nextCost;

                    Color btnColor = Color.White;
                    string btnText = "Upgrade";
                    if (max)
                    {
                        btnColor = Color.Gray;
                        btnText = "MAX LEVEL";
                    }
                    else if (!affordable)
                    {
                        btnColor = Color.Gray;
                        btnText = "INSUFFICIENT FUNDS";
                    }

                    _spriteBatch.Draw(upgradeButton, upgradeRec, btnColor);
                    Vector2 btnTextSize = _font.MeasureString(btnText);
                    _spriteBatch.DrawString(_font, btnText, new Vector2(upgradeRec.Center.X - btnTextSize.X/2, upgradeRec.Center.Y - btnTextSize.Y/2), Color.White);
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
                if (waitingForNextWave)
                {
                    _spriteBatch.DrawString(
                        _font,
                        $"Next Wave In: {nextWaveTimer:F1}",
                        new Vector2(10, 190),
                        Color.Yellow);
                }
                DrawHelpButton();
                if (pauseMenuOpen)
                {
                    DrawRectangle(pausePanelRec, Color.Black * 0.8f);

                    DrawRectangle(resumeRec, Color.Green);
                    DrawRectangle(mainMenuRec, Color.Red);

                    _spriteBatch.DrawString(
                        _font,
                        "PAUSED",
                        new Vector2(430, 220),
                        Color.White);

                    _spriteBatch.DrawString(
                        _font,
                        "Resume",
                        new Vector2(450, 300),
                        Color.White);

                    _spriteBatch.DrawString(
                        _font,
                        "Main Menu",
                        new Vector2(430, 400),
                        Color.White);
                }

                if (victoryPopupOpen)
                {
                    DrawRectangle(victoryPanelRec, Color.Black * 0.85f);
                    DrawRectangle(victoryMenuRec, Color.White);

                    string titleText = "VICTORY!";
                    Vector2 titleSize = _font.MeasureString(titleText);
                    _spriteBatch.DrawString(_font, titleText, new Vector2(victoryPanelRec.Center.X - titleSize.X / 2, victoryPanelRec.Y + 40), Color.Gold);

                    string rewardText = $"+{currentGameMode.CoinReward} Coins";
                    Vector2 rewardSize = _font.MeasureString(rewardText);
                    _spriteBatch.DrawString(_font, rewardText, new Vector2(victoryPanelRec.Center.X - rewardSize.X / 2, victoryPanelRec.Y + 90), Color.LightGreen);

                    string continueText = "Main Menu";
                    Vector2 continueSize = _font.MeasureString(continueText);
                    _spriteBatch.DrawString(_font, continueText, new Vector2(victoryMenuRec.Center.X - continueSize.X / 2, victoryMenuRec.Center.Y - continueSize.Y / 2), Color.Black);
                }

                if (helpPopupOpen)
                {
                    DrawHelpPopup();
                }
                
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
