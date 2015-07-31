using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Zombie.Components.Rendering;

namespace Zombie.Components.Entities
{
    public class DrLight
    {
        private ContentManager _contentRef;
        public AnimatedTexture Texture { get; set; }
        public Vector2 Location;
        public bool IsAlive { get; set; }

        public bool DoneIntro { get; set; }
        public Doctor Owner { get; set; }

        public DrLight(ContentManager content)
        {
            _contentRef = content;
            IsAlive = true;
            DoneIntro = false;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Texture.UpdateFrame(elapsed);
        }

        public void LoadTexture(AnimatedTexture texture, string name, int frames)
        {
            texture.Load(_contentRef, name, frames, 2);
            Texture = texture;
        }

    }
}