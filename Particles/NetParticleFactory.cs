using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using JScreenTest.Rendering;

namespace JScreenTest.Particles
{
    class NetParticleFactory
    {
        SpringParticle[,] particles;
        int gridWidth;
        int gridHeight;
        int edgeBuffer;

        
        public Color color;
        int shift;
        int delta;

        Random r;

        Texture2D whitePixel;

        public NetParticleFactory()
        {
        }

        public void initialize(Viewport viewport, int gridWidth, int gridHeight, int edgeBuffer, Texture2D texture, Color color)
        {
            this.gridHeight = gridHeight;
            this.gridWidth = gridWidth;
            this.edgeBuffer = edgeBuffer;

            whitePixel = texture;

            r = new Random();

            //Rainbow color fields
            this.color = new Color(255, 0, 0, 128);
            shift = 2;
            delta = 1;

            particles = new SpringParticle[gridWidth+1, gridHeight+1];

            for (int i = 0; i <= gridHeight; i++)
            {
                for (int j = 0; j <= gridWidth; j++)
                {
                    particles[j, i] = new SpringParticle(
                        new Vector2(j * (viewport.Width - 2 * edgeBuffer) / gridWidth + edgeBuffer, i * (viewport.Height - 2 * edgeBuffer) / gridHeight + edgeBuffer),
                        texture,
                        color);
                }
            }
        }

        public void update()
        {
            nextColor();
            nextColor();
            nextColor();
            nextColor();

            foreach (Particle particle in particles)
            {
                particle.update();
            }
        }

        public void draw(SpriteBatch sb)
        {
            for (int i = 0; i <= gridHeight; i++)
            {
                for (int j = 0; j <= gridWidth; j++)
                {
                    if (j != gridWidth)
                    {
                        //Primitives.drawLine(sb, whitePixel, 1, Color.RoyalBlue, particles[j, i].position, particles[j + 1, i].position);
                    }
                    if (i != gridHeight)
                    {
                        //Primitives.drawLine(sb, whitePixel, 1, Color.RoyalBlue, particles[j, i].position, particles[j, i + 1].position);
                    }
                }
            }
            sb.End();
            sb.Begin(SpriteSortMode.FrontToBack, BlendState.Additive);
            foreach (Particle particle in particles)
            {
                particle.draw(sb, color);
            }
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        }

        public void pullFromPoint(Vector2 point, int cutoff)
        {
            float cutoffSquared = (float)Math.Pow(cutoff, 2);
            Vector2 pointToParticle;

            foreach (Particle p in particles)
            {
                pointToParticle = p.position - point;
                if (pointToParticle.LengthSquared() <= cutoffSquared)
                {
                    float force = -1 * pointToParticle.Length() + cutoff;
                    pointToParticle.Normalize();
                    p.velocity += 0.35f * pointToParticle * force;
                }
            }
        }

        public void pushToPoint(Vector2 point, int cutoff)
        {
            float cutoffSquared = (float)Math.Pow(cutoff, 2);
            Vector2 pointToParticle;

            foreach (Particle p in particles)
            {
                pointToParticle = -1 * (p.position - point);
                if (pointToParticle.LengthSquared() <= cutoffSquared)
                {
                    float force = (-1 * pointToParticle.Length() + cutoff)/(1.0f * cutoff);
                    //pointToParticle.Normalize();
                    p.velocity += 0.05f * pointToParticle * force;
                }
            }
        }

        private void nextColor()
        {
            switch (shift)
            {
                case 1:
                    if (delta == 1)
                    {
                        color.R += 1;
                    }
                    else
                    {
                        color.R -= 1;
                    }
                    if (color.R == 0 || color.R >= 255)
                    {
                        delta *= -1;
                        shift = 3;
                    }
                    break;
                case 2:
                    if (delta == 1)
                    {
                        color.G++;
                    }
                    else
                    {
                        color.G--;
                    }
                    if (color.G == 0 || color.G == 255)
                    {
                        delta *= -1;
                        shift = 1;
                    }
                    break;
                case 3:
                    if (delta == 1)
                    {
                        color.B++;
                    }
                    else
                    {
                        color.B--;
                    }
                    if (color.B == 0 || color.B == 255)
                    {
                        delta *= -1;
                        shift = 2;
                    }
                    break;
            }

        }
    }
}
