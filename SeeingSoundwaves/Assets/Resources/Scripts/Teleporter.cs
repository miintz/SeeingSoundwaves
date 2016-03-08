using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public  void TeleportTo(int l)
    {
        switch (l)
        { 
            case 0:
                this.transform.position = new Vector3(0,0,0); //positie van de grot x,y,z
                break;
            case 1:
                this.transform.position = new Vector3(0,0,0); //positie van de score kamer x,y,z
                break; 
            case 2:
                this.transform.position = new Vector3(0,0,0); //positie van de start kamer x,y,z
                break;
        }
    }
}
