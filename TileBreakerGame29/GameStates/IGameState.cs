
namespace BreakOut.GameStates
{
    interface IGameState
    {
        void Enter(Game1 game);
        void Leave(Game1 game);
        void OnBack(Game1 game);                
        void Update(Game1 game);
        void Draw(Game1 game);

    }
}