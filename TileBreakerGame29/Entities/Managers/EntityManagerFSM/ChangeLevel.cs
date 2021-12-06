using BreakOut;
using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static BreakOut.TextureContainer;

namespace TileBreakerGame29.Entities.Managers.EntityManagerFSM
{
    class ChangeLevel : IState
    {
        private IState parentState;
        int timer = 0;
        private static ChangeLevel instance = new ChangeLevel();
        public static ChangeLevel GetInstance
        {
            get { return instance; }
        }

        private ChangeLevel()
        {

        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
        { 
            entityManager.DrawGameView(Color.White);
            spriteBatch.Begin();
            entityManager.DrawScore(spriteBatch, (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse));
            spriteBatch.End();
            
        }

        public void Enter(EntityManager entityManager, IState parentState)
        {
            this.parentState = parentState;            
            timer = 0;            
        }

        public void Leave(EntityManager entityManager)
        {
            entityManager.SetNextLevel();            
        }

        public void Update(EntityManager entityManager)
        {            
            if (entityManager.GetBallsOfDoom().GetBallArray()[(timer / 10) % 8].Active == true)
            {
                entityManager.GetBallsOfDoom().GetBallArray()[(timer / 10) % 8].Active = false;
                entityManager.Boom(entityManager.GetBallsOfDoom().GetBallArray()[(timer / 10) % 8].position);
            }
            entityManager.UpdateGameview(false);

            timer++;
            if (timer > 200)  
            {
                entityManager.ChangeState(Serving.GetInstance);
            }            
        }      
    }
}