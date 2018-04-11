using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer
{

    public enum AnimationType
    {
        None,

        Idle,

        Down,
        Up,
        Left,
        Right,
    }

    public class AnimationManager
    {
        public static Dictionary<AnimationType, Rectangle[]> Dic;

        public static void Load()
        {
            Dic = new Dictionary<AnimationType, Rectangle[]>();

            AddAnimation(new int[] { 0, 1, 2, 3 }, 0, 32, 32, AnimationType.Down);
            AddAnimation(new int[] { 0, 1, 2, 3 }, 1, 32, 32, AnimationType.Up);
            AddAnimation(new int[] { 0, 1, 2, 3 }, 2, 32, 32, AnimationType.Right);
            AddAnimation(new int[] { 0, 1, 2, 3 }, 3, 32, 32, AnimationType.Left);

            AddAnimation(new int[] { 0 }, 0, 32, 32, AnimationType.Idle);
        }

        /// <summary>
        ///  standard spritesize: 256
        /// </summary>
        static void AddAnimation(int[] column, int row, AnimationType type)
        {
            var f = column.Length;
            var frames = new Rectangle[f];
            for(int i = 0; i < f; i++)
                frames[i] = new Rectangle(column[i] * 256, row * 256, 256, 256);
            Dic.Add(type, frames);
        }

        static void AddAnimation(int[] column, int row, int width, int height, AnimationType type)
        {
            var f = column.Length;
            Rectangle[] frames = new Rectangle[f];
            for(int i = 0; i < f; i++)
                frames[i] = new Rectangle(column[i] * width, row * height, width, height);
            Dic.Add(type, frames);
        }

        static void AddAnimation(int[] column, int[] row, int width, int height, AnimationType type)
        {
            var f = column.Length;
            Rectangle[] frames = new Rectangle[f];
            for(int i = 0; i < f; i++)
                frames[i] = new Rectangle(column[i] * width, row[i] * height, width, height);
            Dic.Add(type, frames);
        }

    }
}

