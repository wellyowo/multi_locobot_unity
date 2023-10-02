/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class switch_locobot : MonoBehaviour
{
    public Transform[] LoCobotTransform;

    public Transform CameraTransform;

    public GameObject[] locobot;

    public int UserRobotID = 1;
    
    public string FrameId = "Unity";
    

    public ControllersManager ControllerInput;

    public Transform RightController;

    public GameObject[] RobotSelectButton;

    [SerializeField]
    private Material OnHitMaterial;

    [SerializeField]
    private Material ActiveMaterial;

    [SerializeField]
    private Material InactiveMaterial;

    private int robot_select;
    private bool RightSecBtnRelease;
    
    private void Start()
    {
        robot_select = UserRobotID;
        for (int i = 0; i < RobotSelectButton.Length; i++)
        {
            if (i == robot_select - 1)
            {
                RobotSelectButton[i].GetComponent<Renderer>().material =
                    ActiveMaterial;
            }
            else
            {
                RobotSelectButton[i].GetComponent<Renderer>().material =
                    InactiveMaterial;
            }
        }
        disable_robot(robot_select);
    }

    private void FixedUpdate()
    {
        RobotSelect();
    }

    private void disable_robot(int robot_select)
    {
        for (int i = 0; i < locobot.Length; i++)
        {
            if (i == robot_select - 1)
            {
                locobot[i].SetActive(true);
            }
            else
            {
                locobot[i].SetActive(false);
            }
        }
    }
    private void RobotSelect()
    {
        Ray ray =
            new Ray(RightController.position, RightController.forward);

        double rightTriggerValue = ControllerInput.getRightTrigger();

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            for (int i = 0; i < RobotSelectButton.Length; i++)
            {
                if (i == robot_select - 1)
                {
                    RobotSelectButton[i].GetComponent<Renderer>().material =
                        ActiveMaterial;
                }
                else
                {
                    RobotSelectButton[i].GetComponent<Renderer>().material =
                        InactiveMaterial;
                }
            }

            if (rightTriggerValue > 0.7)
            {
                Debug.Log("bobo" + hit.collider.tag);
                switch (hit.collider.tag)
                {
                    case "robot1":
                        RobotSelectButton[0].GetComponent<Renderer>().material = OnHitMaterial;
                        robot_select = 1;
                        disable_robot(robot_select);
                        break;
                    case "robot2":
                        RobotSelectButton[1].GetComponent<Renderer>().material = OnHitMaterial;
                        robot_select = 2;
                        disable_robot(robot_select);
                        break;
                    case "robot3":
                        RobotSelectButton[2].GetComponent<Renderer>().material = OnHitMaterial;
                        robot_select = 3;
                        disable_robot(robot_select);
                        break;
                    case "robot4":
                        RobotSelectButton[3].GetComponent<Renderer>().material = OnHitMaterial;
                        robot_select = 4;
                        disable_robot(robot_select);
                        break;
                }
            }
        }
        // XRTransform.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>().enabled = true;
        CameraTransform.position = LoCobotTransform[robot_select - 1].position;
        Debug.Log(robot_select + " pos:" + LoCobotTransform[robot_select - 1].position);
        CameraTransform.rotation = LoCobotTransform[robot_select - 1].rotation;
        Debug.Log(robot_select + " rot:" + LoCobotTransform[robot_select - 1].position);
    
        
    }
}

