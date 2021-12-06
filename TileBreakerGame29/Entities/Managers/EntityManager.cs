using BreakOut.Entities.Static;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using TileBreakerGame29;
using TileBreakerGame29.Entities.Dynamic;
using TileBreakerGame29.Entities.Dynamic.DustCloud;
using TileBreakerGame29.Entities.Dynamic.FragParticles;
using TileBreakerGame29.Entities.Dynamic.PowerUps;
using TileBreakerGame29.Entities.Dynamic.Weapon;
using TileBreakerGame29.Entities.Static.Effects;
using TileBreakerGame29.Light;
using TileBreakerGame29.Refraction;
using static BreakOut.TextureContainer;
using static TileBreakerGame29.Entities.Dynamic.PowerUps.PowerUp;

namespace BreakOut.EntityMana
{
  
    class EntityManager
    {
        public bool isInerstialAvailable=true;
        public bool isGameOver = false;
        public bool waitForLoading = false;

        private int time = 0;
        public bool gameActive { get; set; } = false;
        public bool keepingScore = true;
        public int levelsPlayed = 0;

        private bool needToUpdateIndexList = false;
        private List<string> levelManifest;
        private int levelIndex = 0;
        private bool loadingFromEditor=true;

        private Matrix world = Matrix.CreateTranslation(0, 0, 0);
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        private Matrix projection = Matrix.CreateOrthographicOffCenter(0, 1080, 1920, 0, 0, 100);
        private Matrix worldViewProjection;

        private RasterizerState rasterizerState = new RasterizerState();

        private ScoreHUD scoreHUD;
        private Bat player;
        private Blaster blaster;
        private Dust dust;
        private ShockRefraction shockRefraction;
        private List<AbstractStaticEntity> staticEntityList;
        private LightManager lightManager;
        private Cracks cracks;
        private Frags frags;
        private Balls ballsOfDoom;
        private Game1 game;
        private IState currentState;
        private Level nextLevel;
        private Vector2 shake; 
        private Vector2 shakeVelocity;
        private Vector2 shakeForce;

        public RenderTarget2D frt1 { get; private set; }
        public RenderTarget2D frt2 { get; private set; }

