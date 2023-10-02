using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocobotSwitch : MonoBehaviour
{
    public GameObject locobot1;
    public GameObject locobot2;
   
    public ControllersManager controllerInput;

    private bool leftsecondarybuttonValue;
    private bool button_pressed = false;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        leftsecondarybuttonValue = controllerInput.GetComponent<ControllersManager>().getLeftSecondaryButton();
        if(leftsecondarybuttonValue == true)
        {
            button_pressed = true;
            
        }
        if(leftsecondarybuttonValue == false && button_pressed == true)
        {
            button_pressed = false;
            counter +=1;
            switch(counter %= 2)
            {
                case 0:
                    locobot1.SetActive(true);
                    locobot2.SetActive(false);
                    break;
                case 1:
                    locobot1.SetActive(false);
                    locobot2.SetActive(true);
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
