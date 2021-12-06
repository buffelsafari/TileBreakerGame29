using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using TileBreakerGame29.Entities.Static.Effects;
using static TileBreakerGame29.Entities.Static.Effects.EffectFactory;

namespace BreakOut.Entities.Static
{
    class StaticEntityFactory
    {

        const int HEXAGON_OFFSET_X = 135;
        const int HEXAGON_OFFSET_Y = 359;

        const int DIAMOND_TOP_OFFSET_X = 332;
        const int DIAMOND_TOP_OFFSET_Y = 79;

        const int DIAMOND_LEFT_OFFSET_X = 332;
        const int DIAMOND_LEFT_OFFSET_Y = 2;

        const int DIAMOND_RIGHT_OFFSET_X = 378;
        const int DIAMOND_RIGHT_OFFSET_Y = 2;

        const int PRISMATIC_TOP_OFFSET_X = 533;
        const int PRISMATIC_TOP_OFFSET_Y = 2;

        const int PRISMATIC_BOTTOM_OFFSET_X = 437;
        const int PRISMATIC_BOTTOM_OFFSET_Y = 304;

        const int FLORAL_TOP_LEFT_OFFSET_X = 176;
        const int FLORAL_TOP_LEFT_OFFSET_Y = 233;

        const int FLORAL_TOP_RIGHT_OFFSET_X = 263;
        const int FLORAL_TOP_RIGHT_OFFSET_Y = 233;

        const int FLORAL_LEFT_OFFSET_X = 176;
        const int FLORAL_LEFT_OFFSET_Y = 86;

        const int FLORAL_RIGHT_OFFSET_X = 305;
        const int FLORAL_RIGHT_OFFSET_Y = 132;

        const int FLORAL_BOTTOM_LEFT_OFFSET_X = 2;
        const int FLORAL_BOTTOM_LEFT_OFFSET_Y = 132;

        const int FLORAL_BOTTOM_RIGHT_OFFSET_X = 89;
        const int FLORAL_BOTTOM_RIGHT_OFFSET_Y = 132;

        const int HALVED_LEFT_OFFSET_X = 89;
        const int HALVED_LEFT_OFFSET_Y = 310;

        const int HALVED_RIGHT_OFFSET_X = 350;
        const int HALVED_RIGHT_OFFSET_Y = 310;

        const int HALVED_TOP_LEFT_OFFSET_X = 424;
        const int HALVED_TOP_LEFT_OFFSET_Y = 2;

        const int HALVED_TOP_RIGHT_OFFSET_X = 434;
        const int HALVED_TOP_RIGHT_OFFSET_Y = 79;

        const int HALVED_BOTTOM_LEFT_OFFSET_X = 350;
        const int HALVED_BOTTOM_LEFT_OFFSET_Y = 233;

        const int HALVED_BOTTOM_RIGHT_OFFSET_X = 2;
        const int HALVED_BOTTOM_RIGHT_OFFSET_Y = 310;

        const int TRIANGLE_LEFT_OFFSET_X = 585;
        const int TRIANGLE_LEFT_OFFSET_Y = 150;

        const int TRIANGLE_RIGHT_OFFSET_X = 585;
        const int TRIANGLE_RIGHT_OFFSET_Y = 203;

        public enum StaticEntityShape
        {
            Hexagon,
            DiamondTop,
            DiamondLeft,
            DiamondRight,
            PrismaticTop,
            PrismaticBottom,
            FloralPentagonTopLeft,
            FloralPentagonTopRight,
            FloralPentagonLeft,
            FloralPentagonRight,
            FloralPentagonBottomLeft,
            FloralPentagonBottomRight,
            HalvedLeft,
            HalvedRight,
            HalvedTopLeft,
            HalvedBottomRight,
            HalvedTopRight,
            HalvedBottomLeft,
            TriangleLeft,
            TriangleRight,

            LeftWall,
            RightWall,
            UpperWall,
                       
        }        

