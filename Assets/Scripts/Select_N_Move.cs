using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_N_Move : MonoBehaviour
{
    [SerializeField] private Material SetectedMaterial;
    [SerializeField] private Material DefaultMaterial;
    //private Material DefaultMaterial = null;
    private Renderer objectRenderer = null;
    public GameObject obj;

    RaycastHit hit;
    private Transform _selection = null;
    public Transform rightController;

    //VR Device
    public ControllersManager controllerInput;
    private bool RightprimaryButtonValue, RightsecondaryButtonValue;
    private bool LeftprimaryButtonValue, LeftsecondaryButtonValue;
    private float gripRightValue, gripLeftValue, rightTriggerValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_selection != null)
        {
            if (objectRenderer == null)
            {
                objectRenderer = _selection.GetComponent<Renderer>();
            }

            if (objectRenderer != null && DefaultMaterial != null)
            {
                objectRenderer.material = DefaultMaterial;
            }
            _selection = null;
        }

        Ray ray = new Ray(rightController.position, rightController.forward);

        //-------RIGHT CONTROLLER------------//
        //RightprimaryButtonValue = controllerInput.getRightPrimaryButton();
        RightprimaryButtonValue = controllerInput.GetComponent<ControllersManager>().getRightPrimaryButton();
        // {   data = RightprimaryButtonValue  };

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "real_duck" && RightprimaryButtonValue == true )
            {
                var selection = hit.transform;
                objectRenderer = selection.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    if (DefaultMaterial == null)
                    {
                        DefaultMaterial = objectRenderer.material;
                    }
                    objectRenderer.material = SetectedMaterial;
                }
                // float distance = hit.distance;
                //Debug.Log(distance); // Distance from the controller_transfrom.forward
                // Debug.Log(selection.position); // hit_Object_global_transform
                _selection = selection;
                obj.transform.SetParent(rightController);
            }
        }
        if (RightprimaryButtonValue == false )
            {
                obj.transform.SetParent(null);
            }
    }
}
