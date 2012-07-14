using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JScreenTest.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JScreenTest.Screens
{
    class ScrollingTextScreen : Screen
    {
        SpriteFont impactFont;
        String message;
        Color color;
        Texture2D whitePixel;

        Vector2 messageSize;
        Vector2 messagePosition;

        float transistion = -1.0f;

        public ScrollingTextScreen(string message, Color color)
        {
            this.message = message;
            this.color = color;
        }

        public override void initialize()
        {
            messageSize = impactFont.MeasureString(message);
            messagePosition = new Vector2(0.5f * (gd.Viewport.Width - messageSize.X) + transistion * gd.Viewport.Width, 0.5f * (gd.Viewport.Height - messageSize.Y));

            this.isTransparent = true;
        }

        public override void  load()
        {
            impactFont = content.Load<SpriteFont>(@"Fonts/modifiedImpact");
            impactFont.Spacing = -12;

            whitePixel = content.Load<Texture2D>("whitePixel");
        }

        public override void  update()
        {
            handleInput();

            transistion += 0.01f;
            messagePosition = new Vector2(0.5f * (gd.Viewport.Width - messageSize.X) + (float)Math.Pow(transistion,3)*gd.Viewport.Width, 0.5f * (gd.Viewport.Height - messageSize.Y));

            if (transistion >= 1.00)
            {
                manager.removeScreen(this);
            }


        }

        public override void  handleInput()
        {
            if (Global.isKeyPressed(Keys.Enter))
            {
                manager.removeScreen(this);
            }
        }

        public override void draw()
        {
            

            int yBuffer = 10;
            Rectangle rectangle = new Rectangle(0, (int)(0.5f * (gd.Viewport.Height - messageSize.Y)) - yBuffer, gd.Viewport.Width, (int)(messageSize.Y + 2 * yBuffer));

            if (transistion < 0)
            {
                sb.Draw(whitePixel, rectangle, new Color(32, 32, 32, 192));
            }
            else
            {
                sb.Draw(whitePixel, rectangle, new Color(32, 32, 32, (int)Math.Round(192 * (1-transistion))));
            }

            
            sb.DrawString(impactFont, message, messagePosition, color);


        }
    }
}
