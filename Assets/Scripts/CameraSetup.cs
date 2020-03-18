using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using SimpleJSON;

public class CameraSetup : MonoBehaviour
{
    public string configFile = "Assets/Resources/output.json";
    public GameObject[] zed_PCL_Prefabs;
    [Range(1, 100)]
    public int confidenceThreshold = 100, textureConfidenceThreshold = 100;

    JSONNode zeds;
    GameObject[] cameras = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        string jsonText = File.ReadAllText(configFile);
        if (jsonText != "")
        {
            zeds = JSON.Parse(jsonText);
            foreach (KeyValuePair<string, JSONNode> zed in zeds)
            {
                int index = int.Parse(zed.Key.Split('_')[1]);
                cameras[index] = Instantiate(zed_PCL_Prefabs[index]);
                cameras[index].name = zed.Key;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<string, JSONNode> zed in zeds)
        {
            int index = int.Parse(zed.Key.Split('_')[1]);
            cameras[index].transform.position = new Vector3(
                zed.Value["LEFT_HANDED_Y_UP"]["translation_meters"]["tx"],
                zed.Value["LEFT_HANDED_Y_UP"]["translation_meters"]["ty"],
                zed.Value["LEFT_HANDED_Y_UP"]["translation_meters"]["tz"]);
            cameras[index].transform.rotation = Quaternion.identity;
            cameras[index].transform.Rotate(new Vector3(
                zed.Value["LEFT_HANDED_Y_UP"]["euler_angles_degrees"]["x"],
                zed.Value["LEFT_HANDED_Y_UP"]["euler_angles_degrees"]["y"],
                zed.Value["LEFT_HANDED_Y_UP"]["euler_angles_degrees"]["z"]));
            cameras[index].GetComponent<ZEDManager>().confidenceThreshold = confidenceThreshold;
            cameras[index].GetComponent<ZEDManager>().textureConfidenceThreshold = textureConfidenceThreshold;
        }
    }
}
