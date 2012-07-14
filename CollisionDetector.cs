using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using JScreenTest.Game;
using JScreenTest.ScreenManagement;
using JScreenTest.Shapes;


namespace JScreenTest
{
    static class CollisionDetector
    {
        public static bool checkCollision(Rectangle r1, Point point)
        {
            if (point.X >= r1.X &&
                point.X <= r1.X + r1.Width &&
                point.Y >= r1.Y &&
                point.Y < r1.Y + r1.Height)
            {
                return true;
            }

            return false;
        }

        public static bool checkCollision(Texture2D t1, Rectangle r1, Point point, bool perPixel)
        {
            if (point.X >= r1.X &&
                point.X <= r1.X + r1.Width &&
                point.Y >= r1.Y &&
                point.Y < r1.Y + r1.Height)
            {
                if (perPixel)
                {
                    return pixelPerfect(t1, r1, point);
                }
                return true;
            }

            return false;
        }

        public static bool checkCollision(Texture2D t1, Rectangle r1, Texture2D t2, Rectangle r2, bool perPixel)
        {
            if (r1.Intersects(r2))
            {
                if (perPixel)
                {
                    return pixelPerfect(t1, r1, t2, r2);
                }
                return true;
            }
            return false;
        }

        /*
        public static bool checkCollision(Sprite s1, Sprite s2, bool perPixel)
        {
            if (s1.getBounds().Intersects(s2.getBounds()))
            {
                if (perPixel)
                {
                    return pixelPerfect(s1, s2);
                }

                return true;
            }

            return false;
        }*/

        public static bool checkCollision(Circle c1, Circle c2)
        {
            float distance  = (c1.position - c2.position).LengthSquared();
            float radiusSumSquare = (float)Math.Pow(c1.radius + c2.radius, 2);

            if (distance < radiusSumSquare)
            {
                return true;
            }

            return false;
        }

        private static bool pixelPerfect(Texture2D t1, Rectangle r1, Point p1)
        {
            Color[] bits = new Color[t1.Width * t1.Height];
            t1.GetData<Color>(bits);

            //Offset the rectangle & point to (0,0)
            int x1 = p1.X - r1.X;
            int y1 = p1.Y - r1.Y;

            if (bits.ElementAt<Color>(x1 + y1 * r1.Width).A > 0)
            {
                return true;
            }

            return false;
        }

        private static bool pixelPerfect(Texture2D t1, Rectangle r1, Texture2D t2, Rectangle r2)
        {
            //Found here: http://gamedev.stackexchange.com/questions/15191/is-there-a-good-way-to-get-pixel-perfect-collision-detection-in-xna
            Color[] bits1 = new Color[t1.Width * t1.Height];
            Color[] bits2 = new Color[t2.Width * t2.Height];

            t1.GetData(bits1);
            t2.GetData(bits2);

            //Intersection Bounds
            int x1 = Math.Max(r1.X, r2.X);
            int x2 = Math.Min(r1.Right, r2.Right);
            int y1 = Math.Max(r1.Y, r2.Y);
            int y2 = Math.Min(r1.Bottom, r2.Bottom);

            for (int y = y1; y < y2; y++)
            {
                for (int x = x1; x < x2; x++)
                {
                    Color colorA = bits1[x - r1.X + (y - r1.Y) * t1.Width];
                    Color colorB = bits2[x - r2.X + (y - r2.Y) * t2.Width];

                    if (colorA.A == 255 && colorB.A == 255)
                    {
                        Debug.WriteLine("Collision Detected.");
                        Debug.WriteLine("Texture 1 Information:");
                        Debug.WriteLine("Bounds: " + t1.Bounds.ToString());
                        Debug.WriteLine("Rect:   " + r1.ToString());
                        Debug.WriteLine("Texture 2 Information:");
                        Debug.WriteLine("Bounds: " + t2.Bounds.ToString());
                        Debug.WriteLine("Rect:   " + r2.ToString());

                        Debug.WriteLine(colorA.ToString() + "    " + colorB.ToString());

                        return true;

                    }
                }
            }
            return false;
        }

        /*
        private static bool pixelPerfect(Sprite sA, Sprite sB)
        {
            //Code found at App Hub: http://create.msdn.com/en-US/education/catalog/tutorial/collision_2d_perpixel_transformed

            
            Color[] dataA = new Color[sA.texture.Width * sA.texture.Height];
            sA.texture.GetData(dataA);
            Color[] dataB = new Color[sB.texture.Width * sB.texture.Height];
            sB.texture.GetData(dataB);

            Matrix transformAToB = sA.getTransformMatrix() * Matrix.Invert(sB.getTransformMatrix());

            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);
            


            for (int yA = 0; yA < sA.texture.Height; yA++)
            {
                Vector2 posInB = yPosInB;

                for (int xA = 0; xA < sA.texture.Width; xA++)
                {
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);


                    //Debug.WriteLine(xB + " " + yB);
                    if (0 <= xB && xB < sB.texture.Width &&
                        0 <= yB && yB < sB.texture.Height)
                    {

                        
                        Color colorA = dataA[xA + yA * sA.texture.Width];
                        Color colorB = dataB[xB + yB * sB.texture.Width];

                        //Debug.WriteLine(colorA.A + " " + colorB.A);
                        if (colorA.A >= 128 && colorB.A >= 128)
                        {
                            return true;
                        }
                    }

                    posInB += stepX;
                }

                yPosInB += stepY;
            }

            return false;
        }
        */
    }
}
