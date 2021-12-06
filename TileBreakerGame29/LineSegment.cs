using System;
using Microsoft.Xna.Framework;

namespace BreakOut
{
    struct LineSegment
    {
        public Vector2 point1;
        public Vector2 point2;
        public Vector2 normal;        
        public Vector2 biNormal;
        public float length;
        public Rectangle box;

        public LineSegment(Vector2 point1, Vector2 point2)
        {
            Vector2 normal = point2 - point1;
            Vector2 biNormal = normal;
            float length = normal.Length();
            biNormal.Normalize();
            normal = new Vector2(normal.Y, -normal.X);
            normal.Normalize();

            this.point1 = point1;
            this.point2 = point2;
            this.normal = normal;
            this.biNormal = biNormal;
            this.length = length;

            int left = (int)Math.Min(point1.X, point2.X);
            int top = (int)Math.Min(point1.Y, point2.Y);
            int right = (int)Math.Max(point1.X, point2.X);
            int bottom = (int)Math.Max(point1.Y, point2.Y);

            this.box= new Rectangle(left, top, right - left, bottom - top);
        }
    }
}

