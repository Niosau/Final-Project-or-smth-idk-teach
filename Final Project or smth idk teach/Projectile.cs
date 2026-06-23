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
        public bool IsFreezerShot { get; private set; }
        public int FreezerLevel { get; private set; }

        public Projectile(Texture2D texture, Vector2 position, Enemy target, int damage, bool isFreezerShot = false, int freezerLevel = 0)
        {
            Texture = texture;
            Position = position;
            Target = target;
            Damage = damage;
            Speed = 30f; // Adjust bullet speed here
            IsActive = true;
            IsFreezerShot = isFreezerShot;
            FreezerLevel = freezerLevel;
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
                if (IsFreezerShot)
                {
                    float slowMultiplier = 0.85f;
                    float freezeDuration = 0f;

                    if (FreezerLevel >= 5)
                    {
                        slowMultiplier = 0.35f;
                        freezeDuration = 0.45f;
                    }
                    else if (FreezerLevel == 4)
                    {
                        slowMultiplier = 0.45f;
                    }
                    else if (FreezerLevel == 3)
                    {
                        slowMultiplier = 0.55f;
                    }
                    else if (FreezerLevel == 2)
                    {
                        slowMultiplier = 0.65f;
                    }
                    else if (FreezerLevel == 1)
                    {
                        slowMultiplier = 0.75f;
                    }

                    if (freezeDuration > 0f)
                    {
                        Target.ApplySlow(1f, freezeDuration);
                    }
                    Target.ApplySlow(slowMultiplier, 0f);
                }

                if (Damage > 0)
                {
                    Target.Health -= Damage;
                }

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
