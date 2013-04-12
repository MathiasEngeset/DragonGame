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
    //
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
        Knight mKnightSprite2;
        dragon mdragonSprite;
        dragon mdragonSprite2;
        dragon mdragonSprite3;
        List<Bushbackground> farBackgroundList;
        List<Bushbackground> nearBackgroundList;
        List<walkingground> walkinggroundList;
        List<walkingground> walkwayList;
        List<GoldCoin> goldCoinList;
        List<Score> highScoreList; 
        KeyboardState CurrentKeyboardState;
        KeyboardState previousKeyboardState;

        // Menu Buttons 
        cButton btnPlay;
        cButton btnControls;
        cButton btnQuit;
        cButton btnPrevious;
        cButton btnRestart;

        enum GameStates
        {
            Paused,
            Playing,
            MainMenu,
            Options,
            GameOver
        }
        
        // Selecting the starting Gamestate
        GameStates CurrentGameState = GameStates.MainMenu;


        int screenWidth = 1024, screenHeight = 768;

        public DragonGame1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

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
            mKnightSprite.SetId(1);
            mKnightSprite.SetIsActive(true);
            mKnightSprite2 = new Knight();
            mKnightSprite2.SetId(2);
            mKnightSprite2.SetIsActive(false);
            mdragonSprite = new dragon(35);
            mdragonSprite2 = new dragon(15);
            mdragonSprite3 = new dragon(25);
            highScoreList = GetHighScore();

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
            mKnightSprite2.LoadContent(this.Content);
            mdragonSprite.LoadContent(this.Content);
            mdragonSprite.SetIsVisible(true);
            mdragonSprite2.LoadContent(this.Content);
            mdragonSprite2.SetIsVisible(true);
            mdragonSprite3.LoadContent(this.Content);
            mdragonSprite3.SetIsVisible(false);

            IsMouseVisible = true;

           
            // Resolution Settings
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            graphics.ApplyChanges();

            // Main menu
            btnPlay = new cButton(Content.Load<Texture2D>("PlayButton"), graphics.GraphicsDevice);
            btnControls = new cButton(Content.Load<Texture2D>("ControlsButton"), graphics.GraphicsDevice);
            btnQuit = new cButton(Content.Load<Texture2D>("QuitButton"), graphics.GraphicsDevice);
            btnRestart = new cButton(Content.Load<Texture2D>("RestartButton"), graphics.GraphicsDevice);

            btnPlay.setPosition(new Vector2(450, 450));
            btnControls.setPosition(new Vector2(450, 500));
            btnQuit.setPosition(new Vector2(450, 550));
            btnRestart.setPosition(new Vector2(450, 450));
            // Menu - Options
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

        public void restart()
        {
            Random rand = new Random();
            mdragonSprite.setPosition(new Vector2(rand.Next(200, 850), -200));
            mdragonSprite.setTotalElapsed(0);
            mKnightSprite2.setHealth(3);
            mKnightSprite.setHealth(3);
            mKnightSprite2.SetScore(0);
            mKnightSprite.SetScore(0);
            mKnightSprite.setPosition(new Vector2(125, 50));
            mKnightSprite2.setPosition(new Vector2(125, 50));
            mKnightSprite.setTimeElapsed(0);
            mKnightSprite2.SetIsActive(false);
            countdownStartime = 180;
        }
        protected override void Update(GameTime gameTime)
        {

            previousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            if (!songstart)
            {
                MediaPlayer.Play(lizzy_elisabethan_period_music_track);
                songstart = true;
                MediaPlayer.Play(bakgrunnsmusikk_2);
            }


           

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

            //Start player 2
            if (CurrentGameState == GameStates.Playing)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.F9))
                {
                    mKnightSprite2.SetIsActive(true);
                }
            }

            //Pause gameplay
            if (CurrentGameState == GameStates.Playing)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
                {
                    MediaPlayer.Pause();
                    CurrentGameState = GameStates.Paused;
                }
            }
            else if (CurrentGameState == GameStates.Paused)
            {
                if (CurrentKeyboardState.IsKeyDown(Keys.P) && previousKeyboardState.IsKeyUp(Keys.P))
                {
                    MediaPlayer.Resume();
                    CurrentGameState = GameStates.Playing;
                }
            }

            //mute/unmute
            if (CurrentKeyboardState.IsKeyDown(Keys.F10) && previousKeyboardState.IsKeyUp(Keys.F10))
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
                            goldCoinList = this.goldCoinList,
                            mdragonSprite = this.mdragonSprite,
                            mdragonSprite2 = this.mdragonSprite2,
                            mdragonSprite3 = this.mdragonSprite3,
                            mKnightSprite = this.mKnightSprite,
                            mKnightSprite2 = this.mKnightSprite2,
                            countdownStartime = this.countdownStartime
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
                        mKnightSprite2 = loadCargo.mKnightSprite2;
                        var loadedKnight2Pos = loadCargo.mKnightSprite2.Position;
                        var loadedKnight2Health = loadCargo.mKnightSprite2.health;
                        mdragonSprite = loadCargo.mdragonSprite;
                        var savedDragonSpritePos = loadCargo.mdragonSprite.Position;
                        mdragonSprite2 = loadCargo.mdragonSprite2;
                        var savedDragonSpritePos2 = loadCargo.mdragonSprite2.Position;
                        mdragonSprite3 = loadCargo.mdragonSprite3;
                        var savedDragonSpritePos3 = loadCargo.mdragonSprite3.Position;
                        var savedDragonSpriteVisible = loadCargo.mdragonSprite3.GetIsVisible();
                        goldCoinList = loadCargo.goldCoinList;
                        countdownStartime = loadCargo.countdownStartime;

                        //Loads texture, sound effects etc.
                        mKnightSprite.LoadContent(this.Content);
                        mKnightSprite.Position = loadedKnightPos;
                        mKnightSprite.health = loadedKnightHealth;

                        mKnightSprite2.LoadContent(this.Content);
                        mKnightSprite2.Position = loadedKnight2Pos;
                        mKnightSprite2.health = loadedKnight2Health;

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
                if(mKnightSprite2.GetIsActive()){
                mKnightSprite2.Update(gameTime);
                }
                mKnightSprite.Update(gameTime);
                mdragonSprite.Update(gameTime);
                mdragonSprite2.Update(gameTime);
                mdragonSprite3.Update(gameTime);

                List<FireBall> fireballs = mdragonSprite.GetFireballs();
                foreach (FireBall fireball in fireballs)
                {
                    mKnightSprite.CollideWithFireBall(fireball);
                    if (mKnightSprite2.GetIsActive())
                    {
                        mKnightSprite2.CollideWithFireBall(fireball);
                    }
                }

                List<FireBall> fireballs2 = mdragonSprite2.GetFireballs();
                foreach (FireBall fireball in fireballs2)
                {
                    mKnightSprite.CollideWithFireBall(fireball);
                    if (mKnightSprite2.GetIsActive())
                    {
                        mKnightSprite2.CollideWithFireBall(fireball);
                    }
                }

                List<FireBall> fireballs3 = mdragonSprite2.GetFireballs();
                foreach (FireBall fireball in fireballs3)
                {
                    mKnightSprite.CollideWithFireBall(fireball);
                    if (mKnightSprite2.GetIsActive())
                    {
                        mKnightSprite2.CollideWithFireBall(fireball);
                    }
                }

                foreach (GoldCoin goldcoin in goldCoinList)
                {
                    goldcoin.Update(gameTime);
                }

                //Maks knight fall if knight is no longer on ground.
                mKnightSprite.isPlayerStandingOnGround = false;
                mKnightSprite2.isPlayerStandingOnGround = false;
                mdragonSprite.isPlayerStandingOnGround = false;
                mdragonSprite2.isPlayerStandingOnGround = false;
                mdragonSprite3.isPlayerStandingOnGround = false;
                //Check if knight is on ground
                foreach (walkingground ground in walkwayList)
                {
                    mKnightSprite.CollideWithWalkingGround(ground, 64, 64);
                    mKnightSprite2.CollideWithWalkingGround(ground, 64, 64);
                    mdragonSprite.CollideWithWalkingGround(ground, 64, 64);
                    mdragonSprite2.CollideWithWalkingGround(ground, 64, 64);
                    mdragonSprite3.CollideWithWalkingGround(ground, 64, 64);
                }

                //Check if knight is on ground
                foreach (walkingground ground in walkinggroundList)
                {
                    mKnightSprite.CollideWithWalkingGround(ground, 128, 128);
                    mKnightSprite2.CollideWithWalkingGround(ground, 128, 128);
                    mdragonSprite.CollideWithWalkingGround(ground, 128, 128);
                    mdragonSprite2.CollideWithWalkingGround(ground, 128, 128);
                    mdragonSprite3.CollideWithWalkingGround(ground, 128, 128);
                }

                //Check if knight collects coin
                foreach (GoldCoin coin in goldCoinList)
                {
                    mKnightSprite.CollideWithGoldCoin(coin);
                    if (mKnightSprite2.GetIsActive())
                    {
                        mKnightSprite2.CollideWithGoldCoin(coin);
                    }
                }

                
            }
                // Menu - Mouse Clicks

                MouseState mouse = Mouse.GetState();

                switch (CurrentGameState)
                {
                    case GameStates.MainMenu:

                        // Main Menu Buttons
                        if (btnPlay.isClicked == true) CurrentGameState = GameStates.Playing;
                        if (btnControls.isClicked == true) CurrentGameState = GameStates.Options;
                        if (btnQuit.isClicked == true) this.Exit();
                        btnPlay.Update(mouse);
                        btnControls.Update(mouse);
                        btnQuit.Update(mouse);
                        break;
                   
                    case GameStates.Playing:
                        btnRestart.isClicked = false;
                        if (mKnightSprite.getHealth() == 0) CurrentGameState = GameStates.GameOver;
                        break;
                    case GameStates.Options:

                        // Options Buttons
                        if (btnPrevious.isClicked == true) CurrentGameState = GameStates.MainMenu;
                        btnPrevious.Update(mouse);
                        break;

                    case GameStates.GameOver:
                        if (btnQuit.isClicked == true) this.Exit();
                        if (btnRestart.isClicked == true) { restart(); MediaPlayer.Resume(); CurrentGameState = GameStates.Playing; }
                            btnRestart.Update(mouse);
                            btnQuit.Update(mouse);
                        
                        break;

                    case GameStates.Paused:

                        if (btnQuit.isClicked == true) this.Exit();
                        if (btnRestart.isClicked == true) { restart(); MediaPlayer.Resume(); CurrentGameState = GameStates.Playing; }
                        btnQuit.Update(mouse);
                        btnRestart.Update(mouse);
                        break;
                }

                base.Update(gameTime);
                
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
            if (CurrentGameState == GameStates.Playing || CurrentGameState == GameStates.Paused)
            {
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

                if (mKnightSprite2.GetIsActive())
                {
                    mKnightSprite2.Draw(this.spriteBatch);
                }
                else
                {
                    var start2playerText = "Hit F9 for player 2";
                    spriteBatch.DrawString(countdownFont, start2playerText, new Vector2((graphics.GraphicsDevice.Viewport.Width - countdownFont.MeasureString(start2playerText).X), graphics.GraphicsDevice.Viewport.Height - countdownFont.MeasureString(start2playerText).Y), Color.White);
                }

                mKnightSprite.Draw(this.spriteBatch);
                mdragonSprite.Draw(this.spriteBatch);
                mdragonSprite2.Draw(this.spriteBatch);
                mdragonSprite3.Draw(this.spriteBatch);
                spriteBatch.DrawString(countdownFont, countdownString, new Vector2((graphics.GraphicsDevice.Viewport.Width - countdownFont.MeasureString(countdownString).X) / 2, graphics.GraphicsDevice.Viewport.Height - countdownFont.MeasureString(countdownString).Y), Color.White);
            }
            
            switch (CurrentGameState)
            {
                case GameStates.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("KnightAndDragon"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnControls.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    break;
                case GameStates.Paused:
                    btnQuit.Draw(spriteBatch);
                    btnRestart.Draw(spriteBatch);
                        break;
                case(GameStates.GameOver):
                        if (countdownStartime > 0)
                        {
                            spriteBatch.DrawString(UVfont, "Game Over", new Vector2((graphics.GraphicsDevice.Viewport.Width - UVfont.MeasureString("Game Over").X) / 2, 0), Color.White);
                            spriteBatch.DrawString(UVfont, "You lost", new Vector2((graphics.GraphicsDevice.Viewport.Width - UVfont.MeasureString("You lost").X) / 2, UVfont.MeasureString("Game Over").Y), Color.White);
                        }
                        else {
                            if (!mKnightSprite2.GetIsActive())
                            {
                                spriteBatch.DrawString(UVfont, "You won", new Vector2((graphics.GraphicsDevice.Viewport.Width - UVfont.MeasureString("You won").X) / 2, 0), Color.White);
                                spriteBatch.DrawString(UVfont, "Score: " + mKnightSprite.GetScore().ToString(), new Vector2((graphics.GraphicsDevice.Viewport.Width - UVfont.MeasureString("Score: " + mKnightSprite.GetScore().ToString()).X) / 2, UVfont.MeasureString("You won").Y), Color.White);
                            }
                            else {
                                if (mKnightSprite2.GetScore() > mKnightSprite.GetScore())
                                {
                                    spriteBatch.DrawString(UVfont, "Player 2 won", new Vector2((graphics.GraphicsDevice.Viewport.Width - UVfont.MeasureString("Player 2 won").X) / 2, 0), Color.White);
                                }
                                else {
                                    spriteBatch.DrawString(UVfont, "Player 1 won", new Vector2((graphics.GraphicsDevice.Viewport.Width - UVfont.MeasureString("Player 1 won").X) / 2, 0), Color.White);
                                }
                            }
                           }

                        btnRestart.Draw(spriteBatch);
                        btnQuit.Draw(spriteBatch);
                        break;

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        //*********************************
        //Save and load of highscore
        //*********************************

        private void SaveHighScore(){
            try
                {
                    using (Stream stream = File.Open("highscore.bin", FileMode.Create)) {

                        HighScore highScoreToSave = new HighScore();
                        highScoreToSave.SetHighScore(this.highScoreList);
                        
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, highScoreToSave);
                    }
                }
                catch (IOException) { 
                
                }
        }
              
        private List<Score> GetHighScore(){
            try
            {
                using (Stream stream = File.Open("highscore.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    var loadHighScore = (HighScore)bin.Deserialize(stream);
                    return loadHighScore.GetHighScore();
                }
            }
            catch (IOException)
            {
                return new List<Score>();
            }
        }  

            
    }
}
