using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MicIn
{
    MonoBehaviour parent;
    public float loudness;
    public float sensitivity;

    AudioClip Clip;

    public MicIn(MonoBehaviour _parent, float _sensitivity)
    {
        sensitivity = _sensitivity;
        parent = _parent;
        loudness = 0;
        
        parent.GetComponent<AudioSource>().clip = Microphone.Start(null, true, 2, 44100);
        parent.GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
        parent.GetComponent<AudioSource>().mute = false; // Mute the sound, we don't want the player to hear it
        
        parent.GetComponent<AudioSource>().bypassEffects = true;
        parent.GetComponent<AudioSource>().bypassReverbZones = true;
        parent.GetComponent<AudioSource>().spatialBlend = 0.0f;
        parent.GetComponent<AudioSource>().reverbZoneMix = 0.0f;

        parent.GetComponent<AudioSource>().Play(); // Play the audio source!
    }

    public void Update()
    {        
        loudness = GetAveragedVolume() * sensitivity;
    }

    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        
        parent.GetComponent<AudioSource>().GetOutputData(data, 0);
        
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        
        return a / 256;
    }
}