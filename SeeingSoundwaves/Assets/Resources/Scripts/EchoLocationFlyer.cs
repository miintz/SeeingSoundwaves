using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using System.Linq;

public class EchoLocationFlyer : MonoBehaviour
{
    private MicIn Microfoon;

    public bool EnableEchoLocation = true;
    public bool KeyMode = true;
    public bool FadeOut = true;
    public bool UseTimer = false;
    public float FadeSpeed = 0.1f;
    public bool Blocking = true;
    public float MicrophoneSensitivity = 100.0f;
    public float MicrophoneLowLimit = 10.0f;
    public float MicrophoneHighLimit = 25.0f;
    public float MaxShaderStrength = 100.0f;

    private Quaternion CameraRotation;
    private Quaternion CharacterRotation;
    private Vector3 CameraPosition;

    private Flyer Character;
    private Boolean fading = false;

    private float EchoTime = 1000;
    private float EchoCounter = 1000;

    List<GameObject> GameObjects;

    // Use this for initialization
    void Start()
    {
        GameObjects = getObjectsByMaterialName("distanceLerp");

        if (EnableEchoLocation)
        {
            Character = GetComponent<Flyer>();
            Character.m_Block = true; //singleton!!! handig            
            
            Microfoon = new MicIn(this, MicrophoneSensitivity);            
        }

        fading = true;

        //if (!Blocking)
        //    Character.m_Block = false;
    }  

    // Update is called once per frame
    void Update() {        
        if (EnableEchoLocation)
        {
            //first update the microphone
            Microfoon.Update();

            if (Microfoon.loudness > MicrophoneLowLimit)
            {
                //if(Blocking)
                //    Character.m_Block = false;

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
                            if (EchoCounter > EchoTime || !UseTimer)
                            {                             
                                if (s + (Microfoon.loudness * 5) < MicrophoneHighLimit)
                                    material.SetFloat("_strength", s + 150);
                                else
                                    material.SetFloat("_strength", s + MicrophoneHighLimit);

                                EchoCounter = 0;
                            }
                        }
                    }
                }
            }
            //else if(Blocking)
            //    Character.m_Block = true;

            EchoCounter += Time.deltaTime * 1000;

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

            GameObjects = getObjectsByMaterialName("distanceLerp");
        }
        else if (KeyMode)
        {
            if (Input.GetKey(KeyCode.K) || Input.GetMouseButtonDown(0))
            {         
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
                            if (EchoCounter > EchoTime || !UseTimer)
                            {                               
                                material.SetFloat("_strength", s + 150);                               
                                EchoCounter = 0;
                            }
                        }
                    }
                }
            }
            //else if (Blocking)
            //    Character.m_Block = true;

            EchoCounter += Time.deltaTime * 1000;

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

            GameObjects = getObjectsByMaterialName("distanceLerp");
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

    public void ChangeMicrophoneSensitivity(int number)
    {
        switch (number)
        {
            case 0:
                MicrophoneSensitivity = 50f;
                break;
            case 1:
                MicrophoneSensitivity = 75f;
                break;
            case 2:
                MicrophoneSensitivity = 100f;
                break;
            case 3:
                MicrophoneSensitivity = 125f;
                break;
            case 4:
                MicrophoneSensitivity = 150f;
                break;
        }
    }

    public void ChangeFadeSpeed(int number)
    {      
        switch(number)
        {
            case 0:
                FadeSpeed = 0.5f;
                break;
            case 1:
                FadeSpeed = 1.0f;
                break;
            case 2:
                FadeSpeed = 1.5f;
                break;
            case 3:
                FadeSpeed = 2.0f;
                break;
            case 4:
                FadeSpeed = 2.5f;
                break;
        }        
    }

    public void ChangeMicrophoneLowLimit(int number)
    {
        switch (number)
        {
            case 0:
                MicrophoneLowLimit = 5.0f;
                break;
            case 1:
                MicrophoneLowLimit = 10.0f;
                break;
            case 2:
                MicrophoneLowLimit = 15.0f;
                break;
            case 3:
                MicrophoneLowLimit = 20.0f;
                break;
            case 4:
                MicrophoneLowLimit = 25.0f;
                break;
        }
    }

    public void ChangeShaderStrength(int number)
    {
        switch(number)
        {
            case 0:
                GameObject.FindGameObjectsWithTag("DistanceLerp").ToList().ForEach(t => t.GetComponent<MeshRenderer>().material.SetFloat("_range", 25));              
            break;
            case 1:
            GameObject.FindGameObjectsWithTag("DistanceLerp").ToList().ForEach(t => t.GetComponent<MeshRenderer>().material.SetFloat("_range", 50));     
            break;
            case 2:
            GameObject.FindGameObjectsWithTag("DistanceLerp").ToList().ForEach(t => t.GetComponent<MeshRenderer>().material.SetFloat("_range", 75));     
            break;
            case 3:
            GameObject.FindGameObjectsWithTag("DistanceLerp").ToList().ForEach(t => t.GetComponent<MeshRenderer>().material.SetFloat("_range", 100));     
            break;
            case 4:
            GameObject.FindGameObjectsWithTag("DistanceLerp").ToList().ForEach(t => t.GetComponent<MeshRenderer>().material.SetFloat("_range", 125));     
            break;
        }
    }

    public void ChangeShaderDropoff(int number)
    {
        switch (number)
        {
            case 0:
                Resources.Load<Material>("Resources/distanceLerp").SetFloat("_dropoff", 0.005f);
                break;
            case 1:
                Resources.Load<Material>("Resources/distanceLerp").SetFloat("_dropoff", 0.02f);
                break;
            case 2:
                Resources.Load<Material>("Resources/distanceLerp").SetFloat("_dropoff", 0.1f);
                break;
            case 3:
                Resources.Load<Material>("Resources/distanceLerp").SetFloat("_dropoff", 0.25f);
                break;
            case 4:
                Resources.Load<Material>("Resources/distanceLerp").SetFloat("_dropoff", 0.5f);
                break;
        }
    }
}
