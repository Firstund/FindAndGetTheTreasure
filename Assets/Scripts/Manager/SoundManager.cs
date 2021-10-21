using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if(instance == null)
                {
                    GameObject temp = new GameObject("SoundManager");

                    instance = temp.AddComponent<SoundManager>();
                }
            }

            return instance;
        }
    }
    private DontDestroyOnLoadManager dontDestroyOnLoadManager = null;
    [SerializeField]
    private AudioSource mainBGM = null;

    private float mainBGMPitchTimer = 0f;
    private float mainBGMPitchTimerOrigin = 0f;
    private float mainBGMPitchStart = 0f;
    private float mainBGMPitchTarget = 0f;

    private float mainBGMVolumeTimer = 0f;
    private float mainBGMVolumeTimerOrigin = 0f;
    private float mainBGMVolumeStart = 0f;
    private float mainBGMVolumeTarget = 0f;

    [SerializeField]
    private List<AudioSource> jumpEffectAudis = new List<AudioSource>();
    public List<AudioSource> JumpEffectAudis = new List<AudioSource>();
    private void Awake() 
    {
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;
    }
    private void Start() 
    {
        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        SetPitch();
        SetVolume();
    }

    private void SetPitch()
    {
        if (mainBGMPitchTimer > 0f)
        {
            mainBGMPitchTimer -= Time.deltaTime;

            mainBGM.pitch = Mathf.Lerp(mainBGMPitchStart, mainBGMPitchTarget, mainBGMPitchTimer / mainBGMPitchTimerOrigin);
        }
        else
        {
            mainBGM.pitch = 1f;
        }
    }
    private void SetVolume()
    {
        if(mainBGMVolumeTimer > 0f)
        {
            mainBGMVolumeTimer -= Time.deltaTime;

            mainBGM.volume = Mathf.Lerp(mainBGMVolumeStart, mainBGMVolumeTarget, mainBGMVolumeTimer / mainBGMVolumeTimerOrigin);
        }
        else
        {
            mainBGM.volume = 1f;
        }
    }

    public void SetMainBGMPitchByLerp(float start, float target, float time)
    {
        mainBGMPitchTimer = time;
        mainBGMPitchTimerOrigin = mainBGMPitchTimer;

        mainBGMPitchStart = start;
        mainBGMPitchTarget = target;
    }
    public void SetMainBGMVolumeByLerp(float start, float target, float time)
    {
        mainBGMVolumeTimer = time;
        mainBGMVolumeTimerOrigin = mainBGMVolumeTimer;

        mainBGMVolumeStart = start;
        mainBGMVolumeTarget = target;
    }
    public void SetMainBGMVolume(float volume)
    {
        mainBGM.volume = volume;
    }
}
