using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumePannel : MonoBehaviour
{
    private SoundManager soundManager = null;

    [SerializeField]
    private Slider masterVolumeSlider = null;
    [SerializeField]
    private Slider mainVolumeSlider = null;
    [SerializeField]
    private Slider effectVolumeSlider = null;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }
    private void Start() 
    {
        masterVolumeSlider.value = soundManager.MasterVolume;
        mainVolumeSlider.value = soundManager.MainVolume;
        effectVolumeSlider.value = soundManager.EffectSoundVolume;
    }
    public void SetMasterVolumeBySlider()
    {
        SoundManager.Instance.SetMasterVolumeBySlider(masterVolumeSlider);
    }
    public void SetMainBGMVolumeBySlider()
    {
        SoundManager.Instance.SetMainVolumeBySlider(mainVolumeSlider);
    }
    public void SetEffectSoundVolumeBySlider()
    {
        SoundManager.Instance.SetEffectVolumeBySlider(effectVolumeSlider);
    }
}
