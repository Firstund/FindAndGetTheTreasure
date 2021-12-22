using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject temp = new GameObject("SoundManager");

                    instance = temp.AddComponent<SoundManager>();
                }
            }

            return instance;
        }
    }
    private GameManager gameManager = null;

    [SerializeField]
    private AudioClip stageSelectMenuMainBGM = null;

    [SerializeField]
    private AudioSource mainBGM = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float masterVolume = 1f;
    public float MasterVolume
    {
        get { return masterVolume; }
    }

    private float mainBGMPitchTimer = 0f;
    private float mainBGMPitchTimerOrigin = 0f;
    private float mainBGMPitchStart = 0f;
    private float mainBGMPitchTarget = 0f;

    [Range(0f, 1f)]
    [SerializeField]
    private float mainVolume = 0f;
    public float MainVolume
    {
        get { return mainVolume; }
    }

    private float mainBGMVolumeTimer = 0f;
    private float mainBGMVolumeTimerOrigin = 0f;
    private float mainBGMVolumeStart = 0f;
    private float mainBGMVolumeTarget = 0f;

    [Range(0f, 1f)]
    [SerializeField]
    private float effectSoundVolume = 0f;
    public float EffectSoundVolume
    {
        get { return effectSoundVolume; }
    }

    [SerializeField]
    private List<AudioSource> jumpEffectAudis = new List<AudioSource>();
    public List<AudioSource> JumpEffectAudis = new List<AudioSource>();

    private bool bSetVolumeByLerp = false;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    private void Start()
    {
        gameManager.WhenGoToStageSelectMenu += () =>
        {
            WhenGoToStageSelectMenu();
        };

        // SetMainBGMVolumeByLerp(1, 0, 5);
        SetMainBGMPitch(1f);
    }
    private void OnDisable()
    {
        gameManager.WhenGoToStageSelectMenu -= () =>
        {
            WhenGoToStageSelectMenu();
        };
    }
    private void Update()
    {
        SetPitch();

        if (bSetVolumeByLerp)
        {
            SetVolume();
        }
        else
        {
            mainBGM.volume = mainVolume * masterVolume;
        }
    }
    private void WhenGoToStageSelectMenu()
    {
        if (mainBGM != null && stageSelectMenuMainBGM != null)
        {
            mainBGM.clip = stageSelectMenuMainBGM;
            mainBGM.Play();
        }
    }
    public void ChangeMainBGM(AudioClip changeToIt)
    {
        mainBGM.clip = changeToIt;
        mainBGM.Play();
    }

    private void SetPitch()
    {
        if (mainBGMPitchTimer < mainBGMPitchTimerOrigin)
        {
            mainBGMPitchTimer += Time.deltaTime;

            mainBGM.pitch = Mathf.Lerp(mainBGMPitchStart, mainBGMPitchTarget, mainBGMPitchTimer / mainBGMPitchTimerOrigin);

            if (mainBGMPitchTimer >= mainBGMPitchTimerOrigin)
            {
                mainBGMPitchTimer = mainBGMPitchTimerOrigin;
            }
        }
    }
    private void SetVolume()
    {
        if (mainBGMVolumeTimer < mainBGMVolumeTimerOrigin)
        {
            mainBGMVolumeTimer += Time.deltaTime;

            mainVolume = Mathf.Lerp(mainBGMVolumeStart, mainBGMVolumeTarget, mainBGMVolumeTimer / mainBGMVolumeTimerOrigin) * masterVolume;

            if (mainBGMVolumeTimer >= mainBGMVolumeTimerOrigin)
            {
                bSetVolumeByLerp = false;
                mainBGMVolumeTimer = mainBGMVolumeTimerOrigin;
            }
        }
    }

    public void SetMainBGMPitchByLerp(float start, float target, float time)
    {
        mainBGMPitchTimer = 0f;
        mainBGMPitchTimerOrigin = time;

        mainBGMPitchStart = start;
        mainBGMPitchTarget = target;
    }
    public void SetMainBGMPitch(float pitch)
    {
        mainBGM.pitch = pitch;
        mainBGMPitchTimer = mainBGMPitchTimerOrigin;
    }
    public void SetMainBGMVolumeByLerp(float start, float target, float time)
    {
        bSetVolumeByLerp = true;

        mainBGMVolumeTimer = 0f;
        mainBGMVolumeTimerOrigin = time;

        mainBGMVolumeStart = start;
        mainBGMVolumeTarget = target;
    }
    public void SetMasterVolume(float volume)
    {
        bSetVolumeByLerp = false;

        masterVolume = volume;
        SetMainBGMVolume(mainVolume);
    }
    public void SetMasterVolumeBySlider(Slider slider)
    {
        bSetVolumeByLerp = false;

        masterVolume = slider.value;
        SetMainBGMVolume(mainVolume);
    }
    public void SetMainBGMVolume(float volume)
    {
        bSetVolumeByLerp = false;

        mainVolume = volume;
        mainBGMVolumeTimer = mainBGMVolumeTimerOrigin;
    }
    public void SetMainVolumeBySlider(Slider slider)
    {
        bSetVolumeByLerp = false;

        mainVolume = slider.value;
    }
    public void SetEffectSoundVolume(float volume)
    {
        effectSoundVolume = volume;
    }
    public void SetEffectVolumeBySlider(Slider slider)
    {
        effectSoundVolume = slider.value;
    }
}
