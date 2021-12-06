using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Entities.Dynamic;

namespace TileBreakerGame29.HUD
{
    class LifeSprites
    {        
        private Stack<Life> lifeStack;
        public Stack<Vector2> eyeStack { get; set; }
        
        public LifeSprites()
        {
            lifeStack = new Stack<Life>();
            eyeStack = new Stack<Vector2>();            
        }
        public void Clear()
        {
            lifeStack.Clear();
            eyeStack.Clear();
        }
        public void Add()
        {
            Life life = new Life(new Vector2(1080 - 90 - lifeStack.Count * 100, 1920 - 200), Color.LightGreen);
            lifeStack.Push(life);
            eyeStack.Push(life.GetLeftEyePosition());
            eyeStack.Push(life.GetRightEyePosition());
        }
        public void Sub()
        {            
            lifeStack.Pop();  
            eyeStack.Pop();
            eyeStack.Pop();            
        }
        public int GetNumberOfLives()
        {
            return lifeStack.Count;
        }
        public void Update(Vector2 shake, Balls balls)
        {
            foreach (Life l in lifeStack)
            {
                l.Update(shake, balls);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach (Life life in lifeStack)
            {
                life.Draw(spriteBatch, texture);                
            }
        }
    }
}