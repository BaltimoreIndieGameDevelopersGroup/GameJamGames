#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Zombie.Components;
using Zombie.Components.General;

#endregion

namespace Zombie.Engine
{

    class BackgroundScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        SoundEffect bgEffect;
        private SoundEffectInstance instance;

        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                    backgroundTexture = content.Load<Texture2D>("PsyBG");

                    //load music
                    
                
                    bgEffect = ScreenManager.Game.Content.Load<SoundEffect>("CV1-3");                    
                    instance = bgEffect.CreateInstance();
                    instance.IsLooped = true;                   
                    instance.Play();
            }
        }

        public override void Unload()
        {
            
           instance.Stop();
            content.Unload();

        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            spriteBatch.End();
        }


        #endregion
    }
}
