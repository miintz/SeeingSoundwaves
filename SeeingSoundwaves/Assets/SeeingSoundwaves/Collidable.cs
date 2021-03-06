﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class Collidable : MonoBehaviour {
    public List<Vector2> Points;
    
    public bool TRIANGLE = true;
    public bool SQUARE = false;
    
    int voidLayer;
        
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision col)
    {
        string FPSname = "MobileFPS";

        try
        {
            switch (col.gameObject.name)
            {
                case "EastWall":
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Velocity.x *= -1;
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Bounces++;
                    break;
                case "WestWall":
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Velocity.x *= -1;
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Bounces++;
                    break;
                case "NorthWall":
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Velocity.z *= -1;
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Bounces++;
                    break;
                case "SouthWall":
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Velocity.z *= -1;
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Bounces++;
                    break;
                case "Ceiling":
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Velocity.y *= -1;
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Bounces++;
                    break;
                case "Floor":
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Velocity.y *= -1;
                    GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(name)).Bounces++;
                    break;
                default:
                    //Debug.Log(name + " ignores " + col.gameObject.name);
                    break;
            }

            GameObject.Find(FPSname).GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Decay(50);
        }
        catch(System.Exception E)
        {
        
        }         
    }   
}
