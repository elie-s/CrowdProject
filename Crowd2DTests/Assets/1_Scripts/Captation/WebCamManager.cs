using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrowdProject.Analytics;

namespace CrowdProject
{
    public class WebCamManager : MonoBehaviour
    {
        [HideInInspector] public Texture2D texture = default;
        [HideInInspector] public Sprite sprite = default;
        [HideInInspector] public Vector3 direction = default;
        [SerializeField] private RawImage render = default;
        [SerializeField] private float displayMultiplier = 8.0f;
        [SerializeField] private Transform displayCenter = default;
        [SerializeField] private Transform mesh = default;
        [SerializeField] private float centerDistance = 1.0f;
        [SerializeField] private bool rotate = false;
        public WebcamManagerData settings = default;
        [Header("Tests")]
        [SerializeField] private bool test = true;
        [SerializeField] private bool resize = false;
        [SerializeField] private bool desaturation = false;
        [SerializeField] private Frame frame = default;
        [SerializeField] private Rect testRect;
        [Header("Analytics")]
        [SerializeField] private RecordingMode recordingMode = RecordingMode.None;
        [SerializeField] private GameRecordAsset recordAsset = default;
        [Header("Debug Controller")]
        [SerializeField] private bool debugControllerEnabled = false;
        [SerializeField] private float debugControllerSpeed = 5.0f;

        private WebCamTexture webCam;
        private Color[] oldPixels;
        private IEnumerator spriteRoutine;
        private bool nullDetection = false;

        [HideInInspector] public bool foldout;

        private void Awake()
        {
            GetWebcam();
            texture = new Texture2D((int)settings.rect.width, (int)settings.rect.height);
            if(render) render.texture = texture;

            spriteRoutine = SpriteRoutine();
        }

        private void Start()
        {
            StartCoroutine(SpriteRoutine());
        }

        private void Update()
        {
            if(debugControllerEnabled) DebugController();
            //UpdateTexture();
            //UpdateMeshPos();
            //UpdateSprite();
        }

        private void OnApplicationQuit()
        {
            StopCoroutine(spriteRoutine);
            webCam.Stop();
        }

        private IEnumerator SpriteRoutine()
        {
            UpdateSprite();

            yield return new WaitForSeconds(1 / (float)settings.perSecondUpdate);
            StartCoroutine(SpriteRoutine());
        }

        private void UpdateMeshPos()
        {
            mesh.localPosition = -displayCenter.localPosition;
        }

        private void GetWebcam()
        {
            webCam = new WebCamTexture(WebCamTexture.devices[1].name);
            webCam.Play();

            oldPixels = new Color[webCam.width / webCam.height * settings.size * settings.size];
        }

        private void DebugController()
        {
            float x = Input.GetKey(KeyCode.LeftArrow) ? Time.deltaTime * debugControllerSpeed : (Input.GetKey(KeyCode.RightArrow) ? -Time.deltaTime * debugControllerSpeed : 0.0f);
            float y = Input.GetKey(KeyCode.DownArrow) ? Time.deltaTime * debugControllerSpeed : (Input.GetKey(KeyCode.UpArrow) ? -Time.deltaTime * debugControllerSpeed : 0.0f);

            settings.rect.Set(settings.rect.x + x, settings.rect.y + y, settings.rect.width, settings.rect.height);
        }


        private void UpdateTexture()
        {

            render.rectTransform.sizeDelta = new Vector2(settings.size * displayMultiplier, settings.size * displayMultiplier);

            Rect tmpRect = new Rect(webCam.width / 2 - settings.rect.width / 2 + settings.rect.x, webCam.height / 2 - settings.rect.height / 2 + settings.rect.y, settings.rect.width, settings.rect.height);

            texture = new Texture2D((int)tmpRect.width, (int)tmpRect.height);

            texture.SetPixels(webCam.GetPixels((int)tmpRect.x, (int)tmpRect.y, (int)tmpRect.width, (int)tmpRect.height));
            //texture.SetPixels(webCam.GetPixels());
            TextureScale.Point(texture, settings.size, settings.size);
            texture.SetPixels(ConvertContrasts(texture.GetPixels()));
            Vector2 centerPos = Center(ref texture) * centerDistance;
            texture.Apply();

            if(rotate) SpriteGenerator.Rotate180(ref texture);

            render.texture = texture;

            direction = centerPos;

            displayCenter.localPosition = centerPos;
        }

        private void UpdateSprite()
        {
            if (!test)
            {
                if (render) render.rectTransform.sizeDelta = new Vector2(settings.size * displayMultiplier, settings.size * displayMultiplier);

                Rect tmpRect = new Rect(webCam.width / 2 - settings.rect.width / 2 + settings.rect.x, webCam.height / 2 - settings.rect.height / 2 + settings.rect.y, settings.rect.width, settings.rect.height);

                texture = new Texture2D((int)tmpRect.width, (int)tmpRect.height);

                texture.SetPixels(webCam.GetPixels((int)tmpRect.x, (int)tmpRect.y, (int)tmpRect.width, (int)tmpRect.height));
                //texture.SetPixels(webCam.GetPixels());
                TextureScale.Point(texture, settings.size, settings.size);
                texture.SetPixels(ConvertContrasts(texture.GetPixels()));
                texture.Apply();
                if (rotate) SpriteGenerator.Rotate180(ref texture);

                if (!nullDetection)
                {

                    sprite = SpriteGenerator.Generate(texture);
                    sprite.texture.filterMode = FilterMode.Point;
                    sprite.texture.Apply();

                    direction = sprite.PivotDirection();
                }
                else
                {
                    direction = Vector2.zero;
                    Debug.LogWarning("Webcam no longer detects any shape !");
                }
            }
            else
            {
                if (render) render.rectTransform.sizeDelta = new Vector2(webCam.width, webCam.height);
                texture = new Texture2D(webCam.width, webCam.height);
                texture.SetPixels(webCam.GetPixels());
                if (resize)
                {
                    int tmpSize = (int)(settings.size * webCam.width / settings.rect.width);

                    TextureScale.Point(texture, tmpSize, tmpSize * webCam.height / webCam.width);
                    texture.filterMode = FilterMode.Point;
                }
                if (desaturation) texture.SetPixels(ConvertContrasts(texture.GetPixels()));
                texture.Apply();
                if (rotate) SpriteGenerator.Rotate180(ref texture);

                frame.Apply(new Rect(webCam.width / 2 - testRect.width / 2 + testRect.x, webCam.height / 2 - testRect.height / 2 + testRect.y, testRect.width, testRect.height));

                if(nullDetection) Debug.LogWarning("Webcam no longer detects any shape !");
            }
            if (render) render.texture = texture;

            
        }


        private Color[] ConvertContrasts(Color[] _colors)
        {
            nullDetection = true;
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
                float value = values[i] > average- settings.ceil ? 0.0f : 1.0f;


                if (value == oldPixels[i].r)
                {
                    result[i] = new Color(value, value, value, value);
                    if(value > 0.5f) nullDetection = false;
                }
                else
                {
                    result[i] = oldPixels[i];
                    oldPixels[i] = new Color(value, value, value, value);
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

        public void ApplyTest()
        {
            settings.rect = testRect;
        }

    }
}