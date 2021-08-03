using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoxScript : MonoBehaviour
{
    [SerializeField]
    private float playTime = 1f;
    private float playTimer = 0f;
    void Start()
    {
        playTimer = playTime;
    }
    void Update()
    {
        if(playTime > 1f)
        {
            playTime -= Time.deltaTime;
        }
        else
        {
            playTimer = playTime;
            gameObject.SetActive(false);
        }
    }


}
