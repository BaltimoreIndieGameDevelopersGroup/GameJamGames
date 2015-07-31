using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Zombie.Components.Rendering;

namespace Zombie.Components.Entities
{
    public class Bullet
    {
        private ContentManager _contentRef;
        public AnimatedTexture Texture { get; set; }
        public Vector2 Location;
        public bool IsAlive { get; set; }

        public float Speed { get; set; }

        public Doctor Owner { get; set; }

        public Bullet (ContentManager content)
        {
            _contentRef = content;
            IsAlive = true;
            Speed = 7.0f;
            
        }

        public void Update(GameTime gameTime )
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Texture.UpdateFrame(elapsed);  
        }

        public void LoadTexture(AnimatedTexture texture, string name, int frames)
        {
            texture.Load(_contentRef, name, frames, 20);
            Texture = texture;
        }

    }
}