using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    public class Enemy
    {
        // Animation constants
        public const int FRAME_WIDTH = 64;
        public const int FRAME_HEIGHT = 64;
        const int WALK_FRAMES = 3;
        const int FIRST_WALK_FRAME = 0;
        const double FRAME_TIME = 100;

        // Physics constants
        const float ACCELERATION = 0.5f;
        const float INERTIA = 0.1f;

        // Position and Speed
        public Vector2 Position;
        public Vector2 Speed;

        // Animation locals
        [JsonProperty]
        int currentFrame = 0;
        [JsonProperty]
        double currentFrameTime = 0;

        [JsonProperty]
        int faceDir = 1;

        // Collisions Bounds
        Rectangle collRect;

        public Enemy(Vector2 pos)
        {
            Position = pos;

            faceDir = Platformer.Rand.Next(2);
            if (faceDir == 0) faceDir = -1;

            // Set up collision rectangle
            collRect = new Rectangle(0, 0, FRAME_WIDTH - 20, FRAME_HEIGHT - 20);
        }

        public void Update(GameTime gameTime, Map gameMap)
        {
            MoveHorizontal(faceDir);

            // Move rectangle to enemy's location
            collRect.X = (int)Position.X - (collRect.Width / 2);
            collRect.Y = (int)Position.Y - collRect.Height;

            // Apply inertia to X speed
            Speed.X = MathHelper.Lerp(Speed.X, 0f, INERTIA);
            // Clamp X floating point value
            if ((Speed.X > 0f && Speed.X < 0.1f) || (Speed.X < 0f && Speed.X > -0.1f)) Speed.X = 0f;
            // Apply gravity to Y speed
            Speed.Y += Map.GRAVITY;

            // Check Collisions
            CheckCollisions(gameMap);

            // Check platform edges
            CheckPlatform(gameMap);

            // Apply speed to position
            Position += Speed;

            // Clamp X position to map boundaries
            if (Position.X < 0)
            {
                Position.X = 0;
                Speed.X = 0;
                faceDir = 1;
            }
            if (Position.X > Map.MAP_WIDTH * Map.TILE_WIDTH)
            {
                Position.X = Map.MAP_WIDTH * Map.TILE_WIDTH;
                Speed.X = 0;
                faceDir = -1;
            }
            
            // If we're not jumping...
            if (Speed.X != 0f)
            {
                // If we're moving left or right, animate!
                // Increment frame time counter by elapsed game frame time (ms)
                currentFrameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currentFrameTime >= FRAME_TIME)
                {
                    // If current animation frame time is reached, reset current frame time to 0 and increment current frame number
                    currentFrameTime = 0;
                    currentFrame++;
                    // If we're at the end of the animation sequence, loop back to first frame
                    if (currentFrame >= WALK_FRAMES + FIRST_WALK_FRAME) currentFrame = FIRST_WALK_FRAME;
                }
            }
                
        }

        public void MoveHorizontal(int dir)
        {
            // Accelerate in our movement direction
            Speed.X += (ACCELERATION * (float)dir);
            // Set facing direction
            faceDir = dir;
        }

        public void Draw(SpriteBatch sb, Camera gameCamera)
        {
            /// CAMERA STUFF: subtract the camera position from the enemy's draw position
            sb.Draw(EnemyManager.Instance.spriteSheets["soldier"],
                    Position - gameCamera.Position,
                    new Rectangle(FRAME_WIDTH * currentFrame, 0, FRAME_WIDTH, FRAME_HEIGHT),
                    Color.White,
                    0f,
                    new Vector2(FRAME_WIDTH / 2, FRAME_HEIGHT),
                    1f,
                    faceDir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0f);
        }

        private void CheckCollisions(Map gameMap)
        {
            Vector2 checkPos;

            // First, check collsions downwards
            if (Speed.Y > 0)
            {
                checkPos.Y = collRect.Bottom + Speed.Y;
                for (checkPos.X = collRect.Left + 4; checkPos.X <= collRect.Right - 4; checkPos.X += collRect.Width / 8)
                    if (gameMap.GetTileAt(checkPos) > 0)
                    {
                        Speed.Y = 0;
                    }
            }

            //  Up
            if (Speed.Y < 0)
            {
                checkPos.Y = collRect.Top + Speed.Y;
                for (checkPos.X = collRect.Left + 4; checkPos.X <= collRect.Right - 4; checkPos.X += collRect.Width / 8)
                    if (gameMap.GetTileAt(checkPos) > 0)
                    {
                        Speed.Y = 0;
                    }
            }

            // Left
            if (Speed.X < 0)
            {
                checkPos.X = collRect.Left + Speed.X;
                for (checkPos.Y = collRect.Top + 4; checkPos.Y <= collRect.Bottom - 4; checkPos.Y += collRect.Height / 8)
                    if (gameMap.GetTileAt(checkPos) > 0)
                    {
                        Speed.X = 0;
                        faceDir = 1;
                    }
            }

            // Right
            if (Speed.X > 0)
            {
                checkPos.X = collRect.Right + Speed.X;
                for (checkPos.Y = collRect.Top + 4; checkPos.Y <= collRect.Bottom - 4; checkPos.Y += collRect.Height / 8)
                    if (gameMap.GetTileAt(checkPos) > 0)
                    {
                        Speed.X = 0;
                        faceDir = -1;
                    }
            }

        }

        private void CheckPlatform(Map gameMap)
        {
            Vector2 checkPos;

            // Left
            if (Speed.X < 0)
            {
                checkPos.X = collRect.Left - (Map.TILE_WIDTH/2);
                checkPos.Y = collRect.Bottom + (Map.TILE_HEIGHT / 2);
                if (gameMap.GetTileAt(checkPos) == 0)
                {
                    faceDir = 1;
                }
            }

            // Right
            if (Speed.X > 0)
            {
                checkPos.X = collRect.Right + (Map.TILE_WIDTH/2);
                checkPos.Y = collRect.Bottom + (Map.TILE_HEIGHT / 2);
                if (gameMap.GetTileAt(checkPos) == 0)
                {
                    faceDir = -1;
                }
            }
        }

        
    }
}