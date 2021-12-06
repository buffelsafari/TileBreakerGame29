using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using TileBreakerGame29.GameStates;

namespace BreakOut.GameStates
{
    class PlayingGameState:IGameState
    {
        private static PlayingGameState instance = new PlayingGameState();
        private Rectangle destinationRectangle = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        private Rectangle fullScreenRect = new Rectangle(0, 0, 1080, 1920);
        public static PlayingGameState GetInstance
        {
            get { return instance; }
        }

        private PlayingGameState()
        {

        }

        public void Update(Game1 game)
        {            
            game.GetController().Update(game.GetEntityManager());
            game.GetEntityManager().Update();            

            if (game.GetEntityManager().isGameOver)
            {
                game.ChangeState(MenuGameState.GetInstance);
            }
        }

        public void Draw(Game1 game)
        {
            game.GetEntityManager().Draw(game.GetSpriteBatch(), (RenderTarget2D)TextureContainer.GetTexture(TextureContainer.TextureReference.renderTarget));            
            game.GraphicsDevice.SetRenderTarget(null);            

            game.GetSpriteBatch().Begin();            
            game.GetSpriteBatch().Draw((Texture2D)TextureContainer.GetTexture(TextureContainer.TextureReference.renderTarget), destinationRectangle, fullScreenRect, Color.White);

            game.GetSpriteBatch().End();
        }

        public void Enter(Game1 game)
        {
            GestureSample gesture = default(GestureSample);
            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
            }
            SoundManager.ResumeSongLoop(SoundManager.song);            
        }

        public void Leave(Game1 game)
        {
            SoundManager.PauseSongLoop(SoundManager.song);

            if (game.settings.player == 0)
            {
                if (game.GetEntityManager().keepingScore)
                {
                    if (game.GetEntityManager().GetScore() > game.settings.highScore)
                    {
                        game.settings.highScore = game.GetEntityManager().GetScore();
                    }
                    if (game.GetEntityManager().GetProgress() > game.settings.progress)
                    {
                        game.settings.progress = game.GetEntityManager().GetProgress();
                    }
                    game.GetMenu().UpdateScore();
                }
            } 
        }

        public void OnBack(Game1 game)
        {
            game.ChangeState(MenuGameState.GetInstance);            
        }
    }
}