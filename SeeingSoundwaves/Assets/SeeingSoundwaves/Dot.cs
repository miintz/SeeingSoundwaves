using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class Dot
{

    public Vector3 Velocity;// { get; set; }

    Vector3 Position;// { get; set; }
    Vector3 Scale;

    float Radius { get; set; }
    float Speed { get; set; }
    public float Strength { get; set; }

    public int Id { get; set; }
    public int Bounces { get; set; }

    public Boolean Disposed = false;
    public Boolean Disabled = false;
    public Boolean DrawLines = false;
    public Boolean DrawMesh = false;

    public GameObject DrawObject;
    public GameObject plane;

    public List<int> Closest = new List<int>();

    public List<Color> Colors = new List<Color>();

    string FPSname = "MobileFPS";

    private Mesh m;


    public Dot(int _id)
    {
        this.Id = _id;
        Disposed = true;

        Bounces = 0;

        Colors.Add(Color.red);
        Colors.Add(Color.yellow);
        Colors.Add(Color.green);
        Colors.Add(Color.cyan);
        Colors.Add(Color.blue);
        Colors.Add(Color.magenta);
        Colors.Add(new Color(255, 51, 0));
        Colors.Add(new Color(51, 51, 0));
        Colors.Add(new Color(102, 0, 102));
        Colors.Add(new Color(0, 153, 153));
    }

    public Dot(float _x, float _y, float _z, float _dirx, float _diry, float _dirz, int _id, Boolean _neglible)
    {
        this.Speed = 3.5f;
        this.Strength = 1.0f;

        this.Position = new Vector3(_x, _y, _z);
        this.Velocity = Vector3.Scale(new Vector3(_dirx, _diry, _dirz), new Vector3(0.1f, 0.1f, 0.1f));

        if (!_neglible)
        {
            this.Scale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        else
        {
            this.Scale = new Vector3(0.001f, 0.001f, 0.001f); //not working
        }

        this.Id = _id;

        DrawObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        DrawObject.transform.localScale = this.Scale;
        DrawObject.transform.position = this.Position;

        DrawObject.AddComponent<Rigidbody>();
        DrawObject.GetComponent<Rigidbody>().mass = 1.0f;
        DrawObject.GetComponent<Rigidbody>().useGravity = false;

        DrawObject.AddComponent<Collidable>();

        DrawObject.name = this.Id.ToString();

        Colors.Add(Color.red);
        Colors.Add(Color.yellow);
        Colors.Add(Color.green);
        Colors.Add(Color.cyan);
        Colors.Add(Color.blue);
        Colors.Add(Color.magenta);
        Colors.Add(new Color(255, 51, 0));
        Colors.Add(new Color(51, 51, 0));
        Colors.Add(new Color(102, 0, 102));
        Colors.Add(new Color(0, 153, 153));
    }

    public void Dispose()
    {
        MonoBehaviour.Destroy(DrawObject);
        this.Disposed = true;
    }

    private void DrawRays()
    {
        if (Closest.Count > 0)
        {
            foreach (int i in Closest)
            {
                Dot a = GameObject.Find(this.FPSname).GetComponent<Master>().getDot(i);
                if (a != null)
                {
                    Vector3 Target = a.DrawObject.transform.position;

                    Vector3 heading = Target - this.DrawObject.transform.position;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;

                    Debug.DrawRay(this.DrawObject.transform.position, direction * distance, Colors[Bounces]); //turn on gizmos in unity window              
                }
            }
        }
    }
    private void DrawMeshes()
    {
        if (m == null)
            m = new Mesh();

        m.name = "ScriptedMesh";
        Vector3[] vertices = new Vector3[Closest.Count];
        int i = 0;

        foreach (int close in Closest)
        {
            Dot a = GameObject.Find(this.FPSname).GetComponent<Master>().getDot(close);
            if (a != null)
            {
                vertices[i] = new Vector3(a.DrawObject.transform.position.x, a.DrawObject.transform.position.y, a.DrawObject.transform.position.z);
            }

            i++;
        }

        m.vertices = vertices;

        m.uv = new Vector2[] {
            new Vector2 (0, 0),
            new Vector2 (0, 1),
            new Vector2(1, 1),
            new Vector2 (1, 0)
        };

        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        if (plane == null)
        {
            plane = new GameObject("Plane");

            MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
            meshFilter.mesh = m;

            MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            renderer.material.shader = Shader.Find("Custom/TransparentSingleColorShader");            
            renderer.receiveShadows = false;
            renderer.useLightProbes = false;
        }
        
        Texture2D tex = new Texture2D(2, 2);
        byte[] imagebytes = File.ReadAllBytes("Assets/SeeingSoundwaves/radial_gradient.png");

        tex.LoadImage(imagebytes);        

        //tex.SetPixel(0, 0, Colors[Bounces]);
        //tex.Apply();
        
        plane.GetComponent<MeshRenderer>().material.mainTexture = tex;
        plane.GetComponent<MeshRenderer>().material.color = Colors[Bounces];

    }
    void Start() { }

    public void Update()
    {
        if (!Disabled)
        {
            Decay(1);

            float form = 0.3f * this.Strength;

            this.Scale = new Vector3(form, form, form);

            DrawObject.transform.localScale = this.Scale;
            DrawObject.transform.position += this.Velocity;

            if (DrawLines)
            {
                DrawRays();
            }
            else if (DrawMesh)
            {
                DrawMeshes();
            }
        }
    }

    public void Decay(int s)
    {
        this.Strength = this.Strength - (float)(s / 500.0);

    }

    void Collide(List<Collidable> c)
    {
        int width = 10;
        int height = 10;

        //right
        if (this.Position.x > width - this.Radius)
        {
            this.Position.x = width - Radius;
            this.Velocity.x *= -1;
            Decay(50);
        }
        // left 
        if (this.Position.x < this.Radius)
        {
            this.Position.x = this.Radius;
            this.Velocity.x *= -1;
            Decay(50);
        }
        // top
        if (this.Position.y < this.Radius)
        {
            this.Position.y = this.Radius;
            this.Velocity.y *= -1;
            Decay(50);
        }
        // bottom
        if (this.Position.y > height - this.Radius)
        {
            this.Position.y = height - this.Radius;
            this.Velocity.y *= -1;
            Decay(50);
        }

        Vector2 test = this.Position;

        int i;
        int j;
        bool result = false;

        if (c.Count != 0)
        {
            for (int u = 0; u < c.Count; u++)
            {
                result = false;

                Collidable Col = c[u];
                List<Vector2> Points = Col.Points;

                for (i = 0, j = Points.Count - 1; i < Points.Count; j = i++)
                {
                    if ((Points[i].y > test.y) != (Points[j].y > test.y) &&
                      (test.x < (Points[j].x - Points[i].x) * (test.y - Points[i].y) / (Points[j].y - Points[i].y) + Points[i].x))
                    {
                        result = !result;
                    }
                }

                if (result)
                {
                    Vector2 base1 = new Vector2(5, 5); // PVector.random2D();
                    Vector2 base2 = new Vector2(5, 5); // PVector.random2D();

                    if (Col.TRIANGLE)
                    {
                        //get 2 closest points                           
                        float d1 = Vector2.Distance(this.Position, Points[0]);
                        float d2 = Vector2.Distance(this.Position, Points[1]);
                        float d3 = Vector2.Distance(this.Position, Points[2]);

                        if (d1 < d2 && d3 < d2) //linkerzijde
                        {
                            base1 = Points[0];
                            base2 = Points[2];
                        }
                        else if (d1 < d3 && d2 < d3) //onderzijde
                        {
                            base1 = Points[0];
                            base2 = Points[1];
                        }
                        else if (d3 < d1 && d2 < d1) //rechterzijde
                        {
                            base1 = Points[2];
                            base2 = Points[1];
                        }

                        Decay(100);
                    }
                    else if (Col.SQUARE) //convex is a bit wonky
                    {
                        float d1 = Vector2.Distance(this.Position, Points[0]);
                        float d2 = Vector2.Distance(this.Position, Points[1]);
                        float d3 = Vector2.Distance(this.Position, Points[2]);
                        float d4 = Vector2.Distance(this.Position, Points[3]);

                        if (d1 < d2 && d1 < d3 && d4 < d2 && d4 < d3) //linkerzijde
                        {
                            base1 = Points[0];
                            base2 = Points[3];
                        }
                        else if (d1 < d3 && d1 < d4 && d2 < d3 && d3 < d4) //onderzijde
                        {
                            base1 = Points[0];
                            base2 = Points[1];
                        }
                        else if (d2 < d1 && d2 < d4 && d3 < d1 && d3 < d4) //rechterzijde
                        {
                            base1 = Points[2];
                            base2 = Points[3];
                        }
                        else if (d3 < d1 && d3 < d2 && d4 < d1 && d4 < d2) //bovenkant
                        {
                            base1 = Points[2];
                            base2 = Points[3];
                        }

                        Decay(100);
                    }

                    //Vector2 incidence = Vector2.sca(velocity, -1);
                    //incidence.normalize();

                    //PVector baseDelta = PVector.sub(base2, base1);
                    //baseDelta.normalize();
                    //PVector normal = new PVector(-baseDelta.y, baseDelta.x);

                    //float dot = incidence.dot(normal);

                    //velocity.set(2 * normal.x * dot - incidence.x, 2 * normal.y * dot - incidence.y, 0);
                }
            }
        }
    }
}
