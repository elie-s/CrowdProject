using UnityEngine;

namespace EANS.Flocks
{
    public class TrailHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sRenderer = default;
        [SerializeField] private TrailRenderer tRender = default;
        [SerializeField] private Gradient debug = default;

        void Update()
        {
            SetTrailColor();
        }

        private void SetTrailColor()
        {
            //tRender.colorGradient.colorKeys[0].color = sRenderer.color;
            //tRender.colorGradient.colorKeys[1].color = sRenderer.color;
            debug.SetKeys(new GradientColorKey[2] { new GradientColorKey(sRenderer.color, 0.0f), new GradientColorKey(sRenderer.color, 1.0f) }, new GradientAlphaKey[2] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
            tRender.colorGradient = debug;
        }
    }
}