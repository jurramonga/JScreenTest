using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace JScreenTest.Shapes
{
    class Circle
    {
        public Vector2 position;
        public float radius;
        

        public Circle()
        {
            position = Vector2.Zero;
            radius = 0;
            
        }

        public Circle(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public Vector2 getEdgeVector(int angle)
        {
            float x = (float)(radius * Math.Cos(angle * Math.PI / 180));
            float y = (float)(-1 * radius * Math.Sin(angle * Math.PI / 180));

            return new Vector2(x, y);
        }
    }
}