        public static AbstractStaticEntity Create(Game game, EntityManager entityManager, StaticEntityShape shape, int x, int y, Color color, int hp, EffectType effectType, ColorType colorType)
        {            
            Vector2 position = new Vector2(x, y);
            AbstractStaticEntity entity;
            switch (shape)
            {
                case StaticEntityShape.Hexagon:
                    entity = Hexagon(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);                    
                    return entity;
                case StaticEntityShape.DiamondTop:
                    entity = DiamondTop(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.DiamondLeft:
                    entity = DiamondLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.DiamondRight:
                    entity = DiamondRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.PrismaticTop:
                    entity = PrismaticTop(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.PrismaticBottom:
                    entity = PrismaticBottom(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.FloralPentagonTopLeft:
                    entity = FloralPentagonTopLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.FloralPentagonTopRight:
                    entity = FloralPentagonTopRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.FloralPentagonLeft:
                    entity = FloralPentagonLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.FloralPentagonRight:
                    entity = FloralPentagonRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.FloralPentagonBottomLeft:
                    entity = FloralPentagonBottomLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.FloralPentagonBottomRight:
                    entity = FloralPentagonBottomRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.HalvedTopLeft:
                    entity = HalvedTopLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.HalvedTopRight:
                    entity = HalvedTopRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.HalvedLeft:
                    entity = HalvedLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.HalvedRight:
                    entity = HalvedRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.HalvedBottomLeft:
                    entity = HalvedBottomLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.HalvedBottomRight:
                    entity = HalvedBottomRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.TriangleLeft:
                    entity = TriangleLeft(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.TriangleRight:
                    entity = TriangleRight(position, color, hp);
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;

                case StaticEntityShape.LeftWall:
                    entity = new LeftWall(color);
                    entity.shape= StaticEntityShape.LeftWall;
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.RightWall:
                    entity = new RightWall(color);
                    entity.shape = StaticEntityShape.RightWall;
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
                case StaticEntityShape.UpperWall:
                    entity = new UpperWall(color);
                    entity.shape = StaticEntityShape.UpperWall;
                    EffectFactory.AddEffects(entityManager, entity, effectType, colorType);
                    return entity;
            }

            return null;
        }

        private static Tile Hexagon(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[6];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X, 24 + position.Y);
            point[3] = new Vector2(82 + position.X, 72 + position.Y);
            point[4] = new Vector2(41 + position.X, 96 + position.Y);
            point[5] = new Vector2(0 + position.X, 72 + position.Y);
            Tile tile = new Tile(point, color, HEXAGON_OFFSET_X, HEXAGON_OFFSET_Y);
            tile.shape = StaticEntityShape.Hexagon;
            tile.hp = hp;
            return tile;
        }

        private static Tile DiamondTop(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X, 24 + position.Y);            
            point[3] = new Vector2(41 + position.X, 96 + position.Y - 48);
            Tile tile = new Tile(point, color, DIAMOND_TOP_OFFSET_X, DIAMOND_TOP_OFFSET_Y);
            tile.shape = StaticEntityShape.DiamondTop;
            tile.hp = hp;
            return tile;
        }

        private static Tile DiamondLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];
            point[0] = new Vector2(0 + position.X, 24 + position.Y-24);
            point[1] = new Vector2(41 + position.X, 48 + position.Y-24);          
            point[2] = new Vector2(41 + position.X, 96 + position.Y-24);
            point[3] = new Vector2(0 + position.X, 72 + position.Y-24);
            Tile tile = new Tile(point, color, DIAMOND_LEFT_OFFSET_X, DIAMOND_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.DiamondLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile DiamondRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];
            point[0] = new Vector2(41 + position.X-41, 48 + position.Y-24);
            point[1] = new Vector2(82 + position.X-41, 24 + position.Y-24);
            point[2] = new Vector2(82 + position.X-41, 72 + position.Y-24);
            point[3] = new Vector2(41 + position.X-41, 96 + position.Y-24);
            Tile tile = new Tile(point, color, DIAMOND_RIGHT_OFFSET_X, DIAMOND_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.DiamondRight;
            tile.hp = hp;
            return tile;
        }

        private static Tile PrismaticTop(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X, 24 + position.Y);
            point[3] = new Vector2(82 + position.X, 72 + position.Y + 48);            
            point[4] = new Vector2(0 + position.X, 72 + position.Y + 48);
            Tile tile = new Tile(point, color, PRISMATIC_TOP_OFFSET_X, PRISMATIC_TOP_OFFSET_Y);
            tile.shape = StaticEntityShape.PrismaticTop;
            tile.hp = hp;
            return tile;
        }

