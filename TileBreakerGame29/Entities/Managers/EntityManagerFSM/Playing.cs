using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29;
using TileBreakerGame29.Entities.Managers.EntityManagerFSM;
using static BreakOut.TextureContainer;

namespace BreakOut.EntityMana
{
    class Playing:IState
    {
        private IState parentState;        
        private static Playing instance = new Playing();
        public static Playing GetInstance
        {            
            get { return instance; }
        }

        private Playing()
        {

        }

        public void Enter(EntityManager entityManager, IState parentState)
        {
            this.parentState = parentState;            
        }

        public void Leave(EntityManager entityManager)
        {
            entityManager.ResetBlaster();
            entityManager.ResetExpander();
            entityManager.DeactivatePowerUps();            
        }


        public void Update(EntityManager entityManager)
        {
            entityManager.UpdateGameview(false);                      

            if (entityManager.IsLevelWon())
            {                
                entityManager.ChangeState(ChangeLevel.GetInstance);                
            }
                        
            if (entityManager.IsLifeLost())
            {
                entityManager.SubLife();

                if (entityManager.IsOutOfLives())
                {
                    if (entityManager.isInerstialAvailable & AdController.adRewardLoaded)
                    {
                        entityManager.isInerstialAvailable = false;
                        entityManager.ChangeState(WatchRewardAd.GetInstance);
                    }
                    else
                    {
                        entityManager.ChangeState(GameOver.GetInstance);
                    }

                }
                else
                {
                    
                    entityManager.ChangeState(Serving.GetInstance);
                }
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