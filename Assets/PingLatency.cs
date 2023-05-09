using System.Collections;
using System.Collections.Generic;
using System.IO;
using RosSharp.RosBridgeClient;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;

public class PingLatency : MonoBehaviour
{
    RosSocket rosSocket;
    public string WebSocketIP; //IP address
    public string Topic_name_pub; //topic name
    public string Topic_name_sub; //topic name
    public TMPro.TextMeshPro text_obj;
    
    private string RosBridgeServerUrl; //IP address
    private double latency, latency_count = 0;
    private uint count, i = 0;

    string twist_pub;
    string twist_sub;
    geo_msgs.TwistStamped message_TwistStamped = new geo_msgs.TwistStamped();

    // string formatString = "{0:G" + 5 + "}\t{1:G" + 5 + "}";

    // Start is called before the first frame update
    void Start()
    {
        //RosSocket
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Debug.Log("Established connection with ros");

        //Topic name
        twist_pub = rosSocket.Advertise<geo_msgs.TwistStamped>(Topic_name_pub);
        twist_sub = rosSocket.Subscribe<geo_msgs.TwistStamped>(Topic_name_sub, data_process);
    }

    // Update is called once per frame
    void Update()
    {
        message_TwistStamped.header.Update();
        rosSocket.Publish(twist_pub, message_TwistStamped);
        latency_count += latency;
        while(count > 15)
        {
            latency_count /= 15;
            text_obj.text = "RTT : " + latency_count.ToString("f1") + " ms";

            // StreamWriter writer = new StreamWriter("Assets/Resources/5G_to_eth_VPN_FloatArray.txt", true);
            // writer.WriteLine(formatString, i, latency);
            // writer.Close();
            count = 0;
            latency_count = 0;
        }
    }

    private void data_process(geo_msgs.TwistStamped message)
    {
        message_TwistStamped.header.Update();
        // Debug.Log("last " + message.header.stamp.secs + "." + message.header.stamp.nsecs + " now " + message_TwistStamped.header.stamp.secs + "." + message_TwistStamped.header.stamp.nsecs);
        if(message.header.stamp.secs == message_TwistStamped.header.stamp.secs)
        {
            latency = (message_TwistStamped.header.stamp.nsecs - message.header.stamp.nsecs) / 1000000.0D;
        }
        else
        {
            latency = (message_TwistStamped.header.stamp.nsecs + message.header.stamp.nsecs) / 1000000.0D - 1000.0D;
        }

        ++count;
        i = message.header.seq;
    }
}
