using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids{
    class Sprite {
        #region Declarations
        public Texture2D texture;

        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth=0; //Tamaño de fotograma
        private int frameHeight = 0; // ^
        private int currentFrame; //Frame actual, el que se muestra
        private float frameTime = 0.1f; //Cuanto tarda en cambiarse
        private float timeForCurrentFrame = 0.0f; //contador para cambiar

        private Color tintColor = Color.White;
        private float rotation = 0.0f;

        public int CollisionRadius = 0; //Radio de colisión
        public int BoundingXPadding = 0; //Detección de colisiones en X
        public int BoundingYPadding = 0; // " " " " en Y

        protected Vector2 location = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;


        #endregion

        #region DefaultMethods

        public Sprite(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity)
        {

                this.location = location;
                this.texture = texture;
                this.velocity = velocity;

                frames.Add(initialFrame);
                frameWidth = initialFrame.Width;
                frameHeight = initialFrame.Height;

        }

        public virtual void Update(GameTime gameTime) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= frameTime) {
                currentFrame = (currentFrame+1) % (frames.Count);
                timeForCurrentFrame = 0.0f;
            }

            location += (velocity*elapsed); //Para evitar los lags XD
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture,
                Center,
                Source,
                tintColor,
                rotation,
                new Vector2(frameWidth/2,frameHeight/2)
                ,1.0f
                ,SpriteEffects.None,
                0.0f);
        }

        public Vector2 Location {
            get { return location ;}
            set { location = value; }
        }

        public Vector2 Velocity {
            get {return velocity;}
            set {velocity=value;}
        }

        public Color TintColor {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public int Frame {
            get { return currentFrame;}
            set { currentFrame = (int)MathHelper.Clamp(value,0,frames.Count-1);}
        }

        public float FrameTime {
            get { return frameTime;}
            set { frameTime = MathHelper.Max(0,value);}
        }

        public Rectangle Source {
            get { return frames[currentFrame];}
        }

        public Rectangle Destination {
            get { return new Rectangle((int)location.X,(int)location.Y,frameWidth,frameHeight);}
            
        }

        public Vector2 Center {
            get { return location + new Vector2(frameWidth/2,frameHeight/2);}
        }



        #endregion

        #region Methods

        public Rectangle BoundingBoxRectangle {
            get { return new Rectangle(
                    (int)location.X+BoundingXPadding,
                    (int)location.Y+BoundingYPadding,
                    frameWidth-(2*BoundingXPadding),
                    frameHeight-(2*BoundingYPadding));
                }
        }

        public bool isBoxColliding(Rectangle OtherBox) {
            return BoundingBoxRectangle.Intersects(OtherBox);
        }

        public bool isCircleColliding(Vector2 otherCenter,float otherRadius) {
            if (Vector2.Distance(Center, otherCenter) < (CollisionRadius + otherRadius)) {
                return true;
            }
            else {
                return false;
            }
        }

        #region Animations

        public void AddFrame(Rectangle frameRectangle) {
            frames.Add(frameRectangle);

        }
            
        #endregion

        #endregion
    }
}
