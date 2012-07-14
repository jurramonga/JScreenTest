using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JScreenTest.Game
{
    class Sprite
    {
        public Texture2D texture;
        public int frameWidth;
        public int frame;
        public int[] updateDelay;
        public int lastUpdate;

        public Sprite()
        {
            texture = null;
            frameWidth = 0;
            frame = 0;
            updateDelay = new int[0];
            lastUpdate = 0;
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            this.frameWidth = texture.Width;
            frame = 0;
            updateDelay = null;
            lastUpdate = 0;            
        }

        public Sprite(Texture2D texture, int frameWidth, int[] updateDelay, Color color)
        {
            this.texture = texture;
            this.frameWidth = frameWidth;
            this.updateDelay = updateDelay;
            this.lastUpdate = (int)Global.gameTime.TotalGameTime.TotalMilliseconds;

            frame = 0;
        }

        public void update()
        {
            int currentTime = (int)Global.gameTime.TotalGameTime.TotalMilliseconds;
            if (updateDelay != null && lastUpdate + updateDelay[frame] <= currentTime)
            {
                lastUpdate = currentTime;
                setFrame(frame + 1);
            }
        }

        public void draw(SpriteBatch sb, Rectangle destination, Color color)
        {
            sb.Draw(texture, destination, new Rectangle(frame * frameWidth, 0, frameWidth, texture.Height), color);
        }

        public void setFrame(int newFrame)
        {
            frame = newFrame;
            if (frame * frameWidth >= texture.Width)
            {
                frame = 0;
            }
        }
    }
}
