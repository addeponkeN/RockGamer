using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RockGamer.Gamer.Screener;
using Obo.GameUtility;
using RockGamer.Gamer.StateMachine;

namespace RockGamer.Screener
{
    public enum ScreenState
    {
        TransitionOn,
        TransitionHalfOn,
        Active,
        TransitionOff,
        TransitionHalfOff,
        Hidden,

        Paused
    }

    public abstract class Screen
    {
        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup { get; set; }

        public bool IsTransition;

        public TimeSpan TransitionOnTime { get; set; } = TimeSpan.FromSeconds(0.25);

        public TimeSpan TransitionOffTime { get; set; } = TimeSpan.FromSeconds(0.25);

        public List<Screen> ChildScreens = new List<Screen>();
        public List<PopupScreen> Popups = new List<PopupScreen>();

        //public SpriteBatch sb;
        public ContentManager Content { get; set; }
        public Screen ParentScreen { get; set; }
        bool IsLoaded = false;
        Screen newScreen;

        /// <summary>
        /// Gets the current position of the screen transition, ranging
        /// from zero (fully active, no transition) to one (transitioned
        /// fully off to nothing).
        /// </summary>
        public float TransitionPosition { get; set; } = 1;

        /// <summary>
        /// Gets the current alpha of the screen transition, ranging
        /// from 255 (fully active, no transition) to 0 (transitioned
        /// fully off to nothing).
        /// </summary>

        public int TransitionAlpha => (int)(TransitionPosition * 255);

        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState { get; set; } = ScreenState.TransitionOn;

        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting { get; set; }

        internal void GiveAnswer(PopupAnswer ok)
        {
            PopupAnswer = ok;
            ParentScreen.ReadAnswer(ok);
            IsPaused = false;
        }

        public virtual void ReadAnswer(PopupAnswer ok)
        {
            IsPaused = false;
        }

        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive => !otherScreenHasFocus &&
                       (ScreenState == ScreenState.TransitionOn ||
                        ScreenState == ScreenState.Active);

        bool otherScreenHasFocus;

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager { get; internal set; }
        public bool IsPaused { get; set; }
        public PopupAnswer PopupAnswer { get; set; } = PopupAnswer.None;

        public T GetChildScreen<T>() where T : Screen => ChildScreens.Find(s => s is T) as T;

        public void AddChildScreen(Screen screen, bool pauseParent = false)
        {
            screen.ScreenManager = ScreenManager;
            screen.ParentScreen = this;
            screen.IsExiting = false;
            IsPaused = pauseParent;
            screen.Load();
            ChildScreens.Add(screen);
        }

        public void AddPopupScreen(string text, bool error, PopupType type, bool pause)
        {
            var pup = new PopupScreen(text, error, type);
            pup.ParentScreen = this;
            pup.ScreenManager = ScreenManager;
            pup.Load();
            Popups.Add(pup);
            PauseScreen();
        }

        public void AddPopupScreen(string text, Color color, PopupType type, bool pause)
        {
            var pup = new PopupScreen(text, color, type);
            pup.ParentScreen = this;
            pup.ScreenManager = ScreenManager;
            pup.Load();
            Popups.Add(pup);
            PauseScreen();
        }

        public void AddPopupScreen(string text, Texture2D texture, PopupType type, bool pause)
        {
            var pup = new PopupScreen(text, texture, type);
            pup.ParentScreen = this;
            pup.ScreenManager = ScreenManager;
            pup.Load();
            Popups.Add(pup);
            PauseScreen();
        }

        public virtual void Load() { }

        public virtual void Unload()
        {
            if(Content == null)
                return;
            Content.Unload();
            Content.Dispose();
            Content = null;

            //Console.WriteLine("UNLOADED " + GetType().ToString());
            GC.Collect();
        }

        /// <summary>
        /// base.LoadAfterLoaded(); at bottom
        /// </summary>
        public virtual void LoadAfterLoad() { IsLoaded = true; }

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>

        public virtual void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;
            if(!IsLoaded)
                LoadAfterLoad();

            foreach(var screen in ChildScreens)
            {
                screen.Update(gt, otherScreenHasFocus, coveredByOtherScreen);
            }

            for(int i = 0; i < Popups.Count; i++)
            {
                var pup = Popups[i];
                if(pup.IsExiting)
                {
                    Popups.RemoveAt(i);
                    continue;
                }
                pup.Update(gt, otherScreenHasFocus, coveredByOtherScreen);
            }

