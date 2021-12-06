using BreakOut.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TileBreakerGame29.Entities.Dynamic;
using TileBreakerGame29.HUD;

namespace BreakOut
{
    class ScoreHUD
    {
        private int nextIndex = 0;

        const int BALL_OFFSET_X = 524;
        const int BALL_OFFSET_Y = 275;

        const int MAX_SPRITES = 24;

        private Vector2 position;
        private int score = 0;        
        private int numberOfStars = 0;
        private int numberOfExpands = 0;
        private int numberOfFlash = 0;

        private string text;
        private Vector2 textPosition;
        private int textDelay = 0;        
        private ScoreSprite[] scoreArray;        
        private Rectangle starSourceRect;
        private Rectangle expanderSourceRect;
        private Rectangle flashSourceRect;
        private LifeSprites lifeSprites;
        private Rectangle[] starRect;
        private Rectangle[] expanderRect;
        private Rectangle[] flashRect;

        public ScoreHUD()
        {


            scoreArray = new ScoreSprite[MAX_SPRITES];
            for (int i = 0; i < scoreArray.Length; i++)
            {
                scoreArray[i] = new ScoreSprite();
            } 

            position = new Vector2(100, 1920 - 150);
            
            starSourceRect = new Rectangle(80*1, 80*6, 80, 80);
            expanderSourceRect = new Rectangle(80*2,80*6,80,80);
            flashSourceRect = new Rectangle(80 * 3, 80 * 6, 80, 80);

            lifeSprites = new LifeSprites();

            starRect = new Rectangle[Globals.maxStar];
            for (int i = 0; i < Globals.maxStar; i++)
            {                
                starRect[i]=new Rectangle(50 + i * 30, 1920 - 40, 40, 40);
            }

            expanderRect = new Rectangle[Globals.maxExpands];
            for (int i = 0; i < Globals.maxExpands; i++)
            {
                expanderRect[i]=new Rectangle(540 + i * 40, 1920 - 40, 40, 40);
            }

            flashRect = new Rectangle[Globals.maxFlash];
            for (int i = 0; i < Globals.maxFlash; i++)
            {
                flashRect[i] = new Rectangle(880 + i * 40, 1920 - 40, 40, 40);
            }
        }

        public void Dispose()
        { 
        
        }

        public int GetScore()
        {
            return score;
        }
        public void NewGame()
        {
            numberOfStars = 0;
            numberOfExpands = 0;
            numberOfFlash = 0;
            score = 0;
            lifeSprites.Clear();
            lifeSprites.Add();
            lifeSprites.Add();
            lifeSprites.Add();
        }
        public int GetNumberOfLives()
        {
            return lifeSprites.GetNumberOfLives();
        }

        public int GetNumberOfExpands()
        {
            return numberOfExpands;
        }
        public int GetNumberOfFlash()
        {
            return numberOfFlash;
        }
        public int GetNumberOfStar()
        {
            return numberOfStars;
        }
        public void ResetScoreMultiplicator()
        {
            numberOfStars = 0;
        }
        public void ResetExpander()
        {
            numberOfExpands = 0;
        }
        public void ResetBlaster()
        {
            numberOfFlash = 0;
        }
        public void AddScoreMultiplicator()
        {
            Vector2 pos = new Vector2((200 - numberOfStars * 15) + (numberOfStars) * 30, 1920 - 40);
            if (numberOfStars < Globals.maxStar)
            {
                SoundManager.Play(SoundId.bonus);
                numberOfStars++;
            }
            else
            {
                SoundManager.Play(SoundId.denial);                
            }
        }

        public void AddExpander()
        {
            Vector2 pos = new Vector2((540 - numberOfExpands * 20) + numberOfExpands * 40 , 1920 - 40 );
            if (numberOfExpands < Globals.maxExpands)
            {
                numberOfExpands++;                
            }            
        }

        public void AddBlaster()
        {
            Vector2 pos = new Vector2((880 - numberOfFlash * 20) + numberOfFlash * 40, 1920 - 40);
            if (numberOfFlash < Globals.maxFlash)
            {
                numberOfFlash++;                
            }            
        }

        public void AddScore(int value, Vector2 position)
        {
            this.score += value*(numberOfStars+1);            

            for (int i = 0; i < scoreArray.Length; i++)
            {
                if (scoreArray[(i + nextIndex) % MAX_SPRITES].lifeTime <= 0)
                {
                    int n = (i + nextIndex) % MAX_SPRITES;
                    scoreArray[n].lifeTime = 50;
                    scoreArray[n].position = position;
                    scoreArray[n].str = value.ToString();                    
                    scoreArray[n].color = Color.LightGreen;
                    nextIndex++;
                    return;
                }
            }            
        }
        public void AddLife()
        {
            if (lifeSprites.GetNumberOfLives() < Globals.maxLives)
            {
                SoundManager.Play(SoundId.extralife);
                lifeSprites.Add();
            }
            else
            {
                SoundManager.Play(SoundId.denial);
            }            
        }

        public void SubLife()
        {
            lifeSprites.Sub();            
        }

        public void ShowText(string str, int delay)
        {
            text = str;
            textDelay = delay;
            Vector2 textSize = Globals.bigFont.MeasureString(str);
            textPosition = new Vector2(540 - textSize.X / 2, 500);
        }

        public void Update(Vector2 shake, Balls balls)
        {
            for (int i = 0; i < scoreArray.Length; i++)
            {
                if (scoreArray[i].lifeTime >= 0)
                {                    
                    scoreArray[i].lifeTime--;
                    scoreArray[i].position.Y -= 2;                    
                }
            }

            lifeSprites.Update(shake, balls);
        }

        public Stack<Vector2> GetEyeStack()
        {
            return lifeSprites.eyeStack;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {            
            spriteBatch.DrawString(Globals.bigFont, score.ToString(), position, Color.Black);
            // stars
            for (int i = 0; i < numberOfStars; i++)
            {
                spriteBatch.Draw(texture, starRect[i], starSourceRect, Color.White); 
            }

            // expanders
            for (int i = 0; i < numberOfExpands; i++)
            {
                spriteBatch.Draw(texture, expanderRect[i], expanderSourceRect, Color.White);
            }

            // flash
            for (int i = 0; i < numberOfFlash; i++)
            {
                spriteBatch.Draw(texture, flashRect[i], flashSourceRect, Color.White);
            }

            for (int i = 0; i < scoreArray.Length; i++)
            {
                if (scoreArray[i].lifeTime >= 0)
                {
                    spriteBatch.DrawString(Globals.smallFont, scoreArray[i].str, scoreArray[i].position, scoreArray[i].color);
                }
            }            
            

            if (textDelay-- > 0)
            {
                spriteBatch.DrawString(Globals.bigFont, text, textPosition, Color.Black);
            }            
        }

        public void DrawLife(SpriteBatch spriteBatch, Texture2D texture)
        { 
                lifeSprites.Draw(spriteBatch, texture);
        }        
    }
}