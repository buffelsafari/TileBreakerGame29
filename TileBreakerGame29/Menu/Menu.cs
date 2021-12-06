using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using static BreakOut.TextureContainer;
using TileBreakerGame29.Menu;

namespace BreakOut.Menu
{
    class Menu
    {
        public RenderTarget2D renderTarget;
        public RenderTarget2D normalTexture;
        public RenderTarget2D diffuseTexture;

        private Wisp[] wispArray;
        private Game1 game;

        private VertexBuffer vertexBuffer;
        private Matrix world = Matrix.CreateTranslation(0, 0, 0);
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        private Matrix projection = Matrix.CreateOrthographicOffCenter(0, 1080, 1920, 0, 0, 100);

        private RasterizerState rasterizerState = new RasterizerState();
        private MenuPage currentPage;
        private Stack<MenuPage> pageStack;

        public bool isNewGame = false;
        public bool isResumed = false;
        public bool isEditor = false;
        public string editorFile = "";

        private bool needToRedrawTextures = true;
        private bool manifestOk = false;
        private Button resumeButton;
        private Button newButton;
        private Button endGameButton;
        private Button editorButton;
        private Button levelButton;
        private RadioButtons playerRadio;
        private RadioButtons levelRadio;
        private HighScore highScore;

        private Slider masterVolume;
        private Slider effectVolume;
        private Slider songVolume;

        private Slider editorOffset;
        private Slider editorArea;

        private ColorSlider leftColor;
        private ColorSlider middleColor;
        private ColorSlider rightColor;
        private PlayerItem playerItem;

        private Color backGroundColor = new Color(0, 0, 0, 0.8f);

