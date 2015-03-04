using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Asteroids {
    class PlayerManager {

        #region Declarations

        public Sprite playerSprite;
        private float playerSpeed = 250.0f;
        private Rectangle playerAreaLimit;

        public long playerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;

        private Vector2 gunOffSet = new Vector2(25, 10);
        private float shotTimer = 0.0f;
        private float minShotTimer = 0.2f;
        private int playerRadius = 15;
        public ShotManager PlayerShotManager;

        PlayerIndex playerIndex;

        #endregion

        #region DefaultMethods

        public PlayerManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            Rectangle screenBounds,
            PlayerIndex playerIndex) {


            this.playerIndex = playerIndex;

                playerSprite = new Sprite(
                    new Vector2(500, 500),
                    texture,
                    initialFrame,
                    Vector2.Zero);

                PlayerShotManager = new ShotManager(
                    texture,
                    new Rectangle(0, 300, 5, 5),
                    4, 2, 250f, screenBounds);

                playerAreaLimit = new Rectangle(
                    0,
                    screenBounds.Height / 2,
                    screenBounds.Width,
                    screenBounds.Height / 2);

                for (int i = 1; i < frameCount; ++i) {
                    playerSprite.AddFrame(
                        new Rectangle(
                            initialFrame.X + (initialFrame.Width * i),
                            initialFrame.Y,
                            initialFrame.Width,
                            initialFrame.Height));
                }

                playerSprite.CollisionRadius = playerRadius;

        }

        public void Update(GameTime gameTime) {
            PlayerShotManager.Update(gameTime);
            if (!Destroyed) {
                playerSprite.Velocity = Vector2.Zero;
                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                HandleKeyboardInput(Keyboard.GetState()); 
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One)); 
                playerSprite.Velocity.Normalize(); 
                playerSprite.Velocity *= playerSpeed;
                playerSprite.Update(gameTime); 
                imposeMovement();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            PlayerShotManager.Draw(spriteBatch);
            if (!Destroyed) { playerSprite.Draw(spriteBatch); }
        }


        #endregion

        #region HelperMethods

        private void FireShot() {
            if (shotTimer >= minShotTimer) {
                PlayerShotManager.FireShot(
                    playerSprite.Location + gunOffSet,
                    new Vector2(0, -1),
                    true);
                shotTimer = 0.0f;
            }
        }

        private void HandleKeyboardInput(KeyboardState keyState) {
            if (keyState.IsKeyDown(Keys.Up)) {
                playerSprite.Velocity += new Vector2(0, -1);
            }
            if (keyState.IsKeyDown(Keys.Down)) {
                playerSprite.Velocity += new Vector2(0, 1);
            }
            if (keyState.IsKeyDown(Keys.Left)) {
                playerSprite.Velocity += new Vector2(-1, 0);
            }
            if (keyState.IsKeyDown(Keys.Right)) {
                playerSprite.Velocity += new Vector2(1, 0);
            }
            if (keyState.IsKeyDown(Keys.Z)) {
                FireShot();
            }
        }

        private void HandleGamepadInput(GamePadState gamePadState) {
            playerSprite.Velocity += new Vector2(
                gamePadState.ThumbSticks.Left.X,
                -gamePadState.ThumbSticks.Left.Y);

            if (gamePadState.Buttons.A == ButtonState.Pressed) {
                FireShot();
            }

        }

        private void imposeMovement() {
            Vector2 location = playerSprite.Location;

            if (location.X < playerAreaLimit.X) {
                location.X = playerAreaLimit.X;
            }
            if (location.X > (playerAreaLimit.Right - playerSprite.Source.Width)) {
                location.X = (playerAreaLimit.Right - playerSprite.Source.Width);
            }
            if (location.Y < playerAreaLimit.Y) {
                location.Y = playerAreaLimit.Y;
            }
            if (location.Y > (playerAreaLimit.Bottom - playerSprite.Source.Height)) {
                location.Y = (playerAreaLimit.Bottom - playerSprite.Source.Height);
            }
        }

        #endregion
    }
}
