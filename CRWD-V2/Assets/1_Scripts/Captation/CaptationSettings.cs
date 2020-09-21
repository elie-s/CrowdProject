using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRWD_V2.Captation
{
    public struct CaptationSettings
    {
        public readonly float ceil;
        public readonly int scale;
        public readonly RectInt rect;

        public CaptationSettings(float _ceil, int _scale, RectInt _rect)
        {
            ceil = _ceil;
            scale = _scale;
            rect = _rect;
        }
    }
}