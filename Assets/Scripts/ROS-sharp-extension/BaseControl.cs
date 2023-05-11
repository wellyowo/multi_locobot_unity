using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using sensor_msgs = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class BaseControl : MonoBehaviour
{
    RosSocket rosSocket;
    public string WebSocketIP; //IP address
    public ControllersManager controllerInput;
    // public string Topic_name_sub; //topic name
    public string Topic_name_pub; //topic name
    private string RosBridgeServerUrl; //IP address

    string twist_target;
    // string twist_recieve;

    private Vector2 Primary2DAxis;
    public float linear_scale = 1;
    private float linear_speed;
    public float angular_scale = 1;
    private float angular_speed;

    geo_msgs.Twist message_twist = new geo_msgs.Twist();
    // geo_msgs.TwistStamped recieve_twist = new geo_msgs.TwistStamped();

    // Start is called before the first frame update
    void Start()
    {
        //RosSocket
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Debug.Log("Established connection with ros");

        //Topic name
        twist_target = rosSocket.Advertise <geo_msgs.Twist> (Topic_name_pub);
        // twist_recieve = rosSocket.Subscribe<geo_msgs.TwistStamped>(Topic_name_sub, data_process);
    }

    // Update is called once per frame
    void Update()
    {
        Primary2DAxis = controllerInput.GetComponent<ControllersManager>().getRightPrimary2DAxis();

        linear_speed = Primary2DAxis.y;
        linear_speed = linear_speed * linear_scale;
        message_twist.linear.x = linear_speed;

        angular_speed = Primary2DAxis.x;
        angular_speed = -angular_speed * angular_scale;
        message_twist.angular.z = angular_speed;

        // message_twist.header.Update();

        rosSocket.Publish(twist_target, message_twist);
    }

    // private void data_process(geo_msgs.TwistStamped message)
    // {
    //     recieve_twist.header.Update();
    //     Debug.Log("last " + message.header.stamp.secs + "." + message.header.stamp.nsecs + " now " + recieve_twist.header.stamp.secs + "." + recieve_twist.header.stamp.nsecs);
    // }
}
