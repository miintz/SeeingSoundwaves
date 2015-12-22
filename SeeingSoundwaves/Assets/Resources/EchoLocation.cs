using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class EchoLocation : MonoBehaviour {

    public bool EnableEchoLocation = false;
    public Texture2D ScreenshotTexture;
    public Material ScreenshotMat;

	// Use this for initialization
	void Start () {        
	}
    
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(screenshotFunc());
        }
	}

    IEnumerator screenshotFunc()
    {
        yield return new WaitForEndOfFrame();
        ScreenshotTexture = new Texture2D(Mathf.RoundToInt(Screen.width), Mathf.RoundToInt(Screen.height), TextureFormat.ARGB32, false);
        ScreenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        ScreenshotTexture.Apply();
        ScreenshotMat.mainTexture = ScreenshotTexture;
    }
}
