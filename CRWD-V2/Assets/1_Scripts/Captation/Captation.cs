using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRWD_V2.Captation
{
    public struct Captation
    {
        public readonly Vector2 direction;
        public readonly RectInt rect;
        public readonly float weight;
        public readonly int mass;
        public readonly Sprite sprite;

        public Captation(Vector2 _direction, RectInt _rect, float _weight, int _mass, Sprite _sprite = null)
        {
            direction = _direction;
            rect = _rect;
            weight = _weight;
            mass = _mass;
            sprite = _sprite;
        }
    }
}