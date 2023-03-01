using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using RosSharp.RosBridgeClient;


public class NetworkRobot : MonoBehaviour
{
    private PhotonView photonView;
    public GameObject targetEndEffector;
    public GameObject targetPitch;
    public GameObject targetRoll;

    private PhotonView controllerPV;
    private Transform vr_controller;
    private InputDevice rightController, leftController;
    private float xVRr, zVRr;
    private List<InputDevice> devices = new List<InputDevice>();

    // Start is called before the first frame update
    void Start()
    {
        controllerPV = GameObject.FindWithTag("Controller").GetComponent<PhotonView>();
        photonView = GetComponent<PhotonView>();


        vr_controller = GameObject.FindWithTag("Right_hand").transform;
        Debug.Log(vr_controller);
       

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
        {
            rightController = devices[0];
        }

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
        {
            leftController = devices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        zVRr = vr_controller.eulerAngles.z;
        xVRr = vr_controller.eulerAngles.x;
        rightController.TryGetFeatureValue(CommonUsages.grip, out float gripRightValue);
        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

        if (photonView.IsMine)
        {
            targetEndEffector.transform.position = vr_controller.position;
            targetEndEffector.transform.rotation = vr_controller.rotation;

            targetPitch.transform.eulerAngles = new Vector3(xVRr, transform.eulerAngles.y, transform.eulerAngles.z);
            targetRoll.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zVRr);

            controllerPV.RPC("getNetworkRightGrip", RpcTarget.All, gripRightValue);
            controllerPV.RPC("getNetworkRightPri", RpcTarget.All, primaryButtonValue);
            controllerPV.RPC("getNetworkRightSec", RpcTarget.All, secondaryButtonValue);
        }

    }

}
