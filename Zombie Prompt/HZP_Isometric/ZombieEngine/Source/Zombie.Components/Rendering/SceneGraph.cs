using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Zombie.Components.General;

namespace Zombie.Components.Rendering
{

    public class SceneGraph
    {
        //private ScreenManager ScreenManagerRef; //may not be needed
        readonly List<IRenderable> _renderableObjects;

        public SceneGraph()
        {
            _renderableObjects = new List<IRenderable>();
        }

        public void Reset()
        {
            // called on the update loop to clear all renderableObjects and insert any new living objects
            _renderableObjects.Clear();
        }

        public void AddRenderable(IRenderable renderable)
        {
            _renderableObjects.Add(renderable);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //sorted back to front using Icomparables interface
            //back depth is 0, front depth is anything greater than 0
            _renderableObjects.Sort();
            spriteBatch.Begin();
            foreach (var r in _renderableObjects)
                spriteBatch.Draw(r.Texture, r.Source, r.Destination, r.Color);
            spriteBatch.End();
        }


    }
}
