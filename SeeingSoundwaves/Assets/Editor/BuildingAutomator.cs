using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class BuildingAutomator : EditorWindow
{
    public GameObject myGameObject;
    public Material myNewMaterial;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        BuildingAutomator window = (BuildingAutomator)EditorWindow.GetWindow(typeof(BuildingAutomator));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Change material to default"))
        {
            //myGameObject.renderer.materials[0] = myNewMaterial; //<--- Error

            GameObject.FindGameObjectsWithTag("DistanceLerp").ToList().ForEach(t => t.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/default"));
        }

        if (GUILayout.Button("Change material to distanceLerp"))
        {
            //myGameObject.renderer.materials[0] = myNewMaterial; //<--- Error

            GameObject.FindGameObjectsWithTag("DistanceLerp").ToList().ForEach(t => t.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/distanceLerp"));
        }
    }
}