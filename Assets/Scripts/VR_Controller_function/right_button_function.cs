using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;


public class right_button_function : MonoBehaviour
{
    RosSocket rosSocket;
    public GameObject updateIP;
    public GameObject controllerInput; // input
    public bool PUN_flag = false;

    public string Topic_name_pub_primary_button = "/vr/right/primarybutton"; //topic name
    public string Topic_name_pub_secondary_button = "/vr/right/secondarybutton"; //topic name
    private string RosBridgeServerUrl; //IP address

    string Rightprimary_button, Rightscondary_button;
    private bool RightprimaryButtonValue, RightsecondaryButtonValue;

    void Start()
    {
        RosBridgeServerUrl = updateIP.GetComponent<Update_rosip>().getRosIP();
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Rightprimary_button = rosSocket.Advertise<std_msgs.Bool>(Topic_name_pub_primary_button);
        Rightscondary_button = rosSocket.Advertise<std_msgs.Bool>(Topic_name_pub_secondary_button);
    }

    void Update()
    {
        if (PUN_flag)
        {
            RightprimaryButtonValue = controllerInput.GetComponent<ControllersManager_globle>().getRightPrimaryButton();
            RightsecondaryButtonValue = controllerInput.GetComponent<ControllersManager_globle>().getRightSecondaryButton();

        }
        else
        {
            RightprimaryButtonValue = controllerInput.GetComponent<ControllersManager>().getRightPrimaryButton();
            RightsecondaryButtonValue = controllerInput.GetComponent<ControllersManager>().getRightSecondaryButton();
        }

        //------------------Pub_Primary Buttom------------------------------//
        std_msgs.Bool message_p = new std_msgs.Bool
        {
            data = RightprimaryButtonValue
        };

        rosSocket.Publish(Rightprimary_button, message_p);

        //------------------Pub_Secondary Buttom------------------------------//
        std_msgs.Bool message_s = new std_msgs.Bool
        {
            data = RightsecondaryButtonValue
        };

        rosSocket.Publish(Rightscondary_button, message_s);
    }
    private void OnApplicationQuit()
        {
            rosSocket.Unadvertise(Rightprimary_button);
            rosSocket.Unadvertise(Rightscondary_button);
            rosSocket.Close();
        }

    
}
