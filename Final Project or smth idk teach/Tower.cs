using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{

    public class Tower
    {
        public TowerType type { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Range { get; set; }
        public bool HiddenDetection { get; set; }
        public int Level { get; set; }
        public int Damage { get; set; }
        public float Scale { get; set; }
        public int Texturelvl { get; set; }
        public int TotalCost { get; set; }
        public int UpgradeCost { get; set; }
        public int TowerCost { get; set; }
        public float BaseRange;
        public float BaseFireRate;

        public float RangeMultiplier = 1f;
        public float FireRateMultiplier = 1f;

        public float UpgradeDiscount = 0f;

        public SoundEffect ShootSound { get; private set; }

        // Support tower buff values (scaled by level)
        public float DJRangeBuff = 0.2f;
        public float DJDiscountBuff = 0.15f;
        public float CommanderFireRateBuff = 0.8f; // Multiplier (lower = faster)

        public int FarmIncomePerWave = 0;

        private float FireTimer;
        public float FireRate;
        public float StatRange;

        private static readonly Dictionary<TowerType, int[]> UpgradeCosts = new Dictionary<TowerType, int[]>
        {
            { TowerType.Basic, new[] { 50, 100, 150, 250, 400 } },
            { TowerType.Sniper, new[] { 125, 250, 400, 700, 1100 } },
            { TowerType.Minigunner, new[] { 200, 350, 600, 950, 1500 } },
            { TowerType.DJ, new[] { 150, 300, 550, 900 } },
            { TowerType.Freezer, new[] { 100, 180, 320, 550, 850 } },
            { TowerType.Commander, new[] { 150, 300, 550, 900 } },
            { TowerType.Farm, new[] { 150, 300, 550, 900, 1400 } },
            { TowerType.Accel, new[] { 1200, 2200, 3500, 5500, 8500 } },
            { TowerType.Soldier, new[] { 75, 150, 300, 550, 850 } }
        };

        // Updated Constructor to accept stats and shooting sound
        public Tower(Texture2D texture, Vector2 position, float desiredWidth, float range, int damage, float fireRate, TowerType type, bool hiddenDetection, int towerCost, int farmIncomePerWave = 0, SoundEffect shootSound = null)
        {
            this.type = type;
            Texture = texture;
            Position = position;
            Range = range;
            Damage = damage;
            FireRate = fireRate;
            Scale = desiredWidth / texture.Width;
            StatRange = range / 10;
            TowerCost = towerCost;
            HiddenDetection = false;
            Level = 0;
            Texturelvl = 0;
            TotalCost = 0;
            BaseRange = range;
            BaseFireRate = fireRate;
            FarmIncomePerWave = farmIncomePerWave;
            ShootSound = shootSound;
        }

        public void Upgrade()
        {
            if (!TryBuyUpgrade())
                return;

            switch (type)
            {
                case TowerType.Basic:
                    if (Level == 1)
                    {
                        Range += 50;
                    }
                    else if (Level == 2)
                    {
                        Damage += 2;
                    }
                    else if (Level == 3)
                    {
                        FireRate = 0.75f;
                        HiddenDetection = true;
                    }
                    else if (Level == 4)
                    {
                        Range += 50;
                        Damage += 2;
                    }
                    else if (Level == 5)
                    {
                        Damage += 4;
                        FireRate = 0.55f;
                    }
                    break;

                case TowerType.Sniper:
                    if (Level == 1)
                    {
                        Range += 100;
                    }
                    else if (Level == 2)
                    {
                        Damage += 35;
                    }
                    else if (Level == 3)
                    {
                        FireRate = 3.5f;
                        HiddenDetection = true;
                    }
                    else if (Level == 4)
                    {
                        Damage = 100;
                        FireRate = 4.4f;
                    }
                    else if (Level == 5)
                    {
                        Range += 150;
                       
                    }
                    break;

                case TowerType.Minigunner:
                    if (Level == 1)
                    {
                        Range += 25;
                    }
                    else if (Level == 2)
                    {
                        Damage += 1;
                    }
                    else if (Level == 3)
                    {
                        FireRate = 0.14f;
                    }
                    else if (Level == 4)
                    {
                        Range += 25;
                        Damage += 2;
                        HiddenDetection = true;
                    }
                    else if (Level == 5)
                    {
                        Damage += 3;
                        FireRate = 0.10f;
                    }
                    break;

                case TowerType.DJ:
                    if (Level == 1)
                    {
                        DJRangeBuff = 0.20f;
                        DJDiscountBuff = 0.10f;
                    }
                    else if (Level == 2)
                    {
                        DJRangeBuff = 0.25f;
                        DJDiscountBuff = 0.15f;
                    }
                    else if (Level == 3)
                    {
                        DJRangeBuff = 0.30f;
                        DJDiscountBuff = 0.20f;
                    }
                    else if (Level == 4)
                    {
                        DJRangeBuff = 0.35f;
                        DJDiscountBuff = 0.25f;
                    }
                    break;

                case TowerType.Freezer:
                    if (Level == 1)
                    {
                        Range += 25;
                    }
                    else if (Level == 2)
                    {
                        Range += 25;
                        FireRate = 1.35f;
                    }
                    else if (Level == 3)
                    {
                        Range += 35;
                        FireRate = 1.20f;
                    }
                    else if (Level == 4)
                    {
                        Range += 35;
                        FireRate = 1.05f;
                    }
                    else if (Level == 5)
                    {
                        Range += 50;
                        FireRate = 0.90f;
                    }
                    break;

                case TowerType.Commander:
                    if (Level == 1)
                    {
                        CommanderFireRateBuff = 0.82f;
                    }
                    else if (Level == 2)
                    {
                        CommanderFireRateBuff = 0.76f;
                    }
                    else if (Level == 3)
                    {
                        CommanderFireRateBuff = 0.70f;
                    }
                    else if (Level == 4)
                    {
                        CommanderFireRateBuff = 0.64f;
                    }
                    break;

                case TowerType.Farm:
                    if (Level == 1)
                    {
                        FarmIncomePerWave += 25;
                    }
                    else if (Level == 2)
                    {
                        FarmIncomePerWave += 45;
                    }
                    else if (Level == 3)
                    {
                        FarmIncomePerWave += 75;
                    }
                    else if (Level == 4)
                    {
                        FarmIncomePerWave += 115;
                    }
                    else if (Level == 5)
                    {
                        FarmIncomePerWave += 170;
                    }
                    break;

                case TowerType.Accel:
                    if (Level == 1)
                    {
                        Range += 40;
                    }
                    else if (Level == 2)
                    {
                        Damage += 25;
                    }
                    else if (Level == 3)
                    {
                        FireRate = 0.38f;
                    }
                    else if (Level == 4)
                    {
                        Range += 50;
                        Damage += 75;
                    }
                    else if (Level == 5)
                    {
                        Damage += 100;
                        FireRate = 0.28f;
                    }
                    break;

                case TowerType.Soldier:
                    if (Level == 1)
                    {
                        Range += 40;
                    }
                    else if (Level == 2)
                    {
                        Damage += 3;
                    }
                    else if (Level == 3)
                    {
                        FireRate = 1.10f;
                    }
                    else if (Level == 4)
                    {
                        Damage += 5;
                        HiddenDetection = true;
                    }
                    else if (Level == 5)
                    {
                        Damage += 8;
                        FireRate = 0.80f;
                    }
                    break;
            }

            StatRange = Range / 10;
        }

        private bool TryBuyUpgrade()
        {
            int baseCost = GetNextUpgradeBaseCost();
            if (baseCost <= 0)
                return false;

            int cost = GetDiscountedUpgradeCost(baseCost);
            if (Gamedata.gold < cost)
                return false;

            Gamedata.gold -= cost;
            TotalCost += cost;
            Level += 1;
            Texturelvl += 1;
            return true;
        }

        public int GetDiscountedUpgradeCost(int baseCost)
        {
            float discount = MathHelper.Clamp(UpgradeDiscount, 0f, 0.75f);
            return Math.Max(1, (int)Math.Ceiling(baseCost * (1f - discount)));
        }

        public void Update(GameTime gameTime, List<Enemy> enemies, List<Projectile> projectiles, Texture2D bulletTex)
        {
            FireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate effective stats with multipliers
            float effectiveFireRate = FireRate * FireRateMultiplier;
            float effectiveRange = Range * RangeMultiplier;

            if (FireTimer >= effectiveFireRate)
            {
                Enemy target = null;

                foreach (Enemy enemy in enemies)
                {
                    float dist = Vector2.Distance(Position, enemy.Position);

                    if (enemy.Hidden && !HiddenDetection)
                        continue;

                    if (dist <= effectiveRange && enemy.IsFurtherAlongThan(target))
                    {
                        target = enemy;
                    }
                }

                if (target != null)
                {
                    bool isFreezerShot = type == TowerType.Freezer;
                    projectiles.Add(
                        new Projectile(bulletTex, Position, target, Damage, isFreezerShot, Level));

                    ShootSound?.Play(0.3f, 0f, 0f);
                    FireTimer = 0f;
                }

            }
                    

        }
    


        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, origin, Scale, SpriteEffects.None, 0f);
        }

        // Expose next-upgrade base cost for UI (0 = none/maxed)
        public int GetNextUpgradeBaseCost()
        {
            if (UpgradeCosts.TryGetValue(type, out int[] costs) && Level >= 0 && Level < costs.Length)
                return costs[Level];
            return 0;
        }

        public string GetNextUpgradeDescription()
        {
            if (IsMaxLevel()) return "Max Level";

            if (type == TowerType.Basic)
            {
                switch (Level)
                {
                    case 0: return "+50 Range";
                    case 1: return "+2 Damage";
                    case 2: return "Firerate -> 0.75 & Hidden Detect";
                    case 3: return "+50 Range, +2 Damage";
                    case 4: return "+4 Damage, Firerate -> 0.55";
                }
            }
            if (type == TowerType.Sniper)
            {
                switch (Level)
                {
                    case 0: return "+100 Range";
                    case 1: return "+35 Damage";
                    case 2: return "Firerate -> 3.5 & Hidden Detect";
                    case 3: return "+100 Damage";
                    case 4: return "+150 Range, Firerate -> 2.4";
                }
            }
            if (type == TowerType.Minigunner)
            {
                switch (Level)
                {
                    case 0: return "+25 Range";
                    case 1: return "+1 Damage";
                    case 2: return "Firerate -> 0.14";
                    case 3: return "+25 Range, +2 Damage, Hidden Detect";
                    case 4: return "+3 Damage, Firerate -> 0.10";
                }
            }
            if (type == TowerType.DJ)
            {
                switch (Level)
                {
                    case 0: return "+20% Range, +10% Upgrade Discount";
                    case 1: return "+25% Range, +15% Upgrade Discount";
                    case 2: return "+30% Range, +20% Upgrade Discount";
                    case 3: return "+35% Range, +25% Upgrade Discount";
                }
            }
            if (type == TowerType.Freezer)
            {
                switch (Level)
                {
                    case 0: return "+25 Range, stronger slow";
                    case 1: return "+25 Range, Firerate -> 1.35";
                    case 2: return "+35 Range, Firerate -> 1.20";
                    case 3: return "+35 Range, Firerate -> 1.05";
                    case 4: return "+50 Range, brief freeze";
                }
            }
            if (type == TowerType.Commander)
            {
                switch (Level)
                {
                    case 0: return "18% faster fire rate";
                    case 1: return "24% faster fire rate";
                    case 2: return "30% faster fire rate";
                    case 3: return "36% faster fire rate";
                }
            }
            if (type == TowerType.Farm)
            {
                switch (Level)
                {
                    case 0: return "+25 income per wave";
                    case 1: return "+45 income per wave";
                    case 2: return "+75 income per wave";
                    case 3: return "+115 income per wave";
                    case 4: return "+170 income per wave";
                }
            }
            if (type == TowerType.Accel)
            {
                switch (Level)
                {
                    case 0: return "+40 Range";
                    case 1: return "+25 Damage";
                    case 2: return "Firerate -> 0.38";
                    case 3: return "+50 Range, +75 Damage";
                    case 4: return "+100 Damage, Firerate -> 0.28";
                }
            }
            if (type == TowerType.Soldier)
            {
                switch (Level)
                {
                    case 0: return "+40 Range";
                    case 1: return "+3 Damage";
                    case 2: return "Firerate -> 1.10";
                    case 3: return "+5 Damage, Hidden Detect";
                    case 4: return "+8 Damage, Firerate -> 0.80";
                }
            }

            return string.Empty;
        }

        public bool IsMaxLevel()
        {
            if (type == TowerType.Basic || type == TowerType.Sniper || type == TowerType.Minigunner || type == TowerType.Farm || type == TowerType.Accel || type == TowerType.Soldier || type == TowerType.Freezer)
                return Level >= 5;
            return Level >= 4; // DJ/Commander
        }
    }
}
