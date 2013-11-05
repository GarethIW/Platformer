using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        int currentFrame = 0;
        double currentFrameTime = 0;

        int faceDir = Platformer.Rand.Next(2);

        // Collisions Bounds
        Rectangle collRect;

        // Spritesheet
        Texture2D spriteSheet;

        public Enemy(Texture2D sheet, Vector2 pos)
        {
            Position = pos;

            spriteSheet = sheet;
        }

        public void Update(GameTime gameTime, Map gameMap)
        {

        }

        public void MoveHorizontal(int dir)
        {
            // Accelerate in our movement direction
            Speed.X += (ACCELERATION * (float)dir);
            // Set facing direction
            faceDir = dir;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet,
                    Position,
                    new Rectangle(FRAME_WIDTH * currentFrame, 0, FRAME_WIDTH, FRAME_HEIGHT),
                    Color.White,
                    0f,
                    new Vector2(FRAME_WIDTH / 2, FRAME_HEIGHT),
                    1f,
                    faceDir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0f);
        }
    }
}