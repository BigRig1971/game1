using UnityEngine;
using System.Collections;

//[ExecuteAlways]
namespace StupidHumanGames
{
    public class LightingManager : MonoBehaviour
    {
        
        public enum State { Day, Night };
        [SerializeField] State _currentState;
      
        [SerializeField] private Light DirectionalLight;
        [SerializeField] private LightingPreset Preset;
        //Variables
        [SerializeField, Range(0, 24)] private float TimeOfDay;
        [System.Serializable]
        public class DayAmbientSounds
        {
            public AudioSource _dayAmbientAudio;
            public float _dayVolume = .3f;
        }
        [Range(1, 100)] public int DayTimeMultiplier = 1;
        [Range(1, 100)] public int NightTimeMultiplier = 1;
        int TimeMultiplier = 1;
        public DayAmbientSounds[] _dayAmbience;
        public float DayStartTime = 7f;
        [System.Serializable]
        public class NightAmbientSounds
        {
            public AudioSource _nightAmbientAudio;
            public float _nightVolume = .3f;
        }
        public NightAmbientSounds[] _nightAmbience;
        public float NightStartTime = 18f;
        bool canPlaySound = true;
        bool Daytime = true;
        float test;
        bool startTheTime = false;
 
        private void Start()
        {
            PlayAllSounds();
            StartCoroutine(CurrentState());
        }
        private void Update()
        {
            if (Preset == null)
                return;
            if (!startTheTime) return;
            if (Application.isPlaying)
            {
                //(Replace with a reference to the game time)
                TimeOfDay += Time.deltaTime / 100 * TimeMultiplier;
                TimeOfDay %= 24; //Modulus to ensure always between 0-24
                UpdateLighting(TimeOfDay / 24f);
            }
            else
            {
                UpdateLighting(TimeOfDay / 24f);
            }
            if (Daytime && TimeOfDay > NightStartTime && TimeOfDay < NightStartTime + 1f) Daytime = false;
            if (!Daytime && TimeOfDay > DayStartTime && TimeOfDay < DayStartTime + 1f) Daytime = true;
			DayOrNight();
		}
        void DayOrNight()
        {
			if (TimeOfDay > DayStartTime && TimeOfDay < NightStartTime)
			{
                Daytime = true;
			}
			else
			{
                Daytime = false;
			}
		}
        private void UpdateLighting(float timePercent)
        {
            //Set ambient and fog
            RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
            RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

            //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
            if (DirectionalLight != null)
            {
                DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            }
        }

        //Try to find a directional light to use if we haven't set one
        private void OnValidate()
        {
            if (DirectionalLight != null)
                return;

            //Search for lighting tab sun
            if (RenderSettings.sun != null)
            {
                DirectionalLight = RenderSettings.sun;
            }
            //Search scene for light that fits criteria (directional)
            else
            {
                Light[] lights = GameObject.FindObjectsOfType<Light>();
                foreach (Light light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        DirectionalLight = light;
                        return;
                    }
                }
            }
        }
        public void PlayAllSounds()
        {
            foreach (var sound in _dayAmbience)
            {
                sound._dayAmbientAudio?.Play();
                sound._dayAmbientAudio.volume = sound._dayVolume;
            }
            foreach (var sound in _nightAmbience)
            {
                sound._nightAmbientAudio.Play();
                sound._nightAmbientAudio.volume = sound._nightVolume;
            }
            PauseDaySounds();
            PauseNightSounds();    
        }
      
        void PlayDaySounds()
        {
            foreach (var sound in _dayAmbience)
            {
                sound._dayAmbientAudio?.UnPause();
                sound._dayAmbientAudio.volume = sound._dayVolume;
            }
        }
        void PlayNightSounds()
        {
            foreach (var sound in _nightAmbience)
            {
                sound._nightAmbientAudio.UnPause();
                sound._nightAmbientAudio.volume = sound._nightVolume;
            }
        }
        void PauseDaySounds()
        {
            foreach (var sound in _dayAmbience)
            {
                sound._dayAmbientAudio?.Pause();
            }
        }
        void PauseNightSounds()
        {
            foreach (var sound in _nightAmbience)
            {
                sound._nightAmbientAudio?.Pause();
            }
        }
        IEnumerator Night()
        {
            TimeMultiplier = NightTimeMultiplier;
            PauseDaySounds();
            PlayNightSounds();
            while (!Daytime)
            {
                yield return null;
            }
            _currentState = State.Day;
        }
        IEnumerator Day()
        {
            TimeMultiplier = DayTimeMultiplier;
            PauseNightSounds();
            PlayDaySounds();
            while (Daytime)
            {
                yield return null;
            }
            _currentState = State.Night;
        }
        IEnumerator CurrentState()
        {
            yield return new WaitForSeconds(1f);
            TimeOfDay = (DirectionalLight.transform.localRotation.eulerAngles.x + 90) / 360 * 24;
            startTheTime = true;
            while (true)
            {
                yield return StartCoroutine(_currentState.ToString());
            }
        }
    }
}