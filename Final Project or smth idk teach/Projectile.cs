using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public class Projectile
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public float Speed { get; private set; }
        public int Damage { get; private set; }
        public Enemy Target { get; private set; }
        public bool IsActive { get; set; }

        public Projectile(Texture2D texture, Vector2 position, Enemy target, int damage)
        {
            Texture = texture;
            Position = position;
            Target = target;
            Damage = damage;
            Speed = 30f; // Adjust bullet speed here
            IsActive = true;
        }

        public void Update()
        {
            if (!IsActive) return;

            // If target dies before hit, kill the projectile
            if (Target == null || Target.IsDead || Target.ReachedEnd)
            {
                IsActive = false;
                return;
            }

            // Move towards target
            Vector2 direction = Target.Position - Position;
            if (direction.Length() < Speed)
            {
                // IMPACT!
                Target.Health -= Damage;
                IsActive = false;
            }
            else
            {
                direction.Normalize();
                Position += direction * Speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive) return;
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Texture.Width / 6, Texture.Height / 6), 0.1f, SpriteEffects.None, 0f);
        }
    }
}
