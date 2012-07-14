using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using JScreenTest.Game;
using JScreenTest.ScreenManagement;



namespace JScreenTest.Screens
{
    class SplashScreen : Screen
    {
        const int LOGO_SIZE = 150;

        GameObject logo;
        Texture2D rubik;
        SoundEffect swordEffect;

        float alpha;
        float delta;

        //Rectangle rect;

        public override void initialize()
        {
            alpha = 0.01f;
            delta = 0.01f;

            logo = new GameObject(
                new Sprite(
                    rubik),
                new ObjectState[]
                {
                    new ObjectState(
                        new Rectangle(
                            (gd.Viewport.Width - LOGO_SIZE) / 2,
                            (gd.Viewport.Height - LOGO_SIZE) / 2,
                            LOGO_SIZE,
                            LOGO_SIZE),
                        new Color(255,255,255,0)),
                    new ObjectState(
                        new Rectangle(
                            (gd.Viewport.Width - LOGO_SIZE) / 2,
                            (gd.Viewport.Height - LOGO_SIZE) / 2,
                            LOGO_SIZE,
                            LOGO_SIZE),
                        new Color(255,255,255,255)),
                });                        
        }

        public override void load()
        {
            rubik = content.Load<Texture2D>("newLogo");
            swordEffect = content.Load<SoundEffect>(@"Sounds/sword");
        }

        public override void update()
        {
            handleInput();
            logo.update(alpha);

            if (alpha >= 1.0f)
            {
                swordEffect.Play();

                alpha = 1f;
                delta *= -1;
            }
            if (alpha < 0f)
            {
                manager.removeScreen(this);
            }

            alpha += delta;
        }

        public override void handleInput()
        {
            if (Global.isKeyPressed(Keys.Escape))
            {
                //manager.removeScreen(this);
            }
        }

        public override void draw()
        {
            logo.draw(sb);
        }
    }
}
