using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class pun_rpc_test_move : MonoBehaviour
{
    private PhotonView photonView;
    private PhotonView controllerPV;
    public GameObject controllerInput;
    private bool RightprimaryButtonValue, netowrked_RightprimaryButtonValue;
    public GameObject cube;

    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
        controllerPV = GameObject.FindWithTag("Controller").GetComponent<PhotonView>();
    }

    void Update()
    {
        RightprimaryButtonValue = controllerInput.GetComponent<ControllersManager_globle>().getRightPrimaryButton();
        if (RightprimaryButtonValue)
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            photonView.RPC("changeColour", RpcTarget.AllBuffered, r, g, b, RightprimaryButtonValue);
        }
        else
        {
            if (netowrked_RightprimaryButtonValue)
            {
                float r = Random.Range(0f, 1f);
                float g = Random.Range(0f, 1f);
                float b = Random.Range(0f, 1f);
                photonView.RPC("changeColour", RpcTarget.AllBuffered, r, g, b, RightprimaryButtonValue);
            }
        }
    }
}