            if(IsExiting)
            {
                // If the screen is going away to die, it should transition off.
                ScreenState = ScreenState.TransitionOff;

                if(UpdateTransition(gt, TransitionOffTime, 1) == 0)
                {
                    // When the transition finishes, remove the screen.
                    if(newScreen == null)
                        ScreenManager.RemoveScreen(this);
                    else
                        ScreenManager.RemoveScreen(this, newScreen);
                }
            }
            else if(coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if(UpdateTransition(gt, TransitionOffTime, 1) == 1)
                {
                    // Still busy transitioning.
                    ScreenState = ScreenState.TransitionOff;
                }
                else if(UpdateTransition(gt, TransitionOnTime, 1) == 2)
                {
                    // half compelte transition
                    ScreenState = ScreenState.TransitionHalfOff;
                }
                else
                {
                    // Transition finished!
                    ScreenState = ScreenState.Hidden;
                }
            }
            else if(IsPaused)
            {
                if(UpdateTransition(gt, TimeSpan.FromSeconds(0.25), 1, true) == 1)
                {
                    ScreenState = ScreenState.TransitionOn;
                }
                else
                {
                    ScreenState = ScreenState.Paused;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if(UpdateTransition(gt, TransitionOnTime, -1) == 1)
                {
                    // Still busy transitioning.
                    ScreenState = ScreenState.TransitionOn;
                }
                else if(UpdateTransition(gt, TransitionOnTime, -1) == 2)
                {
                    // half compelte transition
                    ScreenState = ScreenState.TransitionHalfOn;
                }
                else
                {
                    // Transition finished!
                    ScreenState = ScreenState.Active;
                }
            }
        }

        /// <summary>
        /// Helper for updating the screen transition position.
        /// 0 = transition complete
        /// 1 = transitioning
        /// 2 = transition half complete
        /// </summary>
        int UpdateTransition(GameTime gt, TimeSpan time, int direction, bool pause = false)
        {

            foreach(var screen in ChildScreens)
            {
                screen.UpdateTransition(gt, time, direction);
            }

            // How much should we move by?
            float transitionDelta;

            if(time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gt.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);
            if(pause)
            {
                if(TransitionPosition > .55)
                    return 0;
            }

            // Update the transition position.
            TransitionPosition += transitionDelta * direction;



            // Did we reach the end of the transition?
            if(((direction < 0) && (TransitionPosition <= 0)) ||
                ((direction > 0) && (TransitionPosition >= 1)))
            {
                TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                return 0;
            }
            // when transition is half done, it become halfdone
            if(TransitionPosition < .55/* && TransitionPosition > .05*/)
                return 2;
            // Otherwise we are still busy transitioning.
            return 1;
        }

        /// <summary>
        /// update when focused ONLUY
        /// </summary>
        public virtual void ActiveUpdate(GameTime gt)
        {
            foreach(var screen in ChildScreens)
            {
                screen.ActiveUpdate(gt);
            }
        }

        public virtual void Draw(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();

            if(IsPaused)
                Extras.DrawString(sb, UtilityContent.debugFont, "PAUSED", new Vector2(GHelper.Center(Globals.ScreenBox, new Vector2(40, 12)).X, 2));

            sb.End();
        }

        public void DrawPopups(SpriteBatch sb, GameTime gt)
        {
            foreach(var pup in Popups)
            {
                pup.Draw(sb, gt);
            }
        }

        public void DrawChildScreens(SpriteBatch sb, GameTime gt)
        {
            foreach(var screen in ChildScreens)
            {
                screen.Draw(sb, gt);
            }
        }

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen(bool instantExit = false)
        {
            if(instantExit)
            {
                ScreenManager.RemoveScreen(this);
                return;
            }

            if(TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                IsExiting = true;
            }
        }

        /// <summary>
        /// exit and add newscreen after transition
        /// </summary>
        public virtual void ExitScreen(Screen newscreen, bool instantExit = false)
        {
            newScreen = newscreen;
            if(instantExit)
            {
                ScreenManager.RemoveScreen(this, newscreen);
                return;
            }
            if(TransitionOffTime == TimeSpan.Zero)
                ScreenManager.RemoveScreen(this, newscreen);
            else
                IsExiting = true;
        }

        public void PauseScreen()
        {
            IsPaused = true;
        }

        public void UnpauseScreen()
        {
            IsPaused = false;
        }

        public void Fader()
        {
            ScreenManager.Fader(TransitionAlpha);
        }
    }
}