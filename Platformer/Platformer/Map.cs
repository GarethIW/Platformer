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
    class Map
    {
        public const int MAP_WIDTH = 20;
        public const int MAP_HEIGHT = 11;

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
                        case 9:
                            PlayerSpawn = (new Vector2(x, y) * new Vector2(TILE_WIDTH, TILE_HEIGHT)) + new Vector2(Hero.FRAME_WIDTH/2, Hero.FRAME_HEIGHT);
                            Tiles[x, y] = 0;
                            break;

                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    sb.Draw(tilesTex,
                            new Vector2(x * TILE_WIDTH, y * TILE_HEIGHT),
                            new Rectangle(Tiles[x, y] * TILE_WIDTH, 0, TILE_WIDTH, TILE_HEIGHT),
                            Color.White);
                }
            }
            sb.End();
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
