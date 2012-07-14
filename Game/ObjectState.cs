using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace JScreenTest.Game
{
    class ObjectState
    {
        public Rectangle Rectangle { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }

        public ObjectState()
        {
            this.Rectangle = new Rectangle();
            this.Color = new Color();
            this.Rotation = new float();
        }
        public ObjectState(Rectangle rectangle)
        {
            this.Rectangle = rectangle;
            this.Color = Color.White;
            this.Rotation = 0;
        }
        public ObjectState(Rectangle rectangle, Color color)
        {
            this.Rectangle = rectangle;
            this.Color = color;
            this.Rotation = 0;
        }
        public ObjectState(Rectangle rectangle, Color color, float rotation)
        {
            this.Rectangle = rectangle;
            this.Color = color;
            this.Rotation = rotation;
        }

        public static ObjectState Lerp(ObjectState state1, ObjectState state2, float amount)
        {
            return new ObjectState(
                new Rectangle(
                    (int)(state1.Rectangle.X + amount * (state2.Rectangle.X - state1.Rectangle.X)),
                    (int)(state1.Rectangle.Y + amount * (state2.Rectangle.Y - state1.Rectangle.Y)),
                    (int)(state1.Rectangle.Width + amount * (state2.Rectangle.Width - state1.Rectangle.Width)),
                    (int)(state1.Rectangle.Height + amount * (state2.Rectangle.Height - state1.Rectangle.Height))),
                new Color(),
                0.0f);
        }
    }
}
