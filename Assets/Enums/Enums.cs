using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Enums
{
    public enum CharactorStatus
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Jump = 3,
        Crouch = 4,
        Hit = 5,
        Behit = 6,
        Dead = 7
    }

    [Flags]
    public enum InputActions
    {
        None = 0,
        MoveLeft = 1,
        MoveRight = 2,
        MoveUp = 4,
        MoveDown = 8,
        Jump = 16,
        Crouch = 32,
        Hit = 64
    }

    public enum MovingTowards
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
}
