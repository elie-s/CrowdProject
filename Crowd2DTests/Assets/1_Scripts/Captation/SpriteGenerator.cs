using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public static class SpriteGenerator
    {
        public static int capted { get; private set; }
        public static float ratio { get; private set; }

        public static Sprite Generate(Texture2D _texture)
        {
            Vector2 pivot = Pivot(_texture);

            return Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), pivot, 100, 0, SpriteMeshType.FullRect, Vector4.one, true);
        }

        private static Vector2 Pivot(Texture2D _texture)
        {
            Vector2 result = Vector2.zero;
            Color[] colors = _texture.GetPixels();

            int width = _texture.width;
            int height = _texture.height;
            int amount = 0;

            //Debug.Log(width + " x " + height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (colors[x + y * width].a > 0.5f)
                    {
                        result += new Vector2(x, y);
                        amount++;
                    }
                }
            }

            
            capted = amount;
            ratio = (float)amount / (float)(width * height);
            //Debug.Log(ratio);

            result /= amount;

            result = new Vector2(result.x / width, result.y / height);

            return result;
        }
        
        public static Vector2 PivotDirection(this Sprite _sprite)
        {
            Vector2 pivot = _sprite.pivot;
            Rect rect = _sprite.rect;
            float average = (Mathf.Abs((pivot.x / rect.width * 2 - 1)) + Mathf.Abs(pivot.y / rect.height * 2 - 1));

            return new Vector2(pivot.x * 2 - rect.width, pivot.y * 2 - rect.height).normalized * average;
        }

        public static void Rotate180(ref Texture2D _texture)
        {
            for (int x = 0; x < _texture.width/2; x++)
            {
                for (int y = 0; y < _texture.height; y++)
                {
                    Color a = _texture.GetPixel(x, y);
                    Color b = _texture.GetPixel(_texture.width - x, _texture.height - y);

                    _texture.SetPixel(x, y, b);
                    _texture.SetPixel(_texture.width - x, _texture.height - y, a);
                }
            }

            _texture.Apply();
        }
    }
}