using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMainBGM : MonoBehaviour, IEventTrigger
{
    private SoundManager soundManager = null;

    [SerializeField]
    private AudioClip changeToIt = null;

    private void Awake() 
    {
        soundManager = SoundManager.Instance;    
    }
    public void DoEvent()
    {
        soundManager.ChangeMainBGM(changeToIt);
    }
}
