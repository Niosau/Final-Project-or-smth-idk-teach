using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        // Support tower buff values (scaled by level)
        public float DJRangeBuff = 0.2f;
        public float DJDiscountBuff = 0.15f;
        public float CommanderFireRateBuff = 0.8f; // Multiplier (lower = faster)

        public int FarmIncomePerWave = 0;

        private float FireTimer;
        public float FireRate;
        public float StatRange;

        // Updated Constructor to accept stats
        public Tower(Texture2D texture, Vector2 position, float desiredWidth, float range, int damage, float fireRate, TowerType type, bool hiddenDetection, int towerCost, int farmIncomePerWave = 0)
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
        }

        public void Upgrade()
        {
            if (type == TowerType.Basic)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                    TotalCost += 50;
                    Texturelvl += 1;
                }
                else if (Level == 1 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    Damage += 100;
                    TotalCost += 100;
                    Texturelvl += 1;
                }
                else if (Level == 2 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    FireRate = 0.1f;
                    TotalCost += 100;
                    HiddenDetection = true;
                    Texturelvl += 1;
                }
                else if (Level == 3 && Gamedata.gold >= 200)
                {
                    Gamedata.gold -= 200;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                    Texturelvl += 1;
                    TotalCost += 200;
                }
                else if (Level == 4 && Gamedata.gold >= 250)
                {
                    Gamedata.gold -= 250;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                    Texturelvl += 1;
                    TotalCost += 250;
                }
            }
            else if (type == TowerType.Sniper)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                }
                else if (Level == 1 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    Damage += 100;
                }
                else if (Level == 2 && Gamedata.gold >= 150)
                {
                    Gamedata.gold -= 150;
                    Level += 1;
                    FireRate = 0.1f;
                }
                else if (Level == 3 && Gamedata.gold >= 200)
                {
                    Gamedata.gold -= 200;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                }
                else if (Level == 4 && Gamedata.gold >= 250)
                {
                    Gamedata.gold -= 250;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                }
            }
            else if (type == TowerType.Minigunner)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                }
                else if (Level == 1 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    Damage += 100;
                }
                else if (Level == 2 && Gamedata.gold >= 150)
                {
                    Gamedata.gold -= 150;
                    Level += 1;
                    FireRate = 0.1f;
                }
                else if (Level == 3 && Gamedata.gold >= 200)
                {
                    Gamedata.gold -= 200;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                }
                else if (Level == 4 && Gamedata.gold >= 250)
                {
                    Gamedata.gold -= 250;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                }
            }
            else if (type == TowerType.DJ)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    DJRangeBuff = 0.25f;
                    DJDiscountBuff = 0.20f;
                    TotalCost += 50;
                }
                else if (Level == 1 && Gamedata.gold >= 75)
                {
                    Gamedata.gold -= 75;
                    Level += 1;
                    DJRangeBuff = 0.30f;
                    DJDiscountBuff = 0.25f;
                    TotalCost += 75;
                }
                else if (Level == 2 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    DJRangeBuff = 0.35f;
                    DJDiscountBuff = 0.30f;
                    TotalCost += 100;
                }
                else if (Level == 3 && Gamedata.gold >= 125)
                {
                    Gamedata.gold -= 125;
                    Level += 1;
                    DJRangeBuff = 0.40f;
                    DJDiscountBuff = 0.35f;
                    TotalCost += 125;
                }
            }
            else if (type == TowerType.Freezer)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    Range += 40;
                    StatRange = Range / 10;
                    TotalCost += 50;
                }
                else if (Level == 1 && Gamedata.gold >= 75)
                {
                    Gamedata.gold -= 75;
                    Level += 1;
                    Range += 40;
                    StatRange = Range / 10;
                    TotalCost += 75;
                }
                else if (Level == 2 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    Range += 50;
                    StatRange = Range / 10;
                    TotalCost += 100;
                }
                else if (Level == 3 && Gamedata.gold >= 125)
                {
                    Gamedata.gold -= 125;
                    Level += 1;
                    Range += 60;
                    StatRange = Range / 10;
                    TotalCost += 125;
                }
                else if (Level == 4 && Gamedata.gold >= 150)
                {
                    Gamedata.gold -= 150;
                    Level += 1;
                    Range += 70;
                    StatRange = Range / 10;
                    TotalCost += 150;
                }
            }
            else if (type == TowerType.Commander)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    CommanderFireRateBuff = 0.75f; // 25% faster
                    TotalCost += 50;
                }
                else if (Level == 1 && Gamedata.gold >= 75)
                {
                    Gamedata.gold -= 75;
                    Level += 1;
                    CommanderFireRateBuff = 0.70f; // 30% faster
                    TotalCost += 75;
                }
                else if (Level == 2 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    CommanderFireRateBuff = 0.65f; // 35% faster
                    TotalCost += 100;
                }
                else if (Level == 3 && Gamedata.gold >= 125)
                {
                    Gamedata.gold -= 125;
                    Level += 1;
                    CommanderFireRateBuff = 0.60f; // 40% faster
                    TotalCost += 125;
                }
            }
            else if (type == TowerType.Farm)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    FarmIncomePerWave += 10;
                    TotalCost += 50;
                }
                else if (Level == 1 && Gamedata.gold >= 75)
                {
                    Gamedata.gold -= 75;
                    Level += 1;
                    FarmIncomePerWave += 15;
                    TotalCost += 75;
                }
                else if (Level == 2 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    FarmIncomePerWave += 20;
                    TotalCost += 100;
                }
                else if (Level == 3 && Gamedata.gold >= 150)
                {
                    Gamedata.gold -= 150;
                    Level += 1;
                    FarmIncomePerWave += 25;
                    TotalCost += 150;
                }
                else if (Level == 4 && Gamedata.gold >= 200)
                {
                    Gamedata.gold -= 200;
                    Level += 1;
                    FarmIncomePerWave += 30;
                    TotalCost += 200;
                }
            }
            else if (type == TowerType.Accel)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    Range += 50;
                    StatRange = Range / 10;
                    TotalCost += 50;
                }
                else if (Level == 1 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    Damage += 50;
                    TotalCost += 100;
                }
                else if (Level == 2 && Gamedata.gold >= 150)
                {
                    Gamedata.gold -= 150;
                    Level += 1;
                    FireRate = 0.3f;
                    TotalCost += 150;
                }
                else if (Level == 3 && Gamedata.gold >= 200)
                {
                    Gamedata.gold -= 200;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                    TotalCost += 200;
                }
                else if (Level == 4 && Gamedata.gold >= 250)
                {
                    Gamedata.gold -= 250;
                    Level += 1;
                    Damage += 50;
                    TotalCost += 250;
                }
            }
            else if (type == TowerType.Soldier)
            {
                if (Level == 0 && Gamedata.gold >= 50)
                {
                    Gamedata.gold -= 50;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                    TotalCost += 50;
                }
                else if (Level == 1 && Gamedata.gold >= 100)
                {
                    Gamedata.gold -= 100;
                    Level += 1;
                    Damage += 50;
                    TotalCost += 100;
                }
                else if (Level == 2 && Gamedata.gold >= 150)
                {
                    Gamedata.gold -= 150;
                    Level += 1;
                    FireRate = 1.0f;
                    TotalCost += 150;
                }
                else if (Level == 3 && Gamedata.gold >= 200)
                {
                    Gamedata.gold -= 200;
                    Level += 1;
                    Range += 100;
                    StatRange = Range / 10;
                    TotalCost += 200;
                }
                else if (Level == 4 && Gamedata.gold >= 250)
                {
                    Gamedata.gold -= 250;
                    Level += 1;
                    Damage += 100;
                    TotalCost += 250;
                }
            }
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
            // For Basic/Sniper/Minigunner, levels 0..4 have costs
            if (type == TowerType.Basic || type == TowerType.Sniper || type == TowerType.Minigunner || type == TowerType.Farm || type == TowerType.Accel || type == TowerType.Soldier || type == TowerType.Freezer)
            {
                switch (Level)
                {
                    case 0: return 50;
                    case 1: return 100;
                    case 2: return 100;
                    case 3: return 200;
                    case 4: return 250;
                    default: return 0;
                }
            }

            // DJ / Commander
            if (type == TowerType.DJ || type == TowerType.Commander)
            {
                switch (Level)
                {
                    case 0: return 50;
                    case 1: return 75;
                    case 2: return 100;
                    case 3: return 125;
                    default: return 0;
                }
            }

            return 0;
        }

        public string GetNextUpgradeDescription()
        {
            if (IsMaxLevel()) return "Max Level";

            if (type == TowerType.Basic)
            {
                switch (Level)
                {
                    case 0: return "+100 Range";
                    case 1: return "+100 Damage";
                    case 2: return "Firerate -> 0.1 & Hidden Detect";
                    case 3: return "+100 Range";
                    case 4: return "+100 Range";
                }
            }
            if (type == TowerType.Sniper)
            {
                switch (Level)
                {
                    case 0: return "+100 Range";
                    case 1: return "+100 Damage";
                    case 2: return "Firerate -> 0.1";
                    case 3: return "+100 Range";
                    case 4: return "+100 Range";
                }
            }
            if (type == TowerType.Minigunner)
            {
                switch (Level)
                {
                    case 0: return "+100 Range";
                    case 1: return "+100 Damage";
                    case 2: return "Firerate -> 0.1";
                    case 3: return "+100 Range";
                    case 4: return "+100 Range";
                }
            }
            if (type == TowerType.DJ)
            {
                switch (Level)
                {
                    case 0: return "+25% Range, +20% Upgrade Discount";
                    case 1: return "+30% Range, +25% Upgrade Discount";
                    case 2: return "+35% Range, +30% Upgrade Discount";
                    case 3: return "+40% Range, +35% Upgrade Discount";
                }
            }
            if (type == TowerType.Freezer)
            {
                switch (Level)
                {
                    case 0: return "+40 range";
                    case 1: return "+40 range";
                    case 2: return "+50 range";
                    case 3: return "+60 range";
                    case 4: return "+70 range";
                }
            }
            if (type == TowerType.Commander)
            {
                switch (Level)
                {
                    case 0: return "25% faster fire rate";
                    case 1: return "30% faster fire rate";
                    case 2: return "35% faster fire rate";
                    case 3: return "40% faster fire rate";
                }
            }
            if (type == TowerType.Farm)
            {
                switch (Level)
                {
                    case 0: return "+10 income per wave";
                    case 1: return "+15 income per wave";
                    case 2: return "+20 income per wave";
                    case 3: return "+25 income per wave";
                    case 4: return "+30 income per wave";
                }
            }
            if (type == TowerType.Accel)
            {
                switch (Level)
                {
                    case 0: return "+50 range";
                    case 1: return "+50 damage";
                    case 2: return "Firerate -> 0.3";
                    case 3: return "+100 range";
                    case 4: return "+50 damage";
                }
            }
            if (type == TowerType.Soldier)
            {
                switch (Level)
                {
                    case 0: return "+100 range";
                    case 1: return "+50 damage";
                    case 2: return "Firerate -> 1.0";
                    case 3: return "+100 range";
                    case 4: return "+100 damage";
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