        public Menu(Game1 game)
        {
            this.game = game;
            manifestOk = game.GetEntityManager().GenerateLevelManifest(game.settings.levelSettings);

            wispArray = new Wisp[8];
            wispArray[0] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.White);
            wispArray[1] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.Pink);
            wispArray[2] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.CornflowerBlue);
            wispArray[3] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.LightGreen);
            wispArray[4] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.Purple);
            wispArray[5] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.SeaGreen);
            wispArray[6] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.LightGreen);
            wispArray[7] = new Wisp(new Vector2(Globals.rnd.Next(0, 1080), Globals.rnd.Next(0, 1920)), Color.LightYellow);
            
            renderTarget = new RenderTarget2D(game.GraphicsDevice, 1080, 1920, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            normalTexture = new RenderTarget2D(game.GraphicsDevice, 1080, 1920, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            diffuseTexture = new RenderTarget2D(game.GraphicsDevice, 1080, 1920, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            currentPage = new MenuPage();
            pageStack = new Stack<MenuPage>();
            MenuPage settings = new MenuPage();
            MenuPage editorFileSelect = new MenuPage();
            MenuPage soundSettings = new MenuPage();
            MenuPage playerSettings = new MenuPage();
            MenuPage playerCustomization = new MenuPage();
            MenuPage levelSettings = new MenuPage();
            MenuPage editorSettings = new MenuPage();

            #region settings menuPage definition
            settings.AddItem(new Head(new Rectangle(150, 50, 800, 1750), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Settings)));
            settings.AddItem(new Button(game, new Rectangle(200, 200, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), Back, null, null));  
            settings.AddItem(new Button(game, new Rectangle(200, 500, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Sound), ChangePage, null, soundSettings));
            settings.AddItem(new Button(game, new Rectangle(200, 700, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Player), ChangePage, null, playerSettings));
            levelButton = new Button(game, new Rectangle(200, 900, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Levels), ChangePage, null, levelSettings);
            settings.AddItem(levelButton);
            #endregion

            #region playerSettings menuPage
            playerSettings.AddItem(new Head(new Rectangle(150, 50, 800, 1750), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Player)));
            playerSettings.AddItem(new Button(game, new Rectangle(200, 200, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), Back, null, null));
            playerRadio = new RadioButtons(OnPlayerChange);
            playerSettings.AddItem(playerRadio);
            playerRadio.Add(new Button(game, new Rectangle(300, 500, 600, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Human), null, null, null));
            playerRadio.Add(new Button(game, new Rectangle(300, 660, 600, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Computer), null, null, null));
            playerRadio.SetIndex(game.GetPlayer());
            playerSettings.AddItem(new Button(game, new Rectangle(200, 1000, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Customize), ChangePage, null, playerCustomization));

            #endregion

            #region player customization page
            playerCustomization.AddItem(new Head(new Rectangle(50, 50, 1000, 1850), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Customize)));
            playerCustomization.AddItem(new Button(game, new Rectangle(200, 200, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), Back, null, null));
            playerItem = new PlayerItem(new Rectangle(200, 400, 700, 150));
            playerCustomization.AddItem(playerItem);
            leftColor= new ColorSlider(game, new Rectangle(100, 600, 900, 400), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Left), null, OnLeftPlayerColorChange);
            middleColor = new ColorSlider(game, new Rectangle(100, 1025, 900, 400), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Middle), null, OnMiddlePlayerColorChange);
            rightColor = new ColorSlider(game, new Rectangle(100, 1450, 900, 400), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Right), null, OnRightPlayerColorChange);


            playerCustomization.AddItem(leftColor);
            playerCustomization.AddItem(middleColor);
            playerCustomization.AddItem(rightColor);
            leftColor.SetValue(game.GetPlayerLeftColor());
            middleColor.SetValue(game.GetPlayerMiddleColor());
            rightColor.SetValue(game.GetPlayerRightColor());
            playerItem.SetLeftColor(game.GetPlayerLeftColor());
            playerItem.SetMiddleColor(game.GetPlayerMiddleColor());
            playerItem.SetRightColor(game.GetPlayerRightColor());
            #endregion

            #region editor file selector page
            editorFileSelect.AddItem(new Head(new Rectangle(10, 50, 1060, 1850), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Editor)));
            editorFileSelect.AddItem(new Button(game, new Rectangle(200, 200, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), Back, null, null));
            
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 400, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level1), OnLevel1, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 550, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level2), OnLevel2, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 700, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level3), OnLevel3, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 850, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level4), OnLevel4, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 1000, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level5), OnLevel5, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 1150, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level6), OnLevel6, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 1300, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level7), OnLevel7, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 1450, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level8), OnLevel8, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 1600, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level9), OnLevel9, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(30, 1750, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level10), OnLevel10, null, null));

            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 400, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level11), OnLevel11, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 550, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level12), OnLevel12, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 700, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level13), OnLevel13, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 850, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level14), OnLevel14, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 1000, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level15), OnLevel15, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 1150, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level16), OnLevel16, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 1300, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level17), OnLevel17, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 1450, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level18), OnLevel18, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 1600, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level19), OnLevel19, null, null));
            editorFileSelect.AddItem(new Button(game, new Rectangle(550, 1750, 500, 145), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Level20), OnLevel20, null, null));
            #endregion

            #region soundsetting menuPage
            soundSettings.AddItem(new Head(new Rectangle(50, 50, 1000, 1750), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Sound)));
            soundSettings.AddItem(new Button(game, new Rectangle(100, 200, 900, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), Back, null, null));
            masterVolume = new Slider(game, new Rectangle(100, 600, 900, 300), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.MasterVolume), null, OnMasterVolume);
            effectVolume = new Slider(game, new Rectangle(100, 1000, 900, 300), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.EffectVolume), null, OnEffectVolume);
            songVolume = new Slider(game, new Rectangle(100, 1400, 900, 300), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.SongVolume), null, OnSongVolume);
            
            masterVolume.SetValue(game.GetMasterVolume());
            effectVolume.SetValue(game.GetEffectVolume());
            songVolume.SetValue(game.GetSongVolume());
            

            soundSettings.AddItem((IMenuItem)masterVolume);
            soundSettings.AddItem((IMenuItem)effectVolume);
            soundSettings.AddItem((IMenuItem)songVolume);
            #endregion

            #region levelSetting menupage
            levelSettings.AddItem(new Head(new Rectangle(150, 50, 800, 1750), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Levels)));
            levelSettings.AddItem(new Button(game, new Rectangle(200, 200, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), Back, null, null));
            levelRadio = new RadioButtons(OnLevelSettingChange);
            levelSettings.AddItem(levelRadio);
            levelRadio.Add(new Button(game, new Rectangle(300, 500, 600, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Standard), null, null, null));
            levelRadio.Add(new Button(game, new Rectangle(300, 660, 600, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Custom), null, null, null));
            levelRadio.Add(new Button(game, new Rectangle(300, 820, 600, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.LastEdit), null, null, null));
            //levelRadio.SetIndex(0);
            levelRadio.SetIndex(game.settings.levelSettings);

            #endregion

            #region editorSettings menupage  
            editorSettings.AddItem(new Head(new Rectangle(50, 50, 1000, 1750), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.EditorSettings)));
            editorSettings.AddItem(new Button(game, new Rectangle(100, 200, 900, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), Back, null, null));
            editorOffset = new Slider(game, new Rectangle(100, 600, 900, 300), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.PickOffset), null, OnEditorOffset);
            editorArea = new Slider(game, new Rectangle(100, 1000, 900, 300), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.PickArea), null, OnEditorArea);
            editorOffset.SetValue((game.settings.editorOffset/200.0f));
            editorArea.SetValue(game.settings.editorArea);

            editorSettings.AddItem((IMenuItem)editorOffset);
            editorSettings.AddItem((IMenuItem)editorArea);
            #endregion



            #region the main menuPage
            //------------------------------------------------------
            currentPage.AddItem(new Head(new Rectangle(150, 50, 800, 1750), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.app_name)));

            resumeButton = new Button(game, new Rectangle(200, 200, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Resume), Resume, null, null);
            resumeButton.isGreyed = true;
            newButton = new Button(game, new Rectangle(200, 400, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.NewGame), NewGame, null, null);
            currentPage.AddItem(resumeButton);
            currentPage.AddItem(newButton);
            endGameButton = new Button(game, new Rectangle(200, 600, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.EndGame), EndGame, null, null);
            endGameButton.isGreyed = true;
            currentPage.AddItem(endGameButton); // end game

            currentPage.AddItem(new Button(game, new Rectangle(200, 800, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Settings), ChangePage, null, settings));

            highScore = new HighScore(game, new Rectangle(200, 1000, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.HighScore));
            highScore.Update(game.settings.highScore, game.settings.progress);
            currentPage.AddItem(highScore);


            editorButton = new Button(game, new Rectangle(200, 1200, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Editor), ChangePage, null, editorFileSelect);
            currentPage.AddItem(editorButton);
            currentPage.AddItem(new Button(game, new Rectangle(200, 1400, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.EditorSettings), ChangePage, null, editorSettings));

            currentPage.AddItem(new Button(game, new Rectangle(200, 1600, 700, 150), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Quit), Quit, null, null));
            #endregion

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];
            vertices[0] = new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.White, new Vector2(0,0));
            vertices[1] = new VertexPositionColorTexture(new Vector3(1080, 0, 0), Color.White, new Vector2(1,0));
            vertices[2] = new VertexPositionColorTexture(new Vector3(0, 1920, 0), Color.White, new Vector2(0,1));
            vertices[3] = new VertexPositionColorTexture(new Vector3(0, 1920, 0), Color.White, new Vector2(0,1));
            vertices[4] = new VertexPositionColorTexture(new Vector3(1080, 0, 0), Color.White, new Vector2(1,0));
            vertices[5] = new VertexPositionColorTexture(new Vector3(1080, 1920, 0), Color.White, new Vector2(1,1));

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorTexture), 6, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);


            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            game.GraphicsDevice.RasterizerState = rasterizerState;

        }

        public void Dispose()
        {
            currentPage?.DisposeTextures();
            renderTarget?.Dispose();
            normalTexture?.Dispose();
            diffuseTexture?.Dispose();
            rasterizerState?.Dispose();
            vertexBuffer?.Dispose();
            

        }

        public void ChangePage(object sender, EventArgs eventArgs)
        {
            if (sender is IMenuItem)
            {
                MenuPage page=((IMenuItem)sender).GetChildPage();
                if (page != null)
                {
                    pageStack.Push(currentPage);
                    currentPage.DisposeTextures();
                    currentPage = page;
                    needToRedrawTextures=true;
                }
            }            
        }
        public void Back(object sender, EventArgs eventArgs)
        {
            currentPage.DisposeTextures();
            currentPage = pageStack.Pop();
            needToRedrawTextures = true;            
            manifestOk = game.GetEntityManager().GenerateLevelManifest(game.settings.levelSettings);
        }

        public void NewGame(object sender, EventArgs eventArgs)
        {
            isNewGame = true;
            game.GetEntityManager().gameActive = true;            
        }
        public void Resume(object sender, EventArgs eventArgs)
        {
            isResumed = true;            
        }

        public void EndGame(object sender, EventArgs eventArgs)
        {
            game.GetEntityManager().gameActive = false;           
        }

        public void Quit(object sender, EventArgs eventArgs)
        {
            game.Exit();            
        }        

        #region levels
        public void OnLevel1(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level1.bin";
            game.settings.lastEditedLevel = 0;
        }
        public void OnLevel2(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level2.bin";
            game.settings.lastEditedLevel = 1;
        }
        public void OnLevel3(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level3.bin";
            game.settings.lastEditedLevel = 2;
        }
        public void OnLevel4(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level4.bin";
            game.settings.lastEditedLevel = 3;
        }
        public void OnLevel5(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level5.bin";
            game.settings.lastEditedLevel = 4;
        }
        public void OnLevel6(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level6.bin";
            game.settings.lastEditedLevel = 5;
        }
        public void OnLevel7(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level7.bin";
            game.settings.lastEditedLevel = 6;
        }
        public void OnLevel8(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level8.bin";
            game.settings.lastEditedLevel = 7;
        }
        public void OnLevel9(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level9.bin";
            game.settings.lastEditedLevel = 8;
        }
        public void OnLevel10(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level10.bin";
            game.settings.lastEditedLevel = 9;
        }
        public void OnLevel11(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level11.bin";
            game.settings.lastEditedLevel = 10;
        }
        public void OnLevel12(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level12.bin";
            game.settings.lastEditedLevel = 11;
        }
        public void OnLevel13(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level13.bin";
            game.settings.lastEditedLevel = 12;
        }
        public void OnLevel14(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level14.bin";
            game.settings.lastEditedLevel = 13;
        }
        public void OnLevel15(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level15.bin";
            game.settings.lastEditedLevel = 14;
        }
        public void OnLevel16(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level16.bin";
            game.settings.lastEditedLevel = 15;
        }
        public void OnLevel17(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level17.bin";
            game.settings.lastEditedLevel = 16;
        }
        public void OnLevel18(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level18.bin";
            game.settings.lastEditedLevel = 17;
        }
        public void OnLevel19(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level19.bin";
            game.settings.lastEditedLevel = 18;
        }
        public void OnLevel20(object sender, EventArgs eventArgs)
        {
            isEditor = true;
            editorFile = "level20.bin";
            game.settings.lastEditedLevel = 19;
        }

        public void OnMasterVolume(object sender, EventArgs eventArgs)
        {
            if (sender is Slider)
            {
                game.SetMasterVolume(((Slider)sender).value);                               
            }            
        }

        public void OnEffectVolume(object sender, EventArgs eventArgs)
        {
            if (sender is Slider)
            {
                game.SetEffectVolume(((Slider)sender).value);                
            }            
        }

        public void OnSongVolume(object sender, EventArgs eventArgs)
        {
            if (sender is Slider)
            {
                game.SetSongVolume(((Slider)sender).value);                
            }            
        }

        public void OnPlayerChange(object sender, EventArgs eventArgs)
        {
            if (sender is RadioButtons)
            {
                game.SetPlayer(((RadioButtons)sender).GetIndex());                
            }
        }

        public void OnLeftPlayerColorChange(object sender, EventArgs eventArgs)
        {
            if (sender is ColorSlider)
            {                
                playerItem.SetLeftColor(((ColorSlider)sender).GetValue());
                game.SetPlayerLeftColor(((ColorSlider)sender).GetValue());
            }
        }

        public void OnRightPlayerColorChange(object sender, EventArgs eventArgs)
        {
            if (sender is ColorSlider)
            {
                playerItem.SetRightColor(((ColorSlider)sender).GetValue());
                game.SetPlayerRightColor(((ColorSlider)sender).GetValue());
            }
        }

        public void OnMiddlePlayerColorChange(object sender, EventArgs eventArgs)
        {
            if (sender is ColorSlider)
            {
                playerItem.SetMiddleColor(((ColorSlider)sender).GetValue());
                game.SetPlayerMiddleColor(((ColorSlider)sender).GetValue());
            }
        }

        public void OnLevelSettingChange(object sender, EventArgs eventArgs)
        {
            if (sender is RadioButtons)
            {
                game.settings.levelSettings = ((RadioButtons)sender).GetIndex();                
            }
        }

        public void OnEditorOffset(object sender, EventArgs eventArgs)
        {
            if (sender is Slider)
            {
                game.settings.editorOffset=(int)((Slider)sender).value*200;        
            }
        }

        public void OnEditorArea(object sender, EventArgs eventArgs)
        {
            if (sender is Slider)
            {
                game.settings.editorArea=((Slider)sender).value;                
            }
        }

        public void UpdateScore()
        {
            highScore.Update(game.settings.highScore, game.settings.progress);
            needToRedrawTextures = true;
        }

        #endregion

        public void Update(Game1 game)
        {

            if (game.GetEntityManager().gameActive)
            {
                newButton.isGreyed = true;
                resumeButton.isGreyed = false;
                editorButton.isGreyed = true;
                playerRadio.isGreyed = true;
                endGameButton.isGreyed = false;
                levelButton.isGreyed = true;                
            }
            else
            {
                if (manifestOk)
                {
                    newButton.isGreyed = false;
                }
                else
                {
                    newButton.isGreyed = true;
                }
                resumeButton.isGreyed = true;
                editorButton.isGreyed = false;
                playerRadio.isGreyed = false;
                endGameButton.isGreyed = true;
                levelButton.isGreyed = false;
            }

            TouchCollection tc = TouchPanel.GetState();
            var gesture = default(GestureSample);
            
            foreach(TouchLocation tl in  tc)
            {
                currentPage.UpdateTouch(tl);
                
            }

            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
                currentPage.UpdateGesture(gesture); 
            }

            for (int i = 0; i < wispArray.Length; i++)
            {
                wispArray[i].Update();
            } 
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (needToRedrawTextures)
            {
                needToRedrawTextures = false;
                currentPage.GenerateNormalTexture(spriteBatch);  
                currentPage.GenerateDiffuseTexture(spriteBatch);
            }
            Effect effect = ShaderContainer.GetShader(ShaderReference.tileShader);
            
            game.GraphicsDevice.SetRenderTarget(normalTexture);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            currentPage.DrawNormal(spriteBatch);

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(diffuseTexture);
            game.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            currentPage.DrawDiffuse(spriteBatch);         

            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(renderTarget);
            game.GraphicsDevice.Clear(backGroundColor);

            effect.Parameters["WorldViewProjection"].SetValue(world * view * projection);
            effect.Parameters["diffuseTexture"].SetValue(diffuseTexture);
            effect.Parameters["NormalMap"].SetValue(normalTexture);
            effect.Parameters["SkyBoxTexture"].SetValue(TextureContainer.GetTexture(TextureReference.enviroment));
            
            Vector3[] lightPosition = new Vector3[8];
            Vector4[] lightColor = new Vector4[8];
            
            for (int i = 0; i < wispArray.Length; i++)
            {
                lightPosition[i] = new Vector3(wispArray[i].position, wispArray[i].height);
                lightColor[i] = wispArray[i].color.ToVector4();
           
            }

            effect.Parameters["lightPosition"].SetValue(lightPosition);
            effect.Parameters["lightColor"].SetValue(lightColor);

            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);            

            effect.CurrentTechnique.Passes["P0"].Apply();
            game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
        }
    }
}