        private static Tile PrismaticBottom(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(0 + position.X, 24 + position.Y - 48+24);            
            point[1] = new Vector2(82 + position.X, 24 + position.Y - 48+24);
            point[2] = new Vector2(82 + position.X, 72 + position.Y+24);
            point[3] = new Vector2(41 + position.X, 96 + position.Y+24);
            point[4] = new Vector2(0 + position.X, 72 + position.Y+24);
            Tile tile = new Tile(point, color, PRISMATIC_BOTTOM_OFFSET_X, PRISMATIC_BOTTOM_OFFSET_Y);
            tile.shape = StaticEntityShape.PrismaticBottom;
            tile.hp = hp;
            return tile;
        }

        private static Tile FloralPentagonTopLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X, 24 + position.Y);
            point[3] = new Vector2(82 + position.X, 72 + position.Y + 48);            
            point[4] = new Vector2(0 + position.X, 72 + position.Y);
            Tile tile = new Tile(point, color, FLORAL_TOP_LEFT_OFFSET_X, FLORAL_TOP_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.FloralPentagonTopLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile FloralPentagonTopRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X, 24 + position.Y);
            point[3] = new Vector2(82 + position.X, 72 + position.Y);           
            point[4] = new Vector2(0 + position.X, 72 + position.Y + 48);
            Tile tile = new Tile(point, color, FLORAL_TOP_RIGHT_OFFSET_X, FLORAL_TOP_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.FloralPentagonTopRight;
            tile.hp = hp;
            return tile;

        }

