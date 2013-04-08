using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DragonGame1
{
    class Knight
    {
        enum State
        {
            Walking,
            Jumping
        }
        State mCurrentState = State.Walking;
        Vector2 mStartingPosition = Vector2.Zero;
     
        Vector2 _Direction = Vector2.Zero;
        Vector2 _Speed = Vector2.Zero;
        private Texture2D SpriteTexture;
        public Vector2 Position = new Vector2(0, 0);
        KeyboardState mPreviousKeyboardState;
        

        private const int _knightFrameSizeWidth = 169;
        private const int _knightFrameSizeHeight = 144;
        private int _knightCurrentFrameX = _knightFrameSizeWidth * 3;
        private int _knightCurrentFrameY = 0;
        private int frameCounter = 0;
        private float _totalElapsed = 0;
        private float _timePerFrame = 0.2f;

        const string KNIGHT_ASSETNAME = "Knightsheet";
        const int START_POSITION_X = 125;
        const int START_POSITION_Y = 510;
        const int KNIGHT_SPEED = 160;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        //load methode
        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            SpriteTexture = theContentManager.Load<Texture2D>(KNIGHT_ASSETNAME);
        }

        public void Update(GameTime TheGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState, (float)TheGameTime.ElapsedGameTime.TotalSeconds);
            UpdateJump(aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;
           
            //Updates knights position
            Position += _Direction * KNIGHT_SPEED * (float)TheGameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking) { 
            }

            if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
            {
                Jump();
            }

            if (mCurrentState == State.Jumping)
            {
                if (mStartingPosition.Y - Position.Y > 170)
                {
                    _Direction.Y = MOVE_DOWN;
                }

                if (Position.Y > mStartingPosition.Y)
                {
                    Position.Y = mStartingPosition.Y;
                    mCurrentState = State.Walking;
                    _Direction = Vector2.Zero;
                }
            }
        }


        private void UpdateMovement(KeyboardState aCurrentKeyboardState, float elapsed)
        {
            _totalElapsed += elapsed;

            if (mCurrentState == State.Walking)
            {
                _Speed = Vector2.Zero;
                _Direction = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    _Speed.X = KNIGHT_SPEED;
                    _Direction.X = MOVE_LEFT;
                    if (_totalElapsed > _timePerFrame)
                    {
                        if (frameCounter >= 2)
                        {
                            frameCounter = 0;
                        }
                        else
                        {
                            frameCounter++;
                        }
                        _totalElapsed -= _timePerFrame;
                    }
                    _knightCurrentFrameX = _knightFrameSizeWidth * frameCounter;
                }

                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    _Speed.X = KNIGHT_SPEED;
                    _Direction.X = MOVE_RIGHT;
                    if (_totalElapsed > _timePerFrame)
                    {
                        if (frameCounter <= 3)
                        {
                            frameCounter = 5;
                        }
                        else
                        {
                            frameCounter--;
                        }
                        _totalElapsed -= _timePerFrame;
                    }
                    _knightCurrentFrameX = _knightFrameSizeWidth * frameCounter;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    _Speed.Y = KNIGHT_SPEED;
                    _Direction.Y = MOVE_UP;
                }

                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    _Speed.Y = KNIGHT_SPEED;
                    _Direction.Y = MOVE_DOWN;
                }
            }
            
        }
        private void Jump ()
        {
            if(mCurrentState != State.Jumping)
            {
                mCurrentState = State.Jumping;
                mStartingPosition = Position;
                _Direction.Y = MOVE_UP;
                _Speed = new Vector2(KNIGHT_SPEED, KNIGHT_SPEED);
            }
        }


        //Collisjon detection
        public void CollideWithFireBall(FireBall fireball) {
            Rectangle knightRectangle = new Rectangle((int)Position.X, (int)Position.Y, _knightFrameSizeWidth, _knightFrameSizeHeight);
            Rectangle fireballRecktangle = new Rectangle((int)fireball.Position.X, (int)fireball.Position.Y, fireball.GetWidth(), fireball.GetHeight());
            if (knightRectangle.Intersects(fireballRecktangle)) {
                
                if (this.Position.X < fireball.Position.X)
                {
                    this.Position.X += -15;
                }
                else {
                    this.Position.X += 15;
                }
                fireball.Visible = false;
                fireball.Position.X = 0;
                fireball.Position.Y = 0;
   
            }
        }

        public void CollideWithWalkingGround(walkingground ground, int groundWidth, int groundHeight) {
            Rectangle knightRectangle = new Rectangle((int)Position.X, (int)Position.Y, _knightFrameSizeWidth, _knightFrameSizeHeight);
            Rectangle groundRecktangle = new Rectangle((int)ground.Position.X, (int)ground.Position.Y, groundWidth, groundHeight);
            if (knightRectangle.Intersects(groundRecktangle))
            {
               //Todo: detect if knight is walking on ground

            }
        }

        //draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(SpriteTexture, Position, new Rectangle(_knightCurrentFrameX, _knightCurrentFrameY, _knightFrameSizeWidth, _knightFrameSizeHeight), Color.White);
        }

    }//END CLASS
}
