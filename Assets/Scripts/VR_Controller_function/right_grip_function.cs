using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class right_grip_function : MonoBehaviour
{
    public Transform rightControllerSource; // Source
    public GameObject controllerInput; // input
    public GameObject ConnectorOutput; // output

    //visual aids -- end-effector trajector
    public Material lineMaterial;
    public float startWidth = 0.05f, endWidth = 0.05f;
    public float destroyTimer = 4f;

    private List<GameObject> lines = new List<GameObject>(); //lists of lines
    private LineRenderer newLine;
    private int numClicks = 0;

    private float rightgripValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rightgripValue = controllerInput.GetComponent<ControllersManager>().getRightGrip();
        trajectoryMode(rightgripValue);

    }

    private void trajectoryMode(float rightgripValue)
    {
        if (rightgripValue > 0.7f)
        {
            newLine.enabled = true;
            newLine.positionCount = numClicks + 1;
            newLine.SetPosition(numClicks, rightControllerSource.position);
            numClicks ++ ;

            ConnectorOutput.GetComponent<JointStatePublisher>().enabled = true;
        }
        else
        {   
            if (rightgripValue > 0.1f) { 
                GameObject draw = new GameObject();
                newLine = draw.AddComponent<LineRenderer>();
                newLine.startWidth = startWidth;
                newLine.material = lineMaterial;
                newLine.enabled = false;

                lines.Add(newLine.gameObject);
                numClicks = 0;
            }
            else if (rightgripValue <0.1f)
            {
                delLine();
            }
            ConnectorOutput.GetComponent<JointStatePublisher>().enabled = false;
        }
    }

    private void delLine()
    {
        for(int i = 0; i < lines.Count; i++)
        {
            if (lines[i] != null)
            {
                Destroy(lines[i]);
            }
        }
        lines = new List<GameObject>();
    }
}
