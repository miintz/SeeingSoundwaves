﻿using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Collections.Generic;
public enum InsectPreset
{
    Custom, Mosquito, Fly, Wasp
}
[ExecuteInEditMode]
public class Insect : MonoBehaviour {
	Vector3 InsectTransformPosition;
    Quaternion InsectTransformRotation = new Quaternion(0,0,0,0);

    public InsectPreset Preset;
    public bool Swarm = false;
    public int SwarmSize = 0;

	public bool Jittering = true;
	public float JitterTime = 1.0f;         //seconds
	public float JitterAmount = 1.0f;       //Unity units
	public bool Flying = true;
	public float FlyingTime = 1.0f;
	public float FlyingIntervalRandom = 0.5f;
	public float FlyingStrength = 2.0f;     //Unity units    
	public bool StickToRegion = true;
	public float RegionRadius = 5.0f;
    public bool Fleeing = false;
    public float FleeingDistance = 2.0f;    
	public bool DebugMode = true;
    
    public Color DebugObjectColor = Color.white;

	private Vector3 InsectOrigin;
	private double JitterCounter = 0;
	private double FlyingCounter = 0;

    private List<GameObject> InsectSwarm;

	void Start () {
        if (Application.isPlaying)
        {
            if (Swarm)
            {
                InsectSwarm = new List<GameObject>();
                for (int o = 0; o < SwarmSize; o++)
                {
                    GameObject i = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    i.name = Preset.ToString();

                    i.transform.position = this.transform.position;
                    i.transform.localScale = this.transform.localScale;

                    i.AddComponent<Insect>();

                    i.GetComponent<Insect>().Preset = Preset;
                    i.GetComponent<Insect>().Swarm = false;
                    i.GetComponent<Insect>().SwarmSize = 0;
                    i.GetComponent<Insect>().Jittering = Jittering;
                    i.GetComponent<Insect>().JitterTime = JitterTime;
                    i.GetComponent<Insect>().JitterAmount = JitterAmount;
                    i.GetComponent<Insect>().Fleeing = Fleeing;
                    i.GetComponent<Insect>().Flying = Flying;
                    i.GetComponent<Insect>().FlyingTime = FlyingTime - Random.Range(-FlyingIntervalRandom, FlyingIntervalRandom);
                    i.GetComponent<Insect>().FlyingIntervalRandom = FlyingIntervalRandom;
                    i.GetComponent<Insect>().FlyingStrength = FlyingStrength;
                    i.GetComponent<Insect>().StickToRegion = StickToRegion;
                    i.GetComponent<Insect>().RegionRadius = RegionRadius;
                    i.GetComponent<Insect>().DebugMode = DebugMode;
                    i.GetComponent<Insect>().DebugObjectColor = DebugObjectColor;

                    InsectSwarm.Add(i);                    
                }
            }

            InsectOrigin = this.transform.position;

            InsectTransformPosition = this.transform.position;
            InsectTransformRotation = this.transform.rotation;

            JitterTime *= 1000; //naar ms
            FlyingTime *= 1000; //naar ms
        }
	}
		
	void Update () {

        if (Application.isPlaying)
        {
            if (StickToRegion && RegionRadius < FlyingStrength)
            {
                Debug.LogError("Stick to Region is on while FlyingStrength is higher than DistanceFromOrigin, please make sure FlyingStrengt is lower than DistanceFromOrigin");
                return;
            }


            JitterCounter += Time.deltaTime * 1000;
            if (JitterCounter >= JitterTime)
            {
                JitterCounter = 0;
                Jitter();
            }

            if (Flying)
            {
                FlyingCounter += Time.deltaTime * 1000;
                if (FlyingCounter >= (FlyingTime + Random.Range(-FlyingIntervalRandom, FlyingIntervalRandom)))
                {
                    FlyingCounter = 0;
                    Fly();
                }
            }

            //fleeing overides the flying
            if (Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position) < FleeingDistance)
            {
                Flee();
            }

            if (this.transform.position != InsectTransformPosition)
            {
                Vector3 pos = InsectTransformPosition;
                this.transform.position = Vector3.Slerp(this.transform.position, pos, Time.deltaTime);
            }

            if (this.transform.rotation != InsectTransformRotation)
            {
                Quaternion rot = InsectTransformRotation;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime);
            }

