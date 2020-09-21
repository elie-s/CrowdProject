using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRWD_V2.Captation
{
    public static class ImageProcessing
    {
        public static WebCamTexture webcam { get; private set; }
        public static CaptationSettings[] captationSets { get; private set; }

        public static void SetWebcam(int _index)
        {
            webcam = new WebCamTexture(WebCamTexture.devices[_index].name);
            webcam.Play();
        }

        public static Captation Get(int _index, bool _setSprite = false)
        {
            Vector2 direction = default;
            float weight = default;
            int mass = default;
            Sprite sprite = null;


            CaptationSettings set = captationSets[_index];
            Texture2D texture = new Texture2D(set.rect.width, set.rect.height);

            return new Captation(direction, set.rect, weight, mass, sprite)
        }
    }
}