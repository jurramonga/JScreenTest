using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JScreenTest.Rendering
{
    //Credit: http://www.xnawiki.com/index.php/Drawing_2D_lines_without_using_primitives
    static class Primitives
    {
        public static void drawLine(SpriteBatch batch, Texture2D blank, float width, Color color, 
            Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
