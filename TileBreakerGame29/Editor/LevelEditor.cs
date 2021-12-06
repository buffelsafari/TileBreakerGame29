using BreakOut.Entities.Static;
using BreakOut.EntityMana;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using TileBreakerGame29.Editor;
using TileBreakerGame29.Entities.Static.Effects;
using static BreakOut.Entities.Static.StaticEntityFactory;
using static BreakOut.TextureContainer;
using static TileBreakerGame29.ColorFunctions;
using static TileBreakerGame29.Entities.Static.Effects.EffectFactory;

namespace BreakOut.Editor
{
    class LevelEditor        
    {
        public bool leaveEditor { get; set; } = false; 
        private int time = 0;
        private string currentLoadedLevel;
        private Level level;
        private bool needToUpdateLevel = false;

        const int stepX = 41;
        const int stepY = 24;

        const int wallOffsetX = 7;
        const int wallOffsetY = 7;

        const int yOffset = 6*stepY;

        private List<Tile> wallTileList;  // only to block bad placment

        private List<Tile> tileList;
        private List<Tile> removeList;

        private UpperWall upperWall;
        private LeftWall leftWall;
        private RightWall rightWall;

        private Tile selectorTile;
        private Tile oldTile;
        private Vector2 dragPosition;
        private Vector2 downPosition;

        private int adHeight;
        private Game1 game;

        private bool isTouched=false;
        private bool isFirstDrag = true;

        private TileSelector tileSelector;
        private ColorSelector colorSelector;
        private ListSelector colorEffectSelector;
        private ListSelector effectSelector;
        private ListSelector hpSelector;

        private TrashCan trashCan;
        private InformationBar informationBar;
        private EditorButton setWallButton;
        private EditorButton clearButton;
        private EditorButton backButton;

        private YesNoDialog yesNoDialog;

