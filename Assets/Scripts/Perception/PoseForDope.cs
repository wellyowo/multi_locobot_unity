using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using static RosSharp.TransformExtensions; // for transformation (Ros2Unity)
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;

public class PoseForDope : MonoBehaviour
{
    public string updateIP;
    public GameObject obj1;
    public string topic_name;
    private Vector3 obj1_position_unity;
    private Quaternion obj1_rotation_unity;
    private string obj1_id;

    RosSocket rosSocket;
    private string RosBridgeServerUrl;

    public float offset_x = 0, offset_y = 0, offset_z = 0;
    private bool isMessageReceived;
    public bool IsMessageReceived
    {
        get
        {
            return isMessageReceived;
        }
    }

    private int count = 0; // count if how long no dope message received then use kinematic mode.



    // Start is called before the first frame update
    void Start()
    {
        RosBridgeServerUrl = updateIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        obj1_id = rosSocket.Subscribe<geo_msgs.PoseStamped>(topic_name, Sub_obj1_dope_pose);
    }

    private void Sub_obj1_dope_pose(geo_msgs.PoseStamped message)
    {

        Vector3 dope_unity_position = R2U_Postion(message.pose.position);
        obj1_position_unity = dope_unity_position;
        Quaternion dope_unity_rotation = R2U_Rotation(message.pose.orientation);
        obj1_rotation_unity = dope_unity_rotation;
        count = 0;
        isMessageReceived = true;



    }

    private  Vector3 R2U_Postion(geo_msgs.Point tran_pose)
    {
        Vector3 position_unity;
        Vector3 position_ros;
        position_ros.x = (float)tran_pose.x + offset_x;
        position_ros.y = (float)tran_pose.y + offset_y;
        position_ros.z = (float)tran_pose.z + offset_z;
        position_unity = position_ros.Ros2Unity();
        return position_unity;
    }

    private  Quaternion R2U_Rotation(geo_msgs.Quaternion tran_rotation)
    {
        Quaternion rotation_ros;
        Quaternion rotation_unity;
        rotation_ros.x = (float)tran_rotation.x;
        rotation_ros.y = (float)tran_rotation.y;
        rotation_ros.z = (float)tran_rotation.z;
        rotation_ros.w = (float)tran_rotation.w;
        rotation_unity = rotation_ros.Ros2Unity();
        return rotation_unity;
    }

    // Update is called once per frame
    void Update()
    {
        obj1.transform.localPosition = obj1_position_unity;
        obj1.transform.localRotation = obj1_rotation_unity; 
        count++;
        if(count > 100)
        {
            isMessageReceived = false;
        }

    }
}
