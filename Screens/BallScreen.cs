using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using JScreenTest.Game;
using JScreenTest.ScreenManagement;

namespace JScreenTest.Screens
{
    class BallScreen : Screen
    {
        Ball blueBall;
        Ball greenBall;

        public override void initialize()
        {
            //this.isTransparent = true;
        }

        public override void load()
        {
            Texture2D ballTexture = content.Load<Texture2D>("ball");

            blueBall = new Ball(
                ballTexture,
                new Vector2(0f),
                0f,
                new Vector2(1f),
                Vector2.Zero,
                Vector2.Zero,
                Color.Blue);

            greenBall = new Ball(
                ballTexture,
                new Vector2(0f),
                (float)Math.PI,
                new Vector2(0.75f),
                Vector2.Zero,
                Vector2.Zero,
                Color.Green);



        }

        public override void update()
        {
            handleInput();
        }

        public override void handleInput()
        {
            if (Global.isKeyPressed(Keys.Space))
            {
                manager.addScreen(new BallScreen());
            }
            if (Global.isKeyPressed(Keys.Escape))
            {
                manager.removeScreen(this);
            }
        }

        public override void draw()
        {
            blueBall.draw(sb);
            greenBall.draw(sb);
        }
    }
}
