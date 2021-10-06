using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoxScript : MonoBehaviour
{
    [SerializeField]
    private StageManager stageManager = null;
    [SerializeField]
    private float playTime = 1f;
    private float playTimer = 0f;

    private bool soundPlayed = false;
    void Start()
    {
        stageManager = StageManager.Instance;

        playTimer = playTime;
    }

    private void OnEnable() 
    {
        playTimer = playTime;
    }
    void Update()
    {
        if(playTimer > 0f)
        {
            playTimer -= Time.deltaTime;

            if(!soundPlayed)
            {
                soundPlayed = true;
                GetComponent<AudioSource>().Play();
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
