using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static BreakOut.TextureContainer;

namespace BreakOut.EntityMana
{
    class Serving:IState
    {
        private IState parentState;        
        private bool isServing = true;
        private static Serving instance = new Serving();
        public static Serving GetInstance
        {
            get { return instance; }
        }

        private Serving()
        {

        }
        public void Enter(EntityManager entityManager, IState parentState)
        {
            entityManager.GetBat().isReadyToServe = true;
            this.parentState = parentState;            
            entityManager.ShowText(Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.Serve), 50);             
        }

        public void Leave(EntityManager entityManager)
        {
            entityManager.GetBat().isReadyToServe = false;
            entityManager.GetBat().IsServed = false;
        }

        public void Update(EntityManager entityManager)
        {
            entityManager.UpdateGameview(isServing);

            if (entityManager.GetBat().IsServed)
            {               
                entityManager.ChangeState(Playing.GetInstance);
            }
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            entityManager.DrawGameView(Color.White);

            spriteBatch.Begin();
            entityManager.DrawScore(spriteBatch, (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            spriteBatch.End();
        }       
    }
}