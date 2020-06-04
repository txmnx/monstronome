using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRTPC : MonoBehaviour
{
    public Slider VolumeSliderMaster;
    public Slider VolumeSliderSFXVoice;
    public Slider VolumeSliderMetronome;
    public Slider VolumeSliderMusic;
    public float VolumeMusic;
    public float VolumeSFXvoice;
    public float VolumeMaster;
    public float VolumeMetronome;


    public void Setvalue()
    {
            VolumeMaster = VolumeSliderMaster.value;
            AkSoundEngine.SetRTPCValue("RTPC_SetVolume_Master", VolumeMaster);               // Volume Du master
        
            VolumeMetronome = VolumeSliderMetronome.value;
            AkSoundEngine.SetRTPCValue("RTPC_SetVolume_Metronome", VolumeMetronome);         // Volume Du metronome          
       
            VolumeSFXvoice = VolumeSliderSFXVoice.value;
            AkSoundEngine.SetRTPCValue("RTPC_SetVolume_VoiceSFX", VolumeSFXvoice);           // Volume Des SFX et voix
                                                              
            VolumeMusic = VolumeSliderMusic.value;
            AkSoundEngine.SetRTPCValue("RTPC_SetVolume_Music", VolumeMusic);                   // Volume De la music      
    }
}
