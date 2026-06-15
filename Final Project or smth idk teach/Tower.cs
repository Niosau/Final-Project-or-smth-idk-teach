using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    
    public class Tower
    {
        public TowerType type { get; set; }
        public Texture2D Texture { get;  set; }
        public Vector2 Position { get;  set; }
        public float Range { get;  set; }
        public bool HiddenDetection { get; set; }
        public int Level { get; set; }
        public int Damage { get;  set; }
        public float Scale { get;  set; }
        public int Texturelvl { get; set; }
        public int TotalCost { get; set; }
        public int UpgradeCost { get; set; }
        public int TowerCost { get; set; }

        private float FireTimer;
        public float FireRate; 
        public float StatRange;

        // Updated Constructor to accept stats
        public Tower(Texture2D texture, Vector2 position, float desiredWidth, float range, int damage, float fireRate, TowerType type, bool hiddenDetection, int towerCost)
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
        }

        public void Upgrade()
        {
            if (type == TowerType.Basic)
            {
                if(Level == 0 && Gamedata.gold >= 50)
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
            if (type == TowerType.Sniper)
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
        }

        public void Update(GameTime gameTime, List<Enemy> enemies, List<Projectile> projectiles, Texture2D bulletTex)
        {
            FireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (FireTimer >= FireRate)
            {
                Enemy target = null;
                float closestDistance = Range;

                foreach (var enemy in enemies)
                {
                    float dist = Vector2.Distance(Position, enemy.Position);
                    if (enemy.Hidden == true)
                    {
                        if (HiddenDetection == true)
                        {
                            if (dist < closestDistance)
                            {
                                closestDistance = dist;
                                target = enemy;
                            }
                        }
                        else
                        {
                            target = null;
                        }
                    }
                    if (enemy.Hidden == false)
                    {
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            target = enemy;
                        }
                        if (target != null)
                        {
                            projectiles.Add(new Projectile(bulletTex, Position, target, Damage));
                            FireTimer = 0f;
                        }
                    }
                   
                   
                
                }

                if (target != null)
                {
                    projectiles.Add(new Projectile(bulletTex, Position, target, Damage));
                    FireTimer = 0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, origin, Scale, SpriteEffects.None, 0f);
        }
    }
}
