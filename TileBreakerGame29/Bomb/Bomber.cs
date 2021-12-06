using System;
using System.Collections.Generic;
using BreakOut;
using BreakOut.Entities.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileBreakerGame29.Bomb;
using TileBreakerGame29.Entities.Static.Effects;
using TileBreakerGame29.Light;

namespace TileBreakerGame29
{
    class Bomber : ILightRequester
    {
        private List<Vector2> bombList;
        private List<Tuple<AbstractStaticEntity, Vector2>> hitList;
        private List<Bubble> bubbleList;
        private LightData lightData;

        ///<summary>
        /// Handles the detonation bubbles and the hits.  
        ///</summary>
        public Bomber()
        {
            bombList = new List<Vector2>();
            hitList = new List<Tuple<AbstractStaticEntity, Vector2>>();
            bubbleList = new List<Bubble>();
            lightData = new LightData();

        }
        public void Add(Vector2 position)
        {
            bombList.Add(position);
        }        

        public List<Bubble> GetBubbleList()
        {
            return bubbleList;
        }

        public void Update(List<AbstractStaticEntity> entityList)
        {
            foreach (Vector2 bomb in bombList)
            {
                bubbleList.Add(new Bubble(bomb , Globals.bombRadie));
                // find who is hit by bomb
                foreach (AbstractStaticEntity entity in entityList)
                {
                    float dist = Vector2.Distance(entity.collisionRect.Center.ToVector2(), bomb);
                    
                    if (dist<Globals.bombRadie)
                    {
                        Vector2 dir = entity.collisionRect.Center.ToVector2() - bomb;
                        dir.Normalize();
                        dir *= (Globals.bombRadie-dist)*0.5f;
                        hitList.Add(new Tuple<AbstractStaticEntity, Vector2>(entity, dir));                        
                    }
                } 
            }

            bombList.Clear();

            // kill the ones hit
            foreach (Tuple<AbstractStaticEntity, Vector2> tup in hitList)
            {
                tup.Item1.hp = 0;
                foreach (EffectFactory.EffectFunctionDelegate h in tup.Item1.onHitList)
                {
                    h.Invoke(tup.Item1, tup.Item1.collisionRect.Center.ToVector2(), tup.Item2);
                }
            }

            hitList.Clear();
            
            // update and remove hits
            for (int i = bubbleList.Count - 1; i >= 0; i--)
            {
                bubbleList[i].Update();
                if (!bubbleList[i].active)
                {
                    bubbleList.RemoveAt(i);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach (Bubble b in bubbleList)
            {
                b.Draw(spriteBatch, texture);                
            }            
        }

        public void GetLightData(SortedList<int, LightData> list)
        {
            for (int i = 0; i < bubbleList.Count; i++)
            {                
                lightData.position = bubbleList[i].position;
                lightData.color = bubbleList[i].GetColor()*2;
                list.Add(Globals.bomberLightPriority, lightData);

            }

            
        }
    }
}