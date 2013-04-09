using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DragonGame1
{
    public class FireBall
    {
        const string FIREBALL_ASSETNAME = "fireballs";
        const int DISTANCE = 620;
        public bool Visible = false;
        Vector2 _StartPosition;
        Vector2 _Speed;
        Vector2 _Direction;
        public Vector2 Position = new Vector2(0, 0);
        private Texture2D SpriteTexture;

        public const int _frameSizeWidth = 85;
        public const int _frameSizeHeight = 85;
        private int _currentFrameX = 85 * 3;
        private int _currentFrameY = 0;
        private int _frameCounter = 0;
        private bool _directionIsLeft = false;

        //Loads content and image
        public void LoadContent(ContentManager theContentManager)
        {
            SpriteTexture = theContentManager.Load<Texture2D>(FIREBALL_ASSETNAME); 
        }

        public void Update(GameTime theGameTime)
        {
            if (Vector2.Distance(_StartPosition, Position) > DISTANCE)
            {
                Visible = false;
            }

            if (Visible == true)
            {
                if (!_directionIsLeft)
                {
                    if (_frameCounter >= 5)
                    {
                        _frameCounter = 0;
                    }
                    else
                    {
                        _frameCounter++;
                    }
                    _currentFrameX = _frameSizeWidth * _frameCounter;
                    Position += _Direction * _Speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
                }
                else {
                    if (_frameCounter <= 6)
                    {
                        _frameCounter = 12;
                    }
                    else
                    {
                        _frameCounter--;
                    }
                    _currentFrameX = _frameSizeWidth * _frameCounter;
                    Position -= _Direction * _Speed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
                }
                
            }
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection, bool directionIsLeft)
        {
            Position = theStartPosition;
            _StartPosition = theStartPosition;
            _Speed = theSpeed;
            _Direction = theDirection;
            Visible = true;
            _directionIsLeft = directionIsLeft;

            //If direction is to the right
            if (!directionIsLeft) {
                _frameCounter = 11;
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {
                theSpriteBatch.Draw(SpriteTexture, Position, new Rectangle(_currentFrameX, _currentFrameY, _frameSizeWidth, _frameSizeHeight), Color.White);
            }
        }

        //Getters

        public int GetWidth() {
            return _frameSizeWidth;
        }

        public int GetHeight() {
            return _frameSizeHeight;
        }

    }
}
