using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class FakeBaseControl : MonoBehaviour
{
    public ControllersManager controllerInput;
    public Transform robot_pose;

    private Vector2 Primary2DAxis;
    public float linear_scale = 1;
    private float linear_speed;
    public float angular_scale = 1;
    private float angular_speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Primary2DAxis = controllerInput.GetComponent<ControllersManager>().getRightPrimary2DAxis();

        linear_speed = Primary2DAxis.y;
        linear_speed = linear_speed * linear_scale;

        angular_speed = Primary2DAxis.x;
        angular_speed = angular_speed * angular_scale;

        // robot_pose.position = new Vector3(robot_pose.position.x, robot_pose.position.y, robot_pose.position.z + linear_speed);
        robot_pose.Translate(0, 0, linear_speed,Space.Self);
        robot_pose.Rotate(0.0f, angular_speed, 0.0f, Space.Self);
        
    }
}
