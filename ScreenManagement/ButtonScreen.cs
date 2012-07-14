using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JScreenTest.ScreenManagement
{
    abstract class ButtonScreen : Screen
    {
        protected List<Button> buttons = new List<Button>();
        protected int buttonSizeX;
        protected int buttonSizeY;
        protected int buttonBufferX;
        protected int buttonBufferY;

        public virtual void buttonCheck(bool perPixel)
        {
            if (buttons.Count != 0)
            {
                foreach (Button button in buttons)
                {
                    if (CollisionDetector.checkCollision(button.texture, button.rectangle, new Point(Mouse.GetState().X, Mouse.GetState().Y), perPixel))
                    {

                        if (Global.isMouseLeftPressed())
                        {
                            buttonClicked(buttons.IndexOf(button));
                            button.buttonState = 2;
                        }
                        else
                        {
                            button.buttonState = 1;
                        }
                    }
                    else
                    {
                        button.buttonState = 0;
                    }
                }
            }
        }

        public virtual void buttonClicked(int position)
        {

        }

        public override void draw()
        {
            foreach (Button button in buttons)
            {
                button.draw(sb);
            }
        }

        public virtual void placeButtonsHorizontal()
        {
            int gap = buttonSizeX + (gd.Viewport.Width - 2 * buttonBufferX - buttons.Count() * buttonSizeX) / (buttons.Count() - 1);
            Rectangle buttonPosition = new Rectangle(buttonBufferX, buttonBufferY, buttonSizeX, buttonSizeY);

            foreach (Button button in buttons)
            {
                button.rectangle = buttonPosition;
                buttonPosition.X += gap;
            }

        }
    }
}
