using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer.Sprites
{
    public class AnimatedSprite : Sprite
    {
        public Rectangle[] CurrentAnimation;
        public Rectangle CurrentAnimationFrame;
        public double FrameLength = 0.2;

        public double AnimationDuration => (FrameLength * CurrentAnimation.Length);
        public int frame;
        double frameTimer;

        public bool CustomDraw;

        public AnimationType CurrentAnimationType;

        /// <summary>
        /// IF FALSE, USE SetFrame(x,x) to set FrameRectangle
        /// </summary>
        public bool IsAnimating = true;

        public AnimatedSprite()
        {
            CurrentAnimation = new[] { new Rectangle(0, 0, FrameWidth, FrameHeight) };
        }

        public AnimatedSprite(Texture2D tx) : base(tx)
        {
            CurrentAnimation = new[] { new Rectangle(0, 0, FrameWidth, FrameHeight) };
        }

        new public void SetFrame(int column, int row)
        {
            Column = column;
            Row = row;
            CurrentAnimationFrame = new Rectangle(Column * FrameWidth, row * FrameHeight, FrameWidth, FrameHeight);
        }

        public void PlayAnimation(AnimationType type)
        {
            CurrentAnimationType = type;
            CurrentAnimation = AnimationManager.Dic[type];
            IsAnimating = true;
        }

        public void TogglePlayPauseAnimation()
        {
            IsAnimating = !IsAnimating;
        }

        public void PauseAnimation()
        {
            IsAnimating = false;
        }

        public void ResumeAnimation()
        {
            IsAnimating = true;
        }

        public void StopAnimation()
        {
            if(CurrentAnimation.Length > 0)
                CurrentAnimationFrame = CurrentAnimation[0];
            CurrentAnimationType = AnimationType.None;
            IsAnimating = false;
            ResetAnimation();
        }

        public void ResetAnimation()
        {
            frameTimer = 0;
            frame = 0;
        }

        public void UpdateAnimation(GameTime gt)
        {
            if(IsAnimating)
            {
                frameTimer += gt.ElapsedGameTime.TotalSeconds;

                if(frameTimer >= FrameLength)
                {
                    frame = (frame + 1) % CurrentAnimation.Length;
                    frameTimer = 0;
                }

                if(frame >= CurrentAnimation.Length)
                    frame = 0;

                CurrentAnimationFrame = CurrentAnimation[frame];
            }
        }

        new public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, CurrentAnimationFrame, new Color(Color, Alpha), Rotation, Origin, SpriteEffects, Layer);
        }
    }
}
