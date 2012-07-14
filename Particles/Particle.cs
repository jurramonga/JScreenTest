using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JScreenTest.Particles
{
    abstract class Particle
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;

        public Texture2D texture;

        public Color color;

        public Particle()
        {
            position = new Vector2();
            velocity = new Vector2();
            acceleration = new Vector2();

            color = new Color();
        }

        public Particle(Vector2 position, Texture2D texture, Color color)
        {
            this.position = position;
            this.velocity = new Vector2();
            this.acceleration = new Vector2();
            this.texture = texture;
            this.color = color;
        }

        public Particle(Vector2 position, Vector2 velocity, Vector2 acceleration, Texture2D texture, Color color)
        {
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.texture = texture;
            this.color = color;
        }

        public virtual void update()
        {
            velocity += acceleration;
            position += velocity;
        }

        public virtual void draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, color);
        }

        public virtual void draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, position, color);
        }
        
    }
}
