using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JScreenTest
{
    static class Global
    {
        public static GameTime gameTime;

        /// <summary>
        /// Do not set this.
        /// </summary>
        public static KeyboardState oldKeyboardState;

        /// <summary>
        /// Do not set this.
        /// </summary>
        public static MouseState oldMouseState;

        public static Color backgroundColor;

        /// <summary>
        /// Determine if key is pressed this frame (not being held)
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public static bool isKeyPressed(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        public static bool isMouseLeftPressed()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }

            return false;
        }

        public static bool isMouseRightPressed()
        {
            if (Mouse.GetState().RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
            {
                return true;
            }

            return false;
        }

    }
}
