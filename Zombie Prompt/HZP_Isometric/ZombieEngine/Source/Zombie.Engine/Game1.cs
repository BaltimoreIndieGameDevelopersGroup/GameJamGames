#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Zombie.Components;
using Zombie.Components.General;

#endregion

namespace Zombie.Engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        ScreenFactory screenFactory;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 1050;
            graphics.PreferredBackBufferWidth = 1680;
            Window.IsBorderless = true;
            
            
            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Create the screen factory and add it to the Services
            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();

            //IntPtr hWnd = this.Window.Handle;
            //var control = System.Windows.Forms.Control.FromHandle(hWnd);
            //var form = control.FindForm();
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            //var screen = Screen.AllScreens.First(e => e.Primary);
            //Window.IsBorderless = true;
            //Window.Position = new Point(screen.Bounds.X, screen.Bounds.Y);
            //graphics.PreferredBackBufferWidth = screen.Bounds.Width;
            //graphics.PreferredBackBufferHeight = screen.Bounds.Height;
        }

        private void AddInitialScreens()
        {
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);

            screenManager.AddScreen(new MainMenuScreen(), null);

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            //USE WITH CAUTION -- overrides all screen managers stuff -- might be useful for global key captures (printscreen?/voip) or cheat codes
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
              //  Exit();



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);           

            base.Draw(gameTime);
        }
    }
}
