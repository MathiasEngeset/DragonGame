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
            Walking,
            Jumping
        }
        State _CurrentState = State.Walking;
        Vector2 _StartingPosition = Vector2.Zero;

        Vector2 _Direction = Vector2.Zero;
        Vector2 _Speed = Vector2.Zero;
        private Texture2D SpriteTexture;
        public Vector2 Position = new Vector2(0, 0);
        
        private const int _dragonFrameSizeWidth = 150;
        private const int _dragonFrameSizeHeight = 130;
        private int _dragonCurrentFrameX = 0;
        private int _dragonCurrentFrameY = 0;
        private int _frameCounter = 0;
        private float _totalElapsed = 0;
        private float _timePerFrame = 0.3f;
        public bool isPlayerStandingOnGround = false;
        private bool _isOnWayDownInJump = false;
        private bool _moveLeft = false;
        private int _waitPosition = 620;
        private bool _isWaiting = false;
        private DateTime _startWait = DateTime.Now;
        List<FireBall> _Fireballs = new List<FireBall>();
        private int seed = 1;
        private bool _isVisible = false;

        ContentManager _ContentManager;

        const string DRAGON_ASSETNAME = "dragonsheet";
        const int START_POSITION_Y = -200;
         int DRAGON_SPEED = 160;
        const int DRAGON_SPEED_LIMIT = 300;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        int FIREBALLLIMIT = 1;
        const int SECONDSTOWAIT = 4;

        //constructor
        public dragon(int seed){
            this.seed = seed;
        }

        //load methode

        public void LoadContent(ContentManager theContentManager)
        {
            Random rand = new Random(seed);
            _ContentManager = theContentManager;
            
            foreach (FireBall fireball in _Fireballs)
            {
                fireball.LoadContent(theContentManager);
            }

            _Direction.X = MOVE_RIGHT;
            if (seed > 20)
            {
                DRAGON_SPEED = rand.Next(100, 180);
            }
            else {
                DRAGON_SPEED = rand.Next(90, 200);
            }
            Position = new Vector2(rand.Next(200, 850), START_POSITION_Y);
            SpriteTexture = theContentManager.Load<Texture2D>(DRAGON_ASSETNAME);
        }

        //Updates the dragons movement
        public void Update(GameTime TheGameTime)
        {
            if(_isVisible){
                UpdateMovement((float)TheGameTime.ElapsedGameTime.TotalSeconds);
                UpdateFireball(TheGameTime);
                UpdateJump();
                IsWalkingOrFalling();
                //Updates dragon position
                if (!_isWaiting)
                {
                    Position += _Direction * DRAGON_SPEED * (float)TheGameTime.ElapsedGameTime.TotalSeconds;
                }
                else {
                    finishWait(DateTime.Now);
                }
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

        //Moves dragon in direcion left or right withing game main frame.
        private void UpdateMovement(float elapsed)
        {
            _totalElapsed += elapsed;

            if (_CurrentState == State.Walking)
            {
                _Speed = Vector2.Zero;
                _Direction = Vector2.Zero;

                if (this.Position.X < _waitPosition) {
                    ShootFireball();
                    if(_moveLeft)
                    Wait(DateTime.Now);
                }


                if (Position.X < 0)
                {
                    _Direction.X = MOVE_RIGHT;
                    _moveLeft = false;
                }

                if (Position.X > (1024 - _dragonFrameSizeWidth))
                {
                    //Put the dragon back on top of the game
                    if (Position.Y > 350)
                    {
                        Position.Y = 0;
                        Position.X = 0;
                    }
                    else
                    {
                        _Direction.X = MOVE_LEFT;
                        _moveLeft = true;
                    }
                }

                if (_moveLeft && _isWaiting == false)
                {
                    _Speed.X = DRAGON_SPEED;
                    _Direction.X = MOVE_LEFT;
                    
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
                    _Speed.X = DRAGON_SPEED;
                    _Direction.X = MOVE_RIGHT;

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

        //Updates the dragon jumping
        private void UpdateJump()
        {
            if (_CurrentState == State.Walking)
            {
            }

            if (_CurrentState == State.Jumping)
            {

                if (Position.X < 0)
                {
                    _Direction.X = MOVE_RIGHT;
                    _moveLeft = false;
                }

                if (Position.X > (1024 - _dragonFrameSizeWidth))
                {
                    _Direction.X = MOVE_LEFT;
                    _moveLeft = true;   
                }

                if (_StartingPosition.Y - Position.Y > 190)
                {
                    _isOnWayDownInJump = true;
                    _Direction.Y = MOVE_DOWN;
                }

                if (Position.Y > _StartingPosition.Y)
                {
                    Position.Y = _StartingPosition.Y;
                    _CurrentState = State.Walking;
                    _Direction = Vector2.Zero;
                    _isOnWayDownInJump = false;
                }
            }
        }

        //Make dragon shoot fireballs
        private void ShootFireball()
        {
            if (_CurrentState == State.Walking)
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

        //Makes the dragon jump.
        private void Jump()
        {
            if (_CurrentState != State.Jumping && isPlayerStandingOnGround)
            {
                _CurrentState = State.Jumping;
                _StartingPosition = Position;
                _Direction.Y = MOVE_UP;
                _Speed = new Vector2(DRAGON_SPEED, DRAGON_SPEED);
            }
        }

        //Makes the dragon stand still for some seconds.
        private void Wait(DateTime now) {
            if (_startWait.AddSeconds(SECONDSTOWAIT + 3) < now && isPlayerStandingOnGround)
            {
                _startWait = now;
                _isWaiting = true;
            }
        }

        private void finishWait(DateTime now) {
            if (_startWait.AddSeconds(SECONDSTOWAIT) < now) {
                _isWaiting = false;
                Random rand = new Random();
                if (_moveLeft)
                {
                    _moveLeft = false;
                    Jump();
                }

                if (_waitPosition == 620)
                {
                    _waitPosition =3 * seed + DRAGON_SPEED;
                }
                else
                {
                    _waitPosition = 620;
                }

            }
        }

        //Method for checking if dragon is falling or walking on ground.
        private void IsWalkingOrFalling()
        {
            //Check if dragon is standing on ground or falling down.
            if (isPlayerStandingOnGround)
            {
                if (_CurrentState == State.Jumping && _isOnWayDownInJump)
                {
                    _Speed.Y = 0;
                    _Direction.Y = 0;
                    _isOnWayDownInJump = false;
                    _CurrentState = State.Walking;
                }
                else if (_CurrentState != State.Jumping)
                {
                    _Speed.Y = 0;
                }
            }
            else if (_CurrentState != State.Jumping)
            {
                //Gravity.
                _Speed.Y = DRAGON_SPEED;
                _Direction.Y = MOVE_DOWN;
            }
        }

        //Collision detection

        //Detects is dragon is walking on ground.
        public void CollideWithWalkingGround(walkingground ground, int groundWidth, int groundHeight)
        {
            Rectangle dragonRectangle = new Rectangle((int)Position.X, (int)Position.Y, _dragonFrameSizeWidth, _dragonFrameSizeHeight);
            Rectangle groundRecktangle = new Rectangle((int)ground.Position.X, (int)ground.Position.Y, groundWidth, groundHeight);
            if (dragonRectangle.Intersects(groundRecktangle))
            {
                if (dragonRectangle.Bottom < groundRecktangle.Top + 18 && dragonRectangle.Bottom > groundRecktangle.Top + 12)
                {
                    isPlayerStandingOnGround = true;
                }

            }
        }

        //Speeds things up
        public void SpeedUpTheDragon() {
            if (DRAGON_SPEED < DRAGON_SPEED_LIMIT) {
                DRAGON_SPEED += seed;
            }

            if (FIREBALLLIMIT < 5) {
                FIREBALLLIMIT++;
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

        //setter
        public void SetIsVisible(bool visible) {
            _isVisible = visible;
        }

        //getters

        public List<FireBall> GetFireballs() {
            return _Fireballs;
        }

        public bool GetIsVisible() {
            return _isVisible;
        }

    }//END CLASS
}

    