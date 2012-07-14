using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JScreenTest.ScreenManagement
{
    class ScreenManager
    {
        //public List<Screen> screenList;

        //float is transistion state (0.0-1.0)
        private Dictionary<Screen, float> screenDictionary;

        ContentManager content;
        SpriteBatch sb;
        GraphicsDevice gd;


        public ScreenManager(ContentManager content, SpriteBatch sb, GraphicsDevice gd)
        {
            this.content = content;
            this.sb = sb;
            this.gd = gd;

            screenDictionary = new Dictionary<Screen, float>();
        }

        /// <summary>
        /// Closes all screens, causing the game to exit.
        /// </summary>
        public void close()
        {
            for (int i = screenDictionary.Count - 1; i >= 0; i--)
            {
                this.removeScreen(screenDictionary.ElementAt(i));
            }
        }

        public void addScreen(Screen newScreen)
        {
            newScreen.content = this.content;
            newScreen.sb = this.sb;
            newScreen.manager = this;
            newScreen.gd = this.gd;

            screenList.Add(newScreen);
            newScreen.load();
            newScreen.initialize();
            
        }

        public void removeScreen(Screen exitingScreen, float trans)
        {
            if (screenList.Last() == exitingScreen)
            {
                screenList.Remove(exitingScreen);
            }
        }

        public void update()
        {
            updateTransistions();

            if (screenList.Count > 0)
                screenList.Last().update();
            
        }

        public void draw()
        {
            if (screenList.Count > 0)
            {
                drawScreen(screenList.Last());
            }

        }

        public void drawScreen(Screen screen)
        {
            if (screen.isTransparent && screenList.LastIndexOf(screen) != 0)
            {
                drawScreen(screenList.ElementAt(screenList.LastIndexOf(screen) - 1));
            }

            screen.draw();
        }

        public void updateTransistions()
        {
            for (int i = 0; i < transistionDict.Count(); i++)
            {

            }
        }
    }
}
