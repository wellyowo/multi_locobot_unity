using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class update_dope_state : MonoBehaviour
{
    // public GameObject targetObj;
    private Rigidbody rb;
    private GameObject controllers;
    private Transform right_finger_Pos;
    private Transform left_finger_Pos;
    private bool isGrabbing, collided;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        right_finger_Pos = GameObject.FindWithTag("finger_r").transform;
        left_finger_Pos = GameObject.FindWithTag("finger_l").transform;
        controllers = GameObject.FindWithTag("Controller");
        rb.isKinematic = true;
        
    }

    void FixedUpdate()
    {
        getGrab();
        if (collided && isGrabbing)
        {
            //Debug.Log("grabbing");
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.MovePosition(rb.position + ((right_finger_Pos.position + left_finger_Pos.position) / 2 - rb.worldCenterOfMass));
        }
        else
        {
            //Debug.Log("not grabbing");
            rb.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.name == "finger_r_0" || other.name == "finger_l_0")
        {
            if (isGrabbing)
            {
                collided = true;
                //Debug.Log("collided");
            }
            else
            {
                collided = false;
                //Debug.Log("not collided");
            }
        }
    }
    private void getGrab()
    {
        if (controllers.GetComponent<ControllersManager>().getRightPrimaryButton())
        {
            isGrabbing = true;
            //Debug.Log("True");
        }

        if (controllers.GetComponent<ControllersManager>().getRightSecondaryButton())
        {
            isGrabbing = false;
            //Debug.Log("false");
        }
    }

}