        private Bomber bomber;
        private FreeLight freeLight;

        
        public EntityManager(Game1 game)
        {
            worldViewProjection = world * view * projection;

            nextLevel = new Level(game, this);            
            nextLevel.loadingFailed = OnLoadingFailed;
            nextLevel.loadingComplete = OnLoadingComplete;
            
            blaster = new Blaster();
            dust = new Dust();

            scoreHUD = new ScoreHUD();

            this.game = game;
            staticEntityList = new List<AbstractStaticEntity>();            
            lightManager = new LightManager(game.GraphicsDevice);

            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            game.GraphicsDevice.RasterizerState = rasterizerState;

            currentState = Playing.GetInstance;

            shake = new Vector2(0,0);
            shakeVelocity = new Vector2();
            shakeForce = new Vector2();

            cracks = new Cracks(game.GraphicsDevice);

            frags = new Frags();            
            
            levelManifest = new List<string>();            

            levelIndex = 0;
            lightManager.AddLightRequestor(blaster);
            shockRefraction = new ShockRefraction();
            this.player = new Bat(game.GraphicsDevice);            
            player.SetLeftColor(game.settings.playerLeftColor);
            player.SetRightColor(game.settings.playerRightColor);
            player.SetMiddleColor(game.settings.playerMiddleColor);
            ballsOfDoom = new Balls(game, this);
            lightManager.AddLightRequestor(ballsOfDoom);
            bomber = new Bomber();
            lightManager.AddLightRequestor(bomber);

            freeLight = new FreeLight();
            lightManager.AddLightRequestor(freeLight);

            frt1 = new RenderTarget2D(game.GraphicsDevice, 1080, 1920, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            frt2 = new RenderTarget2D(game.GraphicsDevice, 1080, 1920, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            
        }

        public void Dispose()
        {
            frt1?.Dispose();
            frt2?.Dispose();
            rasterizerState?.Dispose();
            AbstractStaticEntity.indexBuffer?.Dispose();
            AbstractStaticEntity.vertexBuffer?.Dispose();
            scoreHUD.Dispose();
            player.Dispose();
            dust.Dispose();
            shockRefraction.Dispose();
            lightManager.Dispose();
            cracks.Dispose();
            frags.Dispose();
            ballsOfDoom.Dispose();

        }


        public bool WantPowerUp(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.Heart:
                    if (scoreHUD.GetNumberOfLives() == Globals.maxLives)
                    {
                        return false;
                    }
                    break;
                case PowerUpType.Expander:
                    if (scoreHUD.GetNumberOfExpands() == Globals.maxExpands)
                    {
                        return false;
                    }
                    break;
                case PowerUpType.Flash:
                    if (scoreHUD.GetNumberOfFlash() == Globals.maxFlash)
                    {
                        return false;
                    }
                    break;
                case PowerUpType.Star:
                    if (scoreHUD.GetNumberOfStar() == Globals.maxStar)
                    {
                        return false;
                    }
                    break;            
            }
            return true;
        }

        public bool isBlasterOn()
        {
            if (scoreHUD.GetNumberOfFlash() > 0)
            {
                return true;
            }
            return false;
        }

        public int GetScore()
        {
            return scoreHUD.GetScore();
        }

        public float GetProgress()
        {
            float ret= levelsPlayed / levelManifest.Count*1.0f;
            if (ret > 1)
            {
                ret = 1;
            }
            return ret;
        }

        public void OnLoadingComplete()
        {
            waitForLoading = false;            
        }
        public void OnLoadingFailed()
        {
            nextLevel.Load(game, this, levelManifest[levelIndex], loadingFromEditor);
            waitForLoading = true;
            while (waitForLoading)
            { 
            
            }
            levelIndex++;
            if (levelIndex >= levelManifest.Count)
            {
                levelIndex = 0;
            } 
        }

        public void OnPause()
        {
            
        }

        public void OnResume()
        {
            
        }

        private void AddEditorFileToManifest(string str)
        {
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            if (store.FileExists(str))
            {
                levelManifest.Add(str);                
            }

        }
        private void LoadEditorLevelManifest()
        {
            levelManifest.Clear();
            AddEditorFileToManifest("level1.bin");
            AddEditorFileToManifest("level2.bin");
            AddEditorFileToManifest("level3.bin");
            AddEditorFileToManifest("level4.bin");
            AddEditorFileToManifest("level5.bin");
            AddEditorFileToManifest("level6.bin");
            AddEditorFileToManifest("level7.bin");
            AddEditorFileToManifest("level8.bin");
            AddEditorFileToManifest("level9.bin");
            AddEditorFileToManifest("level10.bin");
            AddEditorFileToManifest("level11.bin");
            AddEditorFileToManifest("level12.bin");
            AddEditorFileToManifest("level13.bin");
            AddEditorFileToManifest("level14.bin");
            AddEditorFileToManifest("level15.bin");
            AddEditorFileToManifest("level16.bin");
            AddEditorFileToManifest("level17.bin");
            AddEditorFileToManifest("level18.bin");
            AddEditorFileToManifest("level19.bin");
            AddEditorFileToManifest("level20.bin");
        }

        private void LoadLevelManifest()
        {
            levelManifest.Clear();            
            levelManifest.Add("buzzcut.bin");
            levelManifest.Add("qbert.bin");            
            levelManifest.Add("garden.bin");
            levelManifest.Add("mosaik.bin");
            levelManifest.Add("port.bin");
            levelManifest.Add("prisma.bin");
            levelManifest.Add("maille.bin");
            levelManifest.Add("spiral.bin");
            levelManifest.Add("skypp.bin");
            levelManifest.Add("tritip.bin");
            levelManifest.Add("bird.bin");
            levelManifest.Add("snekskin.bin");
            levelManifest.Add("opener.bin");
        }

        public bool GenerateLevelManifest(int levelSettings)
        {
            switch (levelSettings)
            {
                case 0:
                    keepingScore = true;
                    loadingFromEditor = false;
                    levelIndex = 0;
                    LoadLevelManifest();
                    break;
                case 1:
                    keepingScore = false;
                    loadingFromEditor = true;
                    levelIndex = 0;
                    LoadEditorLevelManifest();
                    break;
                case 2:
                    keepingScore = false;
                    loadingFromEditor = true;
                    levelIndex = game.settings.lastEditedLevel;                    
                    String str = "level"+(levelIndex+1)+".bin";
                    levelManifest.Clear();
                    AddEditorFileToManifest(str);                    
                    levelIndex = 0;
                    break;
            }
            if (levelManifest.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void NewGame()
        {
            
            isGameOver = false;
            isInerstialAvailable = true;
            if (!AdController.adRewardLoaded)
            {
                AdController.LoadRewardAd();
            }           

            gameActive = true;            
            waitForLoading = true;
            nextLevel.Load(game, this, levelManifest[0], loadingFromEditor); 
            SetLevel(nextLevel);
            levelIndex++;
            if (levelIndex >= levelManifest.Count)
            {
                levelIndex = 0;
            }
            while (waitForLoading)
            {
            }

            staticEntityList.Clear();

            SetNextLevel();            
            ballsOfDoom.NewGame();
            scoreHUD.NewGame();
            shockRefraction.NewGame();
            blaster.NewGame();
            player.NewGame();
            PowerUp.NewGame();
            freeLight.NewGame();
            dust.NewGame();
            frags.NewGame();
            cracks.NewGame();

            levelsPlayed = 0;
            
            currentState = Serving.GetInstance;
            currentState.Enter(this, null);
            GC.Collect();
        }       

        public void SwapFrontRenderTarget()
        {
            RenderTarget2D temp;
            temp = frt1;
            frt1 = frt2;
            frt2 = temp;
        }

        public Balls GetBallsOfDoom()
        {
            return ballsOfDoom;
        }
      
        public void SetLevel(Level level)
        {
            staticEntityList.Clear();
            staticEntityList.AddRange(level.GetTiles());
            ResetStaticVertices();
            needToUpdateIndexList = true;            
            GC.Collect();
        }

        public void SetNextLevel()
        {
            levelsPlayed++;
            SetLevel(nextLevel);  
            while (waitForLoading)
            { 
            
            }
            waitForLoading = true;
            nextLevel.Load(game, this, levelManifest[levelIndex], loadingFromEditor);
            levelIndex++;
            if (levelIndex >= levelManifest.Count)
            {
                levelIndex = 0;
            }
        }

        public void ChangeState(IState state)
        {
            currentState.Leave(this);
            IState oldState = currentState;
            currentState = state;
            currentState.Enter(this, oldState);
        }
        public void ShowText(string str, int delay)
        {
            scoreHUD.ShowText(str, delay);
        } 

        public Bat GetBat()
        {
            return player;
        }      

        public List<AbstractStaticEntity> GetStaticEntities()
        {
            return staticEntityList;
        }

        public bool IsLifeLost()
        {
            if (ballsOfDoom.GetNumberOfActiveBalls()==0)
            {
                return true;
            }
            return false;
        }    

        public bool IsOutOfLives()
        {
            if (scoreHUD.GetNumberOfLives() <= 0)
            {
                return true;
            }
            return false;
        }

        public bool IsLevelWon()
        {            
            foreach(AbstractStaticEntity entity in staticEntityList)
            {                
                if (entity.isKillable)
                {
                    return false;
                }
            }
            return true;
        }

        public void ResetBlaster()
        {
            scoreHUD.ResetBlaster();
            blaster.Reset();
        }
        public void ResetScoreMultiplier()
        {
            scoreHUD.ResetScoreMultiplicator();
        }
        public void ResetExpander()
        {
            scoreHUD.ResetExpander();
            player.ResetExpand();
        }       
        
        private void UpdateIndexList()
        {
            AbstractStaticEntity.indexList.Clear();
            foreach (AbstractStaticEntity se in staticEntityList)
            {
                se.AddIndicesToList();
            }
            AbstractStaticEntity.indexBuffer.SetData<short>(AbstractStaticEntity.indexList.ToArray());            
        }

        public void Update()
        {
            time++;
            if (needToUpdateIndexList)
            {
                UpdateIndexList();                
            }
            needToUpdateIndexList = false;
            shakeForce = -shake*0.1f;
            shakeVelocity += shakeForce;
            shake += shakeVelocity;
            shake *= 0.85f;           

            currentState.Update(this);
        }

        public void UpdateScore()
        {
            scoreHUD.Update(shake, ballsOfDoom);
        }       

        public void ResetStaticVertices()
        {
            foreach (AbstractStaticEntity se in staticEntityList)
            {
                se.AddVerticesToList();
            }
            foreach (AbstractStaticEntity se in staticEntityList)
            {
                se.AddIndicesToList();
            }
            AbstractStaticEntity.InitBuffers(game.GraphicsDevice); // fill list before init            
        }

        public void SetupShader()
        {           
            Effect effect = ShaderContainer.GetShader(ShaderReference.tileShader);
            effect.Parameters["WorldViewProjection"].SetValue(worldViewProjection);  
            effect.Parameters["diffuseTexture"].SetValue((Texture2D)TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetDiffuse));
            effect.Parameters["NormalMap"].SetValue((Texture2D)TextureContainer.GetTexture(TextureContainer.TextureReference.spriteSheetNormal));
            effect.Parameters["SkyBoxTexture"].SetValue((TextureCube)TextureContainer.GetTexture(TextureContainer.TextureReference.enviroment));
            effect.Parameters["lightPosition"].SetValue(lightManager.GetLightPosition());
            effect.Parameters["lightColor"].SetValue(lightManager.GetLightColor());
        }

        public void DrawStaticEntities()
        {       
            game.GraphicsDevice.SetVertexBuffer(AbstractStaticEntity.vertexBuffer);
            game.GraphicsDevice.Indices = AbstractStaticEntity.indexBuffer;
            ShaderContainer.GetShader(ShaderReference.tileShader).CurrentTechnique.Passes["P0"].Apply();
            game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, AbstractStaticEntity.indexList.Count / 3);
        }

        public void UpdateDynamicEntities()
        {
            Bat.dynamicVertexList.Clear();  
            Bat.dynamicIndexList.Clear();            
            
            player.AddVerticesToList();
            player.AddIndicesToList();

            Bat.dynamicVertexBuffer.SetData<VertexPositionColorTexture>(Bat.dynamicVertexList.ToArray());
            Bat.dynamicIndexBuffer.SetData<short>(Bat.dynamicIndexList.ToArray());
        }

        public void DrawDynamicEntities()
        {
            game.GraphicsDevice.SetVertexBuffer(Bat.dynamicVertexBuffer);
            game.GraphicsDevice.Indices = Bat.dynamicIndexBuffer;
            ShaderContainer.GetShader(ShaderReference.tileShader).CurrentTechnique.Passes["P0"].Apply();
            game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Bat.dynamicIndexList.Count / 3);
        }

