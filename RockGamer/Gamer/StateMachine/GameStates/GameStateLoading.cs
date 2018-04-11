using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
using Obo.Gui;
using RockGamer.Gamer.Screener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer.StateMachine.GameStates
{
    public class GameStateLoading : GameState
    {


        public GameStateLoading(GameScreen gs) : base(gs)
        {

        }

        public override void Load(ContentManager content)
        {
            base.Load(content);

        }

        public override void Update(GameTime gt, GameScreen gs)
        {
            base.Update(gt, gs);

        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);

        }

        public override void Draw(SpriteBatch sb, Camera cam)
        {
            base.Draw(sb, cam);
            sb.Begin();

            sb.End();
        }

        public override void ExitState()
        {
            base.ExitState();

        }
    }
}
