using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JScreenTest.Particles
{
    class SpringParticle : Particle
    {
        public Vector2 basePosition;

        const float TIGHTNESS = 0.05f;
        const float DAMPENER = 0.9f;

        int size = 15;

        public SpringParticle() : base()
        {
            basePosition = new Vector2();
        }

        public SpringParticle(Vector2 position, Texture2D texture, Color color)
            : base(position, texture, color)
        {
            basePosition = position;
        }

        public SpringParticle(Vector2 position, Vector2 velocity, Vector2 acceleration, Texture2D texture, Color color) : base(position, velocity, acceleration, texture, color)
        {
            basePosition = position;

        }

        public override void update()
        {
            acceleration *= DAMPENER;
            velocity += acceleration + -1f * TIGHTNESS * (position - basePosition);
            velocity *= DAMPENER;
            position += velocity;
        }

        public override void draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X - size/2, (int)position.Y - size/2, size, size), color);
        }

        public override void draw(SpriteBatch sb, Color color)
        {
            sb.Draw(texture, new Rectangle((int)position.X - size / 2, (int)position.Y - size / 2, size, size), color);
            sb.Draw(texture, new Rectangle((int)position.X - size / 4, (int)position.Y - size / 4, size/2, size/2), Color.White);

        }
    }
}
