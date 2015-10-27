using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Master : MonoBehaviour {

    public int N = 25;
    public bool usePlayerPosition = true;

    List<Dot[]> MasterList;
  

	// Use this for initialization
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

	void Update () {
        foreach (Dot[] Dots in MasterList)
        {
            for (int u = 0; u < Dots.Length; u++)
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
                    float mx, my, mz;

                    if (usePlayerPosition)
                    {
                        mx = GameObject.Find("FPSController").transform.position.x;
                        my = GameObject.Find("FPSController").transform.position.y;
                        mz = 0.0f;
                    }
                    else
                    {
                        mx = 0.0f;
                        my = 4.0f;
                        mz = 0.0f;
                    }

                    float x = mx + 1 * Mathf.Cos(pia * i);
                    float y = my + 1 * Mathf.Sin(pia * i);
                    float z = 0.0f;

                    float dist = Vector3.Distance(new Vector3(mx, my, mz), new Vector3(x, y, z));

                    float difx = (x - mx) / dist;
                    float dify = (y - my) / dist;

                    Dots[i] = new Dot(x, y, difx, dify, lastId + i);
                }

                MasterList.Add(Dots);

                int bn = 0;

                foreach (Dot[] dots in MasterList)
                {
                    for (int z = 0; z < N; z++)
                    {
                        foreach (Dot[] dots2 in MasterList)
                        {
                            for (int yy = 0; yy < N; yy++)
                            {
                                bn++;
                                Physics.IgnoreCollision(dots[z].DrawObject.GetComponent<SphereCollider>(), dots2[yy].DrawObject.GetComponent<SphereCollider>());
                            }
                        }
                        //ignore self collisions
                        //

                    }
                }
                Debug.Log(" ignored: " + bn);
            }
        }
                   
	}
}

