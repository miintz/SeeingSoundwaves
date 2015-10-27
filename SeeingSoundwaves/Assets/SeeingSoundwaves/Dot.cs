using UnityEngine;
using System.Collections.Generic;
using System;

public class Dot  {
    
    public Vector3 Velocity;// { get; set; }
    Vector3 Position;// { get; set; }
    Vector3 Scale;

    public float Strength { get; set; }    
    float Radius { get; set; }
    float Speed { get; set; }
    public int Id { get; set; }
    
    private GameObject DrawObject;
    
    public Boolean Disposed = false;

    public Dot(int _id)
    {
        this.Id = _id;
        Disposed = true;
    }

	public Dot(float _x, float _y, float _dirx, float _diry, int _id) 
    {
        this.Speed = 3.5f;
        this.Strength = 1.0f;

        this.Position = new Vector3(_x, _y);
        this.Velocity = Vector3.Scale(new Vector3(_dirx, _diry), new Vector3(0.1f,0.1f));
       
        this.Scale = new Vector3(0.3f, 0.3f, 0.3f);

        this.Id = _id;
        
        DrawObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        DrawObject.transform.localScale = this.Scale;
        DrawObject.transform.position = this.Position;

        DrawObject.AddComponent<Rigidbody>();
        DrawObject.GetComponent<Rigidbody>().mass = 1.0f;
        DrawObject.GetComponent<Rigidbody>().useGravity = false;

        //DrawObject.AddComponent<Collidable>();

        DrawObject.name = this.Id.ToString();               
	}

    public void Dispose()
    {
        MonoBehaviour.Destroy(DrawObject);
        this.Disposed = true;
    }

    void Start() { }

	public void Update () {        
        Decay(1);   

        float form = 0.3f * this.Strength;
         
        this.Scale = new Vector3(form, form, form);        
        DrawObject.transform.localScale = this.Scale;

       
        DrawObject.transform.position += this.Velocity;
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
                    Vector2 base1 = new Vector2(5,5); // PVector.random2D();
                    Vector2 base2 = new Vector2(5,5); // PVector.random2D();

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
