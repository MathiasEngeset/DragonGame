using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    [Serializable]
    public class DragonGame1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont UVfont;
        SpriteFont countdownFont;
        Song lizzy_elisabethan_period_music_track;
        Song bakgrunnsmusikk_2;
        bool songstart = false;
        bool mutesong = false;
        int secondsBeforeSpeedUp = 30;
        float countdown = 0; 
        float countdownStartime = 180; //3min in seconds
        string countdownString = string.Empty;
        private float totalElapsed = 0;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Knight mKnightSprite;
        dragon mdragonSprite;
        dragon mdragonSprite2;
        dragon mdragonSprite3;
        List<Bushbackground> farBackgroundList;
        List<Bushbackground> nearBackgroundList;
        List<walkingground> walkinggroundList;
        List<walkingground> walkwayList;
        List<GoldCoin> goldCoinList;

        enum GameStates
        {
            Paused,
            Playing,
            MainMenu,
            Options,
            GameOver
        }
        // GameStates CurrentGameState = GameStates.Playing;
        GameStates CurrentGameState = GameStates.MainMenu;

        cButton btnPlay;
        cButton btnControls;
        cButton btnQuit;
        cButton btnPrevious;


        int screenWidth = 1024, screenHeight = 768;

        public DragonGame1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
       //     graphics.PreferredBackBufferHeight = 768;
       //     graphics.PreferredBackBufferWidth = 1024;
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
            mdragonSprite = new dragon(35);
            mdragonSprite2 = new dragon(15);
            mdragonSprite3 = new dragon(25);
       
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            UVfont = Content.Load<SpriteFont>("GameOverJingJing");
            countdownFont = Content.Load<SpriteFont>("timerFont");

            lizzy_elisabethan_period_music_track = Content.Load<Song>("lizzy_elizabethan_period_music_track");
            bakgrunnsmusikk_2 = Content.Load<Song>("Bakgrunnsmusikk_2");
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
            mdragonSprite.SetIsVisible(true);
            mdragonSprite2.LoadContent(this.Content);
            mdragonSprite2.SetIsVisible(true);
            mdragonSprite3.LoadContent(this.Content);
            mdragonSprite3.SetIsVisible(false);

           
            // Resolution Settings
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.ApplyChanges();

            // Menu button
            IsMouseVisible = true;

            // Main menu
            btnPlay = new cButton(Content.Load<Texture2D>("PlayButton"), graphics.GraphicsDevice);
            btnControls = new cButton(Content.Load<Texture2D>("ControlsButton"), graphics.GraphicsDevice);
            btnQuit = new cButton(Content.Load<Texture2D>("QuitButton"), graphics.GraphicsDevice);

            btnPlay.setPosition(new Vector2(400, 450));
            btnControls.setPosition(new Vector2(400, 500));
            btnQuit.setPosition(new Vector2(400, 550));
         
            // Options
            btnPrevious = new cButton(Content.Load<Texture2D>("BackButton"), graphics.GraphicsDevice);
            btnPrevious.setPosition(new Vector2(400, 700));
            
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
                MediaPlayer.Play(bakgrunnsmusikk_2);
            }


            KeyboardState CurrentKeyboardState = Keyboard.GetState();

            if (CurrentKeyboardState.IsKeyDown(Keys.F1) && songstart)
            {
                MediaPlayer.Play(lizzy_elisabethan_period_music_track);
            }

            if (CurrentKeyboardState.IsKeyDown(Keys.F2) && songstart)
            {
                MediaPlayer.Play(bakgrunnsmusikk_2);
            }


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

            //mute/unmute
            if (CurrentKeyboardState.IsKeyDown(Keys.F10))
            {
                if (mutesong)
                {
                    MediaPlayer.Resume();
                    mutesong = false;
                }
                else
                {
                    MediaPlayer.Pause();
                    mutesong = true;
                }
            }

            //Volume down
            if (CurrentKeyboardState.IsKeyDown(Keys.F11))
            {
                MediaPlayer.Volume -= 0.1f;
            }

            //Volume up
            if (CurrentKeyboardState.IsKeyDown(Keys.F12))
            {
                MediaPlayer.Volume += 0.1f;
            }

            //***************************
            //Saves and loads th game
            //***************************
            if (CurrentKeyboardState.IsKeyDown(Keys.S))
            {
                try
                {
                    using (Stream stream = File.Open("data.bin", FileMode.Create)) {

                        Cargo cargoToSave = new Cargo()
                        {
                            //farBackgroundList = this.farBackgroundList,
                            goldCoinList = this.goldCoinList,
                            mdragonSprite = this.mdragonSprite,
                            mdragonSprite2 = this.mdragonSprite2,
                            mdragonSprite3 = this.mdragonSprite3,
                            mKnightSprite = this.mKnightSprite
                            //nearBackgroundList = this.nearBackgroundList,
                            //walkinggroundList = this.walkinggroundList,
                            //walkwayList = this.walkwayList};
                        };
                        
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, cargoToSave);
                    }
                }
                catch (IOException) { 
                
                }
            }

            if (CurrentKeyboardState.IsKeyDown(Keys.L))
            {
                try
                {
                    using (Stream stream = File.Open("data.bin", FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        var loadCargo = (Cargo)bin.Deserialize(stream);

                        mKnightSprite = loadCargo.mKnightSprite;
                        var loadedKnightPos = loadCargo.mKnightSprite.Position;
                        var loadedKnightHealth = loadCargo.mKnightSprite.health;
                        mdragonSprite = loadCargo.mdragonSprite;
                        var savedDragonSpritePos = loadCargo.mdragonSprite.Position;
                        mdragonSprite2 = loadCargo.mdragonSprite2;
                        var savedDragonSpritePos2 = loadCargo.mdragonSprite2.Position;
                        mdragonSprite3 = loadCargo.mdragonSprite3;
                        var savedDragonSpritePos3 = loadCargo.mdragonSprite3.Position;
                        var savedDragonSpriteVisible = loadCargo.mdragonSprite3.GetIsVisible();
                        goldCoinList = loadCargo.goldCoinList;

                        //Loads texture, sound effects etc.
                        mKnightSprite.LoadContent(this.Content);
                        mKnightSprite.Position = loadedKnightPos;
                        mKnightSprite.health = loadedKnightHealth;

                        mdragonSprite.LoadContent(this.Content);
                        mdragonSprite.Position = savedDragonSpritePos;

                        mdragonSprite2.LoadContent(this.Content);
                        mdragonSprite2.Position = savedDragonSpritePos2;

                        mdragonSprite3.LoadContent(this.Content);
                        mdragonSprite3.Position = savedDragonSpritePos3;
                        mdragonSprite3.SetIsVisible(savedDragonSpriteVisible);
             
                        foreach (var coin in goldCoinList) {
                            coin.LoadContent(this.Content);
                        }
                        
                    }
                }
                catch (IOException)
                {

                }
            }

            //Updates game objects
            if (CurrentGameState == GameStates.Playing)
            {
                totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                countdown = (float)gameTime.ElapsedGameTime.TotalSeconds;
                countdownStartime += -countdown;

                //Iftime is up, GAME OVER
                if (countdownStartime < 0)
                {
                    CurrentGameState = GameStates.GameOver;
                }

                //Updates time to be drawn to screen
                countdownString = (int)countdownStartime / 60 + ":" + (int)countdownStartime % 60;

                if (totalElapsed > secondsBeforeSpeedUp)
                {

                    if (mdragonSprite3.GetIsVisible() == false)
                    {
                        mdragonSprite3.SetIsVisible(true);
                    }
                    mdragonSprite.SpeedUpTheDragon();
                    mdragonSprite2.SpeedUpTheDragon();
                    totalElapsed = 0;
                }
                //update logic here
                mKnightSprite.Update(gameTime);
                mdragonSprite.Update(gameTime);
                mdragonSprite2.Update(gameTime);
                mdragonSprite3.Update(gameTime);

                List<FireBall> fireballs = mdragonSprite.GetFireballs();
                foreach (FireBall fireball in fireballs)
                {
                    mKnightSprite.CollideWithFireBall(fireball);
                }

                List<FireBall> fireballs2 = mdragonSprite2.GetFireballs();
                foreach (FireBall fireball in fireballs2)
                {
                    mKnightSprite.CollideWithFireBall(fireball);
                }

                List<FireBall> fireballs3 = mdragonSprite2.GetFireballs();
                foreach (FireBall fireball in fireballs3)
                {
                    mKnightSprite.CollideWithFireBall(fireball);
                }

                foreach (GoldCoin goldcoin in goldCoinList)
                {
                    goldcoin.Update(gameTime);
                }

                //Maks knight fall if knight is no longer on ground.
                mKnightSprite.isPlayerStandingOnGround = false;
                mdragonSprite.isPlayerStandingOnGround = false;
                mdragonSprite2.isPlayerStandingOnGround = false;
                mdragonSprite3.isPlayerStandingOnGround = false;
                //Check if knight is on ground
                foreach (walkingground ground in walkwayList)
                {
                    mKnightSprite.CollideWithWalkingGround(ground, 64, 64);
                    mdragonSprite.CollideWithWalkingGround(ground, 64, 64);
                    mdragonSprite2.CollideWithWalkingGround(ground, 64, 64);
                    mdragonSprite3.CollideWithWalkingGround(ground, 64, 64);
                }

                //Check if knight is on ground
                foreach (walkingground ground in walkinggroundList)
                {
                    mKnightSprite.CollideWithWalkingGround(ground, 128, 128);
                    mdragonSprite.CollideWithWalkingGround(ground, 128, 128);
                    mdragonSprite2.CollideWithWalkingGround(ground, 128, 128);
                    mdragonSprite3.CollideWithWalkingGround(ground, 128, 128);
                }

                //Check if knight collects coin
                foreach (GoldCoin coin in goldCoinList)
                {
                    mKnightSprite.CollideWithGoldCoin(coin);
                }

                base.Update(gameTime);
            }
                // Menu

                MouseState mouse = Mouse.GetState();

                switch (CurrentGameState)
                {
                    case GameStates.MainMenu:
                        if (btnPlay.isClicked == true) CurrentGameState = GameStates.Playing;
                        if (btnControls.isClicked == true) CurrentGameState = GameStates.Options;
                        if (btnQuit.isClicked == true) this.Exit();
                        btnPlay.Update(mouse);
                        btnControls.Update(mouse);
                        btnQuit.Update(mouse);
                        break;
                    case GameStates.Playing:

                        break;
                    case GameStates.Options:
                        if (btnPrevious.isClicked == true) CurrentGameState = GameStates.MainMenu;
                        btnPrevious.Update(mouse);
                        break;

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
            switch (CurrentGameState)
            {
                case GameStates.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("MainBG"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnControls.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);

                    break;


                case (GameStates.Options):

                    // Draw control options.

                    btnPrevious.Draw(spriteBatch);
                    break;
                case GameStates.Playing:
                    // Gameplay
                    
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
                mdragonSprite2.Draw(this.spriteBatch);
                mdragonSprite3.Draw(this.spriteBatch);
                spriteBatch.DrawString(countdownFont, countdownString, new Vector2((graphics.GraphicsDevice.Viewport.Width - countdownFont.MeasureString(countdownString).X) / 2, graphics.GraphicsDevice.Viewport.Height - countdownFont.MeasureString(countdownString).Y), Color.White);
                        break;

                case(GameStates.GameOver):
                        spriteBatch.DrawString(UVfont, "Game Over", new Vector2((graphics.GraphicsDevice.Viewport.Width - UVfont.MeasureString("Game Over").X) / 2, graphics.GraphicsDevice.Viewport.Height / 2), Color.White);
                        break;

   

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
