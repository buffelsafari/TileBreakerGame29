using BreakOut;
using BreakOut.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using static BreakOut.TextureContainer;

namespace TileBreakerGame29.GameStates
{
    class MenuGameState : IGameState
    {
        private static MenuGameState instance = new MenuGameState();
        public static MenuGameState GetInstance
        {
            get { return instance; }
        }

        private MenuGameState()
        {
            
        }
        public void Draw(Game1 game)
        {
           
            game.GetMenu().Draw(game.GetSpriteBatch());            
            game.GraphicsDevice.SetRenderTarget(null);

            game.GetSpriteBatch().Begin();
            Rectangle destinationRectangle = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            game.GetSpriteBatch().Draw((Texture2D)TextureContainer.GetTexture(TextureReference.renderTarget), destinationRectangle, new Rectangle(0, 0, 1080, 1920), Color.White);
            game.GetSpriteBatch().Draw(game.GetMenu().renderTarget, destinationRectangle, new Rectangle(0, 0, 1080, 1920), Color.White);

            game.GetSpriteBatch().End();          
            
        }

        public void Update(Game1 game)
        {            
            if (game.GetMenu().isNewGame)
            {
                game.GetMenu().isNewGame = false;
                game.NewGame();
                PlayingGameState.GetInstance.Update(game);
                game.ChangeState(PlayingGameState.GetInstance);
            }
            if (game.GetMenu().isResumed)
            {
                game.GetMenu().isResumed = false;                
                PlayingGameState.GetInstance.Update(game);
                game.ChangeState(PlayingGameState.GetInstance);
            }
            if (game.GetMenu().isEditor)
            {                
                game.GetMenu().isEditor = false;
                game.ChangeState(EditorGameState.GetInstance);
            }            
            
            game.GetMenu().Update(game);            
        }

        public void Enter(Game1 game)
        {
            GestureSample gesture = default(GestureSample);            
            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
            }            
        }

        public void Leave(Game1 game)
        {
            
        }

        public void OnBack(Game1 game)
        {
            
        }
    }
}