using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using Obo.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer
{
    class PopupLabel : Label
    {
        public float LifeTime = 3f;
        public PopupLabel(SpriteFont font, string text) : base(font, text) { }
        public override void Update(GameTime gt)
        {
            base.Update(gt);
            LifeTime -= gt.Delta();
        }
    }

    class MessagePopupManager
    {

        static MessagePopupManager _instance = new MessagePopupManager();

        public static MessagePopupManager Instance => _instance;


        List<PopupLabel> labels = new List<PopupLabel>();

        public MessagePopupManager()
        {

        }

        public static void AddMsg(string msg, bool error)
        {
            PopupLabel lb = new PopupLabel(UtilityContent.debugFont, msg);

            if(error)
                lb.SetColor(Color.DarkRed);
            else
                lb.SetColor(Color.ForestGreen);

            _instance.labels.Insert(0, lb);
        }

        public static void AddMsg(string msg, Color color)
        {
            PopupLabel lb = new PopupLabel(UtilityContent.debugFont, msg);

            lb.SetColor(color);

            _instance.labels.Insert(0, lb);
        }


        public void Update(GameTime gt)
        {
            for(int i = 0; i < labels.Count; i++)
            {
                var lb = labels[i];
                lb.Update(gt);
                if(lb.LifeTime < -.2f)
                {
                    labels.RemoveAt(i);
                    continue;
                }

                if(lb.LifeTime < .5f)
                {
                    lb.Alpha = (int)(255 * (lb.LifeTime * 2));
                    lb.Color = Color.Lerp(Color.Black, lb.BaseColor, lb.LifeTime * 2);
                }
            }
        }


        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < labels.Count; i++)
            {
                var lb = labels[i];
                var pos = GHelper.Center(Globals.ScreenBox, lb.TextSize) - new Vector2(0, Globals.ScreenHeight / 5 + (i * lb.TextSize.Y + 1));
                lb.Position = pos;
                lb.Draw(sb);
            }
        }

    }
}
