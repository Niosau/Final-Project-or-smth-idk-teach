using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public enum EnemyType
    {
        Basic,
        Fast,
        Tank
    }

    public class WaveEnemyGroup
    {
        public EnemyType EnemyType { get; }
        public int Count { get; }
        public float Speed { get; }
        public int Health { get; }
        public float Scale { get; }
        public bool Hidden { get; }

        public WaveEnemyGroup(EnemyType enemyType, int count, float speed, int health, float scale = 2.5f, bool hidden = false)
        {
            EnemyType = enemyType;
            Count = count;
            Speed = speed;
            Health = health;
            Scale = scale;
            Hidden = hidden;
        }
    }

    public class WaveDefinition
    {
        public List<WaveEnemyGroup> EnemyGroups { get; } = new List<WaveEnemyGroup>();

        public WaveDefinition(params WaveEnemyGroup[] enemyGroups)
        {
            EnemyGroups.AddRange(enemyGroups);
        }
    }

    public class WaveManager
    {
        private readonly List<Vector2> _path;
        private readonly Dictionary<EnemyType, Texture2D> _enemyTextures;
        private readonly List<WaveDefinition> _waves;
        private readonly Queue<Enemy> _spawnQueue = new Queue<Enemy>();

        public int WaveNumber { get; private set; }
        public bool IsWaveActive { get; private set; }
        public bool HasMoreWaves => WaveNumber < _waves.Count;

        private float _spawnTimer;
        private readonly float _spawnInterval;

        public WaveManager(List<Vector2> path, Dictionary<EnemyType, Texture2D> enemyTextures, List<WaveDefinition> waves, float spawnInterval = 0.8f)
        {
            _path = path;
            _enemyTextures = enemyTextures;
            _waves = waves;
            _spawnInterval = spawnInterval;
            WaveNumber = 0;
        }

        public void StartNextWave()
        {
            if (!HasMoreWaves || IsWaveActive)
            {
                return;
            }

            WaveDefinition wave = _waves[WaveNumber];
            WaveNumber++;
            IsWaveActive = true;
            _spawnTimer = 0f;
            _spawnQueue.Clear();

            foreach (WaveEnemyGroup group in wave.EnemyGroups)
            {
                Texture2D texture = _enemyTextures[group.EnemyType];
                for (int i = 0; i < group.Count; i++)
                {
                    _spawnQueue.Enqueue(new Enemy(texture, _path, group.Speed, group.Health, group.Scale, group.Hidden));
                }
            }
        }

        public void Update(GameTime gameTime, List<Enemy> activeEnemies)
        {
            if (!IsWaveActive)
            {
                return;
            }

            if (_spawnQueue.Count > 0)
            {
                _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_spawnTimer >= _spawnInterval)
                {
                    activeEnemies.Add(_spawnQueue.Dequeue());
                    _spawnTimer = 0f;
                }
            }
            else if (activeEnemies.Count == 0)
            {
                IsWaveActive = false;
            }
        }
    }
}
