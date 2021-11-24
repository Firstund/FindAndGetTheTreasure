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
    public AudioSource MainBGM
    {
        get { return mainBGM; }
        set { mainBGM = value; }
    }

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
        gameManager = GameManager.Instance;
    }
    private void Start()
    {
        gameManager.WhenGoToStageSelectMenu += () =>
        {
            mainBGM.clip = stageSelectMenuMainBGM;
            mainBGM.Play();
        };

        // SetMainBGMVolumeByLerp(1, 0, 5);
        SetMainBGMPitch(1f);
    }
    private void Update()
    {
        SetPitch();
        SetVolume();
    }

    private void SetPitch()
    {
        if (mainBGMPitchTimer < mainBGMPitchTimerOrigin)
        {
            mainBGMPitchTimer += Time.deltaTime;

            mainBGM.pitch = Mathf.Lerp(mainBGMPitchStart, mainBGMPitchTarget, mainBGMPitchTimer / mainBGMPitchTimerOrigin);
        }
    }
    private void SetVolume()
    {
        if (mainBGMVolumeTimer < mainBGMVolumeTimerOrigin)
        {
            mainBGMVolumeTimer += Time.deltaTime;

            mainBGM.volume = Mathf.Lerp(mainBGMVolumeStart, mainBGMVolumeTarget, mainBGMVolumeTimer / mainBGMVolumeTimerOrigin);
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
        mainBGMVolumeTimer = 0f;
        mainBGMVolumeTimerOrigin = time;

        mainBGMVolumeStart = start;
        mainBGMVolumeTarget = target;
    }
    public void SetMainBGMVolume(float volume)
    {
        mainBGM.volume = volume;
        mainBGMVolumeTimer = mainBGMVolumeTimerOrigin;
    }
}
