using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace JScreenTest.Game
{
    class MoveableSprite : Sprite
    {
        public Vector2 velocity;
        public Vector2 acceleration;

        public MoveableSprite()
        {
            this.velocity = new Vector2();
            this.acceleration = new Vector2();
        }

        public MoveableSprite(Vector2 vel, Vector2 accel)
        {
            this.velocity = vel;
            this.acceleration = accel;
        }

        public void update()
        {
            velocity += acceleration;
            position += velocity;
        }

    }
}
