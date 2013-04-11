using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DragonGame1
{
    [Serializable]
    public class Knight
    {
        [field:NonSerialized]
        SoundEffect Explotion;
        [field:NonSerialized]
        SoundEffect CoinSound;
        [field: NonSerialized]
        SoundEffect JumpSound;

        enum State
        {
            Walking,
            Jumping
        }
        State _CurrentState = State.Walking;
        Vector2 _StartingPosition = Vector2.Zero;

        //Initialize it to 0
        float AlphaTimeSubtract = 0.0f;
        float AlphaTime = 350f;
        Color color = Color.White;

        Vector2 _Direction = Vector2.Zero;
        Vector2 _Speed = Vector2.Zero;
        [field:NonSerialized]
        private Texture2D SpriteTexture;
        public Vector2 Position = new Vector2(0, 0);
        [field:NonSerialized]
        KeyboardState _PreviousKeyboardState;

        //Health texture og posision
        [field:NonSerialized]
        private Texture2D heartTexture;
        public Rectangle heartPosition;
        public int health = 3;
        
        //Status and id.
        private bool isActive = false;
        private int id = 1;

        private const int _knightFrameSizeWidth = 169;
        private const int _knightFrameSizeHeight = 144;
        private int _knightCurrentFrameX = _knightFrameSizeWidth * 3;
        private int _knightCurrentFrameY = 0;
        private int frameCounter = 0;
        private float _totalElapsed = 0;
        private float _timePerFrame = 0.2f;
        public bool isPlayerStandingOnGround = false;
        private bool _isOnWayDownInJump = false;
        private DateTime _hitStartTime = DateTime.MinValue;
        private bool _isHit = false;
        private int _secondsWhenHit = 3;

        const string KNIGHT_ASSETNAME = "Knightsheet";
        const int START_POSITION_X = 125;
        const int START_POSITION_Y = 50;
        const int KNIGHT_SPEED = 140;
        const int MOVE_UP = -2;
        const int MOVE_DOWN = 2;
        const int MOVE_LEFT = -2;
        const int MOVE_RIGHT = 2;

        //load methode
        public void LoadContent(ContentManager theContentManager)
        {
            Explotion = theContentManager.Load<SoundEffect>("explosion_2");
            CoinSound = theContentManager.Load<SoundEffect>("Coin_Sound_Effect");
            JumpSound = theContentManager.Load<SoundEffect>("Jump");
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            SpriteTexture = theContentManager.Load<Texture2D>(KNIGHT_ASSETNAME);
            heartTexture = theContentManager.Load<Texture2D>("heart");
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState, (float)theGameTime.ElapsedGameTime.TotalSeconds);
            UpdateJump(aCurrentKeyboardState);
            IsWalkingOrFalling();
            UpdateHit();

            _PreviousKeyboardState = aCurrentKeyboardState;

            if (_isHit) {
                //Then in the update method increase it (The inverse logic you used in the opaque --> transparent effect)
                AlphaTimeSubtract += (float)(theGameTime.ElapsedGameTime.TotalMilliseconds);
                color = Color.White * MathHelper.Clamp(AlphaTimeSubtract / AlphaTime, 0, 1);
            }
            
            //Updates knights position
            Position += _Direction * KNIGHT_SPEED * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (_CurrentState == State.Walking) { 
            }

            if (this.id == 1)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && _PreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    Jump();
                }
            }
            else {
                if (aCurrentKeyboardState.IsKeyDown(Keys.W) == true && _PreviousKeyboardState.IsKeyDown(Keys.W) == false)
                {
                    Jump();
                }
            }

            if (_CurrentState == State.Jumping)
            {

                if (Position.X < 0)
                {
                    _Speed.X = 0;
                    _Direction.X = 0;
                }

                if (Position.X > (1024 - _knightFrameSizeWidth)) {
                    _Speed.X = 0;
                    _Direction.X = 0;
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

        //Moves the knight in left or right direction.
        private void UpdateMovement(KeyboardState aCurrentKeyboardState, float elapsed)
        {
            _totalElapsed += elapsed;

            if (_CurrentState == State.Walking)
            {
                _Speed = Vector2.Zero;
                _Direction = Vector2.Zero;

                //if player 1
                if (this.id == 1)
                {
                    if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                    {
                        if (Position.X < 0)
                        {
                            _Speed.X = 0;
                            _Direction.X = 0;
                        }
                        else
                        {
                            _Speed.X = KNIGHT_SPEED;
                            _Direction.X = MOVE_LEFT;
                        }
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
                        if (Position.X > (1024 - _knightFrameSizeWidth))
                        {
                            _Speed.X = 0;
                            _Direction.X = 0;
                        }
                        else
                        {
                            _Speed.X = KNIGHT_SPEED;
                            _Direction.X = MOVE_RIGHT;
                        }

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
                }
                else
                {
                    //if player 2
                    if (aCurrentKeyboardState.IsKeyDown(Keys.A) == true)
                    {
                        if (Position.X < 0)
                        {
                            _Speed.X = 0;
                            _Direction.X = 0;
                        }
                        else
                        {
                            _Speed.X = KNIGHT_SPEED;
                            _Direction.X = MOVE_LEFT;
                        }
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

                    else if (aCurrentKeyboardState.IsKeyDown(Keys.D) == true)
                    {
                        if (Position.X > (1024 - _knightFrameSizeWidth))
                        {
                            _Speed.X = 0;
                            _Direction.X = 0;
                        }
                        else
                        {
                            _Speed.X = KNIGHT_SPEED;
                            _Direction.X = MOVE_RIGHT;
                        }

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
                }
            }
        }

        //Checks status on hits on knight
        private void UpdateHit() {
            if (_hitStartTime != DateTime.MinValue)
            {
                if (AlphaTimeSubtract > 500.0f)
                    AlphaTimeSubtract = 0.0f;

                if (_hitStartTime.AddSeconds(_secondsWhenHit).TimeOfDay < DateTime.Now.TimeOfDay)
                {
                    _isHit = false;
                    AlphaTimeSubtract = 0.0f;
                    color = Color.White;
                }
            }
        }

        //Makes the knight jump.
        private void Jump ()
        {
            if (_CurrentState != State.Jumping && isPlayerStandingOnGround)
            {
                JumpSound.Play();
                _CurrentState = State.Jumping;
                _StartingPosition = Position;
                _Direction.Y = MOVE_UP;
                _Speed = new Vector2(KNIGHT_SPEED, KNIGHT_SPEED);
            }
        }

        //The knight is hit by a fireball
        private void HitKnight() {
            if (!_isHit) {
                _hitStartTime = DateTime.Now;
                _isHit = true;
                health -= 1;
            }

        }

        //Method for checking if knight is falling or walking on ground.
        private void IsWalkingOrFalling() {
            //Check if knight is standing on ground or falling down.
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
                _Speed.Y = KNIGHT_SPEED;
                _Direction.Y = MOVE_DOWN;
            }
        }

        //Collisjon detection
        public void CollideWithFireBall(FireBall fireball) {
            Rectangle knightRectangle = new Rectangle((int)Position.X, (int)Position.Y, _knightFrameSizeWidth, _knightFrameSizeHeight);
            Rectangle fireballRecktangle = new Rectangle((int)fireball.Position.X, (int)fireball.Position.Y, fireball.GetWidth(), fireball.GetHeight());
            if (knightRectangle.Intersects(fireballRecktangle)) {

                if (_isHit == false)
                {
                    if (this.Position.X < fireball.Position.X)
                    {
                        this.Position.X += -15;
                    }
                    else
                    {
                        this.Position.X += 15;
                    }
                    Explotion.Play();
                    fireball.Visible = false;
                    fireball.Position.X = 0;
                    fireball.Position.Y = 0;
                }

                HitKnight();

            }
        }

        //Detects is knight is walking on ground.
        public void CollideWithWalkingGround(walkingground ground, int groundWidth, int groundHeight) {
            Rectangle knightRectangle = new Rectangle((int)Position.X, (int)Position.Y, _knightFrameSizeWidth, _knightFrameSizeHeight);
            Rectangle groundRecktangle = new Rectangle((int)ground.Position.X, (int)ground.Position.Y, groundWidth, groundHeight);
            if (knightRectangle.Intersects(groundRecktangle))
            {
                if (knightRectangle.Bottom < groundRecktangle.Top + 8 && knightRectangle.Bottom > groundRecktangle.Top - 2)
                {
                    isPlayerStandingOnGround = true;
                }
   
            }
        }

        //Collision detection for gold coins
        public void CollideWithGoldCoin(GoldCoin coin)
        {
            Rectangle knightRectangle = new Rectangle((int)Position.X, (int)Position.Y, _knightFrameSizeWidth, _knightFrameSizeHeight);
            Rectangle coinRecktangle = new Rectangle((int)coin.Position.X, (int)coin.Position.Y, coin.GetWidth(), coin.GetHeight());
            if (knightRectangle.Intersects(coinRecktangle))
            {
                coin.GenerateNewPosition();
                CoinSound.Play();
                //Todo: get point for getting coin
            }
        }

        //draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (health > 0)
            {
                int hjerteSomTegnes = 0;
                for (int i = 0; i < health; i++)
                {
                    if (this.id == 1) //Player 1
                    {
                        heartPosition = new Rectangle(10 + hjerteSomTegnes, 10, 60, 50);
                        theSpriteBatch.Draw(heartTexture, heartPosition, Color.White);
                        hjerteSomTegnes += 60;
                    }
                    else { //Player 2
                        if (this.isActive)
                        {
                            heartPosition = new Rectangle(960 - hjerteSomTegnes, 10, 60, 50);
                            theSpriteBatch.Draw(heartTexture, heartPosition, Color.White);
                            hjerteSomTegnes += 60;
                        }
                    }
                    
                }
            }
            theSpriteBatch.Draw(SpriteTexture, Position, new Rectangle(_knightCurrentFrameX, _knightCurrentFrameY, _knightFrameSizeWidth, _knightFrameSizeHeight), color);
        }

        //***********************
        //getter/setter
        //***********************

        public void SetIsActive(bool active) {
            this.isActive = active;
        }

        public bool GetIsActive() {
            return this.isActive;
        }

        public void SetId(int id) {
            this.id = id;
        }

        public int GetId() {
            return this.id;
        }



    }//END CLASS
}
