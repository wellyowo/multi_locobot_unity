using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class pun_rpc_test : MonoBehaviour
{
    private PhotonView photonView;
    private PhotonView controllerPV;
    public GameObject controllerInput;
    private bool RightprimaryButtonValue, RightsecondaryButtonValue;

    public GameObject cube;
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
        controllerPV = GameObject.FindWithTag("Controller").GetComponent<PhotonView>();

    }

    void Update()
    {
        RightprimaryButtonValue = controllerInput.GetComponent<ControllersManager_globle>().getRightPrimaryButton();

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
    }

    [PunRPC]
    void changeColour(float r, float g, float b, bool right_pri_button)
    {
        cube.GetComponent<Renderer>().material.color = new Color(r, g, b, 1f);
        Debug.Log("Pressed button_" + right_pri_button.ToString() + "_" + System.DateTime.Now.ToString());
        Debug.Log("Network_" + r.ToString() + "_" + g.ToString() + "_" + b.ToString());

    }

}
