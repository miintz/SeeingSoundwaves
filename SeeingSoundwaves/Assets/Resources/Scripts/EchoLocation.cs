using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class EchoLocation : MonoBehaviour
{

    public bool EnableEchoLocation = true;
    public bool FadeOut = true;
    public float FadeSpeed = 0.1f;

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
        }
    }

    // Update is called once per frame
    void Update() {        
        if (EnableEchoLocation)
        {
            CameraPosition = Character.GetCameraMoveDirection();
            CameraRotation = Character.GetCameraRotation();
            CharacterRotation = Character.GetCharacterRotation();

            //CharacterRotation is de FPSController gameobject (over X)
            //CameraRotation is de camera IN de FPSController (over Y)
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                transform.localRotation = CharacterRotation;
                Camera.main.transform.localRotation = CameraRotation;
                transform.localPosition = CameraPosition;

                fading = true;

                //reset everything too 100
                foreach (GameObject o in GameObjects)
                {
                    //start the fading.
                    var material = o.GetComponent<Renderer>().material;
                    material.SetFloat("_strength", 100);                                           
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
