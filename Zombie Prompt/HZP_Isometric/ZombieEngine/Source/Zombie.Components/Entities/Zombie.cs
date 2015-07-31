using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Zombie.Components.General;
using Zombie.Components.Rendering;

namespace Zombie.Components.Entities
{
    public class Zombie
    {
        public float TimeSinceLastUpdate;
        //gameplay stats
        public int Health { get; set; }
        public int Score { get; set; }
        public bool IsAlive { get; set; }


        private ContentManager _contentRef;
        private const int Frames = 10;
        private const int FramesPerSec = 10;
        private float Direction = 1.0f;
        public float Speed = 3.0f;

        private const float Rotation = 0;
        private const float Scale = 2.0f;
        private const float Depth = 0.5f;
        public enum AnimationTypes
        {
            None,
            [Description("zombie_walk")]
            Walking,
            [Description("zombie_dead")]
            Dead        
        }

        public enum InputStates
        {
            MoveLeft, MoveRight, MoveUp, MoveDown, None
        }

        private Dictionary<string, AnimatedTexture> _textures;

        public AnimatedTexture CurrentTexture
        {
            get
            {
                if (AnimationState == AnimationTypes.None)
                {
                    return _textures[AnimationTypes.Walking.ToDescription()];
                }

                return _textures[AnimationState.ToDescription()];
            }
            set
            {
                //do nothing ...but obey interface contract

            }

        }
        public AnimationTypes AnimationState;

        public Vector2 CurrentLocation;


        public Zombie(ContentManager contentManager)
        {
            AnimationState = AnimationTypes.None;
            _textures = new Dictionary<string, AnimatedTexture>();
            _contentRef = contentManager;
            TimeSinceLastUpdate = 0;
            IsAlive = true;
        }

        protected void MoveLeft()
        {
            Direction = -1.0f;
            CurrentLocation.X += (Direction * Speed);
            //        shipPos.X += (Direction*Speed);
            //        isMoving = true;
        }
        protected void MoveRight()
        {
            Direction = 1.0f;
            CurrentLocation.X += (Direction * Speed);
        }
        protected void MoveUp()
        {
            Direction = -1.0f;
            CurrentLocation.Y += (Direction * Speed);
            //        shipPos.X += (Direction*Speed);
            //        isMoving = true;
        }
        protected void MoveDown()
        {
            Direction = 1.0f;
            CurrentLocation.Y += (Direction * Speed);
            //        shipPos.X += (Direction*Speed);
            //        isMoving = true;
        }

   

        public void Update(GameTime gameTime, InputStates input)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //HANDLE INPUT STATES
            switch (input)
            {
                case InputStates.MoveLeft:
                    if (AnimationState != AnimationTypes.Dead)
                    {
                        // CurrentTexture.Reset();
                        AnimationState = AnimationTypes.Walking;
                        //  CurrentTexture.UpdateFrame(elapsed);            
                        MoveLeft();
                    }

                    break;
                case InputStates.MoveRight:
                    if (AnimationState != AnimationTypes.Dead)
                    {
                        // CurrentTexture.Reset();
                        AnimationState = AnimationTypes.Walking;
                        //CurrentTexture.UpdateFrame(elapsed);  
                        MoveRight();
                    }
                    break;
                case InputStates.MoveUp:
                    if (AnimationState != AnimationTypes.Dead)
                    {
                        // CurrentTexture.Reset();
                        AnimationState = AnimationTypes.Walking;
                        //CurrentTexture.UpdateFrame(elapsed);  
                        MoveUp();
                    }
                    break;
                case InputStates.MoveDown:
                    if (AnimationState != AnimationTypes.Dead)
                    {
                        // CurrentTexture.Reset();
                        AnimationState = AnimationTypes.Walking;
                        //CurrentTexture.UpdateFrame(elapsed);  
                        MoveDown();
                    }
                    break;
           
                case InputStates.None:
                    //if there are multi-frame animations to play -- go. If not, return since we're not updating anything

                    if (AnimationState != AnimationTypes.Dead)
                        return;
                    break;
                default:
                    break;
            }

            //update state infos

            //death cycle was completed
            if (AnimationState == AnimationTypes.Dead && CurrentTexture.completed)
            {
                IsAlive = false;
                CurrentTexture.Pause();
                return; //wait for collection
            }

            CurrentTexture.UpdateFrame(elapsed);
        }

        public void LoadTexture(AnimatedTexture texture, string name, int frames)
        {
            texture.Load(_contentRef, name, frames, FramesPerSec);
            _textures.Add(name, texture);
        }
    }
}