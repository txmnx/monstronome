using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class WwiseCallBack : MonoBehaviour
{
    public SoundEngineTuner soundEngineTuner;
    private string m_MusicCueName;
    public AK.Wwise.Event musicToLaunch;

    public void StopMusic()
    {
        AkSoundEngine.StopAll();
    }

    public void LoadTuning()
    {
        AkSoundEngine.PostEvent("Music_Tuning", gameObject);
    }

    public void StartOrchestra()
    {
        AkSoundEngine.SetState("Music", "Start");
    }

    public void StopOrchestra()
    {
        AkSoundEngine.SetState("Music", "None");
    }
    
    public void LoadOrchestra()
    {
        musicToLaunch.Post (gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue | (uint)AkCallbackType.AK_MusicSyncBeat, CallbackFunction, this);
        AkSoundEngine.SetState("Music", "Metronome");
        AkSoundEngine.SetState("PotionCount", "Left_0");   // Nombre de potions restantes que le joueur doit lancer pour corriger la famille
        AkSoundEngine.SetSwitch("SW_Family_Solist", "Nobody", gameObject);  //Famille soliste qui devra être recaller
        soundEngineTuner.SetTempo(SoundEngineTuner.START_TEMPO);
    }

    void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {

        if (in_type == AkCallbackType.AK_MusicSyncUserCue)                                       // Se déclenche a chaque custom cue placé dans manuellement dans la musique sur Wwise.
        {
            AkMusicSyncCallbackInfo musicInfo = in_info as AkMusicSyncCallbackInfo;
            m_MusicCueName = musicInfo.userCueName;
            Debug.Log(m_MusicCueName);                                                               // Permet de déclencher des actions selon le noms du cue placé dans la musique sur Wwise.

            if (in_type == AkCallbackType.AK_MusicSyncBeat)                                                // Permet de déclencher des actions a chaque battements
            {
                Debug.Log("BEAT - WWise");
            }
            
            OnCue?.Invoke(m_MusicCueName);
        }
    }

    public event Action<string> OnCue;
}
 