        private static Tile FloralPentagonLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X + 42, 24 + position.Y + 24);            
            point[3] = new Vector2(41 + position.X, 96 + position.Y);
            point[4] = new Vector2(0 + position.X, 72 + position.Y);
            Tile tile = new Tile(point, color, FLORAL_LEFT_OFFSET_X, FLORAL_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.FloralPentagonLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile FloralPentagonRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(41 + position.X+42, 0 + position.Y);
            point[1] = new Vector2(82 + position.X+42, 24 + position.Y);
            point[2] = new Vector2(82 + position.X+42, 72 + position.Y);
            point[3] = new Vector2(41 + position.X+42, 96 + position.Y);
            point[4] = new Vector2(0 + position.X - 42+42, 72 + position.Y - 24);
            Tile tile = new Tile(point, color, FLORAL_RIGHT_OFFSET_X, FLORAL_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.FloralPentagonRight;
            tile.hp = hp;
            return tile;
        }

        private static Tile FloralPentagonBottomLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(0 + position.X, 24 + position.Y+24);            
            point[1] = new Vector2(82 + position.X, 24 + position.Y - 48+24);
            point[2] = new Vector2(82 + position.X, 72 + position.Y+24);
            point[3] = new Vector2(41 + position.X, 96 + position.Y+24);
            point[4] = new Vector2(0 + position.X, 72 + position.Y+24);
            Tile tile = new Tile(point, color, FLORAL_BOTTOM_LEFT_OFFSET_X, FLORAL_BOTTOM_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.FloralPentagonBottomLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile FloralPentagonBottomRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[5];
            point[0] = new Vector2(0 + position.X, 24 + position.Y - 48+24);            
            point[1] = new Vector2(82 + position.X, 24 + position.Y+24);
            point[2] = new Vector2(82 + position.X, 72 + position.Y+24);
            point[3] = new Vector2(41 + position.X, 96 + position.Y+24);
            point[4] = new Vector2(0 + position.X, 72 + position.Y+24);
            Tile tile = new Tile(point, color, FLORAL_BOTTOM_RIGHT_OFFSET_X, FLORAL_BOTTOM_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.FloralPentagonBottomRight;
            tile.hp = hp;
            return tile;
        }

        private static Tile HalvedTopLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X, 24 + position.Y);            
            point[3] = new Vector2(0 + position.X, 72 + position.Y);
            Tile tile = new Tile(point, color, HALVED_TOP_LEFT_OFFSET_X, HALVED_TOP_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.HalvedTopLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile HalvedTopRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);
            point[2] = new Vector2(82 + position.X, 24 + position.Y);
            point[3] = new Vector2(82 + position.X, 72 + position.Y);          
            Tile tile = new Tile(point, color, HALVED_TOP_RIGHT_OFFSET_X, HALVED_TOP_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.HalvedTopRight;
            tile.hp = hp;
            return tile;
        }

        private static Tile HalvedLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];
            point[0] = new Vector2(0 + position.X, 24 + position.Y);
            point[1] = new Vector2(41 + position.X, 0 + position.Y);            
            point[2] = new Vector2(41 + position.X, 96 + position.Y);
            point[3] = new Vector2(0 + position.X, 72 + position.Y);
            Tile tile = new Tile(point, color, HALVED_LEFT_OFFSET_X, HALVED_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.HalvedLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile HalvedRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];            
            point[0] = new Vector2(41 + position.X-41, 0 + position.Y);
            point[1] = new Vector2(82 + position.X-41, 24 + position.Y);
            point[2] = new Vector2(82 + position.X-41, 72 + position.Y);
            point[3] = new Vector2(41 + position.X-41, 96 + position.Y);            
            Tile tile = new Tile(point, color, HALVED_RIGHT_OFFSET_X, HALVED_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.HalvedRight;
            tile.hp = hp;
            return tile;
        }

        private static Tile HalvedBottomLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];
            point[0] = new Vector2(0 + position.X, 24 + position.Y-24);           
            point[1] = new Vector2(82 + position.X, 72 + position.Y-24);
            point[2] = new Vector2(41 + position.X, 96 + position.Y-24);
            point[3] = new Vector2(0 + position.X, 72 + position.Y-24);
            Tile tile = new Tile(point, color, HALVED_BOTTOM_LEFT_OFFSET_X, HALVED_BOTTOM_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.HalvedBottomLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile HalvedBottomRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[4];            
            point[0] = new Vector2(82 + position.X, 24 + position.Y-24);
            point[1] = new Vector2(82 + position.X, 72 + position.Y-24);
            point[2] = new Vector2(41 + position.X, 96 + position.Y-24);
            point[3] = new Vector2(0 + position.X, 72 + position.Y-24);
            Tile tile = new Tile(point, color, HALVED_BOTTOM_RIGHT_OFFSET_X, HALVED_BOTTOM_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.HalvedBottomRight;
            tile.hp = hp;
            return tile;
        }

        private static Tile TriangleLeft(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[3];
            point[0] = new Vector2(41 + position.X-41, 48 + position.Y-48);            
            point[1] = new Vector2(82 + position.X-41, 72 + position.Y-48);
            point[2] = new Vector2(41 + position.X-41, 96 + position.Y-48);
            Tile tile = new Tile(point, color, TRIANGLE_LEFT_OFFSET_X, TRIANGLE_LEFT_OFFSET_Y);
            tile.shape = StaticEntityShape.TriangleLeft;
            tile.hp = hp;
            return tile;
        }

        private static Tile TriangleRight(Vector2 position, Color color, int hp)
        {
            Vector2[] point = new Vector2[3];            
            point[0] = new Vector2(41 + position.X, 48 + position.Y-48 );
            point[1] = new Vector2(41 + position.X, 96 + position.Y-48 );
            point[2] = new Vector2(0 + position.X, 72 + position.Y-48 );
            Tile tile = new Tile(point, color, TRIANGLE_RIGHT_OFFSET_X, TRIANGLE_RIGHT_OFFSET_Y);
            tile.shape = StaticEntityShape.TriangleRight;
            tile.hp = hp;
            return tile;
        }

       
    }
}