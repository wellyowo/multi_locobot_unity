using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class pun_player_spawner_test : MonoBehaviourPunCallbacks
{
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

            spawnPlayerprefab = PhotonNetwork.Instantiate("Player_1", transform.position, transform.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate("Player_1", transform.position, transform.rotation);
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

