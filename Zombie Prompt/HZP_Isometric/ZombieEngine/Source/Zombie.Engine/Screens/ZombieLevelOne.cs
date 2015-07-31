#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zombie.Components;
using Zombie.Components.Entities;
using Zombie.Components.General;
using Zombie.Components.Rendering;

#endregion

namespace Zombie.Engine
{
    class ZombieLevelOne : GameScreen
    {

        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        float pauseAlpha;

        InputAction pauseAction;

        Texture2D _levelMap;

       // private AnimatedTexture _doc;

        private Rectangle _camera;
        private Rectangle _screenRectangle;


        //NEW MEMBER FIELDS

        SoundEffect bgEffect;
        SoundEffect bgEffect2;
        private SoundEffectInstance instance;
        public class DialogNode
        {
            public enum DialogCategories { Firing, Other }
            public DialogCategories Category { get; set; }
            public Texture2D DialogueTexture { get; set; }
        }

        protected List<DialogNode> _dialogues;

        private List<Dialogue> _activeDialogs;
        private Doctor _player1;
        private Doctor _player2;
        private DrLight _player3;
        private Dialogue _evilIntro;

        private List<Components.Entities.Zombie> _zombieHorde;
        private List<Bullet> _activeBullets;



        private Texture2D _p1Indicator;
        private Texture2D _p2Indicator;

        private const float Rotation = 0;
        private const float Scale = 3.0f;
        private const float Depth = 0.5f;
        private Viewport viewport;
        private const int Frames = 10;
        private const int FramesPerSec = 10;
        private const int ZOMBIE_COUNT = 10;


        #endregion

        #region Initialization

