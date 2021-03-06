﻿using System;
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
        parent.GetComponent<AudioSource>().bypassReverbZones = true; //also important, remmoves reverb so no feedback loops
        parent.GetComponent<AudioSource>().spatialBlend = 0.0f;
        parent.GetComponent<AudioSource>().reverbZoneMix = 0.0f;
        
        while (!(Microphone.GetPosition(null) > 0)) { } //this seems to fix the latency issue. i know its bad code, fuck off. 
        
        parent.GetComponent<AudioSource>().Play(); //play the audio source!
        parent.InvokeRepeating("PlayAudioSource", 1.0f, 1.0f);
    }

    public void Update()
    {
        if (((EchoLocation)parent).MicrophonelessMode && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(" yes?");
            loudness = 40.0f;
        }
        else
            loudness = GetAveragedVolume() * sensitivity;        
    }

    float GetAveragedVolume()
    {
        //it should *move* the buffer forward when there is sound, else there is a delay. MAAR HOE DE FOK MOET DAT GVD.
        
        float[] data = new float[256];
        float a = 0;
        
        parent.GetComponent<AudioSource>().GetOutputData(data, 0);                
        
        for (int i = 0; i < 256; i++)
        {           
            a += Mathf.Abs(data[i]);                        
        }
        
        return a / 256; 
    }
}