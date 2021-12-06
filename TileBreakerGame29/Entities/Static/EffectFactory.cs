using BreakOut.Entities.Static;
using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using TileBreakerGame29.Entities.Dynamic.PowerUps;

namespace TileBreakerGame29.Entities.Static.Effects
{
    // a composite effect factory
    class EffectFactory
    {
        public delegate void EffectFunctionDelegate(AbstractStaticEntity entity, Vector2 position, Vector2 velocity);
        public enum EffectType
        {            
            normal,
            solidWall,
            multiBall,
            extraLife,
            bonus,
            expander,
            flash,
            bomb,
        }
        public enum ColorType
        { 
            normal,
            rainbow,
            rev_rainbow,
            hor_rainbow,
        }
        
        static public ColorFunctions.ColorFunctionDelegate GetColorFunction(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.normal:
                    return  ColorFunctions.Normal;                    
                case ColorType.rainbow:
                    return ColorFunctions.Rainbow;
                case ColorType.rev_rainbow:
                    return ColorFunctions.ReverseRainbow;
                case ColorType.hor_rainbow:
                    return ColorFunctions.HorizontalRainbow;

            }
            return ColorFunctions.Normal;
        }        

        static public void AddEffects(EntityManager entityManager, AbstractStaticEntity entity, EffectType type, ColorType colorType)
        {
            entity.colorType = colorType;
            entity.effectType = type;
            Vector2 pos = new Vector2(entity.GetCollisionRectangle().Center.X, entity.GetCollisionRectangle().Center.Y);
            entity.colorFunction = GetColorFunction(colorType);            
            
            switch (type)
            {
                case EffectType.normal:
                    entity.isKillable = true;                    
                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitEntity);                    
                    entity.onKillList.Add(entityManager.OnKilledEntity);
                    break;

                case EffectType.solidWall:

                    entity.isKillable = false;
                    entity.onHitList.Add(entityManager.OnHitSparks);
                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitShake);
                    break;

                case EffectType.multiBall:
                    entity.isKillable = true;                    

                    if (((Tile)entity).area>4000)
                    { 
                        entity.showRune = true;
                        entity.runeTextureRect = new Rectangle(15 * 40 - 35, 14 * 40 - 35, 30, 30);
                        int runeSize = 50;
                        entity.runeRect = new Rectangle((int)(entity.collisionRect.Center.X - runeSize * 0.5f), (int)(entity.collisionRect.Center.Y - runeSize * 0.5f), runeSize, runeSize);
                        entity.runeColorFunction = ColorFunctions.Rune;
                    }                    

                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitEntity);
                    
                    entity.onKillList.Add(entityManager.OnKilledEntity);
                    entity.onKillList.Add(entityManager.OnKilledAddBall);
                    break;

                case EffectType.extraLife:
                    
                    entity.powerUp = new PowerUp(PowerUp.PowerUpType.Heart, pos, entityManager.HeartBatCollision, entityManager.DestroyPowerUp);
                    entity.isKillable = true;
                    if (((Tile)entity).area > 4000)
                    {
                        entity.showRune = true;
                        entity.runeTextureRect = new Rectangle(16 * 40 - 35, 15 * 40 - 35, 30, 30);
                        int runeSize = 50;
                        entity.runeRect = new Rectangle((int)(entity.collisionRect.Center.X - runeSize * 0.5f), (int)(entity.collisionRect.Center.Y - runeSize * 0.5f), runeSize, runeSize);
                        entity.runeColorFunction = ColorFunctions.Rune;
                    }
                   
                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitEntity);                    
                    entity.onKillList.Add(entityManager.OnKilledEntity);
                    break;

                case EffectType.bonus:
                    
                    entity.powerUp = new PowerUp(PowerUp.PowerUpType.Star, pos, entityManager.StarBatCollision, entityManager.DestroyPowerUp);
                    entity.isKillable = true;
                    if (((Tile)entity).area > 4000)
                    {
                        entity.showRune = true;
                        entity.runeTextureRect = new Rectangle(16 * 40 - 35, 14 * 40 - 35, 30, 30);
                        int runeSize = 50;
                        entity.runeRect = new Rectangle((int)(entity.collisionRect.Center.X - runeSize * 0.5f), (int)(entity.collisionRect.Center.Y - runeSize * 0.5f), runeSize, runeSize);
                        entity.runeColorFunction = ColorFunctions.Rune;
                    }

                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitEntity);
                    entity.onKillList.Add(entityManager.OnKilledEntity);
                    break;

                case EffectType.expander:

                    entity.powerUp = new PowerUp(PowerUp.PowerUpType.Expander, pos, entityManager.ExpanderBatCollision, entityManager.DestroyPowerUp);
                    entity.isKillable = true;
                    if (((Tile)entity).area > 4000)
                    {
                        entity.showRune = true;
                        entity.runeTextureRect = new Rectangle(16 * 40 - 35, 16 * 40 - 35, 30, 30);
                        int runeSize = 50;
                        entity.runeRect = new Rectangle((int)(entity.collisionRect.Center.X - runeSize * 0.5f), (int)(entity.collisionRect.Center.Y - runeSize * 0.5f), runeSize, runeSize);
                        entity.runeColorFunction = ColorFunctions.Rune;
                    }
                 
                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitEntity);
                    
                    entity.onKillList.Add(entityManager.OnKilledEntity);
                    break;

                case EffectType.flash:

                    entity.powerUp = new PowerUp(PowerUp.PowerUpType.Flash, pos, entityManager.FlashBatCollision, entityManager.DestroyPowerUp);
                    entity.isKillable = true;
                    if (((Tile)entity).area > 4000)
                    {
                        entity.showRune = true;
                        entity.runeTextureRect = new Rectangle(16 * 40 - 35, 13 * 40 - 35, 30, 30);
                        int runeSize = 50;
                        entity.runeRect = new Rectangle((int)(entity.collisionRect.Center.X - runeSize * 0.5f), (int)(entity.collisionRect.Center.Y - runeSize * 0.5f), runeSize, runeSize);
                        entity.runeColorFunction = ColorFunctions.Rune;
                    }
                  
                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitEntity);
                    entity.onKillList.Add(entityManager.OnKilledEntity);
                    
                    break;

                case EffectType.bomb:
                    
                    entity.isKillable = true;
                    if (((Tile)entity).area > 4000)
                    {
                        entity.showRune = true;
                        entity.runeTextureRect = new Rectangle(15 * 40 - 35, 15 * 40 - 35, 30, 30);
                        int runeSize = 50;
                        entity.runeRect = new Rectangle((int)(entity.collisionRect.Center.X - runeSize * 0.5f), (int)(entity.collisionRect.Center.Y - runeSize * 0.5f), runeSize, runeSize);
                        entity.runeColorFunction = ColorFunctions.Rune;
                    }                  
                    
                    entity.onHitList.Add(entityManager.OnHitSound);
                    entity.onHitList.Add(entityManager.OnHitEntity);

                    entity.onKillList.Add(entityManager.OnKilledEntity);
                    entity.onKillList.Add(entityManager.OnKilledDetonate);

                    break;
            }
        }
    }
}