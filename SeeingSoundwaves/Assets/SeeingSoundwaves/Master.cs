﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Master : MonoBehaviour {

    public int N = 25;
    Dot[] Dots;

	// Use this for initialization
	void Start () {
        Dots = new Dot[N];        
	}

    public Dot getDot(int _id)
    {
        return Dots[_id];
    }

	// Update is called once per frame
	void Update () {

        for (int u = 0; u < Dots.Length;u++ )
        {
            if (Dots[u] != null && !Dots[u].Disposed)
                Dots[u].Update();
        }

        for (int i = 0; i < Dots.Length; i++)
        {
            if (Dots[i] != null && !Dots[i].Disposed) //meh...
            {
                if (Dots[i].Strength < 0.0f)
                {
                    Dots[i].Dispose();
                    Dots[i] = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
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

                    Dots[i] = new Dot(x, y, difx, dify, i);
                }
            }
                   
	}
}
