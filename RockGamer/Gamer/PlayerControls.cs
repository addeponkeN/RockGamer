using Microsoft.Xna.Framework.Input;
using Obo.GameUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer
{
    public class PlayerControls
    {
        List<Keys> Key_Up = new List<Keys> { Keys.Up, Keys.W };
        List<Keys> Key_Down = new List<Keys> { Keys.Down, Keys.S };
        List<Keys> Key_Left = new List<Keys> { Keys.Left, Keys.A };
        List<Keys> Key_Right = new List<Keys> { Keys.Right, Keys.D };
        List<Keys> Key_Jump = new List<Keys> { Keys.LeftAlt, Keys.Space };
        List<Keys> Key_Interact = new List<Keys> { Keys.F, Keys.E };

        public bool MoveUp => Input.KeyHold(Key_Up[0]) || Input.KeyHold(Key_Up[1]);
        public bool MoveDown => Input.KeyHold(Key_Down[0]) || Input.KeyHold(Key_Down[1]);
        public bool MoveLeft => Input.KeyHold(Key_Left[0]) || Input.KeyHold(Key_Left[1]);
        public bool MoveRight => Input.KeyHold(Key_Right[0]) || Input.KeyHold(Key_Right[1]);
        public bool Jump => Input.KeyHold(Key_Jump[0]) || Input.KeyHold(Key_Jump[1]);
        public bool Interact => Input.KeyHold(Key_Interact[0]) || Input.KeyHold(Key_Interact[1]);

    }
}
