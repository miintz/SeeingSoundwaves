using UnityEngine;
using System.Collections;
using System.Collections.Generic; 


public class Collidable : MonoBehaviour {
    public List<Vector2> Points;
    
    public bool TRIANGLE = true;
    public bool SQUARE = false;
	
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
       
        switch(name)
        {
            case "EastWall":
                GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Velocity.x *= -1;
                break;
            case "WestWall":                
                GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Velocity.x *= -1;
                break;
            case "NorthWall":
                GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Velocity.z *= -1;
                break;
            case "SouthWall":
                GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Velocity.z *= -1;
                break;
            case "Ceiling":
                GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Velocity.y *= -1;
                break;
            case "Floor":
                GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Velocity.y *= -1;                
                break;

        }

        GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Decay(50);        
    }

}
