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
    private Transform rightConPos;
    private bool isGrabbing, collided;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        XRRig rig = FindObjectOfType<XRRig>();
        rightConPos = GameObject.FindWithTag("Right_hand").transform;
        controllers = GameObject.FindWithTag("Controller");

    }

    void Update()
    {
        getGrab();
        rb.isKinematic = true;
        if (collided && isGrabbing)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.MovePosition(rb.position + (rightConPos.position - rb.worldCenterOfMass));
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
            }
            else
            {
                collided = false;
            }
        }
    }
    private void getGrab()
    {
        if (controllers.GetComponent<ControllersManager>().getRightSecondaryButton())
        {
            isGrabbing = true;
        }

        if (controllers.GetComponent<ControllersManager>().getRightSecondaryButton())
        {
            isGrabbing = false;
        }
    }

}
