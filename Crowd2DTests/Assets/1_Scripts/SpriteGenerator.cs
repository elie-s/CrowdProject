using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public static class SpriteGenerator
    {
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
                    if (colors[x + y * width].r < 0.5f)
                    {
                        result += new Vector2(x, y);
                        amount++;
                    }
                }
            }

            result /= amount;

            result = new Vector2(result.x / width, result.y / height);

            return result;
        }
    }
}