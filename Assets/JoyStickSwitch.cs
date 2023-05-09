using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickSwitch : MonoBehaviour
{
    public GameObject Axis;
    public GameObject JoyStck;
    bool status = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("h"))
        {
            // Debug.Log(Input.GetButtonDown("Fire1"));
            status = !status;
        }

        if(status)
        {
            Axis.SetActive(true);
            JoyStck.SetActive(false);
        }else
        {
            Axis.SetActive(false);
            JoyStck.SetActive(true);
        }
    }
}
