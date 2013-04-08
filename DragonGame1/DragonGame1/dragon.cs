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
    class dragon
    {
        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        private Texture2D SpriteTexture;
        public Vector2 Position = new Vector2(0, 0);
        
        private const int _dragonFrameSizeWidth = 150;
        private const int _dragonFrameSizeHeight = 130;
        private int _dragonCurrentFrameX = 0;
        private int _dragonCurrentFrameY = 0;
        private int _frameCounter = 0;
        private float _totalElapsed = 0;
        private float _timePerFrame = 0.3f; 
        private bool _moveLeft = true;
        private bool _isWaiting = false;
        private DateTime _startWait = DateTime.Now;
        List<FireBall> _Fireballs = new List<FireBall>();

        ContentManager _ContentManager;

        const string DRAGON_ASSETNAME = "dragonsheet";
        const int START_POSITION_X = 825;
        const int START_POSITION_Y = 530;
        const int DRAGON_SPEED = 160;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const int FIREBALLLIMIT = 1;
        const int SECONDSTOWAIT = 4;


        //load methode

        public void LoadContent(ContentManager theContentManager)
        {
            _ContentManager = theContentManager;

            foreach (FireBall fireball in _Fireballs)
            {
                fireball.LoadContent(theContentManager);
            }

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            SpriteTexture = theContentManager.Load<Texture2D>(DRAGON_ASSETNAME);
        }

        public void Update(GameTime TheGameTime)
        {
            UpdateMovement((float)TheGameTime.ElapsedGameTime.TotalSeconds);
            UpdateFireball(TheGameTime);
            //Updates dragon position
            if (!_isWaiting)
            {
                Position += mDirection * DRAGON_SPEED * (float)TheGameTime.ElapsedGameTime.TotalSeconds;
            }
            else {
                finishWat(DateTime.Now);
            }
        }

        //Updates each of the fireballs inn hte list.
        private void UpdateFireball(GameTime theGameTime)
        {
            foreach (FireBall fireball in _Fireballs)
            {
                fireball.Update(theGameTime);
            }
        }

        private void UpdateMovement(float elapsed)
        {
            _totalElapsed += elapsed;

            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (this.Position.X < 620) {
                    ShootFireball();
                    if(_moveLeft)
                    Wait(DateTime.Now);
                }


                if (this.Position.X < 600)
                {
                    _moveLeft = false;
                }

                if (this.Position.X > 1024)
                {
                    _moveLeft = true;
                }

                if (_moveLeft && _isWaiting == false)
                {
                    mSpeed.X = DRAGON_SPEED;
                    mDirection.X = MOVE_LEFT;
                    
                    if(_totalElapsed >_timePerFrame){
                        if (_frameCounter >= 3)
                        {
                            _frameCounter = 0;
                        }
                        else
                        {
                            _frameCounter++;
                        }
                        _totalElapsed -= _timePerFrame;
                    }
                    _dragonCurrentFrameX = _dragonFrameSizeWidth * _frameCounter;
                }
                else if (_moveLeft == false && _isWaiting == false)
                {
                    mSpeed.X = DRAGON_SPEED;
                    mDirection.X = MOVE_RIGHT;

                    if (_totalElapsed > _timePerFrame)
                    {
                        if (_frameCounter <= 4)
                        {
                            _frameCounter = 7;
                        }
                        else
                        {
                            _frameCounter--;
                        }
                        _totalElapsed -= _timePerFrame;
                    }
                    _dragonCurrentFrameX = _dragonFrameSizeWidth * _frameCounter;
                }
            }
        }

        private void ShootFireball()
        {
            if (mCurrentState == State.Walking)
            {
                bool createNew = true;
                foreach (FireBall fireball in _Fireballs)
                {
                    if (fireball.Visible == false)
                    {
                        createNew = false;
                        fireball.Fire(Position + new Vector2(_dragonFrameSizeWidth - 180, _dragonFrameSizeHeight - 120),
                            new Vector2(200, 0), new Vector2(1, 0), _moveLeft);
                        break;
                    }
                }

                if (createNew == true)
                {
                    FireBall fireball = new FireBall();
                    fireball.LoadContent(_ContentManager);
                    fireball.Fire(Position + new Vector2(_dragonFrameSizeWidth - 180, _dragonFrameSizeHeight - 120),
                        new Vector2(200, 200), new Vector2(1, 0), _moveLeft);
                    if(FIREBALLLIMIT > _Fireballs.Count)
                    _Fireballs.Add(fireball);
                }
            }
        }

        //Makes the dragon stand still for some seconds.
        private void Wait(DateTime now) {
            if (_startWait.AddSeconds(SECONDSTOWAIT + 3) < now)
            {
                _startWait = now;
                _isWaiting = true;
            }
        }

        private void finishWat(DateTime now) {
            if (_startWait.AddSeconds(SECONDSTOWAIT) < now) {
                _isWaiting = false;
            }
        }

        //draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (FireBall fireball in _Fireballs)
            {
                fireball.Draw(theSpriteBatch);
            }
            theSpriteBatch.Draw(SpriteTexture, Position, new Rectangle(_dragonCurrentFrameX, _dragonCurrentFrameY, _dragonFrameSizeWidth, _dragonFrameSizeHeight), Color.White);
        }

        //getters

        public List<FireBall> GetFireballs() {
            return _Fireballs;
        }

    }//END CLASS
}

    