            if (DebugObjectColor != Color.white)
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", DebugObjectColor);
            }
        }
        else
        {           
            //presets...
            switch (Preset)
            { 
                case InsectPreset.Wasp:
                    Jittering = true;
                    JitterAmount = 3.0f;
                    JitterTime = 3.0f;
                    Flying = true;
                    FlyingIntervalRandom = 0.5f;
                    FlyingStrength = 1.5f;
                    FlyingTime = 1.0f;
                    DebugObjectColor = Color.yellow;
                    break;
                case InsectPreset.Mosquito:
                    Jittering = true;
                    JitterAmount = 0.01f;
                    JitterTime = 0.1f;
                    Flying = true;
                    FlyingTime = 0.1f;
                    FlyingStrength = 0.1f;
                    DebugObjectColor = Color.red;
                    break;
                case InsectPreset.Fly:
                    Jittering = true;
                    JitterAmount = 0.5f;
                    JitterTime = 0.3f;
                    Flying = true;
                    FlyingStrength = 1.0f;
                    FlyingIntervalRandom = 0.2f;
                    FlyingTime = 0.5f;
                    DebugObjectColor = Color.green;
                    break;
            }
        }
	}

	private void OnDrawGizmos()
	{
		if (DebugMode && Application.isPlaying)		
        {
            try
            {
                //teken wat gizmos.             
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(InsectOrigin, RegionRadius);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, InsectTransformPosition);

             
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Mesh cu = go.GetComponent<MeshFilter>().sharedMesh;
                    
                Gizmos.color = Color.blue;
                Gizmos.DrawWireMesh(cu, InsectTransformPosition, InsectTransformRotation, this.transform.localScale);

                DestroyImmediate(go);
                

            }
            catch(Exception E)
            {}
		}   
	}
 
	void Jitter()
	{        
		float x = Random.Range(-JitterAmount, JitterAmount);
		float y = Random.Range(-JitterAmount, JitterAmount);
		float z = Random.Range(-JitterAmount, JitterAmount);

		InsectTransformPosition += new Vector3(x, y, z);
	} 

	void Fly()
	{ 
		//volg een vector met een bepaalde lengte.
		float strength = Random.Range(-FlyingStrength, FlyingStrength); // de ene of de andere kant op.         

		Quaternion rot = Random.rotation;
		Vector3 cp = InsectTransformPosition;

		Vector3 cprot = rot * cp; //rotated vector

		InsectTransformRotation = rot;

		//ff zien of de dist nog wel in de region zit
		if (StickToRegion)
		{
			Vector3 prospected = InsectTransformPosition + (rot * Vector3.forward) * strength;
			float prospectedDist = Vector3.Distance(InsectOrigin, prospected);
			
			if (prospectedDist < RegionRadius)
				InsectTransformPosition += (rot * Vector3.forward) * strength;
			else
			{
				//go back to origin
				float angle = Vector3.Angle(this.transform.position, InsectOrigin);
				float distance = Vector3.Distance(this.transform.position, InsectOrigin);

				float fx = (InsectOrigin.x - this.transform.position.x) / distance;
				float fy = (InsectOrigin.y - this.transform.position.y) / distance;
				float fz = (InsectOrigin.z - this.transform.position.z) / distance;

				Vector3 target = new Vector3(fx, fy, fz);     

				InsectTransformPosition += target;
			}
		}
	}

    private void Flee()
    {
        float angle = Vector3.Angle(this.transform.position, GameObject.Find("Player").transform.position);
        InsectTransformRotation = Quaternion.AngleAxis(angle, this.transform.forward);
        
        float distance = Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position);

        float fx = (this.transform.position.x - GameObject.Find("Player").transform.position.x) / distance;
        float fy = (this.transform.position.y - GameObject.Find("Player").transform.position.y) / distance;
        float fz = (this.transform.position.z - GameObject.Find("Player").transform.position.z) / distance;

        Vector3 target = new Vector3(fx, fy, fz);

        InsectTransformPosition += target; //ga weg van speler. 
    }
}