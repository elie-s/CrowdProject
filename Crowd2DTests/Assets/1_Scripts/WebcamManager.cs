using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdProject
{
    public class WebcamManager : MonoBehaviour
    {
        public Texture2D texture;
        [SerializeField] private SpriteRenderer sRenderer = default;
        [SerializeField] private Sprite webcamSprite;
        [SerializeField, Range(0.0f, 1.0f)] private float ceil = 0.35f;
        [SerializeField] private Transform center = default;

        private WebCamTexture webcam;

        private void Awake()
        {
            GetWebcam();
            GetComponent<PolygonCollider2D>().autoTiling = true;
        }

        void Start()
        {

        }

        void Update()
        {
            UpdateTexture();
        }

        private void OnApplicationQuit()
        {
            webcam.Stop();
        }

        private void GetWebcam()
        {
            webcam = new WebCamTexture(WebCamTexture.devices[1].name);
            webcam.requestedFPS = 25;
            webcam.Play();
        }

        private void UpdateTexture()
        {
            Texture2D tmp = new Texture2D(webcam.width, webcam.height);

            tmp.SetPixels(ConvertContrasts(webcam.GetPixels()));
            //TextureScale.Bilinear(tmp, (int)((float)tmp.width / (float)tmp.height * (float)size), size);
            tmp.Apply();

            texture = tmp;
            

            Sprite sprite = Sprite.Create(tmp, new Rect(Vector2.zero, Vector2.one * 200), Vector2.zero, 100.0f, 0,SpriteMeshType.FullRect);

            center.localPosition = Center(sprite.texture);
            sRenderer.sprite = sprite;

        }

        private Color[] ConvertContrasts(Color[] _colors)
        {
            Color[] result = new Color[_colors.Length];
            float[] values = new float[_colors.Length];

            float average = 0.0f;

            for (int i = 0; i < result.Length; i++)
            {
                float value = (_colors[i].r + _colors[i].b + _colors[i].g) / 3.0f;

                average += value;

                values[i] = value;
            }

            average /= result.Length;

            for (int i = 0; i < result.Length; i++)
            {
                float value = values[i] > average - ceil ? 1.0f : 0.0f;

                result[i] = new Color(value, value, value, 1-value);
            }

            return result;
        }

        private Vector2 Center(Texture2D _texture)
        {
            Vector2 result = Vector2.zero;
            Color[] colors = _texture.GetPixels();

            int width = _texture.width;
            int height = _texture.height;
            int amount = 0;

            Debug.Log(width + " x " + height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (colors[y + x * height].a > 0.5f)
                    {
                        result += new Vector2(x * 2 / width, y * 2 / height);
                        amount++;
                    }
                }
            }

            result /= amount;

            return result;
        }
    }
}