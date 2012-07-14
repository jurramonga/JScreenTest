using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using JScreenTest.Particles;
using JScreenTest.ScreenManagement;

namespace JScreenTest.Screens
{
    class MainMenuScreen : ButtonScreen
    {
        const int playButton = 0;
        const int optionButton = 1;
        const int quitButton = 2;

        Texture2D button;
        Texture2D mouseCursor;
        Texture2D whitePixel;
        Texture2D glowParticle;
        Texture2D menuOverlay;

        SpriteFont ocrFont;

        MouseState mousePosition;

        NetParticleFactory netParticles;
        Shapes.Circle circle;
        int circlePosition;

        Song song;

        Queue<int> updateFrames;
        Queue<int> drawFrames;

        public override void initialize()
        {
            updateFrames = new Queue<int>();
            drawFrames = new Queue<int>();

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);

            netParticles = new NetParticleFactory();
            netParticles.initialize(gd.Viewport, 100, 50, -50, glowParticle, new Color(10, 13, 225, 128));
            circle = new Shapes.Circle(new Vector2(gd.Viewport.Width, gd.Viewport.Height), 250);
            circlePosition = 0;

            buttonSizeX = 250;
            buttonSizeY = 60;
            buttonBufferX = 200;
            buttonBufferY = 600;

            Rectangle buttonPosition = new Rectangle(gd.Viewport.Width/2-button.Width/4,400,button.Width/2, button.Height/2);
            Color[] textColors =
            {
                new Color(255,0,0),
                new Color(0,255,0),
                new Color(0,0,255),
                new Color(0,255,0)
            };

            buttons = new List<Button>();
            buttons.Add(new Button(button, Color.White, buttonPosition, ocrFont, "Play", textColors));
            buttons.Add(new Button(button, Color.White, buttonPosition, ocrFont, "Options", textColors));
            buttons.Add(new Button(button, Color.White, buttonPosition, ocrFont, "Quit", textColors));

            placeButtonsHorizontal();
        }

        public override void load()
        {
            button = content.Load<Texture2D>(@"Buttons/metalButton");
            mouseCursor = content.Load<Texture2D>("mouseCursor");
            whitePixel = content.Load<Texture2D>("whitePixel");
            glowParticle = content.Load<Texture2D>(@"Particles/glowParticle");
            menuOverlay = content.Load<Texture2D>(@"MainMenu/menuOverlay");

            ocrFont = content.Load<SpriteFont>(@"Fonts/OCRaFont2");

            song = content.Load<Song>(@"Songs/DST-impuretechnology");
        }

        public override void update()
        {
            circlePosition += 2;

            if (circlePosition >= 360)
            {
                circlePosition -= 360;
            }

            updateFrames.Enqueue((int)Math.Round(Global.gameTime.TotalGameTime.TotalMilliseconds));

            bool done = false;
            while (!done)
            {
                if ((int)Math.Round(Global.gameTime.TotalGameTime.TotalMilliseconds) - updateFrames.Peek() >= 1000)
                {
                    updateFrames.Dequeue();
                }
                else
                {
                    done = true;
                }
            }

            mousePosition = Mouse.GetState();
            netParticles.update();

            handleInput();
        }

        public override void handleInput()
        {
            buttonCheck(true);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                netParticles.pullFromPoint(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 100);
                netParticles.pushToPoint(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 500);
            }

