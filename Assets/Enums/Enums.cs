using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Enums
{
    public enum CharactorStatus
    {
        Idle=0,
        Walk=1,
        Run=2,
        Jump=3,
        Crouch=4,
        Hit=5,
        Behit=6,
        Dead=7
    }

    public enum InputActions
    {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Jump,
        Crouch,
        Hit
    }
}
