using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using RockGamer.Gamer.Screener;
using RockGamer.Screener;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RockGamer
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        public Game1()
        {
            Console.WriteLine("its nice with food and eat it!!!!");

            Console.WriteLine("yes agree yum yum and here is nice numbers: ");

            int numId = 5512;

            if(numId == 1115)
            {
                Console.WriteLine("its nice");
            }
            else
            {
                Console.WriteLine("also fake it and win it"  + "   agree yes");
            }

            Console.WriteLine("23232323#");
            Console.WriteLine("#testser");

            int httt = 25;

            Console.WriteLine("nice");


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Globals.ProjectName = typeof(Game1).Namespace;

            graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            graphics.PreferredBackBufferHeight = Globals.ScreenHeight;

            OboGlobals.Load(Globals.ScreenWidth, Globals.ScreenHeight);

            Window.Title = Globals.ProjectName;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = false;

            Window.IsBorderless = false;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            IsFixedTimeStep = true;
            IsMouseVisible = true;

            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 240);

            var screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new GameScreen());
        }

        static bool exitGame;

        public static void ExitGame(List<Action> actions = null)
        {
            exitGame = true;

            if(actions != null)
            {
                for(int i = 0; i < actions.Count; i++)
                    actions[i].Invoke();
                Thread.Sleep(50);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(exitGame)
                Exit();
        }

    }
}
