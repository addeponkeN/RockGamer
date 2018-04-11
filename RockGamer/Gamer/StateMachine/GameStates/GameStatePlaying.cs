using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
using Obo.Gui;
using RockGamer.Gamer.Screener;

namespace RockGamer.Gamer.StateMachine.GameStates
{
    public class GameStatePlaying : GameState
    {

        Label lb;

        public GameStatePlaying(GameScreen gs) : base(gs)
        {
            FadeInTime = 2f;
            FadeOutTime = 2f;

            lb = new Label(UtilityContent.debugFont, "");
            lb.Position = new Vector2(600, 400);
        }

        public override void Load(ContentManager content)
        {
            base.Load(content);

            SoundManager.Load(game.Content);
        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);

            if(Input.KeyClick(Keys.F))
                StartExit(new GameStateLoading(game));

            if(Input.KeyClick(Keys.W))
                SoundManager.PlaySound(GameSoundType.Shoot);
        }

        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);
            sb.Begin();

            lb.Text = $"Playing Percent:  {FadeProgress * 100:N0}%";
            lb.Draw(sb);

            sb.End();
        }

        public override void ExitState()
        {
            base.ExitState();

        }

    }
}
