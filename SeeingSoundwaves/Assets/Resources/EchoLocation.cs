using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class EchoLocation : MonoBehaviour {

    public bool EnableEchoLocation = false;

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().depth = 0;
	}

    Texture2D previousFrame;
    private bool takeHiResShot = false;
    
    static int o = 0;

	// Update is called once per frame
	void Update () {

        //Vector4 v = new Vector4(GetComponent<Camera>().transform.position.x, GetComponent<Camera>().transform.position.y, GetComponent<Camera>().transform.position.z, 0.0f);
        //GameObject cu = GameObject.FindGameObjectWithTag("DebugCube");
        //if (cu != null)
        //{
        //    Material mat = (Material)Resources.Load("distanceLerp", typeof(Material));

        //    mat.SetVector("_CameraPosition", GetComponent<Camera>().transform.position);
        //    mat.SetFloat("_CameraDistance", Vector4.Distance(v, cu.transform.position));
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // = true;
            TakeHiResShot();
            //GetComponent<Camera>().enabled = false;
        }

        if(previousFrame != null)
        {
            byte[] fileData = File.ReadAllBytes(string.Format("{0}/screenshots/screen.png", Application.dataPath));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);

            GameObject.Find("Viewport").GetComponent<Renderer>().material.SetTexture("_MainTex", tex);
        }
	}

   

    public static string ScreenShotName(int width, int height)
    {
        o++;

        return string.Format("{0}/screenshots/screen.png", Application.dataPath);

        //return string.Format("{0}/screenshots/screen_{1}_{2}.png",
                             //Application.dataPath,
                             //o,
                             //(Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        
        
    }

    public void TakeHiResShot()
    {       
        takeHiResShot = EnableEchoLocation;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown(KeyCode.Space);
        if (takeHiResShot)
        {            
            int resWidth = Screen.width;
            int resHeight = Screen.height;
            
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            GetComponent<Camera>().targetTexture = rt;

            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            GetComponent<Camera>().Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

            GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors

            previousFrame = screenShot;

            Destroy(rt);

            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);

            System.IO.File.WriteAllBytes(filename, bytes);
            
            //Debug.Log(string.Format("Took screenshot to: {0}", filename));
            
            takeHiResShot = false;       
        }
    }
}
