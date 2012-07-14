using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using JScreenTest.ScreenManagement;

namespace JScreenTest.Screens
{
    class ExitScreen : ButtonScreen
    {
        const int RESUME_BUTTON = 0;
        const int RESTART_BUTTON = 1;
        const int EXIT_BUTTON = 2;

        Texture2D whitePixel;
        Texture2D mouseCursor;

        SpriteFont tf2Font;
        String message;

        int rectBuffer = 50;

        MouseState mouseState;

        Screen parent;

        public ExitScreen(Screen parentScreen, String message)
        {
            this.parent = parentScreen;
            this.message = message;
        }

        public ExitScreen(Screen parentScreen)
        {
            this.parent = parentScreen;
            this.message = null;
        }

        public override void initialize()
        {
            this.isTransparent = true;

            buttonSizeX = 250;
            buttonSizeY = 60;
            buttonBufferX = 200;
            buttonBufferY = 600;

            Color[] buttonColors = new Color[4] 
            {
                new Color(32, 32, 32, 224),
                new Color(64, 64, 64, 224),
                new Color(32, 32, 32, 224),
                new Color(32, 32, 32, 224)
            };
        

            buttons.Add(new Button(whitePixel, buttonColors, new Rectangle(), tf2Font, "Resume", Color.White));
            buttons.Add(new Button(whitePixel, buttonColors, new Rectangle(), tf2Font, "Restart", Color.White));
            buttons.Add(new Button(whitePixel, buttonColors, new Rectangle(), tf2Font, "Exit", Color.White));

            placeButtonsHorizontal();
        }

        public override void load()
        {
            whitePixel = content.Load<Texture2D>(@"whitePixel");
            mouseCursor = content.Load<Texture2D>(@"mouseCursor");

            tf2Font = content.Load<SpriteFont>(@"Fonts/TF2 Build");
        }

        public override void update()
        {
            handleInput();

            mouseState = Mouse.GetState();
        }

        public override void handleInput()
        {
            buttonCheck(false);
        }

        public override void draw()
        {
            Vector2 stringSize;
            Vector2 stringPosition;

            sb.Draw(whitePixel, new Rectangle(rectBuffer, rectBuffer, gd.Viewport.Width - 2 * rectBuffer, gd.Viewport.Height - 2 * rectBuffer), new Color(64, 64, 64, 192));

            if (message != null)
            {
                stringSize = tf2Font.MeasureString(message);
                stringPosition = new Vector2(
                    (gd.Viewport.Width - stringSize.X) / 2,
                    gd.Viewport.Height / 4);

                sb.DrawString(tf2Font, message, stringPosition, Color.Red);
            }

            foreach (Button button in buttons)
            {
                button.draw(sb);
            }

            sb.Draw(mouseCursor, new Rectangle(mouseState.X, mouseState.Y, mouseCursor.Width / 4, mouseCursor.Height / 4), Color.White);
        }

        public override void buttonClicked(int position)
        {
            switch (position)
            {
                case RESUME_BUTTON:
                    manager.removeScreen(this);
                    break;
                case RESTART_BUTTON:
                    manager.removeScreen(this);
                    parent.initialize();                   
                    break;
                case EXIT_BUTTON:
                    manager.removeScreen(this);
                    manager.removeScreen(parent);                    
                    break;
                default:

                    break;
            }
        }
    }
}
