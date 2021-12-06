using Android.Gms.Ads;
using BreakOut.Controller;
using BreakOut.Editor;
using BreakOut.EntityMana;
using BreakOut.GameStates;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime;
using TileBreakerGame29;
using TileBreakerGame29.GameStates;
using static BreakOut.TextureContainer;

namespace BreakOut
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        bool isBackButtonDown = false;
        IGameState currentState;
        
        public Action ShowAd { get; set; }
        public Action HideAd { get; set; }
        public Action LoadAd { get; set; }
        public Action DestroyAd { get; set; }


        IController controller;        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LevelEditor levelEditor;
        EntityManager entityManager;
        string settingsFilename = "settings";
        private Menu.Menu menu;
        
        public struct Settings
        {
            public float masterVolume;
            public float effectVolume;
            public float songVolume;
            public int player;
            public Color playerLeftColor;
            public Color playerRightColor;
            public Color playerMiddleColor;
            public int lastEditedLevel;
            public int levelSettings;
            public int editorOffset;
            public float editorArea;
            public int highScore;
            public float progress;
        }

        public Settings settings;
        
        public Game1()
        {
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            IsFixedTimeStep = true;
        }              

        public void SetMasterVolume(float volume)
        {
            settings.masterVolume=volume;
            SoundManager.SetMasterVolume(volume);
        }
        public float GetMasterVolume()
        {
            return settings.masterVolume;
        }
        public void SetEffectVolume(float volume)
        {
            settings.effectVolume = volume;
            SoundManager.SetEffectVolume(volume);
        }
        public float GetEffectVolume()
        {
            return settings.effectVolume;
        }
        public void SetSongVolume(float volume)
        {
            settings.songVolume = volume;
            SoundManager.SetSongVolume(volume);
        }
        public float GetSongVolume()
        {
            return settings.songVolume;
        }
        public void SetPlayer(int i)
        {
            settings.player = i;
        }
        
        /// <summary>
        /// gets the player type in settings 
        /// </summary>
        /// <returns>0=human, 1=computer</returns>
        public int GetPlayer()
        {
            return settings.player;
        }
        public void SetPlayerLeftColor(Color color)
        {
            settings.playerLeftColor = color;
            entityManager.GetBat().SetLeftColor(color);
        }
        public void SetPlayerRightColor(Color color)
        {
            settings.playerRightColor = color;
            entityManager.GetBat().SetRightColor(color);
        }
        public void SetPlayerMiddleColor(Color color)
        {
            settings.playerMiddleColor = color;
            entityManager.GetBat().SetMiddleColor(color);
        }
        public Color GetPlayerLeftColor()
        {
            return settings.playerLeftColor;
        }
        public Color GetPlayerRightColor()
        {
            return settings.playerRightColor;
        }
        public Color GetPlayerMiddleColor()
        {
            return settings.playerMiddleColor;
        }

        public void OnPause()
        {
            SaveSettings();
            levelEditor?.SaveLevel(this);
            entityManager?.OnPause();            
        }

        public void OnResume()
        {
            LoadSettings();
            entityManager?.OnResume();            
        }       
       

        public void SaveSettings()
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            
            using (Stream stream = store.OpenFile(settingsFilename, FileMode.Create))
            {
                BinaryWriter binaryWriter = new BinaryWriter(stream);                
                binaryWriter.Write(settings.masterVolume);
                binaryWriter.Write(settings.effectVolume);
                binaryWriter.Write(settings.songVolume);
                binaryWriter.Write(settings.player);
                binaryWriter.Write(settings.playerLeftColor.R);
                binaryWriter.Write(settings.playerLeftColor.G);
                binaryWriter.Write(settings.playerLeftColor.B);
                binaryWriter.Write(settings.playerRightColor.R);
                binaryWriter.Write(settings.playerRightColor.G);
                binaryWriter.Write(settings.playerRightColor.B);
                binaryWriter.Write(settings.playerMiddleColor.R);
                binaryWriter.Write(settings.playerMiddleColor.G);
                binaryWriter.Write(settings.playerMiddleColor.B);
                binaryWriter.Write(settings.lastEditedLevel);
                binaryWriter.Write(settings.levelSettings);
                binaryWriter.Write(settings.editorOffset);
                binaryWriter.Write(settings.editorArea);
                binaryWriter.Write(settings.highScore);
                binaryWriter.Write(settings.progress);
            }             
        }

        public void LoadSettings()
        {           

            var store = IsolatedStorageFile.GetUserStoreForApplication();
            if (store.FileExists(settingsFilename))   
            {                
                try
                {
                    using (Stream stream = store.OpenFile(settingsFilename, FileMode.Open))
                    {
                        BinaryReader binaryReader = new BinaryReader(stream);
                        settings.masterVolume = binaryReader.ReadSingle();
                        settings.effectVolume = binaryReader.ReadSingle();
                        settings.songVolume = binaryReader.ReadSingle();
                        
                        SoundManager.SetMasterVolume(settings.masterVolume);
                        SoundManager.SetEffectVolume(settings.effectVolume);
                        SoundManager.SetSongVolume(settings.songVolume);
                        settings.player = binaryReader.ReadInt32();
                        settings.playerLeftColor = new Color(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
                        settings.playerRightColor = new Color(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
                        settings.playerMiddleColor = new Color(binaryReader.ReadByte(), binaryReader.ReadByte(), binaryReader.ReadByte());
                        settings.lastEditedLevel = binaryReader.ReadInt32();
                        settings.levelSettings = binaryReader.ReadInt32();
                        settings.editorOffset = binaryReader.ReadInt32();
                        settings.editorArea = binaryReader.ReadSingle();
                        settings.highScore= binaryReader.ReadInt32();
                        settings.progress = binaryReader.ReadSingle();
                    }                    

                }
                catch (Exception)
                {
                    SetDefaultSettings();
                }
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private void SetDefaultSettings()
        {
            settings.masterVolume = 1.0f;
            settings.effectVolume = 1.0f;
            settings.songVolume = 1.0f;
            SoundManager.SetMasterVolume(settings.masterVolume);
            SoundManager.SetEffectVolume(settings.effectVolume);
            SoundManager.SetSongVolume(settings.songVolume);
            settings.player = 0;
            settings.playerLeftColor = Color.Red;
            settings.playerRightColor = Color.Red;
            settings.playerMiddleColor = Color.White;
            settings.lastEditedLevel = 0;
            settings.levelSettings = 0;
            settings.editorOffset = 0;
            settings.editorArea = 0;
            settings.highScore = 0;
            settings.progress = 0;
        }



        public SpriteBatch GetSpriteBatch()
        {
            return spriteBatch;
        }

        public EntityManager GetEntityManager()
        {
            return entityManager;
        }
        public LevelEditor GetLevelEditor()
        {
            return levelEditor;
        }

        public BreakOut.Menu.Menu GetMenu()
        {
            return menu;
        }

        public IController GetController()
        {
            return controller;
        }
               
        public void ChangeState(IGameState state)
        {
            currentState.Leave(this);
            currentState = state;
            currentState.Enter(this);
        }

        public void NewGame()
        {
            switch (settings.player)
            {
                case 0:
                    controller = new PlayerController();
                    break;
                case 1:
                    controller = new ComputerController();
                    break;
            }

            entityManager.NewGame();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Globals.adHeight = AdSize.SmartBanner.GetHeightInPixels(Game1.Activity);
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DragComplete | GestureType.Hold | GestureType.None;
            entityManager = new EntityManager(this); 
            base.Initialize();
            LoadSettings();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Globals.smallFont = Content.Load<SpriteFont>("graphics/smallFont");
            Globals.bigFont = Content.Load<SpriteFont>("graphics/bigFont");

            SoundManager.AddSound(SoundId.musicLoop1, Content.Load<SoundEffect>("sounds/cwalkloop"), 1);            
            SoundManager.AddSound(SoundId.ballToBat, Content.Load<SoundEffect>("sounds/bathit2"), 5);
            SoundManager.AddSound(SoundId.ballToWall, Content.Load<SoundEffect>("sounds/ballwallhit"), 5);
            SoundManager.AddSound(SoundId.bigExplosion, Content.Load<SoundEffect>("sounds/detonation"), 5);
            SoundManager.AddSound(SoundId.ballToBall, Content.Load<SoundEffect>("sounds/ballballhit2"), 5);
            SoundManager.AddSound(SoundId.blastershoot, Content.Load<SoundEffect>("sounds/blastershoot2"), 5);
            SoundManager.AddSound(SoundId.blasterhit, Content.Load<SoundEffect>("sounds/blasterhit"), 5);
            SoundManager.AddSound(SoundId.tilebreak, Content.Load<SoundEffect>("sounds/tilebreak2"), 5);
            SoundManager.AddSound(SoundId.tilehit, Content.Load<SoundEffect>("sounds/tilehit"), 5);
            SoundManager.AddSound(SoundId.extender, Content.Load<SoundEffect>("sounds/extender2"), 5);
            SoundManager.AddSound(SoundId.denial, Content.Load<SoundEffect>("sounds/denial"), 5);
            SoundManager.AddSound(SoundId.bonus, Content.Load<SoundEffect>("sounds/bonus"), 5);
            SoundManager.AddSound(SoundId.extralife, Content.Load<SoundEffect>("sounds/extralife"), 5);
            SoundManager.AddSound(SoundId.tap, Content.Load<SoundEffect>("sounds/tap2"), 5);
            SoundManager.AddSound(SoundId.lid, Content.Load<SoundEffect>("sounds/lid"), 5);
            SoundManager.AddSound(SoundId.flush, Content.Load<SoundEffect>("sounds/flush"), 5);

            ShaderContainer.AddShader(ShaderReference.tileShader, Content.Load<Effect>("shaders/shader2"));
            ShaderContainer.AddShader(ShaderReference.blurShader, Content.Load<Effect>("shaders/blur"));
            ShaderContainer.AddShader(ShaderReference.shadowShader, Content.Load<Effect>("shaders/shadow"));
            ShaderContainer.AddShader(ShaderReference.shockShader, Content.Load<Effect>("shaders/shock"));
            ShaderContainer.AddShader(ShaderReference.maskShader, Content.Load<Effect>("shaders/maskShader"));
            ShaderContainer.AddShader(ShaderReference.editorShader, Content.Load<Effect>("shaders/EditorShader"));

            TextureContainer.AddTexture(TextureReference.spriteSheetDiffuse, Content.Load<Texture2D>("graphics/skamligt"));
            TextureContainer.AddTexture(TextureReference.spriteSheetNormal, Content.Load<Texture2D>("graphics/skamligtNormal"));
            TextureContainer.AddTexture(TextureReference.enviroment, Content.Load<TextureCube>("graphics/SkyBox"));
            TextureContainer.AddTexture(TextureReference.floor, Content.Load<Texture2D>("graphics/carbonfibre"));
            TextureContainer.AddTexture(TextureReference.renderTarget, new RenderTarget2D(GraphicsDevice, 1080, 1920, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents));
            TextureContainer.AddTexture(TextureReference.refractionMap, new RenderTarget2D(GraphicsDevice, 512, 512, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents));
        
            TextureContainer.AddTexture(TextureReference.lightball, Content.Load<Texture2D>("graphics/lightball"));
            TextureContainer.AddTexture(TextureReference.editorSprites, Content.Load<Texture2D>("graphics/editorsprites"));
            
            menu = new Menu.Menu(this);
            levelEditor = new LevelEditor(this, entityManager, Globals.adHeight);
            currentState = MenuGameState.GetInstance;
            AdController.InitRewardAd();
            SoundEffectInstance sound= SoundManager.PlaySongLoop(SoundId.musicLoop1);
            SoundManager.song= sound;
            SoundManager.PauseSongLoop(sound);           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            menu.Dispose();
            entityManager.Dispose();
            TextureContainer.GetTexture(TextureReference.renderTarget)?.Dispose();
            TextureContainer.GetTexture(TextureReference.refractionMap)?.Dispose();
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            
            currentState.Update(this);
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back != ButtonState.Pressed)
            {
                isBackButtonDown = false;
            }
            if (!isBackButtonDown && GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                isBackButtonDown = true;                
                currentState.OnBack(this); 
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            currentState.Draw(this);
            base.Draw(gameTime);
        }
    }
}
