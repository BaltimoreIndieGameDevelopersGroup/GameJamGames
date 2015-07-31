using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombie.Components.Entities
{
    public class Dialogue
    {
        public bool IsLarge;
        public bool IsDone
        { get { return Elapsed >= RunTime; } }
        public bool IsAlive;
        public float RunTime;
        public float Elapsed;

        public Texture2D Texture;

        private Vector2 _location;

        private Doctor _docRef;



        public Vector2 Location {
            get
            {
                if (IsAttached)
                    return UpdateAttachedLocation();
                else
                    return _location;                
            }
            set { _location = value; }
        }
        private bool IsAttached;

        public void AttachTo(Doctor d)
        {
            IsAttached = true;
            _docRef = d;       
        }

        private Vector2 UpdateAttachedLocation()
        {
            return new Vector2(_docRef.CurrentLocation.X + (_docRef.CurrentTexture.CurrentFrameWidth / 3), _docRef.CurrentLocation.Y - 40);   
        }

       
    }
}