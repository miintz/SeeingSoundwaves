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
        Debug.Log("collision with: " + col.gameObject.name);
    }

}
