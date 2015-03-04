using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Asteroids {
    class CollisionManager {

        #region Declarationes

        private AsteriodManager asteroidManager;
        private PlayerManager playerManager;
        private ExplosionManager explosionManager;

        private Vector2 offScreen = new Vector2(-500, -500);
        private Vector2 shotToAsteroidImpact = new Vector2(0, -20);

        #endregion

        #region Default Methods
        public CollisionManager(
            AsteriodManager asteroidManager,
            PlayerManager playerManager,
            ExplosionManager explosionManager) {

            this.asteroidManager = asteroidManager;
            this.playerManager = playerManager;
            this.explosionManager = explosionManager;
        }

        #endregion
        /*
         *player -> asteroids
         *asteroid shots player
         *player shots asteroids
         */
        #region
        private void checkShotToAsteroidCollision() {
            foreach (Sprite shot in playerManager.PlayerShotManager.Shots){
                foreach (Sprite asteroid in asteroidManager.Asteroids) {
                    if(shot.isCircleColliding(asteroid.Center,asteroid.CollisionRadius)){
                        shot.Location = offScreen;
                        asteroid.Velocity += shotToAsteroidImpact;
                        
                    }
                }
            }
        }

        private void checkAsteroidToPlayerCollisions() {
            foreach (Sprite asteroid in asteroidManager.Asteroids) {
                if(asteroid.isCircleColliding(playerManager.playerSprite.Center,playerManager.playerSprite.CollisionRadius)){
                    asteroid.Location = offScreen;
                    playerManager.Destroyed = true;
                }
            }
        }

       public void checkCollisions() {
            checkShotToAsteroidCollision();

            if (!playerManager.Destroyed) {
                checkAsteroidToPlayerCollisions();
            }
        }
        

        #endregion
    }

}
