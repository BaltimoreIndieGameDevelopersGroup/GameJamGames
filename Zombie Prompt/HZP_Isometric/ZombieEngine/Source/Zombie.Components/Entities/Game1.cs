#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Remoting.Messaging;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Tears.Components.Rendering;

#endregion

namespace Tears.Components.Entities
{
    public class Game1 : Game
    {
        public class DialogNode
        {
            public enum DialogCategories{ Firing, Other}
            public DialogCategories Category { get; set; }
            public Texture2D DialogueTexture { get; set; }
        }

        protected List<DialogNode> _dialogues;

        private List<Dialogue> _activeDialogs; 
        private Doctor _player1;
        private Doctor _player2;
        private DrLight _player3;
        private Dialogue _evilIntro;
        
        private List<Zombie> _zombieHorde;
        private List<Bullet> _activeBullets; 
        
        

        private Texture2D _p1Indicator;
        private Texture2D _p2Indicator;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const float Rotation = 0;
        private const float Scale = 3.0f;
        private const float Depth = 0.5f;
        private Viewport viewport;
        private Vector2 shipPos;
        private const int Frames = 10;
        private const int FramesPerSec = 10;
        private  float Direction = 1.0f;
        public  float Speed = 3.0f;

        public int ZOMBIE_COUNT = 10;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Set device frame rate to 30 fps.
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
            _player1 = new Doctor(Content);
            _player2 = new Doctor(Content);
            _player3 = new DrLight(Content);

            _dialogues = new List<DialogNode>();
            _activeDialogs = new List<Dialogue>();
            _activeBullets = new List<Bullet>();

            _zombieHorde = new List<Zombie>();

            for (int i =0; i < ZOMBIE_COUNT; i++)
            {
                _zombieHorde.Add(new Zombie(Content));
            }

            _evilIntro = new Dialogue();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
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
                Bullet b1 = new Bullet(Content);
                b1.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_shots", 4);
                b1.Owner = _player1;
                _player1.Bullets.Push(b1);

                Bullet b2 = new Bullet(Content);
                b2.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_shots", 4);
                b2.Owner = _player2;
                _player1.Bullets.Push(b2);
            }
          

            //load all dialogues

            //textures for dialogs
            _dialogues.Add(
                new DialogNode()
                {
                  Category =  DialogNode.DialogCategories.Firing,
                  DialogueTexture = Content.Load<Texture2D>("Z1")                 
                }
                );


           
            //disperse them vertically
           


            // _doctor.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "Scientist_Idle", 8);
           // SpriteTexture.Load(Content, "Scientist_Walking", Frames, FramesPerSec);
            //FiringTexture.Load(Content, "Scientist_Firing", 9, FramesPerSec);

            _p1Indicator = Content.Load<Texture2D>("P1");
            _p2Indicator = Content.Load<Texture2D>("P2");
            viewport = graphics.GraphicsDevice.Viewport;
            _player1.CurrentLocation = new Vector2(viewport.Width / 2, viewport.Height / 2);
            _player2.CurrentLocation = new Vector2(viewport.Width / 2, viewport.Height / 3);

            _player3.Location = new Vector2(0, viewport.Height / 3);
            _evilIntro.Texture = Content.Load<Texture2D>("D1");
            _evilIntro.IsAlive = true;
            _evilIntro.RunTime = 4;
            _evilIntro.Location = new Vector2(_player3.Location.X, _player3.Location.Y-50);
            _evilIntro.IsLarge = true;
            _activeDialogs.Add(_evilIntro);
            Random r = new Random();

