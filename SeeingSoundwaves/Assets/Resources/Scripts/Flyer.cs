using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flyer : MonoBehaviour {

    Camera Cam;
    public float lookSensitivity = 1;
    public float topspeed = 4.0f;
    public float defaultSpeedDecay = 0.01f;
    public float triggeredSpeedDecay = 0.05f;
    public float speedMod = 100.0f;

    float yRot;
    float xRot;
    float currentYrot;
    float currentXrot;
    float yRotV;
    float xRotV;
    float lookSmoothDamp;
    float xVelocity;

    public bool Controller = true;
    
    public bool m_Block = true;

	// Use this for initialization
	void Start () {
        Cam = Camera.main;       
    }
	
	// Update is called once per frame
	void Update () {
        
        //looking
        if (Controller)
        {
            yRot -= Input.GetAxis("Vertical") * lookSensitivity; //invert
            xRot += Input.GetAxis("Horizontal") * lookSensitivity;
        }
        else
        {
            yRot -= Input.GetAxis("Mouse Y") * lookSensitivity; //invert
            xRot += Input.GetAxis("Mouse X") * lookSensitivity;
        }

        yRot = Mathf.Clamp(yRot, -90, 90);

        currentXrot = Mathf.SmoothDamp(currentXrot, xRot, ref xRotV, lookSmoothDamp);
        currentYrot = Mathf.SmoothDamp(currentYrot, yRot, ref yRotV, lookSmoothDamp);

        if (!m_Block)
            transform.rotation = Quaternion.Euler(currentYrot, currentXrot, 0);

        if (Controller)
        {
            float axis = Input.GetAxis("joystick axis 10");
            if (axis > 0.0f)
                xVelocity += axis;
            else
            {
                float slowaxis = Input.GetAxis("joystick axis 9");
                if(slowaxis > 0)
                    xVelocity -= triggeredSpeedDecay; //slerpen nog, nu is het nog niet erg soepel.
                else
                    xVelocity -= defaultSpeedDecay; //slerpen nog, nu is het nog niet erg soepel.
            }
        }
        else
        { 
            //keyboard
            if (Input.GetKey("w"))
                xVelocity += 0.5f / speedMod;                            
            else if (Input.GetKey("s"))
                xVelocity -= 0.5f / speedMod; //slerpen nog, nu is het nog niet erg soepel.            
        }


        xVelocity = Mathf.Clamp(xVelocity, 0.0f, topspeed); //clamp het naar de topspeed                
        
        if(!m_Block)
                transform.position += transform.forward * (xVelocity / 10);

        //checkCloseness();
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Insect")
        {
            GameObject.Find("Empty").GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            //Destroy(col.gameObject);
            col.gameObject.SetActive(true);
        }        
    }
}
    