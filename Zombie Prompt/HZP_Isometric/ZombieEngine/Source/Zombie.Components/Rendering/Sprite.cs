using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Zombie.Components.Rendering
{

  public class Sprite : IAnimatable,  IComparable<Sprite>
{
    private Texture2D _texture;
    public bool IsAnimating { get; set; }
    public int CurrentFrameIndex { get; set; }

    public int AnimationRow { get; set; }
    public Point FrameSize { get; set; }
    public int Depth { get; set; }

    public int CompareTo(Sprite other)
    {
        return this.Depth.CompareTo(other.Depth);
    }
    public void LoadContent(ContentManager Content)
    {
        // Call this method from Game.LoadContent()
       // _texture = Content.Load<Texture2D>("");
    }

    public void Draw(SpriteBatch spriteBatch)
        {
            if (IsAnimating)
            {
                // do something..
            }
        }


}

public interface IAnimatable
{
     bool IsAnimating {get; set;}
     int CurrentFrameIndex {get; set;}
     Point FrameSize {get; set;}
}

    public class Spritez : IRenderable
    {
        public string Name { get; set; }
   
        public Guid Id { get; private set; }
        public int Depth { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Source { get; set; }
        public Rectangle Destination { get; set; }
        public Color Color { get; set; }

        public Spritez()
        {
            Id = Guid.NewGuid(); //generates a new unique ID for this sprite
        }

        public int CompareTo(IRenderable other)
        {
            return this.Depth.CompareTo(other.Depth);
        }
    }

    public class AnimatedSprite : Sprite
    {

    }
}
