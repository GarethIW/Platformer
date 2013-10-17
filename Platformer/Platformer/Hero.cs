using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    class Hero
    {
        public const int FRAME_WIDTH = 64;
        public const int FRAME_HEIGHT = 64;
        const int WALK_FRAMES = 9;
        const int FIRST_WALK_FRAME = 1;
        const int IDLE_FRAME = 0;
        const int JUMP_FRAME = 10;
        const double FRAME_TIME = 100;

        const float ACCELERATION = 0.5f;
        const float INERTIA = 0.1f;
        const float JUMP_SPEED = 8f;

        public Vector2 Position;
        public Vector2 Speed;

        int currentFrame = 0;
        double currentFrameTime = 0;

        bool isJumping = false;
        int faceDir = 1;

        Texture2D spriteSheet;

        public Hero(Vector2 pos)
        {
            Position = pos;
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("hero");
        }

        public void Update(GameTime gameTime, Map gameMap)
        {
            Speed.X = MathHelper.Lerp(Speed.X, 0f,INERTIA);
            if ((Speed.X > 0f && Speed.X < 0.1f) || (Speed.X < 0f && Speed.X > -0.1f)) Speed.X = 0f;
            Speed.Y += Map.GRAVITY;

            Position += Speed;

            if (Position.X < 0)
            {
                Position.X = 0;
                Speed.X = 0;
            }
            if (Position.X > Map.MAP_WIDTH * Map.TILE_WIDTH)
            {
                Position.X = Map.MAP_WIDTH * Map.TILE_WIDTH;
                Speed.X = 0;
            }

            // Temporary!
            if (Position.Y > 720)
            {
                Position.Y = 720;
                Speed.Y = 0;
                isJumping = false;
            }

            // Animation
            if (!isJumping)
            {
                if (Speed.X != 0f)
                {
                    currentFrameTime += gameTime.ElapsedGameTime.TotalMilliseconds;// *Math.Abs((double)Speed.X * 0.25f);
                    if (currentFrameTime >= FRAME_TIME)
                    {
                        currentFrameTime = 0;
                        currentFrame++;
                        if (currentFrame >= WALK_FRAMES + FIRST_WALK_FRAME) currentFrame = FIRST_WALK_FRAME;
                    }
                }
                else
                {
                    currentFrame = IDLE_FRAME;
                }
            }
            else
            {
                currentFrame = JUMP_FRAME;
            }
        }

        public void MoveHorizontal(int dir)
        {
            Speed.X += (ACCELERATION * (float)dir);
            faceDir = dir;
        }

        public void Jump()
        {
            if (!isJumping)
            {
                Speed.Y = -JUMP_SPEED;
                isJumping = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(spriteSheet,
                    Position,
                    new Rectangle(FRAME_WIDTH * currentFrame,0,FRAME_WIDTH, FRAME_HEIGHT),
                    Color.White,
                    0f,
                    new Vector2(FRAME_WIDTH/2, FRAME_HEIGHT),
                    1f,
                    faceDir==1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0f);
            sb.End();
        }
    }
}
