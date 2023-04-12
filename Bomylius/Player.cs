using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShapeUtils;

namespace Bomylius
{
    internal class Player
    {
        private Rectangle rect;

        private float playerSpeedX;
        private Vector2 playerVelocity;
        private Vector2 jumpVelocity;
        private Vector2 playerPosition;
        private Vector2 gravity;

        private GraphicsDevice graphicsDevice;

        private List<Rectangle> obstacleRects;
        private KeyboardState prevKB;

        public Player(GraphicsDevice graphicsDevice)
        {
            rect = new Rectangle(400, 100, 50, 50);

            playerPosition = new Vector2(400, 100);
            playerVelocity = Vector2.Zero;
            jumpVelocity = new Vector2(0, -15.0f);
            gravity = new Vector2(0, 0.5f);

            obstacleRects = new List<Rectangle>();
            obstacleRects.Add(new Rectangle(0, 400, 800, 50));

            playerSpeedX = 5.0f;
            this.graphicsDevice = graphicsDevice;
        }

        public void Update()
        {
            // Handle input, apply gravity and then deal with collisions
            rect.X = (int)playerPosition.X;
            rect.Y = (int)playerPosition.Y;

            ProcessInput();
            ApplyGravity();
            ResolveCollisions();

            // Save the old state at the end of the frame
            prevKB = Keyboard.GetState();
        }

        public void Draw()
        {
            ShapeBatch.Begin(graphicsDevice);

            ShapeBatch.Box(playerPosition.X, playerPosition.Y, rect.Width, rect.Height, Color.White);

            foreach (Rectangle r in obstacleRects)
            {
                ShapeBatch.Box(r, Color.Gray);
            }

            ShapeBatch.End();
        }

        /// <summary>
		/// Handles movement for sidescrolling game with gravity
		/// </summary>
		private void ProcessInput()
        {
            // PRACTICE EXERCISE: Finish this method!

            //sets player speed based on key press
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.A))
            {
                playerSpeedX = -5;
            }
            else if (kb.IsKeyDown(Keys.D))
            {
                playerSpeedX = 5;
            }
            else
            {
                playerSpeedX = 0;
            }

            //sets Y velocity is space is pressed (not held down)
            if (kb.IsKeyDown(Keys.Space) && SingleKeyPress(Keys.Space))
            {
                playerVelocity.Y = jumpVelocity.Y;
            }

            //sets player position to that of the speed
            playerPosition.X += playerSpeedX;
        }

        /// <summary>
		/// Applies gravity to the player
		/// </summary>
		private void ApplyGravity()
        {
            // PRACTICE EXERCISE: Finish this method!

            //changes the y value of the player's position based on gravity
            //adds gravity to the velocity
            playerVelocity += gravity;
            //adds velocity to the position
            playerPosition += playerVelocity;
        }

        /// <summary>
		/// Handles player collisions with obstacles
		/// </summary>
		private void ResolveCollisions()
        {
            // PRACTICE EXERCISE: Finish this method!

            //list of current collisions
            List<Rectangle> collisions = new List<Rectangle>();

            //adds obstacles to the collisions list if they are colliding with the player
            foreach (Rectangle obstacle in obstacleRects)
            {
                if (rect.Intersects(obstacle))
                {
                    collisions.Add(obstacle);
                }
            }

            //checks all the collsions to find out what type they are
            foreach (Rectangle collision in collisions)
            {
                Rectangle overlap = Rectangle.Intersect(rect, collision);

                //if the height is greater than or equal to the width, move the player on the X axis opposite to its collision
                if (overlap.Height >= overlap.Width)
                {
                    if (rect.X > collision.X)
                    {
                        rect.X += overlap.Width;
                    }
                    if (rect.X < collision.X)
                    {
                        rect.X -= overlap.Width;
                    }
                }

                //if the width is greater than or equal to the height, move the player on the Y axis opposite to its collision
                //also note that if the width is small enough do not count the collision. this stops random y collisions when hitting walls.
                if (overlap.Width > overlap.Height && overlap.Width > 5)
                {
                    if (rect.Y > collision.Y)
                    {
                        rect.Y += overlap.Height;
                    }
                    if (rect.Y < collision.Y)
                    {
                        rect.Y -= overlap.Height;
                    }
                    playerVelocity.Y = 0;
                }

                //sets the position to that of the playerRect
                playerPosition.X = rect.X;
                playerPosition.Y = rect.Y;
            }
        }

        /// <summary>
		/// Determines if a key was initially pressed this frame
		/// </summary>
		/// <param name="key">The key to check</param>
		/// <returns>True if this is the first frame the key is pressed, false otherwise</returns>
		private bool SingleKeyPress(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && prevKB.IsKeyUp(key);
        }
    }
}
