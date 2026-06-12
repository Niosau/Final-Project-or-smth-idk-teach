using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public class WaveManager
    {
        private List<Vector2> _path;
        private Queue<Enemy> _spawnQueue = new Queue<Enemy>(); // The list of enemies waiting to spawn

        public int WaveNumber { get; set; }
        public bool IsWaveActive { get; private set; }

        private float _spawnTimer;
        private float _spawnInterval = 0.8f;

        
        public Texture2D _basicTex, _fastTex, _tankTex, _basicBossTex, _hiddenTex, _breaker2Tex, _breakerTex, _necromancerTex, _skeletonTex;
        public Texture2D _armoredTex, _tankBoss, _enragedTex, _breaker3Tex, _bruteTex;
        public WaveManager(List<Vector2> path, Texture2D basic, Texture2D fast, Texture2D tank)
        {
            _path = path;
            _basicTex = basic;
            _fastTex = fast;
            _tankTex = tank;
            WaveNumber = 0;
        }

        public void StartNextWave()
        {
            WaveNumber++;
            IsWaveActive = true;
            _spawnQueue.Clear();

            // --- WAVE DESIGN LOGIC ---
            // Wave 1: 5 Basics
            if (WaveNumber == 1)
            {
                for (int i = 0; i < 1; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 0.01f, 50, 2.5f, true));
            }
            // Wave 2: 4x Normal, 2x Speedy
            else if (WaveNumber == 2)
            {
                for (int i = 0; i < 4; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 2f, 4, 2.5f, false));
                for (int i = 0; i < 2; i++)
                    _spawnQueue.Enqueue(new Enemy(_fastTex, _path, 5f, 4, 2.5f, false)); // Faster, less HP
            }
            // Wave 3: 4x Speedy, 10x Normal
            else if (WaveNumber == 3)
            {
                for (int i = 0; i < 10; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 2f, 4, 2.5f, false));
                for (int i = 0; i < 4; i++)
                    _spawnQueue.Enqueue(new Enemy(_fastTex, _path, 2f, 4, 2.5f, false));
            }
            // Wave 4: 4x Speedy, 6x Normal, 3x Slow
            else if (WaveNumber == 4)
            {
                for (int i = 0; i < 3; i++)
                    _spawnQueue.Enqueue(new Enemy(_tankTex, _path, 1f, 14, 2.5f, false));
                for (int i = 0; i < 6; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 2f, 4, 2.5f, false));
                for (int i = 0; i < 4; i++)
                    _spawnQueue.Enqueue(new Enemy(_fastTex, _path, 5f, 4, 2.5f, false));
            }
            // Wave 5: 5x Normal, 6x Slow
            else if (WaveNumber == 5)
            {
                for (int i = 0; i < 6; i++)
                    _spawnQueue.Enqueue(new Enemy(_tankTex, _path, 2f, 4, 2.5f, false));
                for (int i = 0; i < 5; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 2f, 4, 2.5f, false));
                
            }
            // Wave 6: 4x Slow, 8x Speedy, 1x Normal Boss (Boss)
            else if (WaveNumber == 6)
            {
                for (int i = 0; i < 4; i++)
                    _spawnQueue.Enqueue(new Enemy(_tankTex, _path, 1f, 14, 2.5f, false));
                for (int i = 0; i < 8; i++)
                    _spawnQueue.Enqueue(new Enemy(_fastTex, _path, 5f, 4, 2.5f, false));
                for (int i = 0; i < 1; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicBossTex, _path, 1f, 150, 2.5f, false));
            }
            // Wave 7: 3x Slow, 8x Speedy (Bloated), 3x Slow, 1x Normal Boss
            else if (WaveNumber == 7)
            {
                for (int i = 0; i < 3; i++)
                    _spawnQueue.Enqueue(new Enemy(_tankTex, _path, 1f, 14, 2.5f, false));
                for (int i = 0; i < 8; i++)
                    _spawnQueue.Enqueue(new Enemy(_fastTex, _path, 2f, 8, 2.5f, false));
                for (int i = 0; i < 3; i++)
                    _spawnQueue.Enqueue(new Enemy(_tankTex, _path, 1f, 14, 2.5f, false));
                for (int i = 0; i < 1; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicBossTex, _path, 1f, 150, 2.5f, false));

            }
            // Wave 8: 10x Normal, 8x Slow, 5x Hidden
            else if (WaveNumber == 8)
            {
                for (int i = 0; i < 10; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 2f, 4, 2.5f, false));
                for (int i = 0; i < 8; i++)
                    _spawnQueue.Enqueue(new Enemy(_tankTex, _path, 1f, 4, 2.5f, false));
                for (int i = 0; i < 5; i++)
                    _spawnQueue.Enqueue(new Enemy(_hiddenTex, _path, 2f, 4, 2.5f, false));
            }
            // Wave 9: 1x Normal Boss, 7x Hidden, 5x Speedy (Bloated), 1x Normal Boss
            else if (WaveNumber == 9)
            {
                for (int i = 0; i < 1; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicBossTex, _path, 1f, 150, 2.5f, false));
                for (int i = 0; i < 7; i++)
                    _spawnQueue.Enqueue(new Enemy(_hiddenTex, _path, 2f, 4, 2.5f, false));
                for (int i = 0; i < 5; i++)
                    _spawnQueue.Enqueue(new Enemy(_fastTex, _path, 2f, 8, 2.5f, false));
                for (int i = 0; i < 1; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicBossTex, _path, 1f, 150, 2.5f, false));

            }
            // Wave 10: 6x Slow (Bloated), 7x Speedy (Bloated), 5x Breaker2
            else if (WaveNumber == 10)
            {
                for (int i = 0; i < 3; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 2f, 28, 2.5f, false));
                for (int i = 0; i < 7; i++)
                    _spawnQueue.Enqueue(new Enemy(_fastTex, _path, 4f, 8, 2.5f, false));
                for (int i = 0; i < 3; i++)
                    _spawnQueue.Enqueue(new Enemy(_breaker2Tex, _path, 2f, 20, 2.5f, false));
                for (int i = 0; i < 3; i++)
                    _spawnQueue.Enqueue(new Enemy(_basicTex, _path, 2f, 28, 2.5f, false));
                for (int i = 0; i < 4; i++)
                    _spawnQueue.Enqueue(new Enemy(_breaker2Tex, _path, 2f, 20, 2.5f, false));
            }
        }

        public void Update(GameTime gameTime, List<Enemy> activeEnemies)
        {
            if (!IsWaveActive) return;

            if (_spawnQueue.Count > 0)
            {
                _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_spawnTimer >= _spawnInterval)
                {
                    // Pull the next enemy out of the queue and add to active game
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
