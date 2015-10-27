using UnityEngine;
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
        try
        {
            switch (col.gameObject.name)
            {
                case "EastWall":
                    GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(name)).Velocity.x *= -1;
                    break;
                case "WestWall":                 
                    GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(name)).Velocity.x *= -1;
                    break;
                case "NorthWall":
                    GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(name)).Velocity.z *= -1;
                    break;
                case "SouthWall":
                    GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(name)).Velocity.z *= -1;
                    break;
                case "Ceiling":
                    GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(name)).Velocity.y *= -1;
                    break;
                case "Floor":
                    GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(name)).Velocity.y *= -1;
                    break;
                default:
                    //Debug.Log(name + " ignores " + col.gameObject.name);                    


                    break;
            }

            GameObject.Find("FPSController").GetComponent<Master>().getDot(int.Parse(col.gameObject.name)).Decay(50);
        }
        catch(System.Exception E)
        {
        
        }         
    }   
}
