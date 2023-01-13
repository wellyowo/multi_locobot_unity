using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using static RosSharp.TransformExtensions; // for transformation (Ros2Unity)
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using Photon.Pun;
using UnityEngine;

public class tfSyn : MonoBehaviour
{
    private PhotonView photonView;

    public GameObject updateIP;
    public GameObject obj1, obj2;
    private Vector3 obj1_position_unity, obj2_position_unity;
    private Quaternion obj1_rotation_unity, obj2_rotation_unity;
    private string obj1_id, obj2_id;

    RosSocket rosSocket;
    private string RosBridgeServerUrl;

    public GameObject locobot_arm, locobot_camera, locobot_camera_side;
    private Vector3 oa_position_unity, ac_position_unity, acs_position_unity;
    private Quaternion oa_rotation_unity_quat, ac_rotation_unity_quat, acs_rotation_unity_quat;
    private Vector3 oa_position_ros, ac_position_ros, acs_position_ros;
    private Quaternion oa_rotation_ros, ac_rotation_ros, acs_rotation_ros;
    private string oa_pos_id, ac_pos_id, acs_pos_id;
    private string oa_rot_id, ac_rot_id, acs_rot_id;

    public float offset_x = 0, offset_y = 0, offset_z = 0;


    void Start()
    {
        photonView = GetComponent<PhotonView>();


        RosBridgeServerUrl = updateIP.GetComponent<Update_rosip>().getRosIP();
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));

        oa_pos_id = rosSocket.Subscribe<std_msgs.Float32MultiArray>("/tf_oa_position", sub_oa_position); //obom to arm_base_link position
        oa_rot_id = rosSocket.Subscribe<std_msgs.Float32MultiArray>("/tf_oa_rotation", sub_oa_rotation); //obom to arm_base_link rotation
        ac_pos_id = rosSocket.Subscribe<std_msgs.Float32MultiArray>("/tf_ac_position", sub_ac_position); //arm_base_link to camera_color_optical_frame postion
        ac_rot_id = rosSocket.Subscribe<std_msgs.Float32MultiArray>("/tf_ac_rotation", sub_ac_rotation); //arm_base_link to camera_color_optical_frame rotation
        acs_pos_id = rosSocket.Subscribe<std_msgs.Float32MultiArray>("/tf_acs_position", sub_acs_position); //arm_base_link to ex_camera_side_link postion
        acs_rot_id = rosSocket.Subscribe<std_msgs.Float32MultiArray>("/tf_acs_rotation", sub_acs_rotation); //arm_base_link to ex_camera_side_link rotation

        obj1_id = rosSocket.Subscribe<geo_msgs.PoseStamped>("/dope/pose_butter", Sub_obj1_dope_pose);
        obj2_id = rosSocket.Subscribe<geo_msgs.PoseStamped>("/dope/pose_spaghetti", Sub_obj2_dope_pose);
    }

    private void Sub_obj1_dope_pose(geo_msgs.PoseStamped message)
    {

        Vector3 dope_unity_position = R2U_Postion(message.pose.position);
        obj1_position_unity = dope_unity_position;
        Quaternion dope_unity_rotation = R2U_Rotation(message.pose.orientation);
        obj1_rotation_unity = dope_unity_rotation;

    }

    private void Sub_obj2_dope_pose(geo_msgs.PoseStamped message)
    {

        Vector3 dope_unity_position = R2U_Postion(message.pose.position);
        obj1_position_unity = dope_unity_position;
        Quaternion dope_unity_rotation = R2U_Rotation(message.pose.orientation);
        obj1_rotation_unity = dope_unity_rotation;

    }


    private Vector3 R2U_Postion(geo_msgs.Point tran_pose)
    {
        Vector3 position_unity;
        Vector3 position_ros;
        position_ros.x = (float)tran_pose.x + offset_x;
        position_ros.y = (float)tran_pose.y + offset_y;
        position_ros.z = (float)tran_pose.z + offset_z;
        position_unity = position_ros.Ros2Unity();
        return position_unity;
    }

    private Quaternion R2U_Rotation(geo_msgs.Quaternion tran_rotation)
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

    //obom to arm_base_link
    private void sub_oa_position(std_msgs.Float32MultiArray message)
    {

        oa_position_ros.x = (float)message.data[0];
        oa_position_ros.y = (float)message.data[1];
        oa_position_ros.z = (float)message.data[2];
        oa_position_unity = oa_position_ros.Ros2Unity();

    }

    private void sub_oa_rotation(std_msgs.Float32MultiArray message)
    {

        oa_rotation_ros.x = (float)message.data[0];
        oa_rotation_ros.y = (float)message.data[1];
        oa_rotation_ros.z = (float)message.data[2];
        oa_rotation_ros.w = (float)message.data[3];
        oa_rotation_unity_quat = oa_rotation_ros.Ros2Unity();

    }
    //arm_base_link to camera_color_optical_frame
    private void sub_ac_position(std_msgs.Float32MultiArray message)
    {

        ac_position_ros.x = (float)message.data[0];
        ac_position_ros.y = (float)message.data[1];
        ac_position_ros.z = (float)message.data[2];
        ac_position_unity = ac_position_ros.Ros2Unity();

    }

    private void sub_ac_rotation(std_msgs.Float32MultiArray message)
    {

        ac_rotation_ros.x = (float)message.data[0];
        ac_rotation_ros.y = (float)message.data[1];
        ac_rotation_ros.z = (float)message.data[2];
        ac_rotation_ros.w = (float)message.data[3];
        ac_rotation_unity_quat = ac_rotation_ros.Ros2Unity();


    }

    //arm_base_link to camera_side_link
    private void sub_acs_position(std_msgs.Float32MultiArray message)
    {

        acs_position_ros.x = (float)message.data[0];
        acs_position_ros.y = (float)message.data[1];
        acs_position_ros.z = (float)message.data[2];
        acs_position_unity = acs_position_ros.Ros2Unity();
    }
    private void sub_acs_rotation(std_msgs.Float32MultiArray message)
    {

        acs_rotation_ros.x = (float)message.data[0];
        acs_rotation_ros.y = (float)message.data[1];
        acs_rotation_ros.z = (float)message.data[2];
        acs_rotation_ros.w = (float)message.data[3];
        acs_rotation_unity_quat = acs_rotation_ros.Ros2Unity();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            locobot_arm.transform.localPosition = oa_position_unity; // 0.0001868025 , 0.1075625 , 0.09729982
            locobot_arm.transform.localRotation = oa_rotation_unity_quat; // 0, 0.11 ,0
            locobot_camera.transform.localPosition = ac_position_unity; // -0.0283302 , 0.4797385 , -0.05248966
            locobot_camera.transform.localRotation = ac_rotation_unity_quat; // 0.642 , 92.353 , 92.177
            locobot_camera_side.transform.localPosition = acs_position_unity;
            locobot_camera_side.transform.localRotation = acs_rotation_unity_quat;

            obj1.transform.localPosition = obj1_position_unity;
            obj1.transform.localRotation = obj1_rotation_unity;
            obj2.transform.localPosition = obj2_position_unity;
            obj2.transform.localRotation = obj2_rotation_unity;
        }

    }
}
