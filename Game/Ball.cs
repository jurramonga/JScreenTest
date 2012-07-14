using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using JScreenTest.Shapes;

namespace JScreenTest.Game
{
    class Ball : MoveableSprite
    {
        public Ball(Texture2D texture, Vector2 position, float rotation, Vector2 scale, Vector2 vel, Vector2 accel, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.velocity = vel;
            this.acceleration = accel;
            this.color = color;            
        }

        public Circle toCircle()
        {
            return new Circle(position, texture.Width * scale.X / 2);
        }
    }
}
