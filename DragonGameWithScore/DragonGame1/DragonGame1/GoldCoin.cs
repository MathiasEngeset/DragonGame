using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DragonGame1
{
    [Serializable]
    public class GoldCoin
    {
        const string COIN_ASSETNAME = "gold coins";
        public bool Visible = false;
        public Vector2 Position = new Vector2(0, 0);
        [field:NonSerialized]
        private Texture2D SpriteTexture;

        public const int _frameSizeWidth = 27;
        public const int _frameSizeHeight = 27;
        private int _currentFrameX = 0;
        private int _currentFrameY = 0;
        private int _frameCounter = 0;
        private float _totalElapsed = 0;
        private float _timePerFrame = 0.2f;

        public GoldCoin() { }

        public GoldCoin(ContentManager theContentManager, float x, float y) {
            Visible = true;
            Position = new Vector2(x, y);
            LoadContent(theContentManager);
        }

        //Loads content and image
        public void LoadContent(ContentManager theContentManager)
        {
            SpriteTexture = theContentManager.Load<Texture2D>(COIN_ASSETNAME);
        }

        public void Update(GameTime theGameTime)
        {
            UpdateCoin((float)theGameTime.ElapsedGameTime.TotalSeconds);
        }

        //Updates coin so it spins around
        private void UpdateCoin(float elapsed) {
            _totalElapsed += elapsed;
            if (_totalElapsed > _timePerFrame)
            {
                if (_frameCounter >= 7)
                {
                    _frameCounter = 0;
                }
                else
                {
                    _frameCounter++;
                }
                _totalElapsed -= _timePerFrame;
            }
            _currentFrameX = _frameSizeWidth * _frameCounter;
        }


        //Give coin new position
        public void GenerateNewPosition() {
            Random random = new Random();
            Position = new Vector2(random.Next(1, 1000), random.Next(1, 500));
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {
                theSpriteBatch.Draw(SpriteTexture, Position, new Rectangle(_currentFrameX, _currentFrameY, _frameSizeWidth, _frameSizeHeight), Color.White);
            }
        }

        //Getters

        public int GetWidth()
        {
            return _frameSizeWidth;
        }

        public int GetHeight()
        {
            return _frameSizeHeight;
        }
    }
}
