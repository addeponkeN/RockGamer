using RockGamer.Screener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Obo.Gui;
using Obo.GameUtility;

namespace RockGamer.Gamer.Screener
{
    public abstract class MenuScreen : Screen
    {
        public List<GuiComponent> Components = new List<GuiComponent>();

        public int Index = 0;

        public override void Load()
        {
            base.Load();

        }

        public void AddComponent(GuiComponent comp)
        {
            comp.IsMenu = true;
            Components.Add(comp);
        }

        public void NextComponent()
        {
            Index++;
            if(Index > Components.Count - 1)
                Index = 0;
        }

        public void PreviousComponent()
        {
            Index--;
            if(Index < 0)
                Index = Components.Count - 1;
        }

        public override void Update(GameTime gt, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gt, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void ActiveUpdate(GameTime gt)
        {
            base.ActiveUpdate(gt);

            if(Input.KeyClick(Keys.Up) || (Input.KeyHold(Keys.LeftShift) && Input.KeyClick(Keys.Tab)))
                PreviousComponent();
            else if(Input.KeyClick(Keys.Tab) || Input.KeyClick(Keys.Down))
                NextComponent();

            //bool anyHovered = Components.Any(x => x.IsHovered);
            for(int i = 0; i < Components.Count; i++)
            {
                var c = Components[i];
                c.Update(gt);
                c.IsActive = false;

                //if(!anyHovered)
                if(Index == i)
                {
                    c.IsActive = true;
                    if(c.ComponentType == GuiComponentType.InputBox)
                        c.GetGuiComponent<InputBox>().IsSelected = true;
                }
                else
                    c.IsActive = false;

                if(c.IsClicked)
                {
                    c.IsActive = true;
                    Index = i;
                }

                if(c.IsHolding)
                    c.Color = new Color(c.BaseColor.R - 25, c.BaseColor.G - 25, c.BaseColor.B - 25);
                else if(c.IsHovered)
                    c.Color = new Color(c.BaseColor.R + 25, c.BaseColor.G + 25, c.BaseColor.B + 25);
                else if(c.IsActive)
                    c.Color = new Color(c.BaseColor.R - 25, c.BaseColor.G - 25, c.BaseColor.B - 25);
                else
                    c.Color = c.BaseColor;

                if(c.IsActive)
                    if(Input.KeyClick(Keys.Enter))
                        c.Trigger();

            }

        }

        public override void Draw(SpriteBatch sb, GameTime gt)
        {
            base.Draw(sb, gt);

            sb.Begin();

            for(int i = 0; i < Components.Count; i++)
            {
                Components[i].Draw(sb);
            }

            sb.End();

            Fader();

            DrawPopups(sb, gt);
        }
    }
}
