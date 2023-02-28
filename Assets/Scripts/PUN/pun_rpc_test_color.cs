using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class pun_rpc_test_color : MonoBehaviour
{
    public GameObject cub;
    private PhotonView photonView;
    private PhotonView controllerPV;
    private GameObject controllerInput;
    private bool RightprimaryButtonValue;

    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
        controllerPV = GameObject.FindWithTag("Controller").GetComponent<PhotonView>();
        controllerInput = GameObject.FindWithTag("Controller");
    }

    void Update()
    {
        

        RightprimaryButtonValue = controllerInput.GetComponent<ControllersManager_globle>().getRightPrimaryButton();
        controllerPV.RPC("getNetworkRightPri", RpcTarget.All, RightprimaryButtonValue);

        if (photonView.IsMine)
        {
  
            if (RightprimaryButtonValue)
            {
                float r = Random.Range(0f, 1f);
                float g = Random.Range(0f, 1f);
                float b = Random.Range(0f, 1f);
                photonView.RPC("changeColour", RpcTarget.AllBuffered, r, g, b, RightprimaryButtonValue);
            }
        }
        else
        {
            Debug.Log(RightprimaryButtonValue);
        }

    }

    [PunRPC]
    void changeColour(float r, float g, float b, bool right_pri_button)
    {
        cub.GetComponent<Renderer>().material.color = new Color(r, g, b, 1f);
    }
}

