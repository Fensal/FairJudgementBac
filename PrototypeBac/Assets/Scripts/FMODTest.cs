using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FMOD.Studio;


public class FMODTest : MonoBehaviour
{
    EventInstance fmodevent;

    void Start()
    {
        fmodevent = FMOD_StudioSystem.instance.GetEvent("event:/music/music");

        if (fmodevent != null)
        {
            fmodevent.setVolume(1.0f);

            
            fmodevent.setParameterValue("music_trans", 0.0f);

            fmodevent.start();
        }
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 400, 0, 200, 20), "Outdoors"))
        {
            fmodevent.setParameterValue("music_trans", 0.0f);
        }
        if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 20), "Chapel"))
        {
            fmodevent.setParameterValue("music_trans", 1.0f);
        }
        if (GUI.Button(new Rect(Screen.width - 400, 20, 200, 20), "Combat"))
        {
            fmodevent.setParameterValue("music_trans", 2.0f);
        }
        if (GUI.Button(new Rect(Screen.width - 200, 20, 200, 20), "CombatEnd"))
        {
            fmodevent.setParameterValue("music_trans", 3.0f);
        }
    }

    public IEnumerator Fade(string param1, string param2)
    {
        return null;
    }
}
