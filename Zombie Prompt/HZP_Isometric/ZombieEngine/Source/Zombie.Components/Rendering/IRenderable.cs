using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombie.Components.Rendering
{
    public interface IRenderable : IComparable<IRenderable>
    {
        Guid Id { get;}
        int Depth { get; set; }
        Texture2D Texture { get; set; }
        Rectangle Source { get; set; }
        Rectangle Destination { get; set; }
        Color Color { get; set; }
    }
}
