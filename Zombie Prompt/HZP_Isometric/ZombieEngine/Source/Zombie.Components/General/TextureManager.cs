using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace Zombie.Components.General
{    

    public class TextureManager
    {
        private readonly Dictionary<string, Texture2D> _textureLibrary;

        public TextureManager ()
        {
            _textureLibrary = new Dictionary<string, Texture2D>();           

        }

        public void Load(string id, Texture2D texture )
        {
            Debug.Assert(texture != null, "LOAD() texture != null");
            Debug.Assert(_textureLibrary != null, "_textureLibrary != null");
            if (_textureLibrary.ContainsKey(id))
            {
                _textureLibrary[id] = texture;
            }
            else
            {
                _textureLibrary.Add(id, texture);    
            }            

        }

        public Texture2D GetTexture2D(string id)
        {
            Debug.Assert(_textureLibrary != null, "GetTexture2D _textureLibrary != null");
            Texture2D texture;            
            if (_textureLibrary.TryGetValue(id, out texture))
                return texture;
            throw new KeyNotFoundException("Texture with id [" + id + "] was not found!");
        }

        public bool Remove(string id)
        {
            Debug.Assert(_textureLibrary != null, "_textureLibrary != null");
            return _textureLibrary.Remove(id);
        }

        public void ClearAll()
        {
            Debug.Assert(_textureLibrary != null, "_textureLibrary != null");
            _textureLibrary.Clear();
        }
    }
}
