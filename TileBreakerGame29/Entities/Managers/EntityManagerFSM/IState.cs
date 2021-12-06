using Microsoft.Xna.Framework.Graphics;


namespace BreakOut.EntityMana
{
    interface IState
    {        
        void Enter(EntityManager entityManager, IState parentState);
        void Leave(EntityManager entityManager);
        void Update(EntityManager entityManager);
        void Draw(EntityManager entityManager, SpriteBatch spriteBatch, RenderTarget2D renderTarget);        
    }
}