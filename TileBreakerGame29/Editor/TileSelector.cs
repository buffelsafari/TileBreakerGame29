using BreakOut.Entities.Static;
using BreakOut.EntityMana;
using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TileBreakerGame29.Entities.Static.Effects;
using static TileBreakerGame29.Entities.Static.Effects.EffectFactory;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace BreakOut.Editor
{
    class TileSelector
    {
        private Rectangle textureRect = new Rectangle(2, 2, 160, 80);
        private List<Tile> tileList;
        public Tile tile { get; set; }
        private Game game;        
        private int tileIndex = 0;
        private Rectangle forwardButtonRect;
        private Rectangle backwardButtonRect;
        private Rectangle tileSelectionRect;

        private Rectangle forwardDrawRect;
        private Rectangle backwardDrawRect;
        private Rectangle middleDrawRect;



        private Color color;
        public int hp { get; private set; }
        public ColorType colorType { get; private set; }
        public EffectType effectType { get; private set; }

        public TileSelector(Game game, EntityManager entityManager, int yOffset)
        {
            this.game = game;
            tileList = new List<Tile>();            
            color = new Color(1.0f, 1.0f, 1.0f);
            hp = 1;
            colorType = ColorType.normal;
            effectType = EffectType.normal;
            foreach (StaticEntityFactory.StaticEntityShape ts in StaticEntityFactory.StaticEntityShape.GetValues(typeof(StaticEntityFactory.StaticEntityShape)))
            {
                if (ts == StaticEntityFactory.StaticEntityShape.LeftWall || ts == StaticEntityFactory.StaticEntityShape.RightWall || ts == StaticEntityFactory.StaticEntityShape.UpperWall)
                {
                }
                else
                {
                    tileList.Add((Tile)StaticEntityFactory.Create(game, entityManager, ts, 0, 0, color,hp, EffectType.normal, colorType));  // 200, 1000
                    
                }
            }

            tile = tileList[0];
            forwardButtonRect = new Rectangle(340, 960, 160, 160);
            backwardButtonRect = new Rectangle(10, 960, 160, 160);
            tileSelectionRect = new Rectangle(170, 960, 170, 160);

            forwardDrawRect=new Rectangle(forwardButtonRect.X, forwardButtonRect.Y + yOffset, forwardButtonRect.Width, forwardButtonRect.Height);
            backwardDrawRect=new Rectangle(backwardButtonRect.X, backwardButtonRect.Y + yOffset, backwardButtonRect.Width, backwardButtonRect.Height);
            middleDrawRect=new Rectangle(tileSelectionRect.X, tileSelectionRect.Y + yOffset, tileSelectionRect.Width, tileSelectionRect.Height);

        }

        public void UpdateColor(Color color)
        {
            this.color = color;
            tile.color = color;
        }

        public void UpdateHp(int hp)
        {
            this.hp = hp;
            tile.hp = hp;
        }

        public void UpdateColorType(ColorType colorType)
        {
            this.colorType = colorType;
            tile.colorType = colorType;
            tile.colorFunction = EffectFactory.GetColorFunction(colorType);
        }

        public void UpdateEffectType(EffectType effectType)
        {
            this.effectType = effectType;
            tile.effectType = effectType;            
        }

        public bool IsDraged(Vector2 position)
        {
            if (tileSelectionRect.Contains(position))
            {
                return true;
            }
            return false;
        }
        public void OnOver(Vector2 position)
        {
            if (tileSelectionRect.Contains(position))
            {
                SoundManager.Play(SoundId.tap);
            }
        }

        public void Update(Vector2 tapPosition)
        {
            bool isTaped = false;
            
            if (forwardButtonRect.Contains(tapPosition))
            {
                isTaped = true;
                
                tileIndex++;
            }
            if (backwardButtonRect.Contains(tapPosition))
            {
                isTaped = true;
                
                tileIndex--;
            }


            if (tileIndex >= tileList.Count)
            {
                tileIndex = tileList.Count - 1;
            }
            else if (tileIndex < 0)
            {
                tileIndex = 0;
            }
            else if (isTaped)
            {
                SoundManager.Play(SoundId.tap);
            }

            tile = tileList[tileIndex];
            tile.color = color;
            tile.hp = hp;
            tile.colorType = colorType;
            tile.colorFunction = EffectFactory.GetColorFunction(colorType);
            tile.effectType = effectType;
        }

        public void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Effect effect)
        {
            Vector2 position = new Vector2(100, 100);

            effect.Parameters["TopLeftColor"].SetValue(tile.colorFunction(tile.color, new Vector2(position.X, position.Y )).ToVector4());
            effect.Parameters["TopRightColor"].SetValue(tile.colorFunction(tile.color, new Vector2(position.X + tile.collisionRect.Width, position.Y)).ToVector4());
            effect.Parameters["BottomLeftColor"].SetValue(tile.colorFunction(tile.color, new Vector2(position.X, position.Y + tile.collisionRect.Height )).ToVector4());
            effect.Parameters["BottomRightColor"].SetValue(tile.colorFunction(tile.color, new Vector2(position.X + tile.collisionRect.Width, position.Y + tile.collisionRect.Height )).ToVector4());
            effect.Parameters["CenterTexCoord"].SetValue(new Vector2(tile.textureRect.Center.X / 640.0f, (tile.textureRect.Center.Y / 640.0f)));


            tile.EditorDraw(spriteBatch, new Vector2(middleDrawRect.Center.X - tile.collisionRect.Width / 2, middleDrawRect.Center.Y - tile.collisionRect.Height / 2), texture);

        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, forwardDrawRect, textureRect, Color.Pink);
            spriteBatch.Draw(texture, backwardDrawRect, textureRect, Color.Aqua);
            spriteBatch.Draw(texture, middleDrawRect, textureRect, Color.Beige);
        }
    }
}