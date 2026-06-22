using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public class Enemy
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public float Speed { get; private set; }
        public float Scale { get; private set; }
        public float SlowMultiplier { get; private set; } = 1f;
        public float FreezeTimer { get; private set; } = 0f;
        public float SlowTimer { get; private set; } = 0f;
        public int Health { get; set; }
        public bool Hidden { get; set; }
        public bool IsDead => Health <= 0;
        public bool ReachedEnd { get; private set; }

        private List<Vector2> _path;
        private int _currentWaypointIndex;

        public Enemy(Texture2D texture, List<Vector2> path, float speed, int startingHealth, bool hidden)
        {
            Texture = texture;
            _path = path;
            Speed = speed;
            Health = startingHealth;

            _currentWaypointIndex = 0;
            // Start the enemy at the first waypoint
            if (_path.Count > 0)
                Position = _path[0];
            Hidden = hidden;
        }

        public int GoldReward { get; private set; }

        // Update your constructor
        public Enemy(Texture2D texture, List<Vector2> path, float speed, int startingHealth, float scale, bool hidden)
        {
            Texture = texture;
            _path = path;
            Speed = speed;
            Health = startingHealth;
            Scale = scale;
            Hidden = hidden;

            GoldReward = (int)(startingHealth * 0.5f);

            _currentWaypointIndex = 0;
            if (_path.Count > 0) Position = _path[0];
        }   

        public void Update(GameTime gameTime)
        {
            if (ReachedEnd || IsDead) return;

            Vector2 target = _path[_currentWaypointIndex];
            Vector2 direction = target - Position;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FreezeTimer > 0f)
            {
                FreezeTimer -= elapsed;
                if (FreezeTimer <= 0f)
                {
                    FreezeTimer = 0f;
                }
            }

            if (SlowTimer > 0f)
            {
                SlowTimer -= elapsed;
                if (SlowTimer <= 0f)
                {
                    SlowTimer = 0f;
                    SlowMultiplier = 1f;
                }
            }

            float effectiveSpeed = FreezeTimer > 0f ? 0f : Speed * SlowMultiplier;
            if (effectiveSpeed <= 0f)
                return;

            if (direction.Length() < effectiveSpeed)
            {
                Position = target;
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _path.Count)
                {
                    ReachedEnd = true;
                }
            }
            else
            {
                direction.Normalize();
                Position += direction * effectiveSpeed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, origin, Scale, SpriteEffects.None, 0f);
        }

        public int CurrentWaypointIndex => _currentWaypointIndex;

        public float DistanceToCurrentWaypoint =>
            _path.Count > 0 && _currentWaypointIndex < _path.Count
                ? Vector2.Distance(Position, _path[_currentWaypointIndex])
                : 0f;

        public bool IsFurtherAlongThan(Enemy other)
        {
            if (other == null) return true;
            if (CurrentWaypointIndex != other.CurrentWaypointIndex)
                return CurrentWaypointIndex > other.CurrentWaypointIndex;
            return DistanceToCurrentWaypoint < other.DistanceToCurrentWaypoint;
        }

        public void ApplySlow(float multiplier, float freezeDuration)
        {
            if (freezeDuration > 0f)
            {
                FreezeTimer = Math.Max(FreezeTimer, freezeDuration);
                SlowMultiplier = 0f;
            }
            else if (multiplier < SlowMultiplier || SlowTimer <= 0f)
            {
                SlowMultiplier = multiplier;
                SlowTimer = 1.2f;
            }
        }
        public Rectangle GetBounds()
        {
            int width = (int)(Texture.Width * Scale);
            int height = (int)(Texture.Height * Scale);

            // Position is the center, so we offset by half width/height
            return new Rectangle(
                (int)Position.X - (width / 2),
                (int)Position.Y - (height / 2),
                width,
                height
            );
        }
    public void TakeDamage(int amount)
        {
            Health -= amount;
        }

    }
}