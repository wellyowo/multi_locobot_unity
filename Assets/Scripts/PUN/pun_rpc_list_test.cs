using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class pun_rpc_list_test : MonoBehaviour
{
    public GameObject cube_1, cube_2;

    [PunRPC]
    void changeColour_1(float r, float g, float b, bool right_pri_button)
    {
        cube_1.GetComponent<Renderer>().material.color = new Color(r, g, b, 1f);
    }

    [PunRPC]
    void changeColour_2(float r, float g, float b, bool right_sec_button)
    {
        cube_2.GetComponent<Renderer>().material.color = new Color(r, g, b, 1f);
    }


}
