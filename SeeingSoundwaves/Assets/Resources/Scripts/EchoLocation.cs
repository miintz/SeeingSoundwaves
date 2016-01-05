using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class EchoLocation : MonoBehaviour
{
    private MicIn Microfoon;

    public bool EnableEchoLocation = true;
    public bool FadeOut = true;
    public float FadeSpeed = 0.1f;
    
    public float MicrophoneSensitivity = 100.0f;
    public float MicrophoneLowLimit = 10.0f;
    public float MicrophoneHighLimit = 25.0f;
    public float MaxShaderStrength = 100.0f;

    private Quaternion CameraRotation;
    private Quaternion CharacterRotation;
    private Vector3 CameraPosition;

    private FirstPersonControllerMod Character;
    private Boolean fading = false;
    List<GameObject> GameObjects;

    // Use this for initialization
    void Start()
    {
        if (EnableEchoLocation)
        {
            Character = GetComponent("FirstPersonControllerMod") as FirstPersonControllerMod;
            Character.m_Block = true; //singleton!!! handig
            GameObjects = getObjectsByMaterialName("distanceLerp");
            
            fading = true;

            Microfoon = new MicIn(this, MicrophoneSensitivity);
            
           
        }
    }  

    // Update is called once per frame
    void Update() {        
        if (EnableEchoLocation)
        {
            //first update the microphone
            Microfoon.Update();
            
            CameraPosition = Character.GetCameraMoveDirection();
            CameraRotation = Character.GetCameraRotation();
            CharacterRotation = Character.GetCharacterRotation();

            transform.localRotation = CharacterRotation;
            Camera.main.transform.localRotation = CameraRotation;
            transform.localPosition = CameraPosition;
            
            //CharacterRotation is de FPSController gameobject (over X)
            //CameraRotation is de camera IN de FPSController (over Y)
            
            //if (Input.GetKeyDown(KeyCode.F)) //dit is dus als de buffer gevuld is. 
            if (Microfoon.loudness > MicrophoneLowLimit)
            {
                //Debug.Log("SET:" + Microfoon.loudness);
                fading = true;

                //reset everything too 100
                foreach (GameObject o in GameObjects)
                {
                    //start the fading.
                    var material = o.GetComponent<Renderer>().material;

                    if (!fading) //start fading from the highest strength or microphone input strength
                    {
                        if (Microfoon.loudness * 5 < MicrophoneHighLimit)
                            material.SetFloat("_strength", Microfoon.loudness * 5);
                        else
                            material.SetFloat("_strength", MicrophoneHighLimit);
                    }
                    else
                    {
                        //add to strength, but only if the strength isnt too high yet
                        float s = material.GetFloat("_strength");
                        if (s < MaxShaderStrength)
                        {
                            if (s + (Microfoon.loudness * 5) < MicrophoneHighLimit)
                                material.SetFloat("_strength", s + (Microfoon.loudness * 5));
                            else
                                material.SetFloat("_strength", s + MicrophoneHighLimit);
                        }
                    }
                }
            }            

            if (fading && FadeOut)
            {
                foreach (GameObject o in GameObjects)
                {
                    //start the fading.
                    var material = o.GetComponent<Renderer>().material;

                    float s = material.GetFloat("_strength");
                    if (s > 0)
                    {
                        material.SetFloat("_strength", s - FadeSpeed);
                    }                   
                }
            }
        }
	}
    void PlayAudioSource()
    {
        //GetComponent<AudioSource>().clip = Microphone.Start(null, true, 1, 48000);
        GetComponent<AudioSource>().Play(); // Play the audio source!        
    }

    List<GameObject> getObjectsByMaterialName(string name)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        List<GameObject> r = new List<GameObject>();
        
        foreach (GameObject g in allObjects)
        {            
            if(g.GetComponent<Renderer>() != null && g.GetComponent<Renderer>().material.name.Contains(name) && !r.Contains(g))
            {
                r.Add(g);
            }
        }

        return r;
    }
}
