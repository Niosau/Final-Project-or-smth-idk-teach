using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public class GameModeConfig
    {
        public Screen Screen { get; }
        public string Name { get; }
        public Texture2D MapTexture { get; }
        public int StartingHealth { get; }
        public int StartingGold { get; }
        public List<Vector2> Path { get; }
        public List<Rectangle> PathHitboxes { get; }
        public List<WaveDefinition> Waves { get; }
        public float SpawnInterval { get; }
        public int CoinReward { get; }

        public GameModeConfig(
            Screen screen,
            string name,
            Texture2D mapTexture,
            int startingHealth,
            int startingGold,
            List<Vector2> path,
            List<Rectangle> pathHitboxes,
            List<WaveDefinition> waves,
            float spawnInterval = 0.8f,
            int coinReward = 50)
        {
            Screen = screen;
            Name = name;
            MapTexture = mapTexture;
            StartingHealth = startingHealth;
            StartingGold = startingGold;
            Path = path;
            PathHitboxes = pathHitboxes;
            Waves = waves;
            SpawnInterval = spawnInterval;
            CoinReward = coinReward;
        }
    }
}
