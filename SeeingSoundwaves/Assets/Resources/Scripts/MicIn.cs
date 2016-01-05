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

        parent.GetComponent<AudioSource>().clip = Microphone.Start(null, true, 1, 48000);

        parent.GetComponent<AudioSource>().loop = false; // Set the AudioClip to loop
        parent.GetComponent<AudioSource>().mute = false; // Mute the sound, we don't want the player to hear it
        
        parent.GetComponent<AudioSource>().bypassEffects = true;
        parent.GetComponent<AudioSource>().bypassReverbZones = true;
        parent.GetComponent<AudioSource>().spatialBlend = 0.0f;
        parent.GetComponent<AudioSource>().reverbZoneMix = 0.0f;

        parent.GetComponent<AudioSource>().Play(); // Play the audio source!
        parent.InvokeRepeating("PlayAudioSource", 1.0f, 1.0f);
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
        
        for (int i = 0; i < 256; i++)
        {
            //a += Mathf.Abs(s); //gewoon som van de hele buffer.            
            a += Mathf.Abs(data[i]);            
            //laten de som van de laatste helft doen

        }
        
        return a / 256; 
    }
}