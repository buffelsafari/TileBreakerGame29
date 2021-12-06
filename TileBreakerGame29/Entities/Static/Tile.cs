using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut.Entities.Static
{
    
    class Tile : AbstractStaticEntity        
    {       
        public float area;
        VertexPosition[] triangles;

        public Tile(Vector2[] seg, Color color, int textureOffsetX, int textureOffsetY)
        {
            soundId = Sound.SoundId.tilehit;
            this.color = color;

            for (int counter=0;counter<seg.Length-1 ;counter++)
            {
                segments.Add(new LineSegment(seg[counter], seg[counter+1]));
            }
            segments.Add(new LineSegment(seg[seg.Length-1], seg[0]));

            MakeCollisionRectangle();
            MakeCollisionBitRegions();
            textureRect = new Rectangle(0, 0, collisionRect.Width+1, collisionRect.Height+1);
            textureRect.Offset(textureOffsetX, textureOffsetY);

            CalculateArea();
            fingerDelta = 0;

            if (area < 3000)
            {
                fingerDelta = -10;
            }
            if (area < 2000)
            {
                fingerDelta = -20;
            }            
            if (area < 1000)
            {
                fingerDelta = -30;
            }

            MakeTriangles();
            position = new Vector2(collisionRect.Left, collisionRect.Top);
        }

        private void CalculateArea()  
        {
            float a = 0;
            foreach (LineSegment s in segments)
            {
                a+= s.point1.X * s.point2.Y - s.point1.Y * s.point2.X;
            }

            this.area = Math.Abs(a / 2.0f);            
        }

        private void MakeTriangles()
        {
            int numberOfTriangles = segments.Count - 1;

            triangles = new VertexPosition[numberOfTriangles*3];

            int i = 1;

            for (int counter = 0; counter < numberOfTriangles*3; counter += 3)
            {
                triangles[counter] = new VertexPosition(new Vector3(segments[0].point1, 0));

                triangles[counter+1] = new VertexPosition(new Vector3(segments[i].point1, 0));
                triangles[counter+2] = new VertexPosition(new Vector3(segments[i].point2, 0));
                i++;
            }            
        }

        public VertexPosition[] GetTriangles()
        {
            return triangles;
        }

        public int GetNumberOfTriangles()
        {
            return segments.Count - 1;
        }


        public void EditorDraw(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture)
        {
            spriteBatch.Draw(texture, new Rectangle(collisionRect.X+(int)offset.X, collisionRect.Y+(int)offset.Y, collisionRect.Width, collisionRect.Height), textureRect, color);
        }

        public override void DrawDirectShadow(SpriteBatch spriteBatch, Vector2 offset, Texture2D texture)
        {            
            spriteBatch.Draw(texture, new Rectangle(collisionRect.X + (int)offset.X, collisionRect.Y + (int)offset.Y, collisionRect.Width, collisionRect.Height), textureRect, directShadowColor);            
        }
    }
}