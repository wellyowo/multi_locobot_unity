using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;

public class right_joy_function : MonoBehaviour
{
    RosSocket rosSocket;
    public GameObject updateIP;
    public GameObject controllerInput;
    public bool PUN_flag = false;

    public string Topic_name_pub; //topic name
    private string RosBridgeServerUrl; //IP address


    private Vector2 Primary2DAxis;
    public float linear_scale = 0.25f;
    private float linear_speed;
    public float angular_scale = 2;
    private float angular_speed;
    
    private string twist_target;
    private geo_msgs.TwistStamped message_twist = new geo_msgs.TwistStamped();

    void Start()
    {
        RosBridgeServerUrl = updateIP.GetComponent<Update_rosip>().getRosIP();

        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        twist_target = rosSocket.Advertise<geo_msgs.TwistStamped>(Topic_name_pub);

    }

    void Update()
    {
        if (PUN_flag)
        {
            Primary2DAxis = controllerInput.GetComponent<ControllersManager_globle>().getRightPrimary2DAxis();
        }
        else
        {
            Primary2DAxis = controllerInput.GetComponent<ControllersManager>().getRightPrimary2DAxis();
        }
        

        linear_speed = Primary2DAxis.y;
        linear_speed = linear_speed * linear_scale;
        message_twist.twist.linear.x = linear_speed;

        angular_speed = Primary2DAxis.x;
        angular_speed = -angular_speed * angular_scale;
        message_twist.twist.angular.z = angular_speed;

        message_twist.header.Update();

        rosSocket.Publish(twist_target, message_twist);
    }


    private void OnApplicationQuit()
        {
            rosSocket.Unadvertise(twist_target);
            rosSocket.Close();
        }

}
