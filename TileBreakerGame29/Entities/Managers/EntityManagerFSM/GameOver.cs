using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static BreakOut.TextureContainer;

namespace BreakOut.EntityMana
{
    class GameOver:IState
    {
        private int timer;
        private float fade;
        private IState parentState;
        private static GameOver instance = new GameOver();
        public static GameOver GetInstance
        {
            get { return instance; }
        }

        private GameOver()
        {

        }
        public void Enter(EntityManager entityManager, IState parentState)
        {
            this.parentState = parentState;
            timer = 0;
            fade = 1;
            entityManager.gameActive = false;            
            entityManager.ShowText(Game.Activity.Resources.GetString(TileBreakerGame29.Resource.String.GameOver), 500);  
        }

        public void Leave(EntityManager entityManager)
        {
            
        }
        public void Update(EntityManager entityManager)
        {
            timer++;            
            entityManager.UpdateDynamicEntities();
            entityManager.UpdateScore();

            if (timer >= 200)
            {
                entityManager.isGameOver = true;
            }
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        {
            if (timer > 100)
            {
                fade -= 0.01f;
            }            
            
            entityManager.DrawGameView(Color.White*fade);
            spriteBatch.Begin();
            entityManager.DrawScore(spriteBatch, (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            spriteBatch.End();
        }       
    }
}