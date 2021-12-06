using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace BreakOut.Controller
{
    class PlayerController:IController
    {        
        public PlayerController()
        {
            
        }      

        public void Update(EntityManager entityManager)
        {            
            TouchCollection tc = TouchPanel.GetState();
            entityManager.GetBat().targetPosition.Y = 1920 - 400;
            Vector2 pos;
            if (tc.Count > 0)
            {
                pos = tc[0].Position;

                pos.X *= (1080.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                pos.Y *= (1920.0f / GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                if (pos.Y  > (1920) / 2)
                {
                    entityManager.GetBat().targetPosition.X = pos.X;
                }
                else if (entityManager.GetBat().isReadyToServe)
                {
                    entityManager.GetBallsOfDoom().SetServeDirection(pos-entityManager.GetBat().position);
                    entityManager.GetBat().IsServed = true;                    
                }
            }
        }
    }
}