using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchButton : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject sideCamera;
    public GameObject side_225_Camera;
    public GameObject side_450_Camera;
    public GameObject side_675_Camera;

    public ControllersManager controllerInput;

    private float rightTriggerValue;
    private bool trigger_pressed = false, trigger_released = true;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rightTriggerValue = controllerInput.GetComponent<ControllersManager>().getRightTrigger();
        if(rightTriggerValue > 0.7f && trigger_released == true)
        {
            trigger_pressed = true;
            trigger_released = false;
        }
        if(rightTriggerValue < 0.7f && trigger_pressed == true)
        {
            trigger_pressed = false;
            trigger_released = true;
            counter +=1;
            switch(counter %= 5)
            {
                case 0:
                    sideCamera.SetActive(false);
                    side_225_Camera.SetActive(false);
                    side_450_Camera.SetActive(false);
                    side_675_Camera.SetActive(false);
                    mainCamera.SetActive(true);
                    break;
                case 1:
                    sideCamera.SetActive(false);
                    side_225_Camera.SetActive(true);
                    side_450_Camera.SetActive(false);
                    side_675_Camera.SetActive(false);
                    mainCamera.SetActive(false);
                    break;
                case 2:
                    sideCamera.SetActive(false);
                    side_225_Camera.SetActive(false);
                    side_450_Camera.SetActive(true);
                    side_675_Camera.SetActive(false);
                    mainCamera.SetActive(false);
                    break;
                case 3:
                    sideCamera.SetActive(false);
                    side_225_Camera.SetActive(false);
                    side_450_Camera.SetActive(false);
                    side_675_Camera.SetActive(true);
                    mainCamera.SetActive(false);
                    break;
                case 4:
                    sideCamera.SetActive(true);
                    side_225_Camera.SetActive(false);
                    side_450_Camera.SetActive(false);
                    side_675_Camera.SetActive(false);
                    mainCamera.SetActive(false);
                    break;
            }
        }
    
    }

    // void OnGUI()
    // {
    //     if (GUI.Button (new Rect (10, 10, 100, 30), "Main Camera"))
    //     {
    //         // This code is executed when the Button is clicked
    //         sideCamera.SetActive(false);
    //         side_225_Camera.SetActive(false);
    //         side_450_Camera.SetActive(false);
    //         side_675_Camera.SetActive(false);
    //         mainCamera.SetActive(true);
    //     }

    //     if (GUI.Button (new Rect (510, 10, 100, 30), "Side Camera"))
    //     {
    //         // This code is executed when the Button is clicked
    //         sideCamera.SetActive(true);
    //         side_225_Camera.SetActive(false);
    //         side_450_Camera.SetActive(false);
    //         side_675_Camera.SetActive(false);
    //         mainCamera.SetActive(false);
    //     }

    //     if (GUI.Button (new Rect (135, 10, 100, 30), "22.5 Camera"))
    //     {
    //         // This code is executed when the Button is clicked
    //         sideCamera.SetActive(false);
    //         side_225_Camera.SetActive(true);
    //         side_450_Camera.SetActive(false);
    //         side_675_Camera.SetActive(false);
    //         mainCamera.SetActive(false);
    //     }

    //     if (GUI.Button (new Rect (260, 10, 100, 30), "45 Camera"))
    //     {
    //         // This code is executed when the Button is clicked
    //         sideCamera.SetActive(false);
    //         side_225_Camera.SetActive(false);
    //         side_450_Camera.SetActive(true);
    //         side_675_Camera.SetActive(false);
    //         mainCamera.SetActive(false);
    //     }

    //     if (GUI.Button (new Rect (385, 10, 100, 30), "67.5 Camera"))
    //     {
    //         // This code is executed when the Button is clicked
    //         sideCamera.SetActive(false);
    //         side_225_Camera.SetActive(false);
    //         side_450_Camera.SetActive(false);
    //         side_675_Camera.SetActive(true);
    //         mainCamera.SetActive(false);
    //     }
    // }
}
