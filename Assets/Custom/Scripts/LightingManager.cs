using UnityEngine;
using System.Collections;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    public enum State { Day, Night };
    [SerializeField] State _currentState;
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [System.Serializable]
    public class DayAmbientSounds
    {
        public AudioSource _dayAmbientAudio;
        [Range(0f, 1f)] public float _dayVolume;
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
        [Range(0f, 1f)] public float _nightVolume;
    }
    public NightAmbientSounds[] _nightAmbience;
    public float NightStartTime = 18f;
    bool canPlaySound = true;
    bool Daytime = true;
    private void Awake()
    {
       // _currentState = State.Day;
    }
    private void Start()
    {
       
        PlayAllSounds();
        PauseDaySounds();
        PauseNightSounds();
        StartCoroutine(CurrentState());
    }
    private void Update()
    {
        if (Preset == null)
            return;
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
    }
    public void CanStartSounds()
    {
        canPlaySound = true;

    }
    public void CanStopSounds()
    {
        canPlaySound = false;

    }
    void UnPauseDaySounds()
    {
        
        foreach (var sound in _dayAmbience)
        {
            sound._dayAmbientAudio?.UnPause();
            sound._dayAmbientAudio.volume = sound._dayVolume;
        }    
    }
    void UnPauseNightSounds()
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
        if (canPlaySound)
        {
            UnPauseNightSounds();           
        }       
        while (!Daytime && canPlaySound)
        {
            yield return null;
        }       
        _currentState = State.Day;
    }
    IEnumerator Day()
    {
        TimeMultiplier = DayTimeMultiplier;
        PauseNightSounds();
        if (canPlaySound)
        {
            UnPauseDaySounds();                     
        }
        while (Daytime && canPlaySound)
        {
            yield return null;
        }             
        _currentState = State.Night;
    }
    IEnumerator CurrentState()
    {
        while (true)
        {
            yield return StartCoroutine(_currentState.ToString());
        }
    }
}