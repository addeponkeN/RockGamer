using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Obo.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer.Sprites
{
    public class Sprite
    {
        public Texture2D Texture;

        public Vector2 Position;
        public void SetPosition(int x, int y)
        {
            Position = new Vector2(x, y);
        }
        public void SetPosition(int xy)
        {
            Position = new Vector2(xy);
        }

        public Vector2 Size;
        public Vector2 BaseSize;
        public void SetSize(int x, int y)
        {
            Size = new Vector2(x, y);
            BaseSize = Size;
        }
        public void SetSize(int xy)
        {
            Size = new Vector2(xy);
            BaseSize = Size;
        }

        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X * Globals.Scale.X), (int)(Size.Y * Globals.Scale.Y));

        public int Column = 0;
        public int Row = 0;
        public int FrameWidth = 256;
        public int FrameHeight = 256;

        public void SetSourceSize(int width, int heigth)
        {
            FrameWidth = width;
            FrameHeight = heigth;
        }
        public void SetSourceSize(int wh)
        {
            FrameWidth = wh;
            FrameHeight = wh;
        }
        public Rectangle FrameRectangle => new Rectangle(Column * FrameWidth, Row * FrameHeight, FrameWidth, FrameHeight);

        public int Alpha = 255;
        public Color Color = Color.White;
        public Color BaseColor = Color.White;
        public void SetColor(Color color)
        {
            Color = color;
            BaseColor = color;
        }

        public float Rotation;
        public Vector2 Origin;
        public SpriteEffects SpriteEffects;
        public float Layer = 1f;

        public Sprite()
        {
            Texture = UtilityContent.box;
            FrameWidth = Texture.Width;
            FrameHeight = Texture.Height;
            SetSize(FrameWidth, FrameHeight);
        }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            FrameWidth = Texture.Width;
            FrameHeight = Texture.Height;
            SetSize(FrameWidth, FrameHeight);
        }

        public void FlipHorizontally()
        {
            if(SpriteEffects == SpriteEffects.FlipHorizontally)
                SpriteEffects = SpriteEffects.None;
            else
                SpriteEffects = SpriteEffects.FlipHorizontally;
        }

        public void FlipVertically()
        {
            if(SpriteEffects == SpriteEffects.FlipVertically)
                SpriteEffects = SpriteEffects.None;
            else
                SpriteEffects = SpriteEffects.FlipVertically;
        }

        public void SetFrame(int column, int row)
        {
            Column = column;
            Row = row;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Rectangle, FrameRectangle, new Color(Color, Alpha), Rotation, Origin, SpriteEffects, Layer);
        }

    }
}