            foreach (var zombie in _zombieHorde)
            {
                zombie.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "zombie_walk", 7);
                zombie.LoadTexture(new AnimatedTexture(Vector2.Zero, Rotation, Scale, Depth), "zombie_dead", 7);
                zombie.CurrentLocation.Y = r.Next(0, viewport.Height-128);
                zombie.CurrentLocation.X = 100;
            }

            //load music
            SoundEffect bgEffect;
            bgEffect = Content.Load<SoundEffect>("SF2_Intro");
            SoundEffectInstance instance = bgEffect.CreateInstance();
            instance.IsLooped = true;
            bgEffect.Play(0.1f, 0.0f, 0.0f);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //update elements
            foreach (var diag in _activeDialogs)
            {               
                if (diag.IsDone)
                    diag.IsAlive = false;
            }

            _activeDialogs.RemoveAll(x => !x.IsAlive);

            _activeBullets.RemoveAll(x => !x.IsAlive);

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
                    b.Location.Y += 20;
                    _activeBullets.Add(b);
                }

            }

            if(    Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _player2.Update(gameTime, Doctor.InputStates.Fire);
            }

            // LEFT
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
            {
                _player1.Update(gameTime, Doctor.InputStates.MoveLeft);
            }
        
            if(    Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _player2.Update(gameTime, Doctor.InputStates.MoveLeft);
            }

            // RIGHT
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
            {
                _player1.Update(gameTime, Doctor.InputStates.MoveRight);
            }
        
            if(    Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _player2.Update(gameTime, Doctor.InputStates.MoveRight);
            }

            // UP
            if (GamePad.GetState(PlayerIndex.One).DPad.Up   == ButtonState.Pressed)
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

            //post update all elements
            _player1.Update(gameTime, Doctor.InputStates.None);
            _player2.Update(gameTime, Doctor.InputStates.None);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var d in _activeDialogs)
                d.Elapsed += elapsed;
            

            //update all zombies
            foreach (var zombie in _zombieHorde)
            {
                if(zombie.IsAlive)
                    zombie.Update(gameTime, Zombie.InputStates.None);
            }

            if( _player3.DoneIntro)
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

            base.Update(gameTime);

            

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
                      
                        Rectangle b = new Rectangle((int)bullet.Location.X, (int) bullet.Location.Y, 64, 10 );
                        if (z.Intersects(b))
                        {
                            bullet.IsAlive = false;
                            zombie.AnimationState = Zombie.AnimationTypes.Dead;                            
                        }
                    }


                    //because of the FAUX 3D -- iso -- you're not doing a direct intersect check
                    // check +5 pixels below and above .. so basically mobs must be closely level



                    var p1rect = new Rectangle((int)_player1.CurrentLocation.X,(int) _player1.CurrentLocation.Y,
                        (int)_player1.CurrentTexture.CurrentFrameWidth,(int) _player1.CurrentTexture.CurrentFraneHeight);
                    var p2rect = new Rectangle((int)_player2.CurrentLocation.X, (int)_player2.CurrentLocation.Y,
                      (int)_player2.CurrentTexture.CurrentFrameWidth, (int)_player2.CurrentTexture.CurrentFraneHeight);

                    int PIXEL_BOUNDARY = 3;

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
                        zombie.Update(gameTime, Zombie.InputStates.MoveRight);

                    }

                    zombie.TimeSinceLastUpdate += elapsed; 
                }
                else
                {
                    zombie.TimeSinceLastUpdate = 0;//reset
                }
            }


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();



           spriteBatch.Draw(_p1Indicator, GetIndicatorLocation(_player1), Color.White);
            _player2.CurrentTexture.DrawFrame(spriteBatch, _player2.CurrentLocation, Color.Plum );
          

            //Draw order to put P1 ahead of P2 visually
             spriteBatch.Draw(_p2Indicator,GetIndicatorLocation(_player2), Color.White);
            _player1.CurrentTexture.DrawFrame(spriteBatch, _player1.CurrentLocation, Color.MistyRose);

            _player3.Texture.DrawFrame(spriteBatch, _player3.Location, Color.White);


           


            //draw all zombies
            foreach(var zombie in _zombieHorde)
                zombie.CurrentTexture.DrawFrame(spriteBatch, zombie.CurrentLocation);


            //draw all bullets
            foreach (var bullet in _activeBullets)
            {
                if( bullet.IsAlive)
                    bullet.Texture.DrawFrame(spriteBatch, bullet.Location);
            }

            //draw dialogs
            foreach (var di in _activeDialogs)
            {
                if (di.IsAlive)
                {
                    Rectangle r;
                    if( di.IsLarge)
                            r = new Rectangle((int)di.Location.X, (int)di.Location.Y, 192, 72);
                    else
                               r = new Rectangle((int)di.Location.X, (int)di.Location.Y, 128, 48);
                    spriteBatch.Draw(di.Texture, r, null, Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Vector2 GetIndicatorLocation(Doctor d)
        {
            return new Vector2(d.CurrentLocation.X + (d.CurrentTexture.CurrentFrameWidth / 3), d.CurrentLocation.Y - 10);
        }
    }
}
