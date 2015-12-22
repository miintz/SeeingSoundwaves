using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class EchoLocation : MonoBehaviour {

    public bool EnableEchoLocation = true;

    private Quaternion CameraRotation;
    private Quaternion CharacterRotation;
    private Vector3 PositionOfPlayer;

    private FirstPersonControllerMod Character;

	// Use this for initialization
	void Start () {
        if (EnableEchoLocation)
        {
            Character = GetComponent("FirstPersonControllerMod") as FirstPersonControllerMod;
            Character.m_Block = true; //singleton!!! handig
        }
	}
   
	// Update is called once per frame
	void Update () {        
        if (EnableEchoLocation)
        {
            //PositionOfPlayer = Character.GetCameraPosition();
            CameraRotation = Character.GetCameraRotation();
            CharacterRotation = Character.GetCharacterRotation();

            //CharacterRotation is de FPSController gameobject (over X)
            //CameraRotation is de camera IN de FPSController (over Y)
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.localRotation = CharacterRotation;
                Camera.main.transform.localRotation = CameraRotation;            
            }
        }
	}
}