        public void UpdateFrags()
        {
            frags.Update(staticEntityList, ref ballsOfDoom.GetBallArray() , player);
        }
        public void UpdateShockRefraction()
        {
            shockRefraction.Update();
        }
        public void UpdateDust()
        {
            dust.Update();
        }
        public void UpdateBlaster()
        {
            blaster.Update(player, ref ballsOfDoom.GetBallArray(), staticEntityList, frags);
        }
        public void UpdatePowerUps()
        {
            for(int i= PowerUp.powerUpList.Count-1;i>=0 ;i--)            
            {
                PowerUp.powerUpList[i].Update(player);                
            }
        }
        public void DeactivatePowerUps()
        {
            for (int i = PowerUp.powerUpList.Count - 1; i >= 0; i--)
            {
                PowerUp.powerUpList[i].Active=false;
            }
        }
        public LightManager GetLightManager()
        {
            return lightManager;
        }
        public void UpdateGameview(bool isServing)  // not including balls, powerups and dynamic entities
        {
            for (int i = 0; i < Globals.numberOfBallUpdates; i++)
            {
                player.Update();
                ballsOfDoom.Update(staticEntityList, player, isServing);
            }
            ballsOfDoom.UpdateBuffer();
            UpdateDynamicEntities();
            UpdatePowerUps();
            bomber.Update(staticEntityList);            
            UpdateShockRefraction();            
            UpdateBlaster();
            UpdateFrags();
            UpdateDust();
            UpdateScore();
            lightManager.Update(GetStaticEntities(), ballsOfDoom.GetBallArray(), frags.getParticleArray(), player);
        }

