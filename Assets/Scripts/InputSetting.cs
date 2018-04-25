using Assets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class InputSetting
    {

        public static Dictionary<InputActions, KeyCode> Default
        {
            get
            {
                Dictionary<InputActions, KeyCode> actions = new Dictionary<InputActions, KeyCode>();
                actions.Add(InputActions.MoveLeft, KeyCode.A);
                actions.Add(InputActions.MoveRight, KeyCode.D);
                actions.Add(InputActions.Crouch, KeyCode.S);
                actions.Add(InputActions.Jump, KeyCode.W);
                actions.Add(InputActions.Hit, KeyCode.J);
                return actions;
            }
        }



    }
}
