using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Master : MonoBehaviour {

    public int N = 25;
    
    public bool UsePlayerPosition = true;
    public bool SphereMode = false;
    public bool DisableMovement = false;

    List<Dot[]> MasterList;
  	
	void Start () {
        MasterList = new List<Dot[]>();
	}

    public Dot getDot(int _id)
    {
        foreach(Dot[] dots in MasterList)
        {
            for (int i = 0; i < dots.Length; i++)
            {
                if (dots[i].Id == _id)
                    return dots[i];
            }
        }

        return null;
    }

    float[] randomSpherePoint(float x0, float y0, float z0, float radius){
        Random ra = new Random();
        
        var u = Random.value;
        var v = Random.value;        
        
        var theta = 2 * Mathf.PI * u;
        var phi = Mathf.Acos(2 * v - 1);

        float x = x0 + (radius * Mathf.Sin(phi) * Mathf.Cos(theta));
        float y = y0 + (radius * Mathf.Sin(phi) * Mathf.Sin(theta));
        float z = z0 + (radius * Mathf.Cos(phi));

        return new float[3] { x, y, z };
    }
	void Update () 
    {
        if (MasterList.Count != 0)
        {
            foreach (Dot[] Dots in MasterList) //dit...
            {
                for (int u = 0; u < Dots.Length; u++)
                {
                    if (Dots[u] != null && !Dots[u].Disposed)
                        Dots[u].Update(); //dit gaat soms mis
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

                //throw away empty lists
                for (int m = 0; m < MasterList.Count; m++)
                {
                    bool empty = true;
                    for (int i = 0; i < N; i++)
                    {
                        if (MasterList[m][i] != null)
                            empty = false;
                    }

                    if (empty)
                    {
                        MasterList.Remove(MasterList[m]);
                        m = 0;
                    }
                }
            }
        }

        //click so add thingies
        if (Input.GetMouseButtonDown(0))
        {
            if (MasterList.Count < 5)
            {
                Dot[] Dots = new Dot[N];

                float pia = (Mathf.PI * 2 / N);

                int lastId = MasterList.Count != 0 ? MasterList[MasterList.Count - 1][N - 1].Id : 0;

                for (int i = 0; i < N; i++)
                {
                    float mx = 0;
                    float my = 0;
                    float mz = 0;

                    float x = 0;
                    float y = 0;
                    float z = 0;

                    if (UsePlayerPosition)
                    {
                        mx = GameObject.Find("FPSController").transform.position.x;
                        my = GameObject.Find("FPSController").transform.position.y;
                        mz = 0.0f;

                        if (!SphereMode)
                        {                          
                            x = mx + 1 * Mathf.Cos(pia * i);
                            y = my + 1 * Mathf.Sin(pia * i);
                            z = 0.0f;
                        }
                        else
                        {
                            float[] pos = randomSpherePoint(GameObject.Find("FPSController").transform.position.x, GameObject.Find("FPSController").transform.position.y, GameObject.Find("FPSController").transform.position.z, 1.0f);
                            
                            x = pos[0];
                            y = pos[1];
                            z = pos[2];                            
                        }
                    }
                    else
                    {
                        mx = 0.0f;
                        my = 4.0f;
                        mz = 0.0f;

                        if (!SphereMode)
                        {                            
                            x = mx + 1 * Mathf.Cos(pia * i);
                            y = my + 1 * Mathf.Sin(pia * i);
                            z = 0.0f;
                        }
                        else 
                        {
                            float[] pos = randomSpherePoint(0.0f, 4.0f, 0.0f, 1.0f);                        
                            
                            x = pos[0];
                            y = pos[1];
                            z = pos[2];                        
                        }
                    }

                    float dist = Vector3.Distance(new Vector3(mx, my, mz), new Vector3(x, y, z));

                    float difx = (x - mx) / dist;
                    float dify = (y - my) / dist;
                    float difz = (z - mz) / dist;

                    Dots[i] = new Dot(x, y, z, difx, dify, difz, lastId + i);
                    Dots[i].Disabled = DisableMovement;
                }

                MasterList.Add(Dots);

                foreach (Dot[] dots in MasterList)
                {
                    for (int z = 0; z < N; z++)
                    {
                        foreach (Dot[] dots2 in MasterList)
                        {
                            for (int yy = 0; yy < N; yy++)
                            {                         
                                Physics.IgnoreCollision(dots[z].DrawObject.GetComponent<SphereCollider>(), dots2[yy].DrawObject.GetComponent<SphereCollider>());
                            }
                        }                        
                    }
                }               
            }
        }
                   
	}
}

