using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using RockGamer.Gamer.Screener;
using Microsoft.Xna.Framework.Input;
using RockGamer.Gamer;
using Obo.GameUtility;
using Obo.Utility;

namespace RockGamer.Screener
{

    public enum MessageBoxAnswer
    {
        None,
        Ok,
        Cancel,
        Back,
        Yes,
        No,
    }

    public class ScreenManager : DrawableGameComponent
    {
        List<Screen> screens = new List<Screen>();
        List<Screen> screensToUpdate = new List<Screen>();

        Texture2D blankTexture;

        public bool isInitialized;
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont Font { get; set; }

        //public NetworkManager networkManager;

        private int screenTimer;

        NewFpsCounter frameCounter;

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled { get; set; }

        public ScreenManager(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            frameCounter = new NewFpsCounter();
            isInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
            UtilityContent.Load(content);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });

            //NetworkManager.Instance.Start();


            UtilityContent.debugFont = content.Load<SpriteFont>("Fonts/debugFont");
            Extras.DebugFont = UtilityContent.debugFont;

            foreach(Screen screen in screens)
            {
                screen.Load();
            }
        }

        protected override void UnloadContent()
        {
            foreach(Screen screen in screens)
            {
                screen.Unload();
            }
        }

        public override void Update(GameTime gt)
        {

            Input.Update(gt);
            //NetworkManager.Instance.Update();

            MessagePopupManager.Instance.Update(gt);

            frameCounter.Update(gt);

            if(Input.LeftClick)
            {
                MessagePopupManager.AddMsg("CLICK", false);
            }
            if(Input.RightClick)
            {
                MessagePopupManager.AddMsg("CLOCK", true);
            }

            if(Input.KeyClick(Keys.F1))
                Globals.IsDrawStats = !Globals.IsDrawStats;

            if(Input.KeyClick(Keys.F2))
                Globals.IsDebugging = !Globals.IsDebugging;

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screensToUpdate.Clear();

            foreach(Screen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while(screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                Screen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gt, otherScreenHasFocus, coveredByOtherScreen);

                if(/*screen.ScreenState == ScreenState.TransitionOn ||*/
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if(!otherScreenHasFocus)
                    {
                        if(!screen.IsPaused)
                            screen.ActiveUpdate(gt);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if(!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
            // Print debug trace?
            if(TraceEnabled)
                TraceScreens();

            // if no screen, add menu sreen
            if(screens.Count <= 0)
            {
                screenTimer++;
                if(screenTimer > 100)
                {
                    Log.E("No screen - adding MainMenuScreen");
                    AddScreen(new MainMenuScreen());
                    screenTimer = 0;
                }
            }
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach(Screen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Trace.WriteLine(string.Join(", ", screenNames.ToArray()));
        }


        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gt)
        {
            GraphicsDevice.Clear(ClearOptions.Target, new Color(33, 33, 33), 1f, 0);
            frameCounter.DrawStart();

            Game.Window.Title = $"{Globals.ProjectName} - FPS: {frameCounter.FrameRate}  DMS: {frameCounter.DrawMS}";

            foreach(Screen screen in screens)
            {
                if(screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(SpriteBatch, gt);
            }

            SpriteBatch.Begin();

            Extras.DrawDebug(SpriteBatch, $"Debug: {Globals.IsDebugging}", 1, Color.ForestGreen);

            MessagePopupManager.Instance.Draw(SpriteBatch);

            SpriteBatch.End();

            frameCounter.DrawEnd();
        }

        public void AddScreen(Screen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;
            // If we have a graphics device, tell the screen to load content.
            if(isInitialized)
            {
                screen.Load();
            }
            screens.Add(screen);
        }

        public T GetScreen<T>() where T : Screen => GetScreens().First(s => s is T) as T;

        public Screen ReturnScreen<T>() where T : Screen => this as T;

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(Screen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if(isInitialized)
            {
                screen.Unload();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        public void RemoveScreen(Screen screen, Screen newScreen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if(isInitialized)
            {
                screen.Unload();
            }
            AddScreen(newScreen);
            screens.Remove(screen);
            screensToUpdate.Remove(screen);

            //GC.Collect();
        }

        public void ExitAllScreensAndAdd(Screen screen)
        {
            for(int i = 0; i < screens.Count; i++)
            {
                screens[i].ExitScreen(true);
            }
            AddScreen(screen);
        }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public Screen[] GetScreens()
        {
            return screens.ToArray();
        }

        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public void Fader(int alpha)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(blankTexture, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), new Color(0, 0, 0, alpha));

            SpriteBatch.End();
        }
    }
}