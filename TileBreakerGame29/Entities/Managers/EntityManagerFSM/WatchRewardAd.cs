using BreakOut;
using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.AdStuff;
using static BreakOut.TextureContainer;

namespace TileBreakerGame29.Entities.Managers.EntityManagerFSM
{
    class WatchRewardAd : IState
    {
        private IState parentState;
        private RewardDialog rewardDialog;
        private Color color;

        private static WatchRewardAd instance = new WatchRewardAd();
        public static WatchRewardAd GetInstance
        {
            get { return instance; }
        }

        private WatchRewardAd()
        {
            rewardDialog = new RewardDialog();
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            entityManager.DrawGameView(color);
            spriteBatch.Begin();
            entityManager.DrawScore(spriteBatch, (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            rewardDialog.Draw(spriteBatch, (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            spriteBatch.End();
        }

        public void Enter(EntityManager entityManager, IState parentState)
        {
            this.parentState = parentState;
            rewardDialog.Init();
            color = new Color(1, 1, 1, 1.0f);
            rewardDialog.selection = 0;            
            rewardDialog.FlushTouch();            
        }

        public void Leave(EntityManager entityManager)
        {
            
        }

        public void Update(EntityManager entityManager)
        {
            if (color.R > 128)
            {
                color.R -= 1;
                color.G -= 1;
                color.B -= 1;
            }
            entityManager.UpdateGameview(false);

            rewardDialog.Update();
            
            if (rewardDialog.selection == 1)
            {
                AdController.ShowRewardAd();
                rewardDialog.selection = 0;                
            }

            if (rewardDialog.selection == 2)
            {                
                entityManager.ChangeState(GameOver.GetInstance);
            }            

            if (AdController.adRewardClosed)
            {
                if (AdController.adRewarded)
                {
                    entityManager.AddLife();
                    AdController.adRewarded = false;
                    AdController.adRewardClosed = false;
                    entityManager.ChangeState(Serving.GetInstance);
                }
                else
                {
                    AdController.adRewardClosed = false;
                    entityManager.ChangeState(GameOver.GetInstance);                    
                }
            }            
        }       
    }
}