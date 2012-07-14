using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JScreenTest.Game
{
    class GameObject
    {
        private Sprite sprite;
        private bool activationEffected;
        private float transistion;
        ObjectState[] states;     
        public Vector2 velocity { get; set; }
        public Vector2 acceleration { get; set; }

        public GameObject()
        {
            sprite = new Sprite();
            activationEffected = false;
            transistion = 0;
            states = null;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        public GameObject(Sprite sprite, ObjectState state)
        {
            this.sprite = sprite;
            activationEffected = false;
            transistion = 0;
            states = new ObjectState[2];
            states[1] = state;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        public GameObject(Sprite sprite, ObjectState state, Vector2 vel, Vector2 accel)
        {
            this.sprite = sprite;
            activationEffected = false;
            transistion = 0;
            states = new ObjectState[3];
            states[1] = state;
            velocity = vel;
            acceleration = accel;
        }

        public GameObject(Sprite sprite, ObjectState[] states)
        {
            this.sprite = sprite;
            activationEffected = true;
            transistion = 0;
            this.states = states;
            velocity = Vector2.Zero;
            acceleration = Vector2.Zero;
        }

        public GameObject(Sprite sprite, ObjectState[] states, Vector2 vel, Vector2 accel)
        {
            this.sprite = sprite;
            activationEffected = true;
            transistion = 0;
            this.states = states;
            velocity = vel;
            acceleration = accel;
        }       

        public void update(float transistion)
        {
            sprite.update();

            if (activationEffected)
            {
                this.transistion = transistion;
            }
        }

        public void draw(SpriteBatch sb)
        {
            if (activationEffected)
            {
                ObjectState transistionState = new ObjectState(); 
                transistionState.Rectangle = new Rectangle(
                    (int)(states[0].Rectangle.X + transistion * (states[1].Rectangle.X - states[0].Rectangle.X)),
                    (int)(states[0].Rectangle.Y + transistion * (states[1].Rectangle.Y - states[0].Rectangle.Y)),
                    (int)(states[0].Rectangle.Width + transistion * (states[1].Rectangle.Width - states[0].Rectangle.Width)),
                    (int)(states[0].Rectangle.Height + transistion * (states[1].Rectangle.Height - states[0].Rectangle.Height)));

                transistionState.Color = Color.Lerp(states[0].Color, states[1].Color, transistion);

                transistionState.Rotation = states[0].Rotation + transistion * (states[1].Rotation - states[0].Rotation);

                sprite.draw(sb, transistionState.Rectangle, transistionState.Color);


            }
            else
            {
                sprite.draw(sb, states[1].Rectangle, states[1].Color);
            }
        }

        //public Vector2 getPosition(float transistion)
        //{
            //return transistion * (positions[1] - positions[0]);
        //}
    }
}
