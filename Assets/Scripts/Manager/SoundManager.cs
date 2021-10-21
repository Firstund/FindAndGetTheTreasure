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

    public void SetMainBGMPitchByLerp(float start, float target, float time)
    {
        mainBGMPitchTimer = time;
        mainBGMPitchTimerOrigin = mainBGMPitchTimer;

        mainBGMPitchStart = start;
        mainBGMPitchTarget = target;
    }

}
