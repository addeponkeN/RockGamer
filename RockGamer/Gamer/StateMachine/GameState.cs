using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Gui;
using RockGamer.Gamer.Screener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer.StateMachine
{
    public enum GameStateType
    {
        Entering,
        Running,
        Exiting,
    }

    public class GameState : State
    {
        public GameState QueuedState;
        public GameScreen game;

        public GameStateType State = GameStateType.Entering;

        Label lbTitle;
        float lbLife = 1.5f;

        public float FadeInTime = 1f;
        public float FadeOutTime = 1f;

        public float FadeProgress => State == GameStateType.Running ? 1f : State == GameStateType.Entering ? fadeInLerp : fadeOutLerp;

        float fadeInLerp = 0f;
        float fadeOutLerp = 0f;

        float fadeInValue = 1f;
        float fadeOutValue = 0f;

        public GameState(GameScreen g)
        {
            game = g;

            lbTitle = new Label(UtilityContent.debugFont, GetType().Name);
            lbTitle.Scale = 1.5f;
            lbTitle.Position = new Vector2(GHelper.Center(OboGlobals.ScreenBox, lbTitle.Size * lbTitle.Scale).X, 100);
        }

        public virtual void Load(ContentManager content)
        {

        }

        void UpdateEnter(GameTime gt)
        {
            fadeInLerp += gt.Delta() / FadeInTime;
            fadeInValue = MathHelper.Lerp(1f, 0f, fadeInLerp);

            lbLife -= gt.Delta();

            if(fadeInValue < 0)
                State = GameStateType.Running;
        }

        void UpdateExit(GameTime gt)
        {
            fadeOutLerp += gt.Delta() / FadeOutTime;
            fadeOutValue = MathHelper.Lerp(0f, 1f, fadeOutLerp);

            if(fadeOutValue >= 1f)
                ExitState();
        }

        public virtual void ActiveUpdate(GameTime gt)
        {

        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

            switch(State)
            {
                case GameStateType.Entering:
                    UpdateEnter(gt);
                    break;
                case GameStateType.Running:
                    ActiveUpdate(gt);
                    break;
                case GameStateType.Exiting:
                    UpdateExit(gt);
                    break;
            }

        }

        public virtual void Draw(SpriteBatch sb, Camera cam)
        {
            sb.Begin();

            switch(State)
            {
                case GameStateType.Entering:
                    sb.Draw(UtilityContent.box, OboGlobals.ScreenBox, new Color(Color.Black, (int)(255 * fadeInValue)));

                    lbTitle.Alpha = (int)(lbLife * 255);
                    lbTitle?.Draw(sb);

                    break;

                case GameStateType.Exiting:
                    sb.Draw(UtilityContent.box, OboGlobals.ScreenBox, new Color(Color.Black, (int)(255 * fadeOutValue)));
                    break;
            }

            sb.End();
        }

        public virtual void StartExit(GameState state = null)
        {
            State = GameStateType.Exiting;
            QueuedState = state;
        }

        public override void ExitState()
        {
            base.ExitState();

            if(QueuedState != null)
                StateManager.AddState(QueuedState);

        }

    }
}
