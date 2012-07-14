using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using JScreenTest.Game;

namespace JScreenTest
{
    static class CollisionResolver
    {
        /*
        public static void resolve(Ball b1, Ball b2)
        {
            Vector2 impact = b2.velocity - b1.velocity;
            Vector2 impulse = Vector2.Normalize(b1.position - b2.position);

            float mass1 = (float)(Math.PI * Math.Pow(b1.texture.Width * b1.scale.X / 2, 2.0)) / 100;
            float mass2 = (float)(Math.PI * Math.Pow(b2.texture.Width * b2.scale.X / 2, 2.0)) / 100;
            //float mass1 = 10;
            //float mass2 = 10;

            float impactSpeed = Math.Abs(Vector2.Dot(impact, impulse));



            impulse *= (float)Math.Sqrt(impactSpeed * mass1 * mass2);



            b1.velocity += (impulse / mass1);
            b2.velocity -= (impulse / mass2);

            Debug.WriteLine("------------");
            Debug.WriteLine("Impact Speed: " + impactSpeed);
            Debug.WriteLine("Impulse: " + impulse);
        }*/
    }
}
