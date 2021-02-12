using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmreIlkay
{
    public static class Utility
    {
        public enum GridElementType
        {
            Empty,
            ColorFilled,
            Wall,
            TempFill,
            TrailFill
        }

        public enum Direction
        {
            Nan,
            Up,
            Down,
            Right,
            Left
        }
    }
}
