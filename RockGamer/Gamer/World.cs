using Microsoft.Xna.Framework;
using Obo.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer
{
    public static class World
    {
        public static float WorldSpeed
        {
            get => worldSpeed;
            set
            {
                worldSpeed = value;
                baseWorldSpeed = worldSpeed;
            }
        }
        static float worldSpeed = 1f;
        static float baseWorldSpeed;
        static float slowmoSpeed;
        static float slowmoTimer;

        public static float Delta(GameTime gt)
        {
            return (float)gt.ElapsedGameTime.TotalSeconds * WorldSpeed;
        }

        /// <param name="speed">1f=normal speed,  0.5f=half speed</param>
        public static void DoSlowmotion(float speed, float time)
        {
            slowmoSpeed = speed;
            slowmoTimer = time;
        }

        public static void Update(GameTime gt)
        {
            if(slowmoTimer > 0)
            {
                slowmoTimer -= gt.Delta();

                //  in porgress
            }

        }

    }
}
