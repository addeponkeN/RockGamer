using Microsoft.Xna.Framework;
using RockGamer.Gamer.Screener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer.StateMachine
{
    public class StateManager
    {
        public List<State> states = new List<State>();

        public void AddState(State state)
        {
            state.StateManager = this;
            state.Init();
            states.Add(state);
        }

        public void AddChildState(State parent, State child)
        {
            child.StateManager = this;
            child.Init();
            parent.ChildStates.Add(child);
        }

        public void Clear()
        {
            states.Clear();
        }

        public void Update(GameTime gameTime, GameScreen gs)
        {
            for(int i = 0; i < states.Count; i++)
            {
                states[i].Update(gameTime, gs);
                if(states[i].IsExiting)
                    states.RemoveAt(i);
            }
        }

    }
}

