// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using Boxophobic.StyledGUI;

namespace TheVegetationEngine
{
    [ExecuteInEditMode]
    [AddComponentMenu("BOXOPHOBIC/The Vegetation Engine/TVE Global Motion")]
    public class TVEGlobalMotion : StyledMonoBehaviour
    {
        [StyledBanner(0.890f, 0.745f, 0.309f, "Global Motion", "", "https://docs.google.com/document/d/145JOVlJ1tE-WODW45YoJ6Ixg23mFc56EnB_8Tbwloz8/edit#heading=h.czf8ud5bmaq2")]
        public bool styledBanner;

        [StyledCategory("Wind Settings", 5, 10)]
        public bool windCat;

        [Tooltip("Controls the global wind power.")]
        [StyledRangeOptions("Wind Power", 0, 1,  new string[] { "Min", "Medium", "Max" })]
        public float windPower = 0.5f;

        [StyledCategory("Noise Settings")]
        public bool noiseCat;

        [StyledMessage("Info", "The noise settings cannot be animated in realtime and smooth direction changes are not supported! Direction elements can be used to change the wind direction locally.", 0, 10)]
        public bool styledMessage = true;

        //[Tooltip("Sets the texture used for wind gust and motion highlight.")]
        //public Texture2D noiseTexture;
        [Tooltip("Controls the scale of the motion noise.")]
        [Range(0, 4)]
        public float noiseScale = 1;
        [Tooltip("Controls the speed of the motion noise.")]
        [Range(0, 4)]
        public float noiseSpeed = 1;

        void Start()
        {

#if UNITY_EDITOR
            gameObject.GetComponent<MeshRenderer>().hideFlags = HideFlags.HideInInspector;
            gameObject.GetComponent<MeshFilter>().hideFlags = HideFlags.HideInInspector;
#endif

            // Disable Arrow in play mode
            if (Application.isPlaying == true)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }

            gameObject.name = "Global Motion";
            gameObject.transform.SetSiblingIndex(0);

            //if (noiseTexture == null)
            //{
            //    noiseTexture = Resources.Load<Texture2D>("Internal NoiseTex");
            //}

            SetGlobalShaderProperties();
        }

        void Update()
        {
            gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);

            SetGlobalShaderProperties();
        }

        void SetGlobalShaderProperties()
        {
            var windDirection = transform.forward;

            //float windPacked = (Mathf.Atan2(windDirection.x, windDirection.z) / Mathf.PI) * 0.5f + 0.5f;            
            //Vector3 decode = new Vector3( Mathf.Sin( (windPacked * 2 - 1) * Mathf.PI) , 0, Mathf.Cos((windPacked * 2 - 1) * Mathf.PI));
            //Debug.Log(windPacked + "   " + decode);

            Shader.SetGlobalVector("TVE_MotionParams", new Vector4(windDirection.x * 0.5f + 0.5f, windDirection.z * 0.5f + 0.5f, windPower, 0.0f));
            //Shader.SetGlobalTexture("TVE_NoiseTex", noiseTexture);
            Shader.SetGlobalVector("TVE_NoiseParams", new Vector4(noiseSpeed, noiseScale, 2, 0.4f));
        }
    }
}
