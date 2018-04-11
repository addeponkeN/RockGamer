using RockGamer.Screener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Utility;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using RockGamer.Gamer.StateMachine;
using RockGamer.Gamer.StateMachine.GameStates;

namespace RockGamer.Gamer.Screener
{
    public class GameScreen : Screen
    {

        public StateManager StateManager { get; set; }

        public GameScreen()
        {
            StateManager = new StateManager();
        }

        public override void Load()
        {
            base.Load();
            if(Content == null)
                Content = new ContentManager(ScreenManager.Game.Services, "Content");

            AddState(new GameStatePlaying(this));
        }

        public override void LoadAfterLoad()
        {
            base.LoadAfterLoad();

        }

        public override void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gt, otherScreenHasFocus, coveredByOtherScreen);

            StateManager.Update(gt, this);
        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);

        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            base.Draw(sb, gt);

            foreach(GameState state in StateManager.states)
            {
                state.Draw(sb, null);
            }

            DrawChildScreens(sb, gt);
            DrawPopups(sb, gt);
        }

        public void AddState(GameState state)
        {
            state.Load(Content);
            state.Init();
            StateManager.AddState(state);
        }

    }
}
