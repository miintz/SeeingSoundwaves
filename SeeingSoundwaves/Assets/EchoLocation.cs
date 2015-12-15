using UnityEngine;
using System.Collections;
using System;

public class EchoLocation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    Texture2D previousFrame;
    private bool takeHiResShot = false;
    
    static int o = 0;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeHiResShot();
            //GetComponent<Camera>().enabled = !GetComponent<Camera>().enabled;
        }

        if(previousFrame != null)
        {
            Debug.Log("overlay camera?");   
        }
	}

    public static string ScreenShotName(int width, int height)
    {
        o++;

        return string.Format("{0}/screenshots/screen_{1}_{2}.png",
                             Application.dataPath,
                             o,
                             (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        
        
    }
    public void TakeHiResShot()
    {
        takeHiResShot = true;
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

            Destroy(rt);

            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);

            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            
            takeHiResShot = false;       
        }
    }
}
