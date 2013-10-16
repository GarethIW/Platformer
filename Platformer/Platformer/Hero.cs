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

        const float ACCELERATION = 0.5f;
        const float INERTIA = 0.1f;
        const float JUMP_SPEED = 8f;

        public Vector2 Position;
        public Vector2 Speed;

        bool isJumping = false;

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
            Speed.X = MathHelper.Lerp(Speed.X, 0f, INERTIA);
            Speed.Y += Map.GRAVITY;

            Position += Speed;

            // Temporary!
            if (Position.Y > 720)
            {
                Position.Y = 720;
                Speed.Y = 0;
                isJumping = false;
            }
        }

        public void MoveHorizontal(float dir)
        {
            Speed.X += (ACCELERATION * dir);
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
                    new Rectangle(0,0,FRAME_WIDTH, FRAME_HEIGHT),
                    Color.White,
                    0f,
                    new Vector2(FRAME_WIDTH/2, FRAME_HEIGHT),
                    1f,
                    SpriteEffects.None,
                    0f);
            sb.End();
        }
    }
}
