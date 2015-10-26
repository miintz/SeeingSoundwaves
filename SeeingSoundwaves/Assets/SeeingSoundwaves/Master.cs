using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Master : MonoBehaviour {

    public int N = 25;
    List<Dot> Dots = new List<Dot>();

	// Use this for initialization
	void Start () {
       
	}
    
	// Update is called once per frame
	void Update () {

        foreach (Dot d in Dots)
        {            
            if(!d.Disposed)
                d.Update();
        }

        for (int i = 0; i < Dots.Count; i++)
        {
            if (Dots[i].Strength < 0.0f)
            {
                Dots[i].Dispose();                
            }
        }

        Dots.RemoveAll(d => d.Disposed == true);

        if (Input.GetMouseButtonDown(0))
        {
            Dots = new List<Dot>();
           
            if (Dots.Count < N)
            {
                float pia = (Mathf.PI * 2 / N);

                for (int i = 0; i < N; i++)
                {
                    float mx = GameObject.Find("FPSController").transform.position.x;
                    float my = GameObject.Find("FPSController").transform.position.y;
                    float mz = 0.0f;

                    float x = mx + 5 * Mathf.Cos(pia * i);
                    float y = my + 5 * Mathf.Sin(pia * i);
                    float z = 0.0f;

                    float dist =  Vector3.Distance(new Vector3(mx,my,mz), new Vector3(x,y,z));

                    float difx = (x - mx) / dist;
                    float dify = (y - my) / dist;

                    Dots.Add    (new Dot(x, y, difx, dify, i));
                }
            }
           
        }
	}
}