        public void Draw(SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            currentState.Draw(this, spriteBatch, renderTarget);
        }
        public void DrawGameView(Color color)
        {            
            shockRefraction.DrawToRefractionMap(game.GraphicsDevice, game.GetSpriteBatch(), (RenderTarget2D)TextureContainer.GetTexture(TextureReference.refractionMap), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetNormal), blaster.GetBlasterBolts(), scoreHUD.GetEyeStack(),bomber.GetBubbleList(), shake);
            GetLightManager().DrawShadowsToShadowMaps(game.GraphicsDevice, ShaderContainer.GetShader(ShaderReference.shadowShader), game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.lightball));            
            cracks.DrawToCrackMap(game.GraphicsDevice, game.GetSpriteBatch(), this, (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetNormal), ShaderContainer.GetShader(ShaderReference.maskShader));

            SwapFrontRenderTarget();

            game.GraphicsDevice.SetRenderTarget(frt1);
            
            game.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            game.GetSpriteBatch().Draw((Texture2D)TextureContainer.GetTexture(TextureReference.floor), new Vector2(shake.X / 2.0f, shake.Y / 2.0f), new Rectangle(0, 0, 1080, 1920), new Color(0.5f, 0.5f, 0.5f, 0.95f));
            game.GetSpriteBatch().End();

