using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class Update_rosip : MonoBehaviour
{
    public GameObject[] desired_object;
    public string updated_rosip;

    void Awake()
    {
        for (int i =0; i < desired_object.Length; i++)
        {
            try
            {
                desired_object[i].GetComponent<RosConnector>().RosBridgeServerUrl = updated_rosip;
            }
            catch
            {
                Debug.Log("No RosConnector component found in " + desired_object[i].name);
            }
            
        }
    }

    public string getRosIP()
    {
        return updated_rosip;
    }
}
