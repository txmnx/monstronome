using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CustomMusicInfo : MonoBehaviour
{
    public FreeModeManager freeModeManager;
    public string musicName;
    public float bpm;

    public void OnSelect()
    {
        freeModeManager.OnSelectMusic(musicName, bpm);
    }
}
