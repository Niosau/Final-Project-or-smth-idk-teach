using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public class Enemy
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public float Speed { get; private set; }
        public float Scale { get; private set; }
        public int Health { get; set; }
        public bool IsDead => Health <= 0;
        public bool ReachedEnd { get; private set; }

        private List<Vector2> _path;
        private int _currentWaypointIndex;

        public Enemy(Texture2D texture, List<Vector2> path, float speed, int startingHealth)
        {
            Texture = texture;
            _path = path;
            Speed = speed;
            Health = startingHealth;

            _currentWaypointIndex = 0;
            // Start the enemy at the first waypoint
            if (_path.Count > 0)
                Position = _path[0];
        }

        public int GoldReward { get; private set; }

        // Update your constructor
        public Enemy(Texture2D texture, List<Vector2> path, float speed, int startingHealth, float scale)
        {
            Texture = texture;
            _path = path;
            Speed = speed;
            Health = startingHealth;
            Scale = scale;

            
            GoldReward = (int)(startingHealth * 0.5f);

            _currentWaypointIndex = 0;
            if (_path.Count > 0) Position = _path[0];
        }   

        public void Update(GameTime gameTime)
        {
            if (ReachedEnd || IsDead) return;

            Vector2 target = _path[_currentWaypointIndex];
            Vector2 direction = target - Position;

            if (direction.Length() < Speed)
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
                Position += direction * Speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, origin, Scale, SpriteEffects.None, 0f);
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