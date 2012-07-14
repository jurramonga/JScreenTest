using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using JScreenTest.Game;
using JScreenTest.ScreenManagement;

namespace JScreenTest.Screens
{
    class GameScreen : Screen
    {
        Ball gameBall;
        Ball mouseBall;

        Texture2D ballTexture;
        Texture2D mouseCursor;

        SpriteFont tf2Font;

        int hitCount;

        public override void initialize()
        {
            hitCount = 0;

            gameBall = new Ball(
                ballTexture, 
                new Vector2(50, 100),
                0f,
                new Vector2(0.25f),
                new Vector2(0.5f, 0.0f), 
                new Vector2(0.05f, 0.0f),
                Color.Blue
                );
        }

        public override void load()
        {
            ballTexture = content.Load<Texture2D>("ball");
            mouseCursor = content.Load<Texture2D>("mouseCursor");

            tf2Font = content.Load<SpriteFont>(@"Fonts/TF2 Build");
 	        
        }

        public override void update()
        {
            handleInput();
            gameBall.update();

            if (mouseBall != null)
            {
                mouseBall.update();

                //if (CollisionDetector.checkCollision(mouseBall, gameBall, true))
                if (CollisionDetector.checkCollision(mouseBall.toCircle(), gameBall.toCircle()))
                {
                    hitCount++;
                    CollisionResolver.resolve(mouseBall, gameBall);
                }
            }    
        }

        public override void handleInput()
        {
            if (Global.isKeyPressed(Keys.Escape))
            {
                manager.addScreen(new ExitScreen(this));
            }

            if (Global.isMouseLeftPressed())
            {
                Vector2 scale = new Vector2(0.1f);
                Vector2 position = new Vector2(Mouse.GetState().X - ballTexture.Width / 2, Mouse.GetState().Y - ballTexture.Height / 2);

                mouseBall = new Ball(ballTexture, position, 0f, scale, Vector2.Zero, Vector2.Zero, Color.Orange);
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Pressed && mouseBall.scale.X < 0.5f)
            {
                mouseBall.scale *= 1.05f;
            }
        }

        public override void draw()
        {
            if (mouseBall != null)
            {
                mouseBall.draw(sb);
            }

            sb.DrawString(tf2Font, hitCount.ToString(), new Vector2(0, 480 - tf2Font.MeasureString(hitCount.ToString()).Y), Color.Red);
            //sb.DrawString(tf2Font, Global.gameTime.TotalGameTime.ToString(), Vector2.Zero, Color.Red);

            gameBall.draw(sb);

            MouseState state = Mouse.GetState();
            sb.Draw(mouseCursor, new Rectangle(state.X, state.Y, mouseCursor.Width / 4, mouseCursor.Height / 4), Color.White); 
        }
    }
}