        public ZombieLevelOne()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);

        }

        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                ScreenManager.Game.TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                gameFont = content.Load<SpriteFont>("gamefont");

                //load character map
               
                _levelMap = content.Load<Texture2D>("BG");
                          
                _camera = new Rectangle(0,0, 800, 600);
                _screenRectangle = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width,
                    ScreenManager.GraphicsDevice.Viewport.Height);

                //TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
                _player1 = new Doctor(content);
                _player2 = new Doctor(content);
                _player3 = new DrLight(content);

                _dialogues = new List<DialogNode>();
                _activeDialogs = new List<Dialogue>();
                _activeBullets = new List<Bullet>();

                _zombieHorde = new List<Components.Entities.Zombie>();

                for (int i = 0; i < ZOMBIE_COUNT; i++)
                {
                    _zombieHorde.Add(new Components.Entities.Zombie(content));
                }

                _evilIntro = new Dialogue();



                /// MORE LOADING
                /// 

                _player1.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_Walking", 10);
                _player1.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_Firing", 9);
                _player1.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_Dead", 9);
                _player2.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_Walking", 10);
                _player2.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_Firing", 9);
                _player2.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_Dead", 9);

                _player3.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "doclight", 8);

                //load bullets P1
                for (int i = 0; i < 50; i++)
                {
                    Bullet b1 = new Bullet(content);
                    b1.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_shots", 4);
                    b1.Owner = _player1;
                    _player1.Bullets.Push(b1);

                    Bullet b2 = new Bullet(content);
                    b2.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_shots", 4);
                    b2.Owner = _player2;
                    _player2.Bullets.Push(b2);
                }


                //load all dialogues

                //textures for dialogs
                _dialogues.Add(
                    new DialogNode()
                    {
                        Category = DialogNode.DialogCategories.Firing,
                        DialogueTexture = content.Load<Texture2D>("Z1")
                    }
                    );


                _p1Indicator = content.Load<Texture2D>("P1");
                _p2Indicator = content.Load<Texture2D>("P2");

                viewport = ScreenManager.GraphicsDevice.Viewport;
                _player1.CurrentLocation = new Vector2( (viewport.Width / 4)*3, (viewport.Height / 6)*4);
                _player2.CurrentLocation = new Vector2((viewport.Width / 4) * 3, (viewport.Height / 6) * 3); ;

                _player3.Location = new Vector2(0, viewport.Height / 3);
                _evilIntro.Texture = content.Load<Texture2D>("D1");
                _evilIntro.IsAlive = true;
                _evilIntro.RunTime = 4;
                _evilIntro.Location = new Vector2(_player3.Location.X, _player3.Location.Y - 50);
                _evilIntro.IsLarge = true;
                _activeDialogs.Add(_evilIntro);
                Random r = new Random();

                foreach (var zombie in _zombieHorde)
                {
                    zombie.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "zombie_walk", 7);
                    zombie.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "zombie_dead", 7);
                    zombie.CurrentLocation.Y = r.Next((viewport.Height/3), viewport.Height - 128);
                    zombie.CurrentLocation.X = 100;
                }

                //load music
                bgEffect = ScreenManager.Game.Content.Load<SoundEffect>("SF2_Intro");
                bgEffect2 = ScreenManager.Game.Content.Load<SoundEffect>("CV1-1");

                instance = bgEffect.CreateInstance();
                instance.IsLooped = false;
                instance.Play();

              
                ScreenManager.Game.ResetElapsedTime();
                
            }
        }


        public override void Deactivate()
        {

            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            instance.Stop();
            content.Unload();

        }


        #endregion

        #region Update and Draw


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive) //if not paused
            {                
                HandleUpdate(gameTime);
                if (instance.State == SoundState.Stopped)
                {
                    instance.Stop();
                    instance = bgEffect2.CreateInstance();
                    instance.Play();

                }

            }
        }

        public void HandleUpdate(GameTime gameTime)
        {
            //update elements
            foreach (var diag in _activeDialogs)
            {
                if (diag.IsDone)
                    diag.IsAlive = false;
            }

            _activeDialogs.RemoveAll(x => !x.IsAlive);

            _activeBullets.RemoveAll(x => !x.IsAlive);

            

            //post update all elements
            _player1.Update(gameTime, Doctor.InputStates.None);
            _player2.Update(gameTime, Doctor.InputStates.None);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var d in _activeDialogs)
                d.Elapsed += elapsed;


            //update all zombies
            foreach (var zombie in _zombieHorde)
            {
                if (zombie.IsAlive)
                    zombie.Update(gameTime, Components.Entities.Zombie.InputStates.None);
            }

            if (_player3.DoneIntro)
                RandomMovementToZombies(gameTime);


            //remove any bullets that leave the screen
            foreach (var b in _activeBullets)
            {
                if (b.Location.X < -5)
                    b.IsAlive = false;
                if (b.Location.X > viewport.Width + 10)
                    b.IsAlive = false;

                if (b.IsAlive)
                {
                    b.Location.X -= b.Speed;
                    b.Update(gameTime);
                }

            }

            if (_player3.Texture.completed)
                _player3.DoneIntro = true;

            if (!_player3.DoneIntro)
            {
                _player3.Update(gameTime);
            }



            CheckCollisions(gameTime);
        }


        public void CheckCollisions(GameTime gameTime)
        {
            //check zombie and bullet collisions
            foreach (var zombie in _zombieHorde)
            {
                if (zombie.IsAlive)
                {
                    Rectangle z = new Rectangle((int)zombie.CurrentLocation.X, (int)zombie.CurrentLocation.Y, 64, 128);
                    foreach (var bullet in _activeBullets)
                    {

                        Rectangle b = new Rectangle((int)bullet.Location.X, (int)bullet.Location.Y, 64, 10);
                        if (z.Intersects(b))
                        {
                            bullet.IsAlive = false;
                            zombie.AnimationState = Components.Entities.Zombie.AnimationTypes.Dead;
                            bullet.Owner.Score += 100;
                        }
                    }


                    //because of the FAUX 3D -- iso -- you're not doing a direct intersect check
                    // check +5 pixels below and above .. so basically mobs must be closely level

                    //UPDATE --- this is broken-ish -- checking the top left -- should be checking bottom left -- easy fix

                    var p1rect = new Rectangle((int)_player1.CurrentLocation.X, (int)_player1.CurrentLocation.Y,
                        (int)_player1.CurrentTexture.CurrentFrameWidth, (int)_player1.CurrentTexture.CurrentFraneHeight);
                    var p2rect = new Rectangle((int)_player2.CurrentLocation.X, (int)_player2.CurrentLocation.Y,
                      (int)_player2.CurrentTexture.CurrentFrameWidth, (int)_player2.CurrentTexture.CurrentFraneHeight);

                    int PIXEL_BOUNDARY = 5;

                    if (z.Intersects(p1rect))
                    {
                        if (z.Y < _player1.CurrentLocation.Y + PIXEL_BOUNDARY && z.Y > _player1.CurrentLocation.Y - PIXEL_BOUNDARY)
                        {
                            _player1.IsAlive = false;
                            _player1.AnimationState = Doctor.AnimationTypes.Dead;
                        }
                    }
                    if (z.Intersects(p2rect))
                    {
                        if (z.Y < _player2.CurrentLocation.Y + PIXEL_BOUNDARY && z.Y > _player2.CurrentLocation.Y - PIXEL_BOUNDARY)
                        {
                            _player2.IsAlive = false;
                            _player2.AnimationState = Doctor.AnimationTypes.Dead;
                        }

                    }
                }

            }



        }

        public void RandomMovementToZombies(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random r = new Random();
            foreach (var zombie in _zombieHorde)
            {

                if (zombie.TimeSinceLastUpdate < 4)
                {
                    var z = r.Next(4);
                    if (z < 1)
                    {
                        zombie.Update(gameTime, Components.Entities.Zombie.InputStates.MoveRight);

                    }

                    zombie.TimeSinceLastUpdate += elapsed;
                }
                else
                {
                    zombie.TimeSinceLastUpdate = 0;//reset
                }
            }


        }




        // Lets the game respond to player input. Unlike the Update method,
        // this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {

                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // FIRE
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    _player1.Update(gameTime, Doctor.InputStates.Fire);
                    // trigger firing dialog
                    var d = new Dialogue()
                    {
                        RunTime = 1.5f, //seconds
                        Texture =
                            _dialogues.FirstOrDefault(x => x.Category == DialogNode.DialogCategories.Firing).DialogueTexture,
                        IsAlive = true
                    };
                    d.AttachTo(_player1);

                    _activeDialogs.Add(d);

                    //trigger bullets
                    if (_player1.Bullets.Count > 0)
                    {
                        Bullet b = _player1.Bullets.Pop();
                        b.Location = _player1.CurrentLocation;
                        b.Location.Y += 40;
                        _activeBullets.Add(b);
                    }

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _player2.Update(gameTime, Doctor.InputStates.Fire);
                    //trigger bullets
                    if (_player2.Bullets.Count > 0)
                    {
                        Bullet b = _player2.Bullets.Pop();
                        b.Location = _player2.CurrentLocation;
                        b.Location.Y += 40;
                        _activeBullets.Add(b);
                    }
                }

                // LEFT
                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                {
                    _player1.Update(gameTime, Doctor.InputStates.MoveLeft);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _player2.Update(gameTime, Doctor.InputStates.MoveLeft);
                }

                // RIGHT
                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                {
                    _player1.Update(gameTime, Doctor.InputStates.MoveRight);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    _player2.Update(gameTime, Doctor.InputStates.MoveRight);
                }

                // UP
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                {
                    _player1.Update(gameTime, Doctor.InputStates.MoveUp);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    _player2.Update(gameTime, Doctor.InputStates.MoveUp);
                }

                // DOWN
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                {
                    _player1.Update(gameTime, Doctor.InputStates.MoveDown);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    _player2.Update(gameTime, Doctor.InputStates.MoveDown);
                }

            }
        }



        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //_camera = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 256, 128);
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.CornflowerBlue, 0, 0);
          
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(_levelMap, _screenRectangle, _camera, Color.White);
            //spriteBatch.Draw(_man,
            //    new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/2,
            //        ScreenManager.GraphicsDevice.Viewport.Height/2), Color.White);
            spriteBatch.Draw(_p1Indicator, GetIndicatorLocation(_player1), Color.White);
            _player2.CurrentTexture.DrawFrame(spriteBatch, _player2.CurrentLocation, Color.Plum);


            //Draw order to put P1 ahead of P2 visually
            spriteBatch.Draw(_p2Indicator, GetIndicatorLocation(_player2), Color.White);
            _player1.CurrentTexture.DrawFrame(spriteBatch, _player1.CurrentLocation, Color.MistyRose);

            _player3.Texture.DrawFrame(spriteBatch, _player3.Location, Color.White);





            //draw all zombies
            foreach (var zombie in _zombieHorde)
                zombie.CurrentTexture.DrawFrame(spriteBatch, zombie.CurrentLocation);


            //draw all bullets
            foreach (var bullet in _activeBullets)
            {
                if (bullet.IsAlive)
                    bullet.Texture.DrawFrame(spriteBatch, bullet.Location);
            }

            //draw dialogs
            foreach (var di in _activeDialogs)
            {
                if (di.IsAlive)
                {
                    Rectangle r;
                    if (di.IsLarge)
                        r = new Rectangle((int)di.Location.X, (int)di.Location.Y, 192, 72);
                    else
                        r = new Rectangle((int)di.Location.X, (int)di.Location.Y, 128, 48);
                    spriteBatch.Draw(di.Texture, r, null, Color.White);
                }
            }

            spriteBatch.DrawString(gameFont, "P1:" + _player1.Score, new Vector2(viewport.Width/4,10), Color.Maroon );
            spriteBatch.DrawString(gameFont, "P2:" + _player2.Score, new Vector2(viewport.Width / 2, 10), Color.Maroon);

            spriteBatch.End();
          

            spriteBatch.End();




            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private Vector2 GetIndicatorLocation(Doctor d)
        {
            return new Vector2(d.CurrentLocation.X + (d.CurrentTexture.CurrentFrameWidth / 3), d.CurrentLocation.Y - 10);
        }
        #endregion
    }
}
