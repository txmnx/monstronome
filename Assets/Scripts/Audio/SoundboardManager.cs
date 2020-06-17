using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundboardManager : MonoBehaviour
{
    public SoundSlider masterSlider;
    public SoundSlider sfxSlider;
    public SoundSlider musicSlider;
    public SoundSlider metronomeSlider;


    public (float, float, float, float) GetSliders()
    {
        return (masterSlider.value, sfxSlider.value, musicSlider.value, metronomeSlider.value);
    }
    
    public void SetSliders(float master, float sfx, float music, float metronome)
    {
        masterSlider.value = master;
        sfxSlider.value = sfx;
        musicSlider.value = music;
        metronomeSlider.value = metronome;
    }
}
