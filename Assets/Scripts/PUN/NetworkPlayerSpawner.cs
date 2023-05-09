using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public bool robot_real_state = true;
    public string Name_RobotAvatar = "PunRPC_Real_locobot";

    public bool robot_camera_top = true;
    public string Name_RobotCameraTop = "PunRPC_Camera_view";

    public bool robot_object = false;
    public string Name_RobotObjectPose = "Locobot_tf";

    private GameObject spawnPlayerprefab;

    private bool firstPlayer;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount.CompareTo(1) > 0)
        {
            firstPlayer = true;
        }

        if (!firstPlayer)
        {

            spawnPlayerprefab = PhotonNetwork.Instantiate("PunRPC_Human_locobot", transform.position, transform.rotation);
        }
        else
        {
            
            if (robot_real_state) { PhotonNetwork.Instantiate(Name_RobotAvatar, transform.position, transform.rotation); }
            if (robot_camera_top) { PhotonNetwork.Instantiate(Name_RobotCameraTop, new Vector3(0.139f, 0.601f, 3.22f), transform.rotation * Quaternion.Euler(90, 90, -90)); }
            if (robot_object) { PhotonNetwork.Instantiate(Name_RobotObjectPose, transform.position, transform.rotation); }
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (firstPlayer)
        {
            PhotonNetwork.Destroy(spawnPlayerprefab);
        }

    }
}