            else if (CollisionDetector.checkCollision(
                new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height),
                new Point(Mouse.GetState().X, Mouse.GetState().Y)) &&
                (Mouse.GetState().X != Global.oldMouseState.X ||
                Mouse.GetState().Y != Global.oldMouseState.Y))
            {
                netParticles.pushToPoint(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 300);
            }

            Vector2 center = new Vector2(gd.Viewport.Width / 2, gd.Viewport.Height / 2);
            Vector2 position1 = center + circle.getEdgeVector(circlePosition);
            Vector2 position2 = center + circle.getEdgeVector(circlePosition + 90);
            Vector2 position3 = center + circle.getEdgeVector(circlePosition + 180);
            Vector2 position4 = center + circle.getEdgeVector(circlePosition + 270);
            netParticles.pullFromPoint(position1, 50);
            netParticles.pullFromPoint(position2, 50);
            netParticles.pullFromPoint(position3, 50);
            netParticles.pullFromPoint(position4, 50);


            


        }

        public override void buttonCheck(bool perPixel)
        {
            for(int i = 0; i < buttons.Count; i++)
            {
                Point mousePoint = new Point(Mouse.GetState().X, Mouse.GetState().Y);

                //If the mouse is touching the button
                if (CollisionDetector.checkCollision(
                    buttons[i].texture, 
                    buttons[i].rectangle, 
                    mousePoint, 
                    perPixel))
                {

                    //Button has been clicked
                    if (Mouse.GetState().LeftButton == ButtonState.Released && buttons[i].buttonState == 2)
                    {
                        //Do click
                        buttonClicked(i);
                        buttons[i].buttonState = 3;
                    }

                    //Button has been pressed
                    else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        buttons[i].buttonState = 2;
                    }

                    //Button is being hovered over
                    else
                    {
                        buttons[i].buttonState = 1;
                    }
                }

                //Mouse is not over button
                else
                {
                    buttons[i].buttonState = 0;
                }
               
            }
        }

        public override void buttonClicked(int buttonID)
        {
            switch (buttonID)
            {
                case playButton:
                    playButtonClicked();
                    break;
                case optionButton:
                    optionButtonClicked();
                    break;
                case quitButton:
                    quitButtonClicked();
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Invalid button ID. MainMenuScreen/buttonClicked");
                    break;
            }
        }

        public void playButtonClicked()
        {
            manager.addScreen(new ColorInvasionScreen());
            //manager.addScreen(new TetrisScreen());
            //manager.addScreen(new GameScreen());
            //manager.addScreen(new BallScreen());
        }
        public void optionButtonClicked()
        {

        }

        public void quitButtonClicked()
        {
            manager.addScreen(new ExitScreen(this, "Exit to Desktop?"));
        }

        public override void draw()
        {
            sb.Draw(whitePixel, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), new Color(0, 0, 0, 0));

            drawFrames.Enqueue((int)Math.Round(Global.gameTime.TotalGameTime.TotalMilliseconds));

            bool done = false;
            while (!done)
            {
                if ((int)Math.Round(Global.gameTime.TotalGameTime.TotalMilliseconds) - drawFrames.Peek() >= 1000)
                {
                    drawFrames.Dequeue();
                }
                else
                {
                    done = true;
                }
            }

            netParticles.draw(sb);

            Color overlayColor = netParticles.color;
            overlayColor.A = 1;
            sb.Draw(glowParticle, new Rectangle(gd.Viewport.Width / 4, gd.Viewport.Height / 4, gd.Viewport.Width / 2, gd.Viewport.Height / 2), overlayColor);
            sb.Draw(glowParticle, new Rectangle(gd.Viewport.Width * 3 / 8, gd.Viewport.Height * 3 / 8, gd.Viewport.Width / 4, gd.Viewport.Height / 4), new Color(255,255,255,1));
            
            sb.Draw(menuOverlay, new Rectangle(0, 0, gd.Viewport.Width, gd.Viewport.Height), Color.White);

            foreach (Button button in buttons)
            {
                button.draw(sb);
            }

            sb.Draw(mouseCursor, new Rectangle(mousePosition.X, mousePosition.Y, mouseCursor.Width/4, mouseCursor.Height/4), Color.White);

            string framesPerSecond = updateFrames.Count().ToString();
            Vector2 stringSize = ocrFont.MeasureString(framesPerSecond);
            sb.DrawString(ocrFont, framesPerSecond, new Vector2(gd.Viewport.Width - stringSize.X, gd.Viewport.Height - stringSize.Y), Color.Red);

            framesPerSecond = drawFrames.Count().ToString();
            stringSize = ocrFont.MeasureString(framesPerSecond);
            sb.DrawString(ocrFont, framesPerSecond, new Vector2(gd.Viewport.Width - stringSize.X, gd.Viewport.Height - stringSize.Y - 75), Color.Red);

            
        }
    }
}
