using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public bool Controller = false;
    public bool GearVR = true;

    public bool m_Block = false;

	// Use this for initialization
	void Start () {
        Cam = Camera.main;       
    }
	
	// Update is called once per frame
	void Update () {
        
        //looking
        if (Controller)
        {
            yRot -= Input.GetAxis("Vertical") * lookSensitivity;
            xRot += Input.GetAxis("Horizontal") * lookSensitivity;
        }
        else
        {
            yRot -= Input.GetAxis("Mouse Y") * lookSensitivity;
            xRot += Input.GetAxis("Mouse X") * lookSensitivity;
        }

        yRot = Mathf.Clamp(yRot, -90, 90);

        currentXrot = Mathf.SmoothDamp(currentXrot, xRot, ref xRotV, lookSmoothDamp);
        currentYrot = Mathf.SmoothDamp(currentYrot, yRot, ref yRotV, lookSmoothDamp);

        //if (!m_Block)
        //GameObject.FindGameObjectWithTag("Char").transform.rotation = Quaternion.Euler(currentYrot, currentXrot, 0);

         p = "Showing Input\n";
         p += "Controller = " + Controller.ToString() + "\n";

         if (Input.GetMouseButtonDown(0))
         {
             p += "Input: Tap\n";
             System.Random r = new System.Random();   
             //GameObject.FindGameObjectsWithTag("Insect").ToList().ForEach(t => t.GetComponent<Insect>().DebugObjectColor = new Color(UnityEngine.Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f)));
         }

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
            if (!GearVR)
            {
                //keyboard
                if (Input.GetKey("w"))
                    xVelocity += 0.5f / speedMod;
                else if (Input.GetKey("s"))
                    xVelocity -= 0.5f / speedMod; //slerpen nog, nu is het nog niet erg soepel.            
            }
            else
            {
                if (Input.GetAxis("Mouse X") != 0.0f)
                {                
                    if (Input.GetAxis("Mouse X") > 0.0f)
                        xVelocity -= 0.10f;
                    else
                        xVelocity += 0.10f;
                }

                //if (Input.GetAxis("Mouse Y") < 0.0f)
                //{
                //    Debug.Log("Value: " + Input.GetAxis("Mouse Y") + " " + xVelocity); //het is een delta!!!

                //    if (Input.GetAxis("Mouse Y") > 0.0f)
                //        xVelocity += Input.GetAxis("Mouse Y") / speedMod;
                //    else
                //        xVelocity -= Input.GetAxis("Mouse X") / speedMod;
                //}
                
            }
        }

        xVelocity = Mathf.Clamp(xVelocity, 0.0f, topspeed); //clamp het naar de topspeed                
        
        //if(!m_Block)
           GameObject.FindGameObjectWithTag("Char").transform.position += transform.forward * (xVelocity / 10);

        //checkCloseness();
	}
    string p = "";
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), p);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Insect")
        {
            GameObject.Find("Empty").GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);                 
            col.gameObject.SetActive(false);
        }        
    }
}
    