using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public class Tower
    {
        public Texture2D Texture { get;  set; }
        public Vector2 Position { get;  set; }
        public float Range { get;  set; }
        public int Damage { get;  set; }
        public float Scale { get;  set; }

        private float _fireTimer;
        private float _fireRate; // Now dynamic!

        // Updated Constructor to accept stats
        public Tower(Texture2D texture, Vector2 position, float desiredWidth, float range, int damage, float fireRate)
        {
            Texture = texture;
            Position = position;           
            Range = range;
            Damage = damage;
            _fireRate = fireRate;
            Scale = desiredWidth / texture.Width;
        }

        public void Update(GameTime gameTime, List<Enemy> enemies, List<Projectile> projectiles, Texture2D bulletTex)
        {
            _fireTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_fireTimer >= _fireRate)
            {
                Enemy target = null;
                float closestDistance = Range;

                foreach (var enemy in enemies)
                {
                    float dist = Vector2.Distance(Position, enemy.Position);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        target = enemy;
                    }
                }

                if (target != null)
                {
                    projectiles.Add(new Projectile(bulletTex, Position, target, Damage));
                    _fireTimer = 0f;
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