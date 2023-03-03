using System.Collections;
using System.Collections.Generic;
using System.IO;
using RosSharp.RosBridgeClient;
using sensor_msgs = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;

public class ImageLatency : MonoBehaviour
{
    RosSocket rosSocket;
    public string WebSocketIP; //IP address
    public string Topic_name_pub; //topic name
    public string Topic_name_sub; //topic name
    
    private string RosBridgeServerUrl; //IP address

    string image_pub;
    string image_sub;
    sensor_msgs.CompressedImage pub_image = new sensor_msgs.CompressedImage();

    // Start is called before the first frame update
    void Start()
    {
        //RosSocket
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Debug.Log("Established connection with ros");

        //Topic name
        image_pub = rosSocket.Advertise<sensor_msgs.CompressedImage>(Topic_name_pub);
        image_sub = rosSocket.Subscribe<sensor_msgs.CompressedImage>(Topic_name_sub, data_process);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void data_process(sensor_msgs.CompressedImage message)
    {
        pub_image.header.stamp = message.header.stamp;
        rosSocket.Publish(image_pub, pub_image);
    }
}

