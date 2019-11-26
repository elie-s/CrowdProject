using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CrowdProject
{
    public class WebCamManager : MonoBehaviour
    {
        [HideInInspector] public Texture2D texture;
        [HideInInspector] public Sprite sprite;
        [HideInInspector] public Vector3 direction;
        [SerializeField] private RawImage render = default;
        [SerializeField, Range(0.0f, 1.0f)] private float ceil = 0.35f;
        [SerializeField] private int size = 32;
        [SerializeField] private float displayMultiplier = 8.0f;
        [SerializeField] private Rect rect;
        [SerializeField] private Transform displayCenter = default;
        [SerializeField] private Transform mesh = default;
        [SerializeField] private float centerDistance = 1.0f;
        [Header("Compute Shader")]
        [SerializeField] private ComputeShader webcamProcessing = default;
        [SerializeField] private Vector3Int threadGroups = new Vector3Int(8, 8, 1);

        private WebCamTexture webCam;
        private Color[] oldPixels;

        private void Awake()
        {
            GetWebcam();
            texture = new Texture2D((int)rect.width, (int)rect.height);
            if(render) render.texture = texture;
        }

        private void Update()
        {
            //UpdateTexture();
            //UpdateMeshPos();
            UpdateSprite();
        }

        private void OnApplicationQuit()
        {
            webCam.Stop();
        }

        private void UpdateMeshPos()
        {
            mesh.localPosition = -displayCenter.localPosition;
        }

        //private void SetComputeShader()
        //{
        //    int kernelIndex = webcamProcessing.FindKernel("CSMain");

        //    render.rectTransform.sizeDelta = new Vector2(size * displayMultiplier, size * displayMultiplier);

        //    Rect tmpRect = new Rect(webCam.width / 2 - rect.width / 2 + rect.x, webCam.height / 2 - rect.height / 2 + rect.y, rect.width, rect.height);

        //    texture = new Texture2D((int)tmpRect.width, (int)tmpRect.height);

        //    texture.SetPixels(webCam.GetPixels((int)tmpRect.x, (int)tmpRect.y, (int)tmpRect.width, (int)tmpRect.height));

        //    RenderTexture tmp = new RenderTexture(texture.width/2, texture.height/2, 0, RenderTextureFormat.ARGB32);
        //    tmp.
        //    RenderTexture.active = tmp;
        //    Graphics.Blit(texture, tmp);
        //    tmp.enableRandomWrite = true;


        //    threadGroups = new Vector3Int(tmp.width / 8, tmp.height / 8, 1);

        //    webcamProcessing.SetTexture(kernelIndex, "Result", tmp);
        //    webcamProcessing.Dispatch(kernelIndex, threadGroups.x, threadGroups.y, threadGroups.z);

        //}

        private void GetWebcam()
        {
            webCam = new WebCamTexture(WebCamTexture.devices[1].name);
            webCam.Play();

            oldPixels = new Color[webCam.width / webCam.height * size * size];
        }


        private void UpdateTexture()
        {

            render.rectTransform.sizeDelta = new Vector2(size* displayMultiplier,size* displayMultiplier);

            Rect tmpRect = new Rect(webCam.width / 2 - rect.width / 2 + rect.x, webCam.height / 2 - rect.height / 2 + rect.y, rect.width, rect.height);

            texture = new Texture2D((int)tmpRect.width, (int)tmpRect.height);

            texture.SetPixels(webCam.GetPixels((int)tmpRect.x, (int)tmpRect.y, (int)tmpRect.width, (int)tmpRect.height));
            //texture.SetPixels(webCam.GetPixels());
            TextureScale.Point(texture, size, size);
            texture.SetPixels(ConvertContrasts(texture.GetPixels()));
            Vector2 centerPos = Center(ref texture) * centerDistance;
            texture.Apply();

            render.texture = texture;

            direction = centerPos;

            displayCenter.localPosition = centerPos;
        }

        private void UpdateSprite()
        {
            if (render) render.rectTransform.sizeDelta = new Vector2(size * displayMultiplier, size * displayMultiplier);

            Rect tmpRect = new Rect(webCam.width / 2 - rect.width / 2 + rect.x, webCam.height / 2 - rect.height / 2 + rect.y, rect.width, rect.height);

            texture = new Texture2D((int)tmpRect.width, (int)tmpRect.height);

            texture.SetPixels(webCam.GetPixels((int)tmpRect.x, (int)tmpRect.y, (int)tmpRect.width, (int)tmpRect.height));
            //texture.SetPixels(webCam.GetPixels());
            TextureScale.Point(texture, size, size);
            texture.SetPixels(ConvertContrasts(texture.GetPixels()));
            texture.Apply();

            sprite = SpriteGenerator.Generate(texture);
            sprite.texture.filterMode = FilterMode.Point;
            sprite.texture.Apply();
            if (render) render.texture = texture;
        }


        private Color[] ConvertContrasts(Color[] _colors)
        {
            Color[] result = new Color[_colors.Length];
            float[] values = new float[_colors.Length];

            if (oldPixels.Length != result.Length) oldPixels = new Color[result.Length];

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
                float value = values[i] > average-ceil ? 0.0f : 1.0f;


                if (value == oldPixels[i].r)
                {
                    result[i] = new Color(value, value, value, value);
                }
                else
                {
                    result[i] = oldPixels[i];
                    oldPixels[i] = new Color(value, value, value);
                }
            }

            return result;
        }

        private Vector2 Center(ref Texture2D _texture)
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
                        result += new Vector2((x - width / 2.0f) / width, (y - height / 2.0f) / height);
                        amount++;
                    }
                }
            }

            _texture.SetPixel((int)result.x + width/2, (int)result.y + height/2, Color.red);
            result /= amount;

            return result;
        }

    }
}