            // direct shadows
            game.GetSpriteBatch().Begin();
            foreach (AbstractStaticEntity entity in staticEntityList)
            {
                entity.DrawDirectShadow(game.GetSpriteBatch(), new Vector2(shake.X / 2.0f, (shake.Y / 2.0f)+20),(Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));                
            }
            ballsOfDoom.DrawDirectShadow(game.GetSpriteBatch(), new Vector2(shake.X / 2.0f, (shake.Y / 2.0f) + 20), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            player.DrawDirectShadow(game.GetSpriteBatch(), new Vector2(shake.X / 2.0f, (shake.Y / 2.0f) + 20), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            game.GetSpriteBatch().End();

            game.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.Additive);
            game.GetSpriteBatch().Draw(frt2, new Rectangle(0, 0, 1080, 1920), new Rectangle(0, 0, 1080, 1920), new Color(0.5f, 0.5f, 0.5f, 0.5f));
            
            dust.Draw(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            game.GetSpriteBatch().End();            
            
            lightManager.DrawShadows(game.GetSpriteBatch());
            
            frags.Draw(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            SetupShader();
            DrawDynamicEntities();
            DrawStaticEntities();

            ballsOfDoom.Draw(game.GraphicsDevice);
            
            cracks.Draw(game.GraphicsDevice, game.GetSpriteBatch());

            game.GetSpriteBatch().Begin();
            DrawLife(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));

            
            foreach (AbstractStaticEntity entity in staticEntityList)
            {
                entity.DrawRune(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetNormal), time);
            }            
            foreach (PowerUp pu in PowerUp.powerUpList)
            {
                pu.Draw(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            }
            blaster.Draw(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            
            dust.Draw(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));

            bomber.Draw(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));

            game.GetSpriteBatch().End();

            game.GraphicsDevice.SetRenderTarget((RenderTarget2D)TextureContainer.GetTexture(TextureReference.renderTarget));
            game.GetSpriteBatch().Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, ShaderContainer.GetShader(ShaderReference.shockShader));
            ShaderContainer.GetShader(ShaderReference.shockShader).Parameters["RefractionMap"].SetValue(TextureContainer.GetTexture(TextureReference.refractionMap));
            game.GetSpriteBatch().Draw(frt1, new Rectangle(0, 0, 1080, 1920), new Rectangle((int)shake.X, (int)shake.Y, 1080, 1920), color);
            game.GetSpriteBatch().End();
        } 

        public void DrawScore(SpriteBatch spriteBatch, Texture2D texture)
        {
            scoreHUD.Draw(spriteBatch, texture);
        }

        public void DrawLife(SpriteBatch spriteBatch, Texture2D texture)
        {
            scoreHUD.DrawLife(spriteBatch, texture);
        }      

        public void AddLife()
        {
            scoreHUD.AddLife();
        }

        public void SubLife()
        {
            scoreHUD.SubLife();
        }

        public void ExpanderBatCollision(PowerUp powerUp)
        {
            player.ExpandBat();
            scoreHUD.AddExpander();
        }

        public void FlashBatCollision(PowerUp powerUp)
        {
            blaster.Activate();
            scoreHUD.AddBlaster();
        }

        public void HeartBatCollision(PowerUp powerUp)
        {
            scoreHUD.AddLife();
        }

        public void StarBatCollision(PowerUp powerUp)
        {
            scoreHUD.AddScoreMultiplicator();
        }

        public void DestroyPowerUp(PowerUp powerUp)
        { 
            lightManager.RemoveLightRequestor(powerUp);
            PowerUp.powerUpList.Remove(powerUp);
        }

        public void Boom(Vector2 position)
        {
            SoundManager.Play(SoundId.bigExplosion);
            bomber.Add(position);
            ShockRefraction.Add(position, 500);
        }

        public void OnKilledEntity(AbstractStaticEntity entity, Vector2 position, Vector2 velocity)
        {
            // add some dust---------------------------------------
            Color c = entity.color;
            int area = 6000;
            if (entity is Tile)
            {
                area = (int)((Tile)entity).area;
            }
            dust.Add(position, velocity*0.5f, c, area);
            //------------------------------------------------------            
            scoreHUD.AddScore(entity.score, position);
            //------------------------------------------------------
            int number = area/200;
            for (int counter = 0; counter < number; counter++)
            {
                int lifeTime = Globals.rnd.Next(20, 100);
                double v = Globals.rnd.NextDouble() * 2 * Math.PI;
                double sp = Globals.rnd.NextDouble() * 5.0 + 1.0;
                float x = (float)(Math.Cos(v) * sp);
                float y = (float)(Math.Sin(v) * sp);
                Vector2 vel = new Vector2(x, y) + velocity * 0.5f;

                frags.AddParticle(position, vel, c, lifeTime);
            }
        }

        public void OnHitSparks(AbstractStaticEntity entity, Vector2 position, Vector2 velocity)
        {
            int number = 40; 
            for (int counter = 0; counter < number; counter++)
            {
                int lifeTime = Globals.rnd.Next(5, 15);
                double v = Globals.rnd.NextDouble() * 2 * Math.PI;
                double sp = Globals.rnd.NextDouble() * 5.0;
                float x = (float)(Math.Cos(v) * sp);
                float y = (float)(Math.Sin(v) * sp);
                Vector2 vel = new Vector2(x, y) - new Vector2(velocity.Y,-velocity.X);                
                Color c = new Color(0.16f, 0.16f, 0, 0.5f);
                frags.AddSparkParticle(position - velocity,  vel * 2, lifeTime);
                frags.AddSparkParticle(position - velocity, -vel * 2, lifeTime);
                freeLight.Add(position-velocity, c, 1, Globals.sparkLightPriority);
            }
        }

        public void OnHitEntity(AbstractStaticEntity entity, Vector2 position, Vector2 velocity)
        {            
            if (entity.isAlive && entity.hp <= 0)
            {
                SoundManager.Play(SoundId.tilebreak);
                entity.isAlive = false;
                foreach (EffectFactory.EffectFunctionDelegate ok in entity.onKillList)
                {
                    ok(entity, position, velocity);
                }
                
                staticEntityList.Remove((AbstractStaticEntity)entity);
                
                if (entity is Tile)
                {                    
                    cracks.Sub((Tile)entity);                    
                }
                if (((AbstractStaticEntity)entity).powerUp != null)
                {
                    PowerUp.powerUpList.Add(((AbstractStaticEntity)entity).powerUp);
                    lightManager.AddLightRequestor(((AbstractStaticEntity)entity).powerUp);
                }
                needToUpdateIndexList = true;                
            }
            else
            {
                if (entity is Tile)
                {                    
                    cracks.Add((Tile)entity, position);                    
                }
            }            
        }

        public void OnHitShake(AbstractStaticEntity entity, Vector2 position, Vector2 velocity)
        {
            shake =- velocity;
        }

        public void OnHitSound(AbstractStaticEntity entity, Vector2 position, Vector2 velocity)
        {            
            SoundManager.Play(entity.soundId);
        }

        public void OnKilledDetonate(AbstractStaticEntity entity, Vector2 position, Vector2 velocity)  
        {
            SoundManager.Play(SoundId.bigExplosion);
            bomber.Add(entity.collisionRect.Center.ToVector2());
            ShockRefraction.Add(position, 500);
        }
        public  void OnKilledAddBall(AbstractStaticEntity entity, Vector2 position, Vector2 velocity)
        {
            Color c = entity.color;
            velocity.Normalize();
            ballsOfDoom.AddBall(position, velocity*Balls.ballSpeed, c, 1); 
        }

        public void OnBallCollision(Vector2 position, Vector2 normal, SoundId sound)
        {
            SoundManager.Play(sound);

            position = position+normal*Balls.radie;
            int number = 40;
            for (int counter = 0; counter < number; counter++)
            {
                int lifeTime = Globals.rnd.Next(5, 15);
                double v = Globals.rnd.NextDouble() * 2 * Math.PI;
                double sp = Globals.rnd.NextDouble() * 5.0;
                float x = (float)(Math.Cos(v) * sp);
                float y = (float)(Math.Sin(v) * sp);
                Vector2 vel = new Vector2(x, y) - new Vector2(normal.Y, -normal.X);                
                Color c = new Color(0.16f, 0.16f, 0, 0.5f);
                frags.AddSparkParticle(position, vel*5 , lifeTime);
                frags.AddSparkParticle(position, -vel*5 , lifeTime);
                freeLight.Add(position, c, 1, Globals.sparkLightPriority);
            }            
        }
    }
}