using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JScreenTest.ScreenManagement
{
    class Button
    {
        public Texture2D texture;
        public Color[] buttonColors;
        public Rectangle rectangle;
        public SpriteFont font;
        public String text;
        public Color[] textColors;

        /*  
         *  0- Default
         *  1- Mouse is hovered over button
         *  2- Mouse is pressed on button
         *  3- Mouse is released on button
         */
        public int buttonState;

        /// <summary>
        /// Button with 1 texture, 1 color, no text
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        /// <param name="rect"></param>
        public Button(Texture2D texture, Color color, Rectangle rect)
        {
            buttonState = 0;
            this.texture = texture;
            rectangle = rect;
            buttonColors = new Color[4];

            for (int i = 0; i < 3; i++)
            {
                buttonColors[i] = color;
            }
        }

        /// <summary>
        /// Button with 1 texture, 1 color, and text
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="colors">default, hover, press, release</param>
        /// <param name="rect"></param>
        /// <param name="font"></param>
        /// <param name="buttonText"></param>
        public Button(Texture2D texture, Color color, Rectangle rect, SpriteFont font, string buttonText, Color textColor)
        {
            buttonState = 0;
            this.texture = texture;
            rectangle = rect;
            text = buttonText;
            this.font = font;

            buttonColors = new Color[4];
            textColors = new Color[4];
            for (int i = 0; i < 4; i++)
            {
                buttonColors[i] = color;
                textColors[i] = textColor;
            }
        }

        public Button(Texture2D texture, Color color, Rectangle rect, SpriteFont font, string buttonText, Color[] textColors)
        {
            buttonState = 0;
            this.texture = texture;
            rectangle = rect;
            text = buttonText;
            this.font = font;

            buttonColors = new Color[4];
            for (int i = 0; i < 4; i++)
            {
                buttonColors[i] = color;
            }

            this.textColors = new Color[4];
            for (int i = 0; i < 4; i++)
            {
                this.textColors[i] = textColors[i];
            }
        }

        /// <summary>
        /// Button with 1 texture, 4 colors, and text
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="colors">default, hover, press, release</param>
        /// <param name="rect"></param>
        /// <param name="font"></param>
        /// <param name="buttonText"></param>
        public Button(Texture2D texture, Color[] colors, Rectangle rect, SpriteFont font, string buttonText, Color textColor)
        {
            buttonState = 0;
            this.texture = texture;
            rectangle = rect;
            text = buttonText;
            this.font = font;

            buttonColors = new Color[4];
            for (int i = 0; i < 3; i++)
            {
                buttonColors[i] = colors[i];
            }
            buttonColors[3] = colors[2];

            textColors = new Color[4];
            for (int i = 0; i < 3; i++)
            {
                textColors[i] = textColor;
            }
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, rectangle, buttonColors[buttonState]);

            if (text != null)
            {
                Vector2 fontOffset = font.MeasureString(text);
                sb.DrawString(
                    font,
                    text,
                    new Vector2(
                        rectangle.X + rectangle.Width / 2 - fontOffset.X / 2,
                        rectangle.Y + rectangle.Height / 2 - fontOffset.Y / 2),
                    textColors[buttonState]);
            }
        }
    }
}
