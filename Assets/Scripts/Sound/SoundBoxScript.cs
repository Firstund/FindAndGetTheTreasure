using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoxScript : MonoBehaviour
{
    private StageManager stageManager = null;
    private SoundManager soundManager = null;

    private AudioSource audioSource = null;

    [SerializeField]
    private float playTime = 1f;
    private float playTimer = 0f;

    private float originVolume = 0f;

    private bool soundPlayed = false;
    void Start()
    {
        stageManager = StageManager.Instance;
        soundManager = SoundManager.Instance;
        
        audioSource = GetComponent<AudioSource>();

        playTimer = playTime;
        originVolume = audioSource.volume;
    }

    private void OnEnable() 
    {
        playTimer = playTime;
    }
    void Update()
    {
        audioSource.volume = originVolume * soundManager.EffectSoundVolume * soundManager.MasterVolume;

        if(playTimer > 0f)
        {
            playTimer -= Time.deltaTime;

            if(!soundPlayed)
            {
                soundPlayed = true;
                audioSource.Play();
            }
        }
        else
        {
            playTimer = playTime;
            soundPlayed = false;
            stageManager.DesapwnSoundBox(gameObject);
        }
    }
}
