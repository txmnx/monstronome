using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseCallBack : MonoBehaviour
{
    string musicCueName;

    private void Start()
    {
        AkSoundEngine.PostEvent("Play_Music", gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue | (uint)AkCallbackType.AK_MusicSyncBeat, CallbackFunction, this);
        AkSoundEngine.SetState("Music", "Start");
    }

    public void StopMusic()
    {
        AkSoundEngine.StopAll();
    }


    void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {

        if (in_type == AkCallbackType.AK_MusicSyncUserCue)                                       // Se déclenche a chaque custom cue placé dans manuellement dans la musique sur Wwise.
        {
            AkMusicSyncCallbackInfo musicInfo = in_info as AkMusicSyncCallbackInfo;
            musicCueName = musicInfo.userCueName;
            Debug.Log(musicCueName);                                                               // Permet de déclencher des actions selon le noms du cue placé dans la musique sur Wwise.

            if (in_type == AkCallbackType.AK_MusicSyncBeat)                                                // Permet de déclencher des actions a chaque battements
            {
                Debug.Log("BEAT - WWise");
            }
            
            switch (musicCueName) {
                case "Start":
                    OnStartBlocBegin?.Invoke();
                    break;
            }
        }
    }
    
    public event Action OnStartBlocBegin;
}
