using UnityEngine;
using System.Collections;

public class SpawnbyLoudness : MonoBehaviour
{

    public GameObject audioInputObject;
    public float threshold = 1.0f;
    public GameObject objectToSpawn;
    MicrophoneInput micIn;
    int i = 1;
    void Start()
    {
        if (objectToSpawn == null)
            Debug.LogError("You need to set a prefab to Object To Spawn -parameter in the editor!");
        if (audioInputObject == null)
            audioInputObject = GameObject.Find("AudioInputObject");
        micIn = (MicrophoneInput)audioInputObject.GetComponent("MicrophoneInput");

    }

    void Update()
    {

        float l = micIn.loudness;
        if (l > threshold)
        {

            Vector3 scale = new Vector3(i, 0, 0);

            GameObject newObject = (GameObject)Instantiate(objectToSpawn, scale, Quaternion.identity);
            //newObject.transform.localScale += scale;
            i++;
        }
    }
}