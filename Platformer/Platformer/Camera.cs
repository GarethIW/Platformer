using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer
{
    public class Camera
    {
        // Current camera position (always top-left of camera rectangle)
        public Vector2 Position;

        // Target camera position (so that we can move the camera smoothly)
        public Vector2 Target;

        // Bounds = size of world, VisibleArea = camera's current position and size
        public Rectangle Bounds;
        public Rectangle VisibleArea;

        float speed = 0.1f; // How much we interpolate between our current position and target position each frame
        Viewport viewPort;

        public Camera(Viewport vp)
        {
            // A viewport represents our drawable surface - the bit we're interested in is Viewport.Bounds (a rectangle describing the size of our viewport, in this case our game's resolution)
            viewPort = vp;

            // Camera.Bounds is a rectangle describing the size of our world, we need this to be the pixel size of our map 
            Bounds = new Rectangle(0, 0, Map.MAP_WIDTH * Map.TILE_WIDTH, Map.MAP_HEIGHT * Map.TILE_HEIGHT);
        }

        public void Update(GameTime gameTime, Hero gameHero)
        {
            // Target our hero's position
            // Because we want the hero to be in the center of the screen, we subtract half the viewport's size
            Target = gameHero.Position - new Vector2(viewPort.Width/2, viewPort.Height/2);

            // Clamp our camera to our map boundary
            // Becuase the camera position is top left, we subtract the vewport's width and height (our screen resolution) from the bottom right of our map boundary
            Target = Vector2.Clamp(Target, new Vector2(Bounds.Left, Bounds.Top), new Vector2(Bounds.Right, Bounds.Bottom) - new Vector2(viewPort.Width, viewPort.Height));

            // Linearly intERPolate between our current position and our target
            Position = Vector2.Lerp(Position, Target, speed);

            // Update the camera' visible area
            VisibleArea = new Rectangle((int)Position.X, (int)Position.Y, viewPort.Width, viewPort.Height);
        }
    }
}
