using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer
{
    public static class Globals
    {
        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;

        public static Vector2 ScreenSize => new Vector2(ScreenWidth, ScreenHeight);
        public static Vector2 ScreenCenter => new Vector2(ScreenWidth / 2, ScreenHeight / 2);
        public static Rectangle ScreenBox => new Rectangle(0, 0, ScreenWidth, ScreenHeight);

        public static Vector2 Scale => new Vector2(1);      //new Vector2(ScreenWidth / 1280, ScreenHeight / 720);
        public static Vector2 UIScale => new Vector2(1);    //((int)(ScreenWidth / 854), (int)(ScreenHeight / 480));

        // update this with camera pos
        public static Vector2 CamPosition { get; set; }

        public static Rectangle CamRectangle => new Rectangle((int)CamPosition.X - (ScreenWidth / 2), (int)CamPosition.Y - (ScreenHeight / 2), ScreenWidth, ScreenHeight);
        public static Rectangle GameRectangle => new Rectangle((int)CamPosition.X - ScreenWidth, (int)CamPosition.Y - ScreenHeight, (int)(ScreenWidth * 1.5f), (int)(ScreenHeight * 1.5f));

        public static Vector2 CamTopLeft => new Vector2(CamRectangle.X, CamRectangle.Y);
        public static Vector2 CamTopRight => new Vector2(CamRectangle.X + CamRectangle.Width, CamRectangle.Y);
        public static Vector2 CamBotLeft => new Vector2(CamRectangle.X, CamRectangle.Y + CamRectangle.Height);
        public static Vector2 CamBotRight => new Vector2(CamRectangle.X + CamRectangle.Width, CamRectangle.Y + CamRectangle.Height);

        public static Rectangle CamTop => new Rectangle(CamRectangle.X, CamRectangle.Y, CamRectangle.Width, 0);
        public static Rectangle CamBot => new Rectangle(CamRectangle.X, (CamRectangle.Y + ScreenHeight) - 0, CamRectangle.Width, 0);
        public static Rectangle CamLeft => new Rectangle(CamRectangle.X, CamRectangle.Y, 0, ScreenHeight);
        public static Rectangle CamRight => new Rectangle(CamRectangle.X + ScreenWidth - 0, CamRectangle.Y, 0, ScreenHeight);

        static bool isDrawStats = false;
        static bool isDebugging = false;
        public static bool IsDrawStats
        {
            get => isDrawStats;
            set
            {
                isDrawStats = value;
                Obo.GameUtility.OboGlobals.IsDrawStats = isDrawStats;
            }
        }   // F1 toggle
        public static bool IsDebugging
        {
            get => isDebugging;
            set
            {
                isDebugging = value;
                Obo.GameUtility.OboGlobals.IsDebugging = isDebugging;
            }
        }   // F2 toggle

        public static string ProjectName;
    }
}
