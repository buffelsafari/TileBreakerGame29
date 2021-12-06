using BreakOut;
using BreakOut.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using static BreakOut.TextureContainer;

namespace TileBreakerGame29.GameStates
{
    class EditorGameState : IGameState
    {
        private static EditorGameState instance = new EditorGameState();
        public static EditorGameState GetInstance
        {
            get { return instance; }
        }

        private EditorGameState()
        {

        }

        public void Draw(Game1 game)
        {
            game.GraphicsDevice.SetRenderTarget((RenderTarget2D)TextureContainer.GetTexture(TextureReference.renderTarget));            
            game.GraphicsDevice.Clear(Color.DarkKhaki);
            game.GetLevelEditor().Draw(game.GetSpriteBatch(), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetDiffuse), (Texture2D)TextureContainer.GetTexture(TextureReference.spriteSheetNormal), (Texture2D)TextureContainer.GetTexture(TextureReference.editorSprites), ShaderContainer.GetShader(ShaderReference.editorShader));
            game.GraphicsDevice.SetRenderTarget(null);

            game.GetSpriteBatch().Begin();
            Rectangle destinationRectangle = new Rectangle(0, Globals.adHeight, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            game.GetSpriteBatch().Draw((Texture2D)TextureContainer.GetTexture(TextureReference.renderTarget), destinationRectangle, new Rectangle(0, 0, 1080, 1920), Color.White);
            
            game.GetSpriteBatch().End();
        }

        public void Update(Game1 game)
        {            
            game.GetLevelEditor().Update(game.GetEntityManager());
            
            if (game.GetLevelEditor().leaveEditor)
            {
                game.GetLevelEditor().leaveEditor = false;
                game.ChangeState(MenuGameState.GetInstance);
            }
        }

        public void Enter(Game1 game)
        {
            GestureSample gesture = default(GestureSample);
            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
            }

            game.GetLevelEditor().LoadLevel(game, game.GetMenu().editorFile);
            game.LoadAd();
            game.ShowAd();            
        }

        public void Leave(Game1 game)
        {            
            game.GetLevelEditor().SaveLevel(game);
            game.HideAd();
            game.DestroyAd();            
        }

        public void OnBack(Game1 game)
        {
            game.ChangeState(MenuGameState.GetInstance);            
        }
    }
}