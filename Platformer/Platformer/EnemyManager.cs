using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    public class EnemyManager
    {
        // Pointer to the one instance of EnemyManager - a simple unenforced Singleton implementation
        public static EnemyManager Instance;

        // The most important part - our list/pool of enemies
        public List<Enemy> Enemies = new List<Enemy>();

        // Initialise a dictionary to hold spritesheets by name
        public Dictionary<string, Texture2D> spriteSheets = new Dictionary<string, Texture2D>();

        public EnemyManager()
        {
            // Set the singleton reference to this new instance
            Instance = this;
        }

        public void LoadContent(ContentManager content)
        {
            // Add "soldier" sheet to our sheet dictionary
            // We can add more sheets to the dictionary for different enmy types
            spriteSheets.Add("soldier", content.Load<Texture2D>("soldier"));
        }

        public void Update(GameTime gameTime, Map gameMap)
        {
            // Update all enemies
            foreach (Enemy e in Enemies) e.Update(gameTime, gameMap);
        }

        public void Draw(SpriteBatch sb, Camera gameCamera)
        {
            // Start/end drawing at Manager level so all enemies are drawn in one batch
            sb.Begin();
            foreach (Enemy e in Enemies) e.Draw(sb, gameCamera);
            sb.End();
        }

        public void Spawn(int x, int y)
        {
            // X and Y is tile position, so work out a pixel position to spawn at
            // Remember that our sprite origin is located bottom centre (at the feet), so add half width and full height
            Vector2 spawnPos = new Vector2((x * Map.TILE_WIDTH) + (Enemy.FRAME_WIDTH / 2), (y * Map.TILE_HEIGHT) + Enemy.FRAME_HEIGHT);

            // Make a new enemy, using the soldier spritesheet
            Enemy newEnemy = new Enemy(spawnPos);

            // Add the enemy to the pool
            Enemies.Add(newEnemy);
        }
    }
}
