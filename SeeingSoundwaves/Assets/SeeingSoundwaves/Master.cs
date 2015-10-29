using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Master : MonoBehaviour {

    public int N = 25;
    public int NET = 4;

    public bool UsePlayerPosition = true;
    public bool SphereMode = false;
    public bool DisableMovement = false;
    public bool ColorBounce = false;

    List<Dot[]> MasterList;
  	
	void Start () {
        MasterList = new List<Dot[]>();
	}

    public Dot getDot(int _id)
    {
        if (MasterList.Count != 0)
        {
            foreach (Dot[] dots in MasterList)
            {
                for (int i = 0; i < dots.Length; i++)
                {
                    if (dots[i] != null)
                    {
                        if (dots[i].Id == _id)
                            return dots[i];
                    }
                }
            }
        }
        return null;
    }

    Vector3 randomSpherePoint(int I)
    {
        int k = I;

        float fPoints = (float)N;

        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2 / fPoints;        
        
        float y = k * off - 1 + (off / 2);
        float r = Mathf.Sqrt(1 - y * y);

        float phi = k * inc;
        
        Vector3 points = new Vector3(Mathf.Cos(phi) * r, y + 4.0f, Mathf.Sin(phi) * r);
     
        return points;
    }

    Mesh CreateMesh(float width, float height)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";

        m.vertices = new Vector3[] 
        {
            new Vector3(-width, -height, 0.01f),
            new Vector3(width, -height, 0.01f),
            new Vector3(width, height, 0.01f),
            new Vector3(-width, height, 0.01f)
        };
        
        m.uv = new Vector2[] 
        {
            new Vector2 (0, 0),
            new Vector2 (0, 1),
            new Vector2 (1, 1),
            new Vector2 (1, 0)
        };

        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        return m;
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
            //Mesh m = CreateMesh(100, 100);
            
            //GameObject plane = new GameObject("Plane");
            
            //MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
            //meshFilter.mesh = CreateMesh(1, 0.2f);
            
            //MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            //renderer.material.shader = Shader.Find("Particles/Additive");
            
            //Texture2D tex = new Texture2D(1, 1);
            //tex.SetPixel(0, 0, Color.green);
            //tex.Apply();
            
            //renderer.material.mainTexture = tex;
            //renderer.material.color = Color.green;

            //plane.transform.position += new Vector3(0.0f, 4.0f);
            
            //m.vertices = new Vector3[]{
            //    new Vector3(-20, -20, 0.01f),
            //    new Vector3(20, -20, 0.01f),
            //    new Vector3(20, 20, 0.01f),
            //    new Vector3(-20, 20, 0.01f)
            //};
            
            //plane.transform.localScale = new Vector3(1.0f, 1.0f);

            //return;    

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
                            Vector3 pos = randomSpherePoint(i);

                            x = pos.x;
                            y = pos.y;
                            z = pos.z;                         
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
                            Vector3 pos = randomSpherePoint(i);

                            x = pos.x;
                            y = pos.y;
                            z = pos.z;                        
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

                
                foreach (Dot Lists in MasterList[0])
                {
                    //int lowestValue = groups.SelectMany(group => group.Items).Min(item => item.Val);
                    List<Dot> Neighbors = new List<Dot>();
                    Dot FirstOne = Lists;

                    for (int i = 0; i < NET; i++)
                    {                        
                        Dot LowestOne = null;

                        float closest1 = float.MaxValue;

                        foreach (Dot Dot in MasterList[0])
                        {
                            //skip the first one
                            if (FirstOne != Dot)                                
                            {
                                if (Vector3.Distance(FirstOne.DrawObject.transform.position, Dot.DrawObject.transform.position) < closest1)
                                {
                                    //is it in list?
                                    if (!Neighbors.Contains(Dot))
                                    {
                                        closest1 = Vector3.Distance(FirstOne.DrawObject.transform.position, Dot.DrawObject.transform.position);
                                        LowestOne = Dot;
                                    }
                                }
                            }
                        }

                        Neighbors.Add(LowestOne);
                    }

                    foreach (Dot a in Neighbors)
                    {                        
                        FirstOne.Closest.Add(a.Id);
                    }                   
                }
            }
        }                   
	}

    public Dot ArrayList { get; set; }
}

