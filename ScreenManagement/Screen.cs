using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JScreenTest.ScreenManagement
{
    abstract class Screen
    {
        public ContentManager content;
        public SpriteBatch sb;
        public ScreenManager manager;
        public GraphicsDevice gd;

        public bool isTransparent = false;

        public abstract void initialize();
        public abstract void load();
        public abstract void update();
        public abstract void handleInput();
        public abstract void draw();
    }
}