        public LevelEditor(Game1 game,EntityManager entityManager, int adHeight)
        {
            this.game = game;
            this.adHeight = adHeight;
            tileList = new List<Tile>();
            removeList = new List<Tile>();
            wallTileList = new List<Tile>();
            // create som collision walls in a lazy way
            for (int counter = -1; counter < 26; counter+=2)
            {
                wallTileList.Add((Tile)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.Hexagon, 41 * counter + 7, 72 * -1 + 7, Color.YellowGreen,1, EffectType.normal, ColorType.normal));
            }
            for (int counter = 1; counter < 10; counter += 2)
            {
                wallTileList.Add((Tile)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.Hexagon, 41 * -1 + 7, 72 * counter + 7, Color.Green,1 ,EffectType.normal, ColorType.normal));
                wallTileList.Add((Tile)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.Hexagon, 41 * (25) + 7, 72 * counter + 7, Color.Green,1, EffectType.normal, ColorType.normal));

            }
            for (int counter = 1; counter < 12; counter += 2)
            {
                wallTileList.Add((Tile)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.Hexagon, 41 * -2 + 7, 72 * (counter-1) + 7, Color.White,1, EffectType.normal, ColorType.normal));
                wallTileList.Add((Tile)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.Hexagon, 41 * (26) + 7, 72 * (counter - 1) + 7, Color.White,1, EffectType.normal, ColorType.normal));
            }

            tileSelector = new TileSelector(game, entityManager, yOffset);
            colorSelector = new ColorSelector(yOffset);

            //-----------------------------------------
            string[] names = new string[Enum.GetNames(typeof(EffectFactory.ColorType)).Length];
            int i = 0;
            foreach (EffectFactory.ColorType ct in Enum.GetValues(typeof(EffectFactory.ColorType)))
            {
                names[i++] = ct.ToString();
            }
            colorEffectSelector = new ListSelector(names, new Rectangle(10,1140, 490, 100), yOffset);
            
            string[] effectNames = new string[Enum.GetNames(typeof(EffectFactory.EffectType)).Length];
            i = 0;
            foreach (EffectFactory.EffectType et in Enum.GetValues(typeof(EffectFactory.EffectType)))
            {
                effectNames[i++] = et.ToString();
            }
            effectSelector = new ListSelector(effectNames, new Rectangle(10, 1260, 490, 100), yOffset);

            string[] hpNames = new string[5];
            for (i = 0; i < hpNames.Length; i++)
            {
                hpNames[i] = "hp:"+(i+1);
            }

            hpSelector = new ListSelector(hpNames, new Rectangle(10, 1380, 490, 100), yOffset);
            trashCan = new TrashCan(new Rectangle(10,830,130,130), yOffset);
            informationBar = new InformationBar(yOffset);
            setWallButton = new EditorButton(new Rectangle(1080 - 270, 840, 260, 100), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Wall), yOffset);
            clearButton = new EditorButton(new Rectangle(1080 - 550, 840, 260, 100), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Clear), yOffset);
            backButton = new EditorButton(new Rectangle(1080 - 830, 840, 260, 100), Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Back), yOffset);
            yesNoDialog = new YesNoDialog(Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.ClearLevel), yOffset);

            upperWall = (UpperWall)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.UpperWall, 0, 0, Color.White, 0, EffectType.solidWall, ColorType.rainbow);
            leftWall = (LeftWall)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.LeftWall, 0, 0, Color.White, 0, EffectType.solidWall, ColorType.rainbow);
            rightWall = (RightWall)StaticEntityFactory.Create(game, entityManager, StaticEntityShape.RightWall, 0, 0, Color.White, 0, EffectType.solidWall, ColorType.rainbow);
            
            selectorTile = null;
            oldTile = null;

            dragPosition = new Vector2(0, 0);
            downPosition = new Vector2(0,0);


            level = new Level(game, entityManager);
            level.loadingComplete=OnLevelLoaded;
                        
        }
                
        public void Update(EntityManager entityManager)
        {
            time++;
            foreach (Tile t in removeList)
            {
                tileList.Remove(t);
            }
            removeList.Clear();            

            TouchCollection tc = TouchPanel.GetState();
            GestureSample gesture;

            if (yesNoDialog.isActive)
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    gesture = TouchPanel.ReadGesture();
                    if (yesNoDialog.OnTap(ConvertTap(gesture.Position)) == 1)
                    {
                        yesNoDialog.isActive = false;
                        tileList.Clear();                        
                    }
                    if (yesNoDialog.OnTap(ConvertTap(gesture.Position)) == 2)
                    {
                        yesNoDialog.isActive = false;                        
                    }
                }

                return; // dialog steals focus 
            }
            
            if (tc.Count > 0)
            {
                if (selectorTile == null)
                {
                    colorSelector.UpdateSliders(ConvertTap(tc[0].Position));
                    tileSelector.UpdateColor(colorSelector.GetColor());
                    informationBar.UpdateTileInformation(null);
                    informationBar.UpdateColorInformation(colorSelector.GetColor());                    
                    
                }
                else
                { 
                    trashCan.OnOver(dragPosition);
                }

                if (!isTouched)
                {
                    downPosition = tc[0].Position;
                    tileSelector.OnOver(ConvertTap(downPosition));

                    foreach (Tile t in tileList)
                    {
                        if (IsTouched(t, ConvertTap(downPosition)))
                        {
                            SoundManager.Play(SoundId.tap);
                        }
                    }
                    isTouched = true;
                }
            }
            else
            {
                isTouched = false;                
            }
            
            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.Hold)
                {
                   
                }

                if (gesture.GestureType == GestureType.Tap)
                {                    
                    tileSelector.Update(ConvertTap(gesture.Position));

                    if (backButton.OnTap(ConvertTap(gesture.Position)))
                    {                        
                        leaveEditor = true;                        
                    }
                    if (clearButton.OnTap(ConvertTap(gesture.Position)))
                    {
                        yesNoDialog.isActive = true;                        
                    }
                    if (setWallButton.OnTap(ConvertTap(gesture.Position)))
                    {
                        Color c= colorSelector.GetColor();
                        ColorType ct = (ColorType)colorEffectSelector.GetIndex();
                        ColorFunctionDelegate cf = EffectFactory.GetColorFunction(ct);

                        upperWall.color = c;
                        upperWall.colorType = ct;
                        upperWall.colorFunction = cf;

                        leftWall.color = c;
                        leftWall.colorType = ct;
                        leftWall.colorFunction = cf;

                        rightWall.color = c;
                        rightWall.colorType = ct;
                        rightWall.colorFunction = cf;
                    }
                    
                    if (colorEffectSelector.OnTap(ConvertTap(gesture.Position)))
                    {
                        tileSelector.UpdateColorType((ColorType)colorEffectSelector.GetIndex());                        
                    }
                    
                    if (effectSelector.OnTap(ConvertTap(gesture.Position)))
                    {
                        tileSelector.UpdateEffectType((EffectType)effectSelector.GetIndex());                        
                    }
                    
                    if (hpSelector.OnTap(ConvertTap(gesture.Position)))
                    {
                        tileSelector.UpdateHp(hpSelector.GetIndex()+1);                        
                    }

                    if (colorSelector.OnTap(ConvertTap(gesture.Position)))
                    {
                        colorSelector.SetColor(colorSelector.GetColor());
                        tileSelector.UpdateColor(colorSelector.GetColor());
                        informationBar.UpdateColorInformation(colorSelector.GetColor());
                    }

                    foreach (Tile t in tileList)
                    {
                        if (IsTouched(t, ConvertTap(gesture.Position)))
                        {                            
                            colorSelector.SetColor(t.color);
                            tileSelector.UpdateColor(t.color);
                            informationBar.UpdateTileInformation(t);
                            break;
                        }
                        informationBar.UpdateTileInformation(null);
                        informationBar.UpdateColorInformation(colorSelector.GetColor());
                    }
                }
                if (gesture.GestureType == GestureType.DragComplete)
                {                    
                    isFirstDrag = true;
                    if (selectorTile != null)
                    {
                        bool test = true;
                        bool isTrashed = false;

                        if (trashCan.OnDrop(dragPosition))
                        {                            
                            informationBar.UpdateTileInformation(null);
                            informationBar.UpdateColorInformation(colorSelector.GetColor());
                            oldTile = null;
                            test = false;
                            isTrashed = true;
                        }                        
                        if ((dragPosition.Y+selectorTile.collisionRect.Height) > (72 * 11+7+24) || dragPosition.Y < (72 * 0 + 7-72))
                        {                            
                            test = false;
                        }

                        foreach (Tile t in tileList)
                        {
                            if (TestIfOverlap(selectorTile, t, dragPosition))
                            {
                                test = false;
                                break;
                            }                            
                        }
                        foreach (Tile t in wallTileList)
                        {
                            if (TestIfOverlap(selectorTile, t, dragPosition))
                            {
                                test = false;
                                break;
                            }
                        }                        

                        if (test)
                        {
                            SoundManager.Play(SoundId.tilehit);
                            Tile nt = (Tile)StaticEntityFactory.Create(game, entityManager, selectorTile.shape, (int)dragPosition.X, (int)dragPosition.Y, selectorTile.color, selectorTile.hp, selectorTile.effectType, selectorTile.colorType);
                            tileList.Add(nt);
                            informationBar.UpdateTileInformation(nt);
                            colorSelector.AddRecentColor(selectorTile.color);
                        }
                        else
                        {
                            if (!isTrashed)
                            {
                                SoundManager.Play(SoundId.denial);
                            }
                            
                            if (oldTile != null)
                            {                                
                                tileList.Add(oldTile);
                                oldTile = null;
                            }
                        }
                    }
                    selectorTile = null;
                }
                if (gesture.GestureType == GestureType.FreeDrag)
                {                    
                    if (isFirstDrag)
                    {                        
                        if(tileSelector.IsDraged(ConvertTap(downPosition)))
                        {
                            selectorTile = (Tile)StaticEntityFactory.Create(game, entityManager, tileSelector.tile.shape, 0, 0, tileSelector.tile.color,tileSelector.hp, tileSelector.effectType, tileSelector.colorType);
                            dragPosition = ConvertToDiscreteSteps(ConvertToGameSpace(downPosition));
                            oldTile = null;                            
                        }
                        else
                        {
                            foreach (Tile t in tileList)
                            {
                                if (IsTouched(t, ConvertTap(downPosition)))
                                {                                    
                                    selectorTile = (Tile)StaticEntityFactory.Create(game, entityManager, t.shape, 0, 0, t.color,t.hp, t.effectType, t.colorType);
                                    dragPosition = ConvertToDiscreteSteps(ConvertToGameSpace(downPosition));
                                    oldTile = t;
                                    removeList.Add(t);
                                    break;
                                }
                            }
                        }                   
                    }

                    if (selectorTile != null)
                    {
                        dragPosition = ConvertToDiscreteSteps(ConvertToGameSpace(gesture.Position));                        
                    }
                    isFirstDrag = false;
                }
            }

            trashCan.Update();

            if (needToUpdateLevel)
            {
                UpdateLevel();
            }
        }

        private bool TestIfOverlap(AbstractStaticEntity tile, AbstractStaticEntity tile2, Vector2 offset)
        {
            List<Vector2> separatingAxis = new List<Vector2>();
            foreach (LineSegment ls in tile.segments)
            {
                separatingAxis.Add(ls.normal);
            }
            foreach (LineSegment ls in tile2.segments)
            {
                separatingAxis.Add(ls.normal);
            }

            foreach (Vector2 axe in separatingAxis)
            {                
                float tile1AxisMax = float.MinValue;
                float tile1AxisMin = float.MaxValue;
                foreach (LineSegment ls in tile.segments)
                {
                    float projection = Vector2.Dot(ls.point1 + offset, axe);
                    if (projection > tile1AxisMax)
                    {
                        tile1AxisMax = projection;
                    }
                    if (projection < tile1AxisMin)
                    {
                        tile1AxisMin = projection;
                    }
                }
                float tile2AxisMax = float.MinValue;
                float tile2AxisMin = float.MaxValue;
                foreach (LineSegment ls in tile2.segments)
                {
                    float projection = Vector2.Dot(ls.point1, axe);
                    if (projection > tile2AxisMax)
                    {
                        tile2AxisMax = projection;
                    }
                    if (projection < tile2AxisMin)
                    {
                        tile2AxisMin = projection;
                    }
                }

                float delta = 10;
                if ((tile2AxisMin) > (tile1AxisMax - delta) || (tile2AxisMax) < (tile1AxisMin + delta))
                {                    
                    return false;
                }
            }
            return true;
        }

        private bool IsTouched(AbstractStaticEntity tile, Vector2 touchPoint)
        {
            List<Vector2> separatingAxis = new List<Vector2>();
            foreach (LineSegment ls in tile.segments)
            {
                separatingAxis.Add(ls.normal);
            }            

            foreach (Vector2 axe in separatingAxis)
            {
                float touchProjection = Vector2.Dot(touchPoint, axe);
                float tile1AxisMax = float.MinValue;
                float tile1AxisMin = float.MaxValue;
                foreach (LineSegment ls in tile.segments)
                {
                    float projection = Vector2.Dot(ls.point1, axe);
                    if (projection > tile1AxisMax)
                    {
                        tile1AxisMax = projection;
                    }
                    if (projection < tile1AxisMin)
                    {
                        tile1AxisMin = projection;
                    }
                }                
                
                float delta = tile.fingerDelta*game.settings.editorArea;  // utöka pick område
                if ((touchProjection) > (tile1AxisMax - delta) || (touchProjection) < (tile1AxisMin + delta))
                {                    
                    return false;
                }
            }            
            return true;
        }

        private Vector2 ConvertTap(Vector2 position)
        {
            position.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            position.Y = (position.Y - adHeight) * (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            position.Y -= yOffset;
            return position;
        }
        private Vector2 ConvertToGameSpace(Vector2 position)
        {
            position.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            position.Y = (position.Y - adHeight) * (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            return position;
        }

        private Vector2 ConvertToDiscreteSteps(Vector2 position)
        {
            position.X -=selectorTile.collisionRect.Width / 2-16;  
            position.Y -= (selectorTile.collisionRect.Height / 2)-8+game.settings.editorOffset;

            position.X = (int)(position.X / stepX) * stepX + wallOffsetX;
            position.Y = (int)(position.Y / stepY) * stepY + wallOffsetY-yOffset;
            return position;
        }


        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Texture2D normalTexture,Texture2D editorTexture, Effect effect)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);
            spriteBatch.Draw((Texture2D)TextureContainer.GetTexture(TextureReference.floor), new Vector2(0,yOffset), new Rectangle(0, 0, 1080, 830), new Color(0.5f, 0.5f, 0.5f, 0.95f));
            spriteBatch.Draw((Texture2D)TextureContainer.GetTexture(TextureReference.floor), new Vector2(0, yOffset+830), new Rectangle(0, 0, 1080, 830), new Color(0.7f, 0.7f, 0.7f, 1.00f));
            spriteBatch.End();

            spriteBatch.Begin();
            tileSelector.Draw(spriteBatch, texture);            
            colorEffectSelector.Draw(spriteBatch, texture);
            effectSelector.Draw(spriteBatch, texture);
            hpSelector.Draw(spriteBatch, texture);
            setWallButton.Draw(spriteBatch, texture);
            clearButton.Draw(spriteBatch, texture);
            backButton.Draw(spriteBatch, texture);

            colorSelector.Draw(spriteBatch, editorTexture);
            trashCan.Draw(spriteBatch, editorTexture);

            spriteBatch.End();

            effect.Parameters["ViewportSize"].SetValue(new Vector2(1080, 1920)); 
            
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, effect);
            
            rightWall.EditorDraw(spriteBatch, new Vector2(0, yOffset), texture, effect);
            leftWall.EditorDraw(spriteBatch, new Vector2(0, yOffset), texture, effect);
            upperWall.EditorDraw(spriteBatch, new Vector2(0, yOffset), texture, effect);
            
            foreach (Tile t in tileList)
            {
                effect.Parameters["TopLeftColor"].SetValue(t.colorFunction(t.color, new Vector2(t.collisionRect.Left, t.collisionRect.Top - yOffset*0)).ToVector4());
                effect.Parameters["TopRightColor"].SetValue(t.colorFunction(t.color, new Vector2(t.collisionRect.Right, t.collisionRect.Top - yOffset*0)).ToVector4());
                effect.Parameters["BottomLeftColor"].SetValue(t.colorFunction(t.color, new Vector2(t.collisionRect.Left, t.collisionRect.Bottom - yOffset*0)).ToVector4());
                effect.Parameters["BottomRightColor"].SetValue(t.colorFunction(t.color, new Vector2(t.collisionRect.Right, t.collisionRect.Bottom - yOffset*0)).ToVector4());
                effect.Parameters["CenterTexCoord"].SetValue(new Vector2(t.textureRect.Center.X/640.0f, (t.textureRect.Center.Y / 640.0f)));
                t.EditorDraw(spriteBatch, new Vector2(0, yOffset), texture); 
            }

            if (selectorTile != null)
            {
                
                effect.Parameters["TopLeftColor"].SetValue(selectorTile.colorFunction(selectorTile.color, new Vector2(dragPosition.X, dragPosition.Y - yOffset*0)).ToVector4());
                effect.Parameters["TopRightColor"].SetValue(selectorTile.colorFunction(selectorTile.color, new Vector2(dragPosition.X+selectorTile.collisionRect.Width, dragPosition.Y - yOffset*0)).ToVector4());
                effect.Parameters["BottomLeftColor"].SetValue(selectorTile.colorFunction(selectorTile.color, new Vector2(dragPosition.X, dragPosition.Y+selectorTile.collisionRect.Height - yOffset*0)).ToVector4());
                effect.Parameters["BottomRightColor"].SetValue(selectorTile.colorFunction(selectorTile.color, new Vector2(dragPosition.X + selectorTile.collisionRect.Width, dragPosition.Y + selectorTile.collisionRect.Height - yOffset*0)).ToVector4());
                effect.Parameters["CenterTexCoord"].SetValue(new Vector2(selectorTile.textureRect.Center.X / 640.0f, (selectorTile.textureRect.Center.Y / 640.0f)));
                selectorTile.EditorDraw(spriteBatch, dragPosition + new Vector2(0, yOffset), texture);
            }
            
            tileSelector.DrawTile(spriteBatch, texture, effect);
            spriteBatch.End();

            spriteBatch.Begin();            
            informationBar.Draw(spriteBatch, texture);

            foreach (Tile t in tileList)
            {                
                t.DrawRune(spriteBatch, normalTexture, yOffset, time);
            }
            
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, effect);
            informationBar.DrawTile(spriteBatch, texture, effect);
            spriteBatch.End();

            if (yesNoDialog.isActive)
            {
                spriteBatch.Begin();
                yesNoDialog.Draw(spriteBatch, texture);
                spriteBatch.End();            
            }
        }


        public void SaveLevel(Game1 game)
        {
            if (currentLoadedLevel != null)
            {
                if (tileList.Count > 0)
                {
                    level.SetLevel(tileList, upperWall, leftWall, rightWall);
                    level.Save(game, currentLoadedLevel);
                }
                else
                {
                    level.Delete(game, currentLoadedLevel);
                }
            }            
        }

        public void LoadLevel(Game1 game, string filename)
        {
            tileList.Clear();            
            level.Load(game, game.GetEntityManager(), filename, true);
            currentLoadedLevel = filename;
        }

        private void OnLevelLoaded()
        {
            needToUpdateLevel = true;            
        }

        private void UpdateLevel()
        {
            tileList.Clear();

            foreach (AbstractStaticEntity entity in level.GetTiles())
            {
                needToUpdateLevel = false;
                if (entity is Tile)
                {
                    tileList.Add((Tile)entity);
                }
                else if (entity is UpperWall)
                {
                    upperWall = (UpperWall)entity;
                }
                else if (entity is LeftWall)
                {
                    leftWall = (LeftWall)entity;
                }
                else if (entity is RightWall)
                {
                    rightWall = (RightWall)entity;
                }
            }
        }     
    }
}