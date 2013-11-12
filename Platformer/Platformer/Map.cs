using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer
{
    public class Map
    {
        public const int MAP_WIDTH = 40;
        public const int MAP_HEIGHT = 20;

        public const int TILE_WIDTH = 64;
        public const int TILE_HEIGHT = 64;

        public const float GRAVITY = 0.2f;

        public int[,] Tiles = new int[MAP_WIDTH, MAP_HEIGHT];

        public Vector2 PlayerSpawn;

        Texture2D tilesTex;

        public Map(string mapText, Texture2D tex)
        {
            tilesTex = tex;

            string[] lines = mapText.Split('\n');

            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    Tiles[x, y] = Convert.ToInt16(lines[y][x].ToString());

                    switch (Tiles[x, y])
                    {
                        case 8:
                            EnemyManager.Instance.Spawn(x, y);
                            Tiles[x, y] = 0;
                            break;
                        case 9:
                            PlayerSpawn = (new Vector2(x, y) * new Vector2(TILE_WIDTH, TILE_HEIGHT)) + new Vector2(Hero.FRAME_WIDTH/2, Hero.FRAME_HEIGHT);
                            Tiles[x, y] = 0;
                            break;

                    }
                }
            }
        }

        public void Draw(SpriteBatch sb, Camera gameCamera)
        {
            /// CAMERA STUFF: A lot has changed here!
            
            // Need to use SamplerState.PointClamp to avoid tile artifacting with floating point positioning. Try setting it to null to see the difference!
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    // We don't want to draw a blank tile (saves triangles!)
                    if (Tiles[x, y] == 0) continue;

                    // First calculate the position we want to draw the tile at
                    Vector2 tilePos = new Vector2(x * TILE_WIDTH, y * TILE_HEIGHT);
                    
                    // Next, check if that position is inside the camera's visible area
                    // We subtract from left/top to allow for tiles to be drawn helf-off the screen
                    if (tilePos.X >= gameCamera.VisibleArea.Left - Map.TILE_WIDTH &&
                       tilePos.Y >= gameCamera.VisibleArea.Top - Map.TILE_HEIGHT &&
                       tilePos.X <= gameCamera.VisibleArea.Right &&
                       tilePos.Y <= gameCamera.VisibleArea.Bottom)
                    {
                        sb.Draw(tilesTex,
                                // Subtract camera position from draw position
                                tilePos - gameCamera.Position,
                                new Rectangle(Tiles[x, y] * TILE_WIDTH, 0, TILE_WIDTH, TILE_HEIGHT),
                                Color.White);
                    }
                }
            }
            sb.End();
        }

        public int GetTileAt(Vector2 pos)
        {
            int tx = (int)pos.X / TILE_WIDTH;
            int ty = (int)pos.Y / TILE_WIDTH;

            if (tx >= 0 && tx < MAP_WIDTH && ty >= 0 && ty < MAP_HEIGHT)
                return Tiles[tx, ty];
            else 
                return 0;
        }

        public static Map Load(string fn, ContentManager content)
        {
            Map newMap;
            string mapText;
            Texture2D tilesTex;

            mapText = File.ReadAllText(Path.Combine(content.RootDirectory, fn));
            tilesTex = content.Load<Texture2D>("tiles");
            newMap = new Map(mapText, tilesTex);

            return newMap;
        }
    }
}
