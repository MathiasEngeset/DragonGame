using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DragonGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DragonGame1 : Microsoft.Xna.Framework.Game
    {
        Song lizzy_elisabethan_period_music_track;
        bool songstart = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Knight mKnightSprite;
        dragon mdragonSprite;
        List<Bushbackground> farBackgroundList;
        List<Bushbackground> nearBackgroundList;
        List<walkingground> walkinggroundList;
        List<walkingground> walkwayList;
        List<GoldCoin> goldCoinList;

        enum GameStates{
            Paused,
            Playing
        }
        GameStates CurrentGameState = GameStates.Playing;


        public DragonGame1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
      
            farBackgroundList = new List<Bushbackground>();
            nearBackgroundList = new List<Bushbackground>();
            walkinggroundList = new List<walkingground>();
            walkwayList = new List<walkingground>();
            goldCoinList = new List<GoldCoin>();
            mKnightSprite = new Knight();
            mdragonSprite = new dragon();
       
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            lizzy_elisabethan_period_music_track = Content.Load<Song>("lizzy_elizabethan_period_music_track");
            MediaPlayer.IsRepeating = true;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 0, 0));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256, 0));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256 * 2, 0));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256 *3, 0));

            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 0, 256));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256, 256));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256 * 2, 256));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256 * 3, 256));

            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 0, 256 * 2));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256, 256 * 2));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256 * 2, 256 * 2));
            farBackgroundList.Add(new Bushbackground("Far Background", this.Content, 256 * 3, 256 * 2));

            nearBackgroundList.Add(new Bushbackground("Near Background", this.Content, 0,0));
            nearBackgroundList.Add(new Bushbackground("Near Background", this.Content, 512, 0));
            nearBackgroundList.Add(new Bushbackground("Near Background", this.Content, 0, 512));
            nearBackgroundList.Add(new Bushbackground("Near Background", this.Content, 512 , 512));

            walkinggroundList.Add(new walkingground("Wall 1 NW", this.Content, 0, 640));
            walkinggroundList.Add(new walkingground("Wall 2 NE", this.Content, 128, 640));
            walkinggroundList.Add(new walkingground("Wall 2 NW", this.Content, 128 * 2, 640));
            walkinggroundList.Add(new walkingground("Wall 2 NE", this.Content, 128 * 3, 640));
            walkinggroundList.Add(new walkingground("Wall 2 NW", this.Content, 128 * 4, 640));
            walkinggroundList.Add(new walkingground("Wall 2 NE", this.Content, 128 * 5, 640));
            walkinggroundList.Add(new walkingground("Wall 2 NW", this.Content, 128 * 6, 640));
            walkinggroundList.Add(new walkingground("Wall 1 NE", this.Content, 128 * 7, 640));

            //first level wakways
            walkwayList.Add(new walkingground("Walkway 1 W", this.Content, 64 * 1, 480));
            walkwayList.Add(new walkingground("Walkway 2 E", this.Content, 64 * 2, 480));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 3, 480));
            walkwayList.Add(new walkingground("Walkway 1 E", this.Content, 64 * 4, 480));
            walkwayList.Add(new walkingground("Walkway 1 W", this.Content, 64 * 9, 480));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 10, 480));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 11, 480));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 12, 480));
            walkwayList.Add(new walkingground("Walkway 1 E", this.Content, 64 * 13, 480));

            //Scond level walkways
            walkwayList.Add(new walkingground("Walkway 1 W", this.Content, 64 * 0, 300));
            walkwayList.Add(new walkingground("Walkway 2 E", this.Content, 64 * 1, 300));
            walkwayList.Add(new walkingground("Walkway 1 w", this.Content, 64 * 3, 300));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 4, 300));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 5, 300));
            walkwayList.Add(new walkingground("Walkway 1 E", this.Content, 64 * 6, 300));
            walkwayList.Add(new walkingground("Walkway 1 W", this.Content, 64 * 12, 300));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 13, 300));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 14, 300));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 15, 300));
            walkwayList.Add(new walkingground("Walkway 1 E", this.Content, 64 * 16, 300));

            //Third level walkways
            walkwayList.Add(new walkingground("Walkway 1 W", this.Content, 64 * 2, 130));
            walkwayList.Add(new walkingground("Walkway 2 E", this.Content, 64 * 3, 130));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 4, 130));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 5, 130));
            walkwayList.Add(new walkingground("Walkway 1 E", this.Content, 64 * 6, 130));
            walkwayList.Add(new walkingground("Walkway 1 W", this.Content, 64 * 11, 130));
            walkwayList.Add(new walkingground("Walkway 2 E", this.Content, 64 * 12, 130));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 13, 130));
            walkwayList.Add(new walkingground("Walkway 2 W", this.Content, 64 * 14, 130));
            walkwayList.Add(new walkingground("Walkway 1 E", this.Content, 64 * 15, 130));

            goldCoinList.Add(new GoldCoin(this.Content, 600, 300));
            goldCoinList.Add(new GoldCoin(this.Content, 100, 230));
            goldCoinList.Add(new GoldCoin(this.Content, 800, 400));
            goldCoinList.Add(new GoldCoin(this.Content, 400, 100));

            mKnightSprite.LoadContent(this.Content);
            mdragonSprite.LoadContent(this.Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!songstart)
            {
                MediaPlayer.Play(lizzy_elisabethan_period_music_track);
                songstart = true;
            }
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            // Allows the game to exit
            if ((CurrentKeyboardState.IsKeyDown(Keys.Escape)))
            {
                MediaPlayer.Stop();
                this.Exit();
            }
            
            //Pause gameplay
            if (CurrentGameState == GameStates.Playing)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.P))
                {
                    MediaPlayer.Pause();
                    CurrentGameState = GameStates.Paused;
                }
            }
            else if (CurrentGameState == GameStates.Paused)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.P))
                {
                    MediaPlayer.Resume();
                    CurrentGameState = GameStates.Playing;
                }
            }

            if (CurrentGameState != GameStates.Paused)
            {
                //update logic here
                mKnightSprite.Update(gameTime);
                mdragonSprite.Update(gameTime);

                List<FireBall> fireballs = mdragonSprite.GetFireballs();
                foreach (FireBall fireball in fireballs)
                {
                    mKnightSprite.CollideWithFireBall(fireball);
                }

                foreach (GoldCoin goldcoin in goldCoinList)
                {
                    goldcoin.Update(gameTime);
                }

                //Maks knight fall if knight is no longer on ground.
                mKnightSprite.isPlayerStandingOnGround = false;
                //Check if knight is on ground
                foreach (walkingground ground in walkwayList)
                {
                    mKnightSprite.CollideWithWalkingGround(ground, 64, 64);
                }

                //Check if knight is on ground
                foreach (walkingground ground in walkinggroundList)
                {
                    mKnightSprite.CollideWithWalkingGround(ground, 128, 128);
                }

                //Check if knight collects coin
                foreach (GoldCoin coin in goldCoinList)
                {
                    mKnightSprite.CollideWithGoldCoin(coin);
                }

                base.Update(gameTime);
            }
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (Bushbackground background in farBackgroundList)
            {
                background.Draw(spriteBatch);
            }

            foreach (Bushbackground background in nearBackgroundList)
            {
                background.Draw(spriteBatch);

            }

            foreach (walkingground background in walkinggroundList)
            {
                background.Draw(spriteBatch);
            }

            foreach (walkingground walkway in walkwayList)
            {
                walkway.Draw(spriteBatch);
            }

            foreach (GoldCoin goldcoin in goldCoinList)
            {
                goldcoin.Draw(spriteBatch);
            }

            mKnightSprite.Draw(this.spriteBatch);
            mdragonSprite.Draw(this.spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
