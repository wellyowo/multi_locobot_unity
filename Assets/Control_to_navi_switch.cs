using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_to_navi_switch : MonoBehaviour
{
    public MeshRenderer leftCamera;
    public GameObject leftpointcloud;
    public GameObject midpointcloud;
    public GameObject rightpointcloud;
    

    public ControllersManager controllerInput;

    private bool leftprimarybuttonValue;
    private bool button_pressed = false;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        leftprimarybuttonValue = controllerInput.GetComponent<ControllersManager>().getLeftPrimaryButton();
        if(leftprimarybuttonValue == true)
        {
            button_pressed = true;
            
        }
        if(leftprimarybuttonValue == false && button_pressed == true)
        {
            button_pressed = false;
            counter +=1;
            switch(counter %= 2)
            {
                case 0:
                    leftCamera.enabled = true;
                    leftpointcloud.SetActive(true);
                    midpointcloud.SetActive(true);
                    rightpointcloud.SetActive(true);
                    break;
                case 1:
                    leftCamera.enabled = false;
                    leftpointcloud.SetActive(false);
                    midpointcloud.SetActive(false);
                    rightpointcloud.SetActive(false);
                    break;
            
            }
        }
        